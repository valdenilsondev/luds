using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share.Enums;
using Assets.Scripts.Share.Kinect;
using Share.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Share.EventsSystem;
using Assets.Scripts.Share.Managers;
using Share.Controllers;
using UnityEngine.UI;

public enum Target {
	Left,
	Center,
	Right
}

public class ThrowManager : MonoBehaviour {

	private static ThrowManager instance;
	private int targetsCount = 0;
	private float remaining_min, remaining_sec;
	private bool startTimer;
	private float initialTime = 10f;
	private float time;
	[SerializeField]
	private float remainingTime;
	public Text clock_txt;

	//gesto configurado 0 default e 1 eh risehands/levantar a mao
	private int gesture_choice;
	[SerializeField]
	private bool gesture_default, gesture_risehand;

	//acertos
	public Text apple_txt;
	//jogadas
	public Text hits_txt;
	//erros
	public Text miss_txt;
	[HideInInspector]
	public float hits, miss, totalapple;
	//public GameObject hand;
	public Text userFeedback;
	public Sprite gameover_board;

	public Text debug;
	public Animator[] targetsAnimators;
	public Target currentTarget;
	public Dictionary<KinectGestures.Gestures, Movement> playerMovements;
	public List<ScoreItem> scoreItems;

	float gameTime;

	void Awake() {
		instance = this;
		//ThrowSoundManager.Instance.PlayEnviromentSfx();
		scoreItems = new List<ScoreItem>();
	
		//scoreItems.Add(new ScoreItem { multiplyingFactor = 2, type = ScoreItemsType.TotalThrow });
		scoreItems.Add(new ScoreItem { multiplyingFactor = 2, type = ScoreItemsType.Hits });//acertos
		scoreItems.Add(new ScoreItem { multiplyingFactor = 2, type = ScoreItemsType.Miss });//erros
		scoreItems.Add(new ScoreItem { multiplyingFactor = 1, type = ScoreItemsType.Apple });//total de arremeços
		//Carregar parametrização feita pela fisioterapeuta. Ex: Levantar a mão faz com que o Pig execute a ação de pular
		playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();

		//olha qual configuraçao dos gestos
		gesture_choice = PlayerPrefsManager.GetIdPerfilThrow();

		remainingTime = PlayerPrefsManager.GetRemaininigtimethrow();
		time = PlayerPrefsManager.GetPlaqueTime();
		InitTimer();
		AddPlayerMovements ();
	}

	void Start(){
		if(SoundManager.Instance != null)
			SoundManager.Instance.StopStartscreenSound();
		GameManagerShare.instance.SetActiveBlur(false);
	
		//hand.gameObject.SetActive(true);
		//Aim_controller.instance.gameObject.SetActive(true);
	}

	public void StartCount(){
		StartTargets ();
		hits = 0;
		miss = 0;
		totalapple = 0;
		startTimer = true;
		CountDownManager.instance.Initialize();
	}

	public void StartGame(){
		//time = PlayerPrefsManager.GetBorgTime ();
	}

	public static ThrowManager Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType<ThrowManager>();
				
				if(instance == null) {
					Debug.LogWarning ("Nenhum objeto do tipo TargetsManager foi encontrado");
				}
			}
			return instance;
		}
	}

	void Update () {
		UpdateTimer();//tempo das placas
	
		if(Input.GetKeyDown(KeyCode.Return)) {
			Events.RaiseEvent<GameStart>();
		}
		if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()
		   && GameManagerShare.IsStarted()){
			StartGameClock();
		//	Aim_controller.instance.gameObject.SetActive(true);

		}
		if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && GameManagerShare.IsStarted())
		{
			//pause se sair do kinect
			if(GameManagerShare.instance.IsUsingKinect() && DistanceCalibrator.instance.CheckOutOfScreen())
			{
				GameManagerShare.instance.PauseGame();// PauseWithoutBorg();
			}
			if(remainingTime <= 0){
				GameManagerShare.instance.PauseGame();
			}

			miss_txt.text = miss.ToString();
		}
		if(GameManagerShare.IsStarted() && GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			if(BorgManager.instance.borgItemSelected)
			{
				GameManagerShare.instance.GameOver();
			}
		}

		if(Input.GetKeyDown(KeyCode.U) && !GameManagerShare.IsPaused()){
			GameManagerShare.instance.PauseGame();
		}
	}

	//tempo das placas
	private void UpdateTimer() {
		time -= Time.deltaTime; 
		if(time < 0) {
			OnTimer ();
		}
	}
	public Sprite GetGameOverBoard()
	{
		return gameover_board;
	}
	public float GetHits()
	{
		return hits;
	}
	public void SetHits(float value)
	{
		hits = value;
	}

	public void UnPause()
	{
		for (int i = 0; i < targetsAnimators.Length; i++) {
			targetsAnimators[i].enabled = true;
		}
		GameManagerShare.instance.SetActiveBlur(false);
	}
	public void GameOver() {
		BorgManager.instance.borgItemSelected = false;
		BorgManager.instance.SendBorg();
		startTimer = false;
		time = initialTime;
		//IncreaseCurrentMatch ();
		//SendRoundTime ();
		if (SoundManager.Instance != null) 
		{
			SoundManager.Instance.StopBGmusic();
			/*if(ThrowSoundManager.Instance != null)
			{
				ThrowSoundManager.Instance.StopEnviromentSfx();
			}*/
			SoundManager.Instance.PlayGameTransition();
		}
		GameOverScreenController.instance.Enable();
		print ("game over do throw");
	}

	private void OnTimer() {
		time = initialTime;
		Down();
	}

	private void InitTimer() {
		initialTime = time;
	}

	private void StartGameClock()
	{
		if(startTimer = true && remainingTime >= 0)
		{
			remaining_min = (int)(remainingTime/60);
			remaining_sec = remainingTime % 60;
			remainingTime -= Time.deltaTime; //PlayerPrefsManager.GetRemainingTime();
			var segundos = remaining_sec;
			if(segundos >= 10){
				clock_txt.text = (remaining_min + ":" + (int)(remaining_sec));
			}
			else{
				clock_txt.text = (remaining_min + ":" + "0" + (int)(remaining_sec));
			}
		}
		else{
			print ("StartClock eh falso");
		}
		gameTime = remainingTime;
	}

	private void StartTimer(bool state) {
		startTimer = state;
	}

	private void StartTargets() {
		TargetUp();
	}

	public void StopTargets()
	{
		//desativar as placas no pause
		for (int i = 0; i < targetsAnimators.Length; i++) {
			targetsAnimators[i].enabled = false;
		}
	}
	public void Pause() {
		//Aim_controller.instance.gameObject.SetActive(false);
		StopTargets();
		if(GameManagerShare.instance.IsUsingKinect() == true)
		{
			if(DistanceCalibrator.instance != null)
			{
				if(DistanceCalibrator.instance.CheckOutOfScreen())
				{
					PauseWithoutBorg();
					print ("pause saiu do kinect");
				}
			}if(remainingTime <= 0)
			{
				BorgManager.instance.ShowBorgScreen();
			}
			else if(GameManagerShare.instance.stopGesture == true)
			{
				PauseWithoutBorg();
				print ("pause do gesto");
			}
		}else{
			BorgManager.instance.ShowBorgScreen();
		}
		GameManagerShare.instance.SetActiveBlur(true);
	}

	private void PauseWithoutBorg()
	{
		for (int i = 0; i < targetsAnimators.Length; i++) {
			targetsAnimators[i].enabled = false;
		}
		GameManagerShare.instance.stopGesture = false;
		GameManagerShare.instance.press_Stop = false;
		GameManagerShare.instance.PauseOn();
	}

	public void TargetHit(GameObject goHit) {
		if(goHit.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("TargetUp"))
		{
			startTimer = false;
			time = initialTime;
			goHit.GetComponent<Animator>().SetTrigger("Hit");

		}
	}

	private void RaffleTarget() {
		int random = 1;
		do {
			random = UnityEngine.Random.Range (0, targetsAnimators.Length);
		} while(random == (int)currentTarget);

		currentTarget = (Target)random;
	}

	public void TargetUp() {
		startTimer = true;
		targetsCount++;
		RaffleTarget();
		ThrowSoundManager.Instance.PlayRisePlaqueSfx();
		targetsAnimators[(int)currentTarget].SetTrigger("Up");
	}

	public void TargetDown() {

		StartTimer(true);
	}

	private void Down() {
		ThrowSoundManager.Instance.PlayRisePlaqueSfx();
		targetsAnimators[(int)currentTarget].SetTrigger("Down");
	}

	private void IncreaseCurrentMatch()
	{
		var currentMatch = PlayerPrefsManager.GetCurrentMatch();
		PlayerPrefsManager.SetCurrentMatch(currentMatch + 1);
	}

	public void ExecuteGesture(KinectGestures.Gestures gesture)
	{
		var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();
		
		if(movement != null)
			Bullseye.PlayerThrow.instance.ExecuteMovement(movement);
	}


	public void AddPlayerMovements(){
		//gesture_choice 0 ou 1
		if(gesture_choice == 0){
			KinectGestures.Gestures gTL = (KinectGestures.Gestures) PlayerPrefsManager.GetThrowLeft ();
			KinectGestures.Gestures gTR = (KinectGestures.Gestures) PlayerPrefsManager.GetThrowRight ();
			KinectGestures.Gestures gTRdif = (KinectGestures.Gestures) PlayerPrefsManager.GetThrowRight_Diferentao();
			KinectGestures.Gestures gTLdif = (KinectGestures.Gestures) PlayerPrefsManager.GetThrowLeft_Diferentao();
			playerMovements.Add(gTL, Movement.Throw);
			playerMovements.Add(gTR, Movement.Throw);
			playerMovements.Add(gTRdif, Movement.Throw);
			playerMovements.Add(gTLdif, Movement.Throw);
		}else if(gesture_choice == 1)
		{
			KinectGestures.Gestures gRaiseR = (KinectGestures.Gestures) PlayerPrefsManager.GetThrow_RaiseRight();
			KinectGestures.Gestures gRaiseL = (KinectGestures.Gestures) PlayerPrefsManager.GetThrow_RaiseLeft();
			playerMovements.Add(gRaiseR, Movement.RaiseHand);
			playerMovements.Add(gRaiseL, Movement.RaiseHand);
		}

		KinectGestures.Gestures gStopR = (KinectGestures.Gestures) PlayerPrefsManager.GetStopRightHand ();
		KinectGestures.Gestures gStopL = (KinectGestures.Gestures) PlayerPrefsManager.GetStopLeftHand ();
		playerMovements.Add(gStopR, Movement.StopHand);
		playerMovements.Add(gStopL, Movement.StopHand);
	}
	//descomentasdo Valdenilsdon 
	/*
	private void SendRoundTime(){
		string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedRound" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Hour +
			"-" + System.DateTime.Today.Minute + ".txt";
		
		string roundInfo;
		string id_player = PlayerPrefsManager.GetPlayerID().ToString();
		int id_game = (int) GameManagerShare.instance.GetCurrentGame();
		string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();	 
		
		roundInfo = "@" + gameTime.ToString() + "@" + id_player + "@" + id_game + "@" + current_match;
		System.IO.File.WriteAllText(folder, roundInfo);
		
		new HttpController().InsertRoundTime(roundInfo);
	}*/
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Share.Kinect;
using Assets.Scripts.Share.Enums;
using Share.Controllers;
using Share.Managers;
using Assets.Scripts.Share.Managers;
using Assets.Scripts.Share.Controllers;

public class SupManager : MonoBehaviour {
	public static SupManager instance;
	public Dictionary<KinectGestures.Gestures, Movement> playerMovements;
	public List<ScoreItem> scoreItems;
	[SerializeField]
	private float remainingTime;
	private float timeToReturn;

	//escolher gesto da remada
	[SerializeField]
	private bool RowWith_Default;
	private int using_hands;

	public bool startclock = true;
	public bool backToIslandTime;
	public bool freezeClock;
	private bool warnningPlayed = false;// showBorgScreen = false;

	private float elapsedTime, time;
	private float remaining_min, remaining_sec;
	//private float bonusTime;
	public float TotalAppleScore, TotalTimeScore;
	public GameObject arrow;
	public GameObject victory;
	public GameObject warnningTime;
	private Animator warnningTimeAnim;
	public Animator victoryAnim;
	public bool victoryPlayed = false;
	public Text clock_txt;
	public Sprite gameover_board;
	//referencias do spot do respawn
	private int respawn;
	private GameObject sup_boy, cameraSup;

	// Use this for initialization
	void Awake () {
		instance = this;
		remainingTime = PlayerPrefsManager.GetRemaingTime();
		playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();

		//define o peso de cada score Item
		scoreItems = new List<ScoreItem>();
		scoreItems.Add(new ScoreItem { multiplyingFactor = 1, type = ScoreItemsType.Apple });
		scoreItems.Add(new ScoreItem { multiplyingFactor = 1, type = ScoreItemsType.TimerSup });

		warnningTime.transform.GetComponent<Text>() ;
		warnningTime.SetActive(false);
		victory.transform.GetComponent<Text>();
		victory.SetActive(false);
		//referencias dos spots do respawn
		respawn = UnityEngine.Random.Range(0,6);
		//respawn = 5;
		sup_boy = GameObject.Find("batata_sup").gameObject;
	}

	void Start(){
		if(SoundManager.Instance != null)
			SoundManager.Instance.StopStartscreenSound();
		using_hands = PlayerPrefsManager.GetIdPerfilSup();
		if(using_hands == 0){
			RowWith_Default = true;
		}else if(using_hands == 1){
			RowWith_Default = false;
		}
		AddPlayerMovements (); 

		cameraSup = CameraZoomControl.instance.GetCamSup();
		arrow.gameObject.SetActive(false);
		warnningTimeAnim = warnningTime.GetComponent<Animator>();
		GameManagerShare.instance.SetActiveBlur(false);
		timeToReturn = (remainingTime/2);
		SupSoundManager.Instance.PlaySup_Environment();
		SupSoundManager.Instance.PlaySupWatter_Environment();

		//setando batata e camera do respawn escolhido
		RespawnsGenerator();
	}

	public void StartCount(){
		startclock = true;
		backToIslandTime = false;
		freezeClock = false;

		Mathf.Ceil(timeToReturn);
		TotalAppleScore = 0;
		TotalTimeScore = 0;
		//camera inicial
		CameraZoomControl.instance.PlaySupStartCam_1();
	
		CountDownManager.instance.Initialize();
		arrow.gameObject.SetActive(false);
		victoryAnim = victory.GetComponent<Animator>();
	}

	public void StartGame(){
		time = PlayerPrefsManager.GetBorgTime ();
		//CountDownManager.instance.Initialize();
	}

	private void StartClock()
	{
		if(startclock = true && victoryPlayed == false && freezeClock == false)
		{
			if(remainingTime >= 0)
			{
				remaining_min = (int)(remainingTime/60);
				remaining_sec = remainingTime % 60;
				remainingTime -= Time.deltaTime; //PlayerPrefsManager.GetRemainingTime();
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.TimerSup, remaining_min, remaining_sec);
				if(remainingTime <= timeToReturn){
					backToIslandTime = true;
					arrow.gameObject.SetActive(true);
				}
			}
		}
		else{
			print ("StartClock eh falso");
		}
	}

	//chama o pause do sup que decide se eh com borg ou sem borg, 
	//vai entrar no pause com borg e o update decide se eh gameover ou continuar/parar
	private void StopClock()
	{
		GameManagerShare.instance.PauseGame();
		startclock = false;
	}

	//prorpiedades de retorno para ilha iniciam aqui
	private void ReturnClock()
	{
		if(victoryPlayed == false && freezeClock == false)
		{
			remaining_sec = remainingTime % 60;
			if(remaining_min >= 0){
				remaining_min = (int)(remainingTime/60);
				remainingTime -= Time.deltaTime; //PlayerPrefsManager.GetRemainingTime();
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.TimerSup, remaining_min,remaining_sec);
			}
			else{
				remainingTime -= Time.deltaTime;
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.TimerSup, 0,remaining_sec);
			}
			clock_txt = clock_txt.GetComponent<Text>();
			clock_txt.color = Color.red;
		}
		if(warnningPlayed == false)
		{
			warnningTime.SetActive(true);
			StartCoroutine("PlayWarnningAnim"); 
		}
	}

	// Update is called once per frame
	void Update () {
		if(victoryPlayed == true)
		{
			victory.SetActive(false);
		}
		if(warnningPlayed == true)
		{
			warnningTime.SetActive(false);
		}
		if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && backToIslandTime == false && !TutorialScreen.instance.isTutorialOn && !Banner_Storytelling.instance.isBannerOn
		   && !Grandpa_Screen.instance.isGrandpaScreenOn) //&& remainingTime <= maxTime) 
		{
			StartClock();
		}
		if(backToIslandTime == true && !GameManagerShare.IsPaused() && startclock == true && freezeClock == false)
		{
			ReturnClock();
		}
		if (GameManagerShare.IsStarted() && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			//pause se sair do kinect
			if(GameManagerShare.instance.IsUsingKinect() && DistanceCalibrator.instance.CheckOutOfScreen())
			{
				GameManagerShare.instance.PauseGame();// PauseWithoutBorg();
			}
			if(remainingTime <= 0 && backToIslandTime == true)
			{
				StopClock();
			}
			/*if(elapsedTime >= time){
				elapsedTime = 0;
				GameManagerShare.instance.PauseGame();
			}
			else{
				elapsedTime += Time.deltaTime;
			}*/
		}
		if(GameManagerShare.IsStarted() && GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			if(victoryPlayed == true)
			{
				GameManagerShare.instance.GameOver();
			}
			if(BorgManager.instance.borgItemSelected)
			{
				if(startclock == false)
					GameManagerShare.instance.GameOver();
			}
		}
		if(Sup_Controller.instance.GetTriggerBridge())
			CheckPoint();
	}

	//variavel criada para nao entrar varias vezes no metodo
	//TODO(Elaynne): fazer verificaçao para que nao seja necessaria essa variavel. com isPaused e isGameOver nao esta funcionando
	//bool ended = false;
	public void CheckPoint()
	{
		if(backToIslandTime == true && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && freezeClock == false)
		{
			//futuramente essa variavel sera trocada sempre q entrar no checkpoint pq o boy fica indo e vindo na ilha
			//backToIslandTime = false;
			//resetAppleScore apenas soma a quantidade de maças em TotalAppliScore
			ResetAppleScore(ScoreItemsType.Apple);
			FreezeClockOnIsland(ScoreItemsType.TimerSup);
			arrow.SetActive(false);
			warnningTime.gameObject.SetActive(false);
			victory.gameObject.SetActive(true);
			StartCoroutine("PlayEndingVictory");
		}
	}
	public Sprite GetGameOverBoard()
	{
		return gameover_board;
	}

	IEnumerator PlayWarnningAnim()
	{
		yield return new WaitForSeconds(3f);
		warnningTimeAnim.SetTrigger("warnningPlay");
		warnningPlayed = true;
	}

	//seta a posiçao do boy e camera, algo esta fazendo uma diferença 312.2, 0, -458.62
	private void RespawnsGenerator()
	{
		if(respawn == 0){
			sup_boy.transform.position = new Vector3(1896.7f,0.38f,932.78f);//meta 1584.5,0,1391.4
			sup_boy.transform.rotation = Quaternion.Euler(0f,21.75f,0f);	
			cameraSup.transform.rotation = Quaternion.Euler(0f,18.47f,0f);
		}else if(respawn == 1){
			sup_boy.transform.position = new Vector3(2216.6f,0.38f,368.5f);//meta 1854.4,0,782.5
			sup_boy.transform.rotation = Quaternion.Euler(0f,-9f,0f);	
			cameraSup.transform.rotation = Quaternion.Euler(0f,334.75f,0f);
		}else if(respawn == 2){
			sup_boy.transform.position = new Vector3(620.1f,0.38f,1912.78f);//meta 307.9, 0.38, 2371,4
			sup_boy.transform.rotation = Quaternion.Euler(0f,430.45f,0f);		
			cameraSup.transform.rotation = Quaternion.Euler(0f,65.91f,0f);
		}else if(respawn == 3){
			sup_boy.transform.position = new Vector3(260.8f,0.38f,2350.58f);//meta (-51, 0, 2809.2)
			sup_boy.transform.rotation = Quaternion.Euler(0f,120.76f,0f);	
			cameraSup.transform.rotation = Quaternion.Euler(0f,121.78f,0f);
		}else if(respawn == 4){
			sup_boy.transform.position = new Vector3(1551.2f,0.38f,2230.88f);//meta 1239.0, 0, 2689.5
			sup_boy.transform.rotation = Quaternion.Euler(0f,120.76f,0f);
			cameraSup.transform.rotation = Quaternion.Euler(0f,124.94f,0f);
		}

		else if(respawn == 5){
			sup_boy.transform.position = new Vector3(1815.1f,0.38f,2600.78f);//meta 1497.9,0,3025.4
			sup_boy.transform.rotation = Quaternion.Euler(0f,261.04f,0f);
			cameraSup.transform.rotation = Quaternion.Euler(14.46f,256.5f,359f);
		}
	}

	IEnumerator PlayEndingVictory()
	{
		if(warnningPlayed = true)
		{
			yield return new WaitForSeconds(3f);
		}
		else
		{
			yield return new WaitForSeconds(6f);
		}
		victoryAnim.SetTrigger("warnningPlay");
		victoryPlayed = true;
		GameManagerShare.instance.PauseGame();
		//depois daqui reseta o tempo

	}

	public float GetRemainingTime(){
		return remainingTime;
	}
	
	public void AddTime(int bonus_time){
		remainingTime += bonus_time;
	}

	public bool Get_ReadyToBackToIsland(){
		return backToIslandTime;
	}
	public GameObject GetSupBoy(){
		return sup_boy;
	}


	public void ExecuteGesture(KinectGestures.Gestures gesture){
		var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();
		
		if (movement != null)
			Sup_Controller.instance.ExecuteMovement (movement);
	}

	public void ResetAppleScore(ScoreItemsType scoreItem)
	{
		var apple = ScoreManager.instance.scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
		TotalAppleScore += apple.GetValue();
		//print ("Total apple score: " + SupManager.instance.TotalAppleScore);
		//apple.SetValue(0);
	}
	
	public void FreezeClockOnIsland(ScoreItemsType scoreItem)
	{
		var clock = ScoreManager.instance.scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
		//remainingTime = GetRemainingTime();
		TotalTimeScore += clock.GetValue();
		freezeClock = true;

	}

	public void AddPlayerMovements()
	{
		if(RowWith_Default){
			print("RowWith_Default");
			KinectGestures.Gestures gRemarL = (KinectGestures.Gestures) PlayerPrefsManager.GetRemarLeft ();
			KinectGestures.Gestures gRemarR = (KinectGestures.Gestures) PlayerPrefsManager.GetRemarRight ();
			playerMovements.Add(gRemarL, Movement.RemarLeft);
			playerMovements.Add(gRemarR, Movement.RemarRight);
		}/*else if(RowWith_ArmsElevation){
			print("RowWith_ArmsElevation");
			KinectGestures.Gestures gElevationR = (KinectGestures.Gestures) PlayerPrefsManager.GetRightArmElevation ();
			KinectGestures.Gestures gElevationL = (KinectGestures.Gestures) PlayerPrefsManager.GetLeftArmElevation ();
			playerMovements.Add(gElevationR, Movement.RemarRight);
			playerMovements.Add(gElevationL, Movement.RemarLeft);
		}*/
		//KinectGestures.Gestures gStopR = (KinectGestures.Gestures) PlayerPrefsManager.GetStopRightHand ();
		//KinectGestures.Gestures gStopL = (KinectGestures.Gestures) PlayerPrefsManager.GetStopLeftHand ();
		else{
			KinectGestures.Gestures gArmsUp = (KinectGestures.Gestures) PlayerPrefsManager.GetTwoArmsUp ();
			playerMovements.Add(gArmsUp, Movement.ArmsUp);
		}
		//playerMovements.Add(gStopR, Movement.StopHand);
		//playerMovements.Add(gStopL, Movement.StopHand);
		

	}

	//eu ja nao sei mais o que isso quer dizer
	public void Pause()
	{
		if(GameManagerShare.instance.IsUsingKinect() == true)
		{
			if(GameManagerShare.instance.stopGesture == true){
				PauseWithoutBorg();
				print ("pause do gesto");
			}
			else if((DistanceCalibrator.instance != null) && (DistanceCalibrator.instance.CheckOutOfScreen())){
				PauseWithoutBorg();
				print ("pause saiu do kinect");
			}
			if(victoryPlayed == true){
				print ("apenas para entrar na chamada do pause pelo GameManager");
			}
			if(remainingTime <= 0){
				BorgManager.instance.ShowBorgScreen();
				arrow.gameObject.SetActive(false);
			}
		}
		else{
			if(victoryPlayed == true){
				//	print ("IsPaused: " + GameManagerShare.IsPaused());
				print ("apenas para entrar na chamada do pause pelo GameManager");
			}
			else{
				BorgManager.instance.ShowBorgScreen();
				arrow.gameObject.SetActive(false);
			}
		}
		GameManagerShare.instance.SetActiveBlur(true);
	}
	public void UnPause()
	{
		GameManagerShare.instance.SetActiveBlur(false);
	}
	//pause quando sair do kinect,
	public void PauseWithoutBorg()
	{
		GameManagerShare.instance.stopGesture = false;
		GameManagerShare.instance.press_Stop = false;
		GameManagerShare.instance.PauseOn();
	}

	//parar musica, tocar audio de transiçao e chamar tela de game over
	public void GameOver()
	{
		BorgManager.instance.borgItemSelected = false;
		BorgManager.instance.SendBorg();
		if (SoundManager.Instance != null) 
		{
			SoundManager.Instance.StopBGmusic();
			SoundManager.Instance.PlayGameTransition();
		}
		if(SupSoundManager.Instance != null) 
		{
			SupSoundManager.Instance.StopSup_Environment();
			SupSoundManager.Instance.StopSupWatter_Environment();
			GameOverScreenController.instance.Enable();
		}
	}

}

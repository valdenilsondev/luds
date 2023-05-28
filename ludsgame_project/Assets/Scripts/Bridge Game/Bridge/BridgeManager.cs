using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Assets.Scripts.Share.Controllers;
using BridgeGame.Player;
using BridgeGame.UI;
using Share.Managers;
using BridgeGame.KinectControl;
using System.Collections.Generic;
using Share.Controllers;
using Assets.Scripts.Share.Enums;
using BridgeGame.GameUtilities;
using Share.KinectUtils;
using Assets.Scripts.Share.Kinect;
using System.Linq;

public class BridgeManager : MonoBehaviour {
	
	public static BridgeManager instance;
	public Dictionary<KinectGestures.Gestures, Movement> playerMovements;
	
	private bool isready = false;
	private bool playerOnTheWater = false;
	
	//camera
	private bool overviewCameraStarted = false;
	private bool hasAnimationBeenPlayed = false;	
	private bool positionAndRotationFixed = false;
	private Vector3 playerCameraInitialPosition;
	private Quaternion playerCameraInitialRotation;
	private GameObject playerCamera;
	public List<ScoreItem> scoreItems;
	public Text userFeedback_bridge;

	//gui
	private GameObject balanceBar;
	//placa que aparece na posiçao central da barra
	public GameObject playerBar;
	//placa que aparece quando o player esta fora da posiçao aconselhada
	public GameObject playerBarCentral;

	public Sprite gameover_board;
	//private GameObject boxRaiseHandToStart;
	//dificuldade 1 facil 2 medio 3 dificil
	public int difficult;

	//trigger
	private bool checkStart = false;
	private bool checkGameOver = false;
	private bool showFeedback = false;
	private bool gameover_status = false;

	private GameObject[] piranhas;

	//dados para relatorio
	private int qtdFalls;
	private int qtdPiranhaHits;
	private float tempoInclinado;
	private float tempoIncRight, tempoIncLeft;
	private int vitoria, derrota;

	void Awake(){
		instance = this;
		//define o peso de cada score Item
		scoreItems = new List<ScoreItem>();
		scoreItems.Add(new ScoreItem { multiplyingFactor = 1, type = ScoreItemsType.Bridge_Distance });
		scoreItems.Add(new ScoreItem { multiplyingFactor = 1, type = ScoreItemsType.X });

		//camera
		playerCamera = GameObject.Find("PlayerCamera").gameObject;
		playerCameraInitialPosition = playerCamera.transform.position;
		playerCameraInitialRotation = playerCamera.transform.rotation;

		//gui
		balanceBar = GameObject.Find("BalanceBar").gameObject;
		//playerBar = GameObject.Find("BalanceBar").transform.Find("player_bar").gameObject;
		playerBarCentral = 	GameObject.Find("BalanceBar").transform.Find("player_bar_central").gameObject;
		//boxRaiseHandToStart = GameObject.Find("boxRaiseHandToStart").gameObject;
		//Carregar parametrização feita pela fisioterapeuta. Ex: Levantar a mão faz com que o Pig execute a ação de pular
		playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();
		AddPlayerMovements();
	}
	
	void Start(){
		if(SoundManager.Instance != null)
			SoundManager.Instance.StopStartscreenSound();
		BridgeSoundManager.Instance.PlayRiverRunning();
		GameManagerShare.instance.SetActiveBlur(false);
		piranhas = GameObject.FindGameObjectsWithTag("piranha");

		if(difficult == 1)
		{
			foreach(GameObject p in piranhas)
			{
				p.SetActive(false);
			}
		}
		else if(difficult == 2)
		{
			piranhas[0].SetActive(false);
			piranhas[1].SetActive(false);
			piranhas[3].SetActive(false);
			piranhas[5].SetActive(false);
		}
		else if(difficult == 3)
		{
			//ajustar inclinaçao
			//checar dificuldade na hora que ele cai da ponte no PlayerControll
			print ("Modo hard, se cair vai do começo");
		}
		if(!overviewCameraStarted){
			overviewCameraStarted = true;
			//start camera anim
		//	CameraIntroControl.instance.StartInitialCameraAnimation();					
			//esconder guis
		//	GUIController.instance.Disable();
		}
	}
	
	void Update(){
		if(PlayerControl.instance.GetPigWalking()){
			BridgeSoundManager.Instance.steps_sounds.SetActive(true);
		}
		else{
			BridgeSoundManager.Instance.steps_sounds.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.T)){
			GameManagerShare.instance.GameOver();
		}
		//press space to start game
        /*if (BridgeManager.instance.IsCameraOverviewAnimComplete() && CountDownManager.instance.IsCounting() == false)
        {
			//CountDownManager.instance.Initialize();
			HideGUIStartToPlay();
		}*/	

		//ao terminar animacao da camera
		if(hasAnimationBeenPlayed && !GameManagerShare.IsStarted())
		{
			hasAnimationBeenPlayed = false;
			//fazer tween ao terminar animacao para a camera se ajustar a visao do player
			if(positionAndRotationFixed == false){
				positionAndRotationFixed = true;
				iTween.MoveTo(playerCamera.transform.GetChild(0).gameObject, iTween.Hash("position", PlayerControl.instance.GetPlayerCameraInitialPosition(), "time", 0.5f, "easytype", iTween.EaseType.linear));
				iTween.RotateTo(playerCamera.transform.GetChild(0).gameObject, GetPlayerCameraInitialRotation().eulerAngles, 0.5f);
				//ShowGUIStartToPlay();
			}
		}
		//verificaçao de fim de ponte
		if(FinalPosition.instance.isFinalPos && showFeedback == false)
		{
			userFeedback_bridge.text = "CONSEGUIU!";
			Invoke("HideFeedback", 1);
			showFeedback = true;
			SetVictory(1);
			SetDefeat(0);
		}
		if(LifesManager.instance.IsDead() == true && gameover_status == false)
		{
			gameover_status = true;
			SetVictory(0);
			SetDefeat(1);
		}
		//verificaçao de condiçoes que venham a chamar o pause ou game over
		if(GameManagerShare.instance.IsUsingKinect() && GameManagerShare.IsStarted())
		{
			if((LifesManager.instance.IsDead() || FinalPosition.instance.isFinalPos || 
			   DistanceCalibrator.instance.CheckOutOfScreen()) && GameManagerShare.IsStarted())
			{
				if(checkGameOver == false)
				{
					checkGameOver = true;
					GameManagerShare.instance.PauseGame();
				}
				else if(BorgManager.instance.borgItemSelected && !GameManagerShare.IsGameOver())
				{
					//BorgManager.instance.OnBorgSelected();
					GameManagerShare.instance.GameOver();
				}
			}
			//verificacao do tempo inclinado
			if(GenericKinectMethods.instance.IsInclinated()){
			 	//tempoInclinado += Time.deltaTime;
				if(GenericKinectMethods.instance.IsIncRight()){
					tempoIncRight += Time.deltaTime; 
				}else if(GenericKinectMethods.instance.IsIncLeft())
				{
					tempoIncLeft += Time.deltaTime;
				}
				tempoInclinado = tempoIncLeft + tempoIncRight;
			}
		}else{
			if((LifesManager.instance.IsDead() || FinalPosition.instance.isFinalPos) && GameManagerShare.IsStarted())
			{
				if(checkGameOver == false)
				{
					checkGameOver = true;
					GameManagerShare.instance.PauseGame();
				}
				else if(BorgManager.instance.borgItemSelected && !GameManagerShare.IsGameOver())
				{
					//BorgManager.instance.OnBorgSelected();
					GameManagerShare.instance.GameOver();
				}
			}
		}

		//verificaçao de condiçoes que venham a dar inicio ao jogo
		if(GameManagerShare.IsStarted() == true && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() 
		   && playerOnTheWater == false && !CountDownManager.instance.IsCounting())
		{	
			PlayerControl.instance.Walking();

			if(PlayerControl.instance.CheckIfPlayerFelt())
			{
				IncrementQtdFalls();
				PlayerControl.instance.PlayPigFallingAnimation();
				BridgeSoundManager.Instance.PlayPigFallSplash();
				userFeedback_bridge.text = "CAIU!";
				Invoke("HideFeedback", 1);
			}

			PlayerControl.instance.MovePlayerBar();

			if(PlayerControlKinect.instance.IsBoostOn())
			{
				PlayerControl.instance.GetPigAnimtorController().speed = 2;
				BridgeSoundManager.Instance.PlaySpeedSteps();
			}
			else
			{
				PlayerControl.instance.GetPigAnimtorController().speed = 1;
				BridgeSoundManager.Instance.PlaySteps();
			}
		}		
	}

	//main =============================================================
	public void StartGame()
	{
		playerOnTheWater = false;
		PlayerControl.instance.SetPigBridgeWalking();
		GUIController.instance.Enable();
	}
	
	public bool IsReady()
	{
		if(BridgeManager.instance.IsCameraOverviewAnimComplete() && !CountDownManager.instance.IsCounting() 
		   && !GameManagerShare.IsStarted())
			isready = true;
		else{
			isready = false;
		}
		return isready;
	}
	
	public void SetIsReady(bool var)
	{
		isready = var;
	}
	private void HideFeedback()
	{
		userFeedback_bridge.text = string.Empty;
	}
	//vitoria e derrotas
	public void SetVictory(int value)
	{
		vitoria = value;
	}
	public int GetVictory()
	{
		return vitoria;
	}
	public void SetDefeat(int value)
	{
		derrota = value;
	}
	public int GetDefeat()
	{
		return derrota;
	}
	///

	public void AddPlayerMovements(){
		KinectGestures.Gestures gStopR = (KinectGestures.Gestures) PlayerPrefsManager.GetStopRightHand ();
		KinectGestures.Gestures gStopL = (KinectGestures.Gestures) PlayerPrefsManager.GetStopLeftHand ();

		playerMovements.Add(gStopR, Movement.StopHand);
		playerMovements.Add(gStopL, Movement.StopHand);
	}

	public void ExecuteGesture(KinectGestures.Gestures gesture){
		var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();
		if (movement != null)
			PlayerControlKinect.instance.ExecuteMovement (movement);
		print ("movement: " + movement);
	}

	//cuidado gambiarra ahead
	public Sprite GetGameOverBoard()
	{
		return gameover_board;
	}
	
	public void GameOver()
	{ 
		BorgManager.instance.borgItemSelected = false;
		BorgManager.instance.SendBorg();
		if(SoundManager.Instance != null){
			SoundManager.Instance.StopBGmusic();
			if(BridgeSoundManager.Instance != null){
				BridgeSoundManager.Instance.StopRiverRunning();
			}
			SoundManager.Instance.PlayGameTransition();
		}
		GameOverScreenController.instance.Enable();
		//tempoInclinado = 0;
	}

	public GameObject piranhaAnim;

    //pause normal com borg no fim da ponte ou das vidas
	public void StopPiranhas()
	{
		foreach(GameObject p in piranhas)
		{
			p.GetComponent<Animator>().speed = 0;
		}
	}
	public void Pause()
	{        
		StopPiranhas();
		BridgeSoundManager.Instance.StopRiverRunning();
		BridgeSoundManager.Instance.StopSteps();
		BridgeSoundManager.Instance.StopBridgeNoise();

		if(GameManagerShare.instance.IsUsingKinect() == true)
		{
			if(GameManagerShare.instance.stopGesture == true)
			{//fazer variavel de controle no gamemanagerShare
				PauseWithoutBorg();
				print ("pause do gesto no Bridge");
			} 
			else if((DistanceCalibrator.instance != null) && (DistanceCalibrator.instance.CheckOutOfScreen()))
			{
				PauseWithoutBorg();
				print ("pause saiu do kinect");
			}
			else if(checkGameOver == true)
				BorgManager.instance.ShowBorgScreen();
		}
		else{
			BorgManager.instance.ShowBorgScreen();
		}
		GameManagerShare.instance.SetActiveBlur(true);
	}

    //pause quando sair da frente do kinect
    private void PauseWithoutBorg()
    {
		GameManagerShare.instance.stopGesture = false;
		GameManagerShare.instance.press_Stop = false;
		GameManagerShare.instance.PauseOn();
		PlayerControl.instance.SetPigBridgeIddle(); 
    }

    public void UnPause()
    {
		foreach(GameObject p in piranhas){
			p.GetComponent<Animator>().speed = 1;
		}

        PlayerControl.instance.SetPigBridgeWalking();
		checkGameOver = false;
		GameManagerShare.instance.SetActiveBlur(false);
    }

	public void Reset(){
		tempoInclinado = 0;
		playerOnTheWater = true;
	}

	public void SetPlayerOnTheWater(bool var){
		playerOnTheWater = var;
	}

	public bool IsPlayerOnTheWater(){
		return playerOnTheWater;
	}
	//main =============================================================


	//camera ===========================================================
	private void BeginInitialCameraAnimation()
	{
		CameraIntroControl.instance.StartInitialCameraAnimation();
	}
	
	public void OverviewCameraCompleted()
	{
		overviewCameraStarted = false;
		hasAnimationBeenPlayed = true;
	}
	
	public bool IsCameraOverviewAnimComplete()
	{
		return positionAndRotationFixed;
	}
	
	public GameObject GetPlayerCamera()
	{
		return playerCamera;
	}
	
	public Quaternion GetPlayerCameraInitialRotation()
	{
		return new Quaternion(playerCameraInitialRotation.x,180,playerCameraInitialRotation.z-45,1);
	}
	//camera ===========================================================

	
	//GUI ==============================================================

	public void ResetSphereBar()
	{
		//sphere.transform.position = new Vector3(0, -4, sphere.transform.position.z);
	}

	public GameObject GetBalanceBarRedPart()
	{
		return balanceBar.transform.Find("bar").gameObject;
	}
	//GUI ==============================================================


	//========== gest e sets dos dados gerados pro relatorio

	/// <summary>
	/// Increments the qtd falls.
	/// </summary>
	/// <returns>The qtd falls.</returns>
	private void IncrementQtdFalls(){
		qtdFalls ++;
	}

	/// <summary>
	/// Gets the qtd falls.
	/// </summary>
	/// <returns>The qtd falls.</returns>
	public int GetQtdFalls(){
		return qtdFalls;
	}

	/// <summary>
	/// Increments the qtd piranha hits.
	/// </summary>
	public void IncrementQtdPiranhaHits(){
		qtdPiranhaHits ++;
	}

	/// <summary>
	/// Gets the qtd piranhas hits.
	/// </summary>
	/// <returns>The qtd piranhas hits.</returns>
	public int GetQtdPiranhasHits(){
		return qtdPiranhaHits;
	}
	
	/// <summary>
	/// Gets the inclination time.
	/// </summary>
	/// <returns>The inclination time.</returns>
	public int GetInclinationTime(){
		return (int)tempoInclinado; 
	}
	public int GetTempoIncRight()
	{
		return (int)tempoIncLeft;
	}
	public int GetTempoIncLeft()
	{
		return (int) tempoIncRight;
	}
	//==========

}


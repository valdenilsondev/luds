using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share.Enums;
using Assets.Scripts.Share.Kinect;
using Share.Managers;
using Share.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Share.EventsSystem;
using Assets.Scripts.Share.Managers;
using UnityEngine.UI;


public class FishingManager : MonoBehaviour {

	public Dictionary<KinectGestures.Gestures, Movement> playerMovements;
	public List<ScoreItem> scoreItems;
	public static FishingManager instance;
	//score
	public float qt_fishYellow, qt_fishBlue, qt_fishBaiacu;
	public Text fishYellow, fishBaiacu, fishBlue;

	private List<Fish> fish;

	private float rd;

	//setup do movimento
	private int pull_gesture_setup;
	//quantidade de peixes por dificuldade
	private int fishBySpot;

	public GameObject[] fishSpots;

	public bool fishAnimation_gone;
	//[SerializeField]
	public Sprite gameover_board;
	//private int randspot;
	private bool gui_active = false;

	private bool gameOverCalled = false;
	// Use this for initialization
	void Awake () {
		instance = this;
		scoreItems = new List<ScoreItem>();
		scoreItems.Add(new ScoreItem { multiplyingFactor = 1, type = ScoreItemsType.Bait });
		scoreItems.Add(new ScoreItem { multiplyingFactor = 3, type = ScoreItemsType.Fish1 });
		scoreItems.Add(new ScoreItem { multiplyingFactor = 3, type = ScoreItemsType.Fish2 });
		scoreItems.Add(new ScoreItem { multiplyingFactor = 3, type = ScoreItemsType.Fish3 });

		//Carregar parametrização feita pela fisioterapeuta. Ex: Levantar a mão faz com que o Pig execute a ação de pular
		playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();
		pull_gesture_setup = PlayerPrefsManager.GetFishMovementSetup();
		AddPlayerMovements();

	}
	void Start()
	{
		if(SoundManager.Instance != null)
			SoundManager.Instance.StopStartscreenSound();
		qt_fishBlue = 0;
		qt_fishYellow = 0;
		qt_fishBaiacu = 0;
		fish = new List<Fish>();
		GameManagerShare.instance.SetActiveBlur(false);
		gameOverCalled = false;
	}

	public void StartCount(){
		CameraMovementControlFish.instance.PlayFishStartCam_1();
		//CameraMovementControlFish.instance.PlayFishStartCam_1();
		CountDownManager.instance.Initialize();
		fishAnimation_gone = false;
		FishingSoundManager.Instance.PlayWatter_Environment();
		FishingSoundManager.Instance.PlayFishing_Environment();
	}

	bool started;
	public void StartGame()
	{
		if(!started){
			started = true;
			//indexRdSpot = PlayerPrefsManager.GetFishingDifficult(); feito no batata_fishing_control	
			//iniciar em um dos 6 spots
			//BoatControl.instance.MoveItToFishingSpot(fishSpots[indexRdSpot]);
			fishBySpot = GetNumberofFishBySpot();
		}
	}



	public int GetNumberofFishBySpot()
	{
		return GetFishList().Count;
	}

	public void DecreaseFishOnCountList()
	{
		if(fishBySpot >= 0)
		{
			fishBySpot--;
		}
	}
	public void IncreaseScoreFishType(int fish_type)
	{
		GotFishScreenControl.instance.ShowGotFishScreen(fish_type);

		switch(fish_type)
		{
			case 1:
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Fish1, 1,0);
				FishMovementControl.instance.BlueFishJump();
				break;
			case 2:
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Fish2,1,0);
				FishMovementControl.instance.YellowFishJump();
				break;
			case 3:
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Fish3, 1,0);
				FishMovementControl.instance.BrownFishJump();
				break;
			default:
				break;
		}
		FishingSoundManager.Instance.PlayDelaied();
		if(SoundManager.Instance != null)
			SoundManager.Instance.PlayBGDelayed(3.2f);
	}

	private void WaitForFishLeave()
	{
		GameManagerShare.instance.PauseGame();
	}

	// Update is called once per frame
	void Update () 
	{
		/*if(Input.GetKeyDown(KeyCode.S) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() 
		   && !GameManagerShare.IsGameOver())
		{
			GameManagerShare.instance.press_Stop = true;
			GameManagerShare.instance.PauseGame();
		}*/
		if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && gui_active == false)
		{
			UIHandler.instance.ActivateAll();
			gui_active = true;
		}
		if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && GameManagerShare.IsStarted() &&
		   (BucketBaitsControl.instance.GetNumberOfBaits() <= 0 || fishBySpot == 0)&& fishAnimation_gone == false) 
		{
				//chamar pause depois que a animaçao do ultimo peixe terminar
				BatataFishAnimatorController.instance.PlayBatataWin();
				Invoke("WaitForFishLeave", 3);
				fishAnimation_gone = true;
		}
		if(GameManagerShare.IsStarted() && !GameManagerShare.IsPaused() && GameManagerShare.IsGameOver())
		{
			if(BorgManager.instance.borgItemSelected && !gameOverCalled)
			{
				gameOverCalled = true;
				GameManagerShare.instance.GameOver();
			}
		}
		if(GameManagerShare.IsStarted() && GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			if(BorgManager.instance.borgItemSelected && !gameOverCalled)
			{
				gameOverCalled = true;
				GameManagerShare.instance.GameOver();
			}
		}

		if(started && !jumpedA){
			elapsedTimeA += Time.deltaTime;
		}

		if(elapsedTimeA>3){
			jumpedA = true;
			Batata_Fishing_Control.instance.GetBarco().transform.GetChild(0).GetComponent<Animator>().SetTrigger("jump");
		}

		if(started && !jumpedB){
			elapsedTimeB += Time.deltaTime;
		}
		
		if(elapsedTimeB>6){
			jumpedB = true;
			Batata_Fishing_Control.instance.GetBarco().transform.GetChild(1).GetComponent<Animator>().SetTrigger("jump");
		}

		if(started && !jumpedC){
			elapsedTimeC += Time.deltaTime;
		}
		
		if(elapsedTimeC>9){
			jumpedC = true;
			Batata_Fishing_Control.instance.GetBarco().transform.GetChild(2).GetComponent<Animator>().SetTrigger("jump");
		}
	}

	float elapsedTimeA, elapsedTimeB, elapsedTimeC;
	bool jumpedA, jumpedB, jumpedC;
	public void AddPlayerMovements(){
		if(pull_gesture_setup == 0)
		{
			KinectGestures.Gestures gRowL = (KinectGestures.Gestures) PlayerPrefsManager.GetRowLeft();// GetPullLeft ();
			KinectGestures.Gestures gRowR = (KinectGestures.Gestures) PlayerPrefsManager.GetRowRight();//GetPullRight ();
			KinectGestures.Gestures gRowM = (KinectGestures.Gestures) PlayerPrefsManager.GetRowMid();//GetPullRight ();
			KinectGestures.Gestures gThrowL = (KinectGestures.Gestures) PlayerPrefsManager.GetThrowLeft ();
			KinectGestures.Gestures gThrowR = (KinectGestures.Gestures) PlayerPrefsManager.GetThrowRight ();

			playerMovements.Add(gRowL, Movement.Pull);
			playerMovements.Add(gRowR, Movement.Pull);
			playerMovements.Add(gRowM, Movement.Pull);
			playerMovements.Add(gThrowL, Movement.Throw);
			playerMovements.Add(gThrowR, Movement.Throw);
		}else if(pull_gesture_setup == 1){
			KinectGestures.Gestures gArmsUp = (KinectGestures.Gestures) PlayerPrefsManager.GetTwoArmsUp ();//testar elevaçao frontal para pescar
			playerMovements.Add(gArmsUp, Movement.ArmsUp);//testar elevaçao frontal
			//elevaçoes no MoveWithKinect.cs
		}
		//bigpull contra indicado na maior dos pacientes devido ao estimulo de 
		/*KinectGestures.Gestures gBigpullL = (KinectGestures.Gestures) PlayerPrefsManager.GetPullLeft ();
		KinectGestures.Gestures gBigpullR = (KinectGestures.Gestures) PlayerPrefsManager.GetPullRight ();
		playerMovements.Add(gBigpullL, Movement.BigPullL);
		playerMovements.Add(gBigpullR, Movement.BigPullR);*/

		KinectGestures.Gestures gStopR = (KinectGestures.Gestures) PlayerPrefsManager.GetStopRightHand ();//testar
		KinectGestures.Gestures gStopL = (KinectGestures.Gestures) PlayerPrefsManager.GetStopLeftHand ();//testar
		playerMovements.Add(gStopR, Movement.StopHand);
		playerMovements.Add(gStopL, Movement.StopHand);
	}

	public void ExecuteGesture(KinectGestures.Gestures gesture){
		var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();
		
		if (movement != null)
			Batata_Fishing_Control.instance.ExecuteMovement (movement);
		print ("movement: " + movement);
	}

	public Sprite GetGameOverBoard()
	{
		print ("trocou de gameover");
		return gameover_board;
	}

	public void Pause()
	{
		if(GameManagerShare.instance.IsUsingKinect() == true)
		{
			/*if(GameManagerShare.instance.stopGesture == true){
				PauseWithoutBorg();
				print ("pause do gesto");
			}*/
			if((DistanceCalibrator.instance != null) && (DistanceCalibrator.instance.CheckOutOfScreen()))
			{
				PauseWithoutBorg();
				print ("pause saiu do kinect");
			}
			else if(BucketBaitsControl.instance.GetNumberOfBaits() <= 0 || fishBySpot == 0)
			{
				BorgManager.instance.ShowBorgScreen();
			}
		}
		//pause automatico no fim de game
		else if(BucketBaitsControl.instance.GetNumberOfBaits() <= 0 || fishBySpot == 0)
		{
			BorgManager.instance.ShowBorgScreen();
		}
		GameManagerShare.instance.SetActiveBlur(true);
	}

	public void UnPause()
	{
		GameManagerShare.instance.SetActiveBlur(false);
	}

	public void GameOver()
	{

		if(BorgManager.instance.borgItemSelected)
		{
			BorgManager.instance.borgItemSelected = false;
			BorgManager.instance.SendBorg();
			if (SoundManager.Instance != null) 
			{
				SoundManager.Instance.StopBGmusic();
				SoundManager.Instance.PlayGameTransition();
			}
			GameOverScreenController.instance.Enable();
			if(FishingSoundManager.Instance != null)
			{
				FishingSoundManager.Instance.StopBoatIddle();
				FishingSoundManager.Instance.StopWatter_Environment();
			}
			print ("game over do Fishing");
		}
		else{
				BorgManager.instance.ShowBorgScreen();
				//FishingManager.instance.UnPause();
			}

	}

	private void PauseWithoutBorg()
	{
		print ("PauseWithoutBorg");
		fishAnimation_gone = false;
		GameManagerShare.instance.stopGesture = false;
		GameManagerShare.instance.press_Stop = false;
		GameManagerShare.instance.PauseOn();
	}

	public int fishPlus = 1;
	/// <summary>
	/// Fills the fish list.
	/// </summary>
	public void FillFishList(){
		//int dif =  fishSpots[indexRdSpot].GetComponent<FishingSpot>().difficulty; //HandReferenceFish.instance.GetFishingSpot().difficulty;
		/*switch(dif){
		case 0:
			for(int i = 0; i<3; i++){
				fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
			}		
			print("easy");
			break;
		case 1:
			for(int i = 0; i<4; i++){
				fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
			}		
			print("medium");
			break;
		case 2:
			for(int i = 0; i<5; i++){
				fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
			}	
			print("hard");
			break;
		case 3:
			for(int i = 0; i<3; i++){
				fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
			}	
			print("easy");
			break;
		case 4:
			for(int i = 0; i<4; i++){
				fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
			}	
			print("medium");
			break;
		case 5:
			for(int i = 0; i<5; i++){
				fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
			}	
			print("hard");
			break;
		default:
			print("nothing");
			break;
		}*/
		int qtdfish = PlayerPrefsManager.GetFishingQuantity(); //dif + fishPlus;
		for(int i = 0; i< qtdfish; i++){
			fish.Add(new Fish((Fish.FishType)UnityEngine.Random.Range(1,4)));
		}	
	}

	public List<Fish> GetFishList(){
		return fish;
	}

	public Fish GetCurrenFish(){
		return fish[currentFishIndex];
	}

	private int currentFishIndex = 0;

	public void EncreaseCurrentFish(){
		currentFishIndex ++;
	}
	
	/*public void SetActiveBlur(bool act){
		Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = act;
	}*/
}

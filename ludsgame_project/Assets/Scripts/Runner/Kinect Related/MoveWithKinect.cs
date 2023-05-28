using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;
using Assets.Scripts.Share;

public class MoveWithKinect : MonoBehaviour {
	private float playerInclination;
	private KinectManager kinect;
	private bool moving;

	private bool movingLeft;
	private bool movingRight;
	
	private Game game;
	private int gesture_choice;

	private float lateralAmountAnalog;		//inclinacao do torso analogico
	private float lateralAmountDigital;		//inclinacao do torso digital
	private float movingSideAmount;	//marcha lateral
	private float moveHandsensibility;	//movimento com as maos
	private float jumpAmountNeeded;	//necessario para pular
	private float rollAmountNeeded;	//necessario para rolar

	public bool movingWithDigitalInclination;
	public bool movingWithAnalogInclination;
	//[HideInInspector]
	private bool movingToSides;
	//[HideInInspector]
	private bool movingWithHands;

	public Text deb;

	private float initialY;
	private float initialKneeLeftY;
	private float initialKneeRightY;

	public static MoveWithKinect instance;

	void Awake()
	{
		instance = this;

	}
	// Use this for initialization
	void Start () {		

		lateralAmountAnalog = PlayerPrefsManager.GetLateralAmountAnalog ();
		lateralAmountDigital = PlayerPrefsManager.GetLateralAmountDigital ();
		moveHandsensibility = PlayerPrefsManager.GetHandSensibility ();
		movingSideAmount = PlayerPrefsManager.GetMovingSideAmount ();
		jumpAmountNeeded = PlayerPrefsManager.GetJumpAmountNeeded ();
		rollAmountNeeded = PlayerPrefsManager.GetRollAmountNeeded ();

		kinect = KinectManager.Instance;
        if (GameManagerShare.instance.IsUsingKinect() && kinect != null)
        {
			initialY = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).y;
			initialKneeLeftY = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.KneeLeft).y;
			initialKneeRightY = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.KneeRight).y;
		} else {
			initialY = 1;
		}
		//CheckGesture();
	}

	/*private void CheckGesture()
	{
		switch(game)
		{
		case Game.Pig:
			gesture_choice = PlayerPrefsManager.GetMoveWithHands_PigRunner();
			break;
		case Game.Sup:
			gesture_choice = PlayerPrefsManager.GetMoveWithHands_Sup();
			break;
		default:
			print ("No gesture variation available.");
			break;
		}
	}*/

	void Update () {
		if (movingWithDigitalInclination) {
			MovingWithInclinationDigital();
		}
		if (movingWithAnalogInclination) {
			MovingWithInclinationAnalog();
		}
		if (GameManagerShare.instance.game == Game.Pig && PlayerPrefsManager.GetIdPerfilPigrunner() == 0){//movingToSides) {
			if(GameManagerShare.instance.IsUsingKinect())
				MovingToSides();
		}
		if ((PlayerPrefsManager.GetIdPerfilPigrunner() == 1 && GameManagerShare.instance.game == Game.Pig)
		    || (PlayerPrefsManager.GetIdPerfilSup() == 1 && GameManagerShare.instance.game == Game.Sup) ) {//movingWithHands) {

			if(GameManagerShare.instance.IsUsingKinect()){
				MovingWithHands();
			}
		}

		/*if (GameManagerShare.instance.IsUsingKinect()) {
			//DetectJump ();
		  //  DetectRoll ();
		} else {
			movingWithInclinationDigital = false;
			movingWithInclinationAnalog = false;
			movingToSides = false;
			movingWithHands = false;
		}*/
	}


private bool jumping;
private bool rolling;
	float elapsedJumpTime;
	float elapsedRollTime;
	bool detectoupulo;
	/*
	private void DetectJump(){
	//	float currentYrightFoot = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.FootRight).y;
		//float currentYleftFoot = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.FootLeft).y;

		float currentYlLeftKnee = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.KneeLeft).y;
		float currentYlRightKnee = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.KneeRight).y;

		if (currentYlRightKnee >= initialKneeRightY + jumpAmountNeeded || 
		    currentYlLeftKnee >= initialKneeLeftY + jumpAmountNeeded) {
			if (!jumping) {
				PlayerJumpSlideControl.instance.Jump ();
				jumping = true;
				elapsedJumpTime = 0;
			}
		}
		elapsedJumpTime += Time.deltaTime;
		if (elapsedJumpTime > 2) {
			jumping = false;
		}
	}

	private void DetectRoll(){
		if(kinect.IsUserDetected()){
			float currentYhead = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y;
			float heightToRoll = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y * 0.8f;
			//if (currentYhead <= rollAmountNeeded) {
			if (currentYhead <= heightToRoll) {
				if (!rolling) {
					PlayerJumpSlideControl.instance.Roll ();
					rolling = true;
					elapsedRollTime = 0;
				}
			}
			elapsedRollTime += Time.deltaTime;
			if (elapsedRollTime > 2) {
				rolling = false;
			}
		}
	}*/

	private void MovingWithInclinationDigital(){
		if(AllowKinectToMovePlayerIf()){
			playerInclination = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter).x - 
				kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).x;
			
			if (moving == false) {
				if (playerInclination > lateralAmountDigital) {
					moving = true;
					PlayerTrailMovement.instance.GoRight();		
				}
				if (playerInclination < -lateralAmountDigital) {
					moving = true;
					PlayerTrailMovement.instance.GoLeft();		
				}
			}
			
			if (playerInclination <= lateralAmountDigital && playerInclination >= -lateralAmountDigital) {
				moving = false;
			}
		}
	}

	float elapsedTimeLeft;
	float elapsedTimeRight;
	private void MovingWithInclinationAnalog(){
		if(AllowKinectToMovePlayerIf()){
			playerInclination = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter).x - 
				kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).x;
				
			if (playerInclination > lateralAmountAnalog) {
				elapsedTimeRight += Time.deltaTime;
				if (movingRight == false) {
					movingRight = true;
					elapsedTimeRight = 0;
					elapsedTimeLeft = 0f;
					PlayerTrailMovement.instance.GoRight();		
				}
			}

			if (playerInclination < -lateralAmountAnalog) {
				elapsedTimeLeft += Time.deltaTime;
				if (movingLeft == false) {
					movingLeft = true;
					elapsedTimeLeft = 0;
					elapsedTimeRight = 0f;
					PlayerTrailMovement.instance.GoLeft();		
				}
			}

			if (elapsedTimeLeft > 0.5f) {
				movingLeft = false;
			}
			if (elapsedTimeRight > 0.5f) {
				movingRight = false;
			}
			//deb.text = "time left " + elapsedTimeLeft.ToString () + " \n time right "+ elapsedTimeRight.ToString()+" moving " + moving.ToString();
		}
	}

	private void MovingToSides(){
        if (AllowKinectToMovePlayerIf() && kinect != null)
        {
			float playerPos = kinect.GetUserPosition(kinect.GetPlayer1ID()).x;
			var pigCurrentPosix = PlayerTrailMovement.GetCurrentPosition();

			if (playerPos <= movingSideAmount && playerPos >= -movingSideAmount) {
				//meio
				//checar a posiçao atual, se for 0 chamar animaçao de mover pra direita PlayerTrailMovement.instance.GoRight(); e setar(1)
				//se a posiçao atual for 2, chamr animaçao de mover pra esquerda e setar(1)

				if(pigCurrentPosix == 0)
				{
					PlayerTrailMovement.instance.GoRight();
				}				
				else if(pigCurrentPosix == 2)
				{
					PlayerTrailMovement.instance.GoLeft();
					
				}
				PlayerTrailMovement.SetCurrentPosition(1);
			}

			if(playerPos > movingSideAmount)
			//direita
			{
				//se estiver no meio
				if(pigCurrentPosix == 1){
					PlayerTrailMovement.instance.GoRight();
				}
				PlayerTrailMovement.SetCurrentPosition(2);
			}

			if(playerPos < -movingSideAmount)
			//esquerda
			{
				//se estiver no meio
				if(pigCurrentPosix == 1){
					PlayerTrailMovement.instance.GoLeft();
				}
				PlayerTrailMovement.SetCurrentPosition(0);
			}
			
		}
	}



	//mover com elevaçao lateral
	private void MovingWithHands()
	{
		Vector3 handLeftPos = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft);
		Vector3 handRightPos = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight);
		Vector3 torsoPos = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter);
		
		float leftHandDistanceToTorso = handLeftPos.x - torsoPos.x;
		float rightHandDistanceToTorso = handRightPos.x - torsoPos.x;

		if(GameManagerShare.instance.game == Assets.Scripts.Share.Game.Pig){
			if(AllowKinectToMovePlayerIf())
			{
				//handsjoints
				if(leftHandDistanceToTorso < -moveHandsensibility){
					if(!movingLeft){
						movingLeft = true;
						PlayerTrailMovement.instance.GoLeft();
					}
				}
				if(rightHandDistanceToTorso > moveHandsensibility){
					if(!movingRight){
						movingRight = true;
						PlayerTrailMovement.instance.GoRight();
					}
				}
				if(leftHandDistanceToTorso > -0.35f){
					movingLeft = false;
				}
				if(rightHandDistanceToTorso < 0.35f){
					movingRight = false;
				}
			}
		}else if(GameManagerShare.instance.game == Assets.Scripts.Share.Game.Sup)
		{
			if(leftHandDistanceToTorso < -moveHandsensibility){
				if(!movingLeft){
					movingLeft = true;
					if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()){
						Sup_Controller.instance.LeftEngineImpulse();
						print ("remar pra direita elevations");
					}
				}
			}
			if(rightHandDistanceToTorso > moveHandsensibility){
				if(!movingRight){
					movingRight = true;
					if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()){
						Sup_Controller.instance.RightEngineImpulse();
						print ("remar pra direita elevations");
					}
				}
			}
			if(leftHandDistanceToTorso > -0.35f){
				movingLeft = false;
			}
			if(rightHandDistanceToTorso < 0.35f){
				movingRight = false;
			}

		}/*else if(GameManagerShare.instance.game == Assets.Scripts.Share.Game.Fishing)
		{
						
			if(leftHandDistanceToTorso < -moveHandsensibility)
			{
				if(!movingLeft)
				{
					movingLeft = true;
					if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting()
					   && (!PullBaitControl.instance.pullingRodHope || !PullBaitControl.instance.bigRodHopePull))
					{
						if(!PullBaitControl.instance.IsBaitThrow()){
							PullBaitControl.instance.ThrowBait("2hands");
						}
					}
				}
			}
			if(rightHandDistanceToTorso > moveHandsensibility){
				if(!movingRight)
				{
					movingRight = true;
					if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting()
					   && (!PullBaitControl.instance.pullingRodHope || !PullBaitControl.instance.bigRodHopePull))
					{
						if(!PullBaitControl.instance.IsBaitThrow()){
							PullBaitControl.instance.ThrowBait("2hands");
						}
					}
				}
			}
			if(leftHandDistanceToTorso > -0.35f){
				movingLeft = false;
			}
			if(rightHandDistanceToTorso < 0.35f){
				movingRight = false;
			}
		}*/
		//gk
		/*else if(GameManagerShare.instance.game == Assets.Scripts.Share.Game.Goal_Keeper)
		{
						
			if(leftHandDistanceToTorso < -moveHandsensibility)
			{
				if(!movingLeft)
				{
					movingLeft = true;
					if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting())
					{
						//
					}
				}
			}
			if(rightHandDistanceToTorso > moveHandsensibility){
				if(!movingRight)
				{
					movingRight = true;
					if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting())
					{
						//
					}
				}
			}
			if(leftHandDistanceToTorso > -0.35f){
				movingLeft = false;
			}
			if(rightHandDistanceToTorso < 0.35f){
				movingRight = false;
			}
		}*/
	}

	private bool AllowKinectToMovePlayerIf(){
		if(PlayerTrailMovement.GetCurrentPosition() >= 0 && PlayerTrailMovement.IsFloor3Ways() && FloorMovementControl.instance.IsFloorMoving()){
			return true;
		}
		return false;
	}

}

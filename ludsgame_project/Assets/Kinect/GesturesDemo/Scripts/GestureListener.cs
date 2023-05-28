using UnityEngine;
using System.Collections;
using System;
//using Share.EventSystem;
using Share.EventsSystem;
using System.Collections.Generic;
using Runner.Managers;
using Share.Managers;
using Sandbox.UI;
using Sandbox.GameUtils;
using BridgeGame.Player;
using Ludsgame;
using Share.KinectUtils;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	// GUI Text to display the gesture messages.
	public Text GestureInfo;

	private bool swipeLeft;
	private bool swipeRight;
	
	private const int leftHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	private const int rightHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	
	private const int leftElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowLeft;
	private const int rightElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowRight;
	
	private const int leftShoulderIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderLeft;
	private const int rightShoulderIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderRight;
	
	private const int hipCenterIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter;
	private const int shoulderCenterIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
	private const int leftHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipLeft;
	private const int rightHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipRight;
	private const int rightKneeIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.KneeRight;
	private const int leftKneeIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.KneeLeft;
	private const int rightWristIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.WristRight;
	private const int leftWristIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.WristLeft;



	public static List<string> motionInserts;

	void Start(){
		motionInserts = new List<string> ();
	}



	public bool IsSwipeLeft()
	{
		if(swipeLeft)
		{
			swipeLeft = false;
			return true;
		}
		
		return false;
	}
	
	public bool IsSwipeRight()
	{
		if(swipeRight)
		{
			swipeRight = false;
			return true;
		}
		
		return false;
	}
	
	private KinectManager manager;
	private GameStartClass gs;

	public void UserDetected(uint userId, int userIndex)
	{
		// detect these user specific gestures
		manager = KinectManager.Instance;
		
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
		manager.DetectGesture(userId, KinectGestures.Gestures.Wave); 
		manager.DetectGesture(userId, KinectGestures.Gestures.Squat);
		//manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
		manager.DetectGesture(userId, KinectGestures.Gestures.MoveLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.MoveRight);
		//manager.DetectGesture(userId, KinectGestures.Gestures.Push);
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftKnee);
		manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightKnee);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowBothHandsFromRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowBothHandsFromRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowBothHandsFromLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.PullLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.PullRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.RowLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.RowRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.RowMid);

		manager.DetectGesture(userId, KinectGestures.Gestures.RightHandStop);
		manager.DetectGesture(userId, KinectGestures.Gestures.LeftHandStop);
		manager.DetectGesture(userId, KinectGestures.Gestures.RemarLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.RemarRight);
		manager.DetectGesture(userId, KinectGestures.Gestures.TwoArmsUp);//testar
		manager.DetectGesture(userId, KinectGestures.Gestures.RightArmElevation);//nao e usado
		manager.DetectGesture(userId, KinectGestures.Gestures.LeftArmElevation);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowRightDiferentao);
		manager.DetectGesture(userId, KinectGestures.Gestures.ThrowLeftDiferentao);


		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<Text>().text = "Swipe left or right to change the slides.";
		}

		/* leftHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
		 rightHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight;
		
		 leftElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowLeft;
		 rightElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowRight;
		
		 leftShoulderIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderLeft;
		 rightShoulderIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderRight;
		
		 hipCenterIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter;
		 shoulderCenterIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
		 leftHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipLeft;
		 rightHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipRight;*/

	}
	
	public void UserLost(uint userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<Text>().text = string.Empty;
		}
	}
	
	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		// don't do anything here
	}
	
	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		var activeSceneName = SceneManager.GetActiveScene().name;
		
		string sGestureText = gesture + " detected";

        GameManagerShare.instance.ExecuteKinectGesture(gesture);

        if (GestureInfo != null)
        {
            GestureInfo.GetComponent<Text>().text = sGestureText;
        }

        if (gesture == KinectGestures.Gestures.RaiseRightHand)
        {        
            ExecuteActionRaiseRightHand();
        }
        else if (gesture == KinectGestures.Gestures.RaiseLeftHand)
        {
            ExecuteActionRaiseLeftHand();

        }
        else if (gesture == KinectGestures.Gestures.Wave)
        {
            if (manager.GetJointPosition(manager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft).y >
               manager.GetJointPosition(manager.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y)
            {
                PlayerHandController.instance.SetHand(PlayerHandController.UsedHand.Left);
                DistanceCalibrator.instance.SetGestureDone();
            }
            else
            {
                PlayerHandController.instance.SetHand(PlayerHandController.UsedHand.Right);
                DistanceCalibrator.instance.SetGestureDone();
            }
        }
        else if (gesture == KinectGestures.Gestures.ThrowLeft)
        {
         /*   float dealta = KinectGestures.GetFinalJointPosition(leftHandIndex).x;
            ExecuteActionThrow(dealta);*/
            AddMotionToList(gesture, leftHandIndex, 2);
        }
        else if (gesture == KinectGestures.Gestures.ThrowRight)
        {
       /*     float dealta = KinectGestures.GetFinalJointPosition(rightHandIndex).x;
            ExecuteActionThrow(dealta);*/
            AddMotionToList(gesture, rightHandIndex, 2);
        }
        else if (gesture == KinectGestures.Gestures.Jump)
        {
            //PlayerJumpSlideControl.instance.Jump ();
            print("gesto de pulo");
        }
        else if (gesture == KinectGestures.Gestures.MoveLeft)
        {
            if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                AddMotionToList(gesture, leftHipIndex, 0);
            }
        }
        else if (gesture == KinectGestures.Gestures.MoveRight)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                AddMotionToList(gesture, rightHipIndex, 0);
            }
        }
        else if (gesture == KinectGestures.Gestures.Push)
        {
            ExecuteActionPushGesture(joint.ToString());
        }
        else if (gesture == KinectGestures.Gestures.RaiseLeftKnee)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
              //  PlayerJumpSlideControl.instance.Jump();
                lastAngle = (int)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex, rightHipIndex, rightKneeIndex);
                AddMotionToList(gesture, leftKneeIndex, 3);
            }
        }
        else if (gesture == KinectGestures.Gestures.RaiseRightKnee)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
              //  PlayerJumpSlideControl.instance.Jump();
                lastAngle = (int)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex, rightHipIndex, rightKneeIndex);
                AddMotionToList(gesture, rightKneeIndex, 3);
            }
        }
        else if (gesture == KinectGestures.Gestures.ThrowBothHandsFromRight)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //PlayerControlLake.Throw();
                AddMotionToList(gesture, rightWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.ThrowBothHandsFromLeft)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //PlayerControlLake.Throw();
                AddMotionToList(gesture, leftWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.ThrowOneHandLeft)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.ThrowOneHand();
                AddMotionToList(gesture, leftWristIndex, 2);
            }
        }
        else if (gesture == KinectGestures.Gestures.ThrowOneHandRight)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.ThrowOneHand();
                AddMotionToList(gesture, rightWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.WheelFrontalLeftHand)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.WheelLef();
                AddMotionToList(gesture, leftWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.WheelFrontalRightHand)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.WheelRight();
                AddMotionToList(gesture, rightWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.WheelFrontal)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.WheelRight();
                AddMotionToList(gesture, rightWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.PullLeft)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.WheelRight();
                AddMotionToList(gesture, leftWristIndex, 2);
            }
        }

        else if (gesture == KinectGestures.Gestures.PullRight)
        {
			if (activeSceneName != "Calibration" && activeSceneName != "startScreenNew")
            {
                //FishingControl.WheelRight();
                AddMotionToList(gesture, rightWristIndex, 2);
            }
        }
		return true;
	}


	private static float lastAngle;

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		// don't do anything here, just reset the gesture state
		return true;
	}

	private void ExecuteActionRaiseRightHand(){
		PlayerHandController.instance.SetHand(PlayerHandController.UsedHand.Right);
		if(SceneManager.GetActiveScene().name == "GoalKeeper"){
			gs = GameObject.Find ("GameManager_Goalkeeper").GetComponent<GameStartClass> ();

			if (!gs.IsGameStarted ()) {
				print("chamou evento gamestart");
				Events.RaiseEvent<GameStart>();	
				Events.RaiseEvent<UnPauseEvent>();
			}
		}else if (SceneManager.GetActiveScene().name == "Bridge" && BridgeManager.instance.IsCameraOverviewAnimComplete()){
			BridgeManager.instance.SetIsReady(true);
		}
	}

	private void ExecuteActionRaiseLeftHand(){
		PlayerHandController.instance.SetHand (PlayerHandController.UsedHand.Left);
		if (SceneManager.GetActiveScene().name == "GoalKeeper") {
			gs = GameObject.Find ("GameManager_Goalkeeper").GetComponent<GameStartClass> ();
			print("gs.IsGameStarted " + gs.IsGameStarted ());
			print("levantou no penalt");
			if (!gs.IsGameStarted ()) {			
				Events.RaiseEvent<GameStart>();
				Events.RaiseEvent<UnPauseEvent>();
			}
		}else if (SceneManager.GetActiveScene().name == "Bridge" && BridgeManager.instance.IsCameraOverviewAnimComplete()){
			BridgeManager.instance.SetIsReady(true);
		}
	}


	private void ExecuteActionPushGesture(string hand) {
		if (SceneManager.GetActiveScene().name == "Sandbox") {
			if(!Sandbox.GameUtils.GameManager.instance.isPlaying){
				HidePresentationSandbox();
				if(hand == "HandRight")
					MenuManager.instance.SetHandReference(true);
				else
					MenuManager.instance.SetHandReference(false);

				Events.RaiseEvent<PushGestureGame>();
			}
		}
	}

	public void ExecuteActionThrow(float finalHandPosX){
		if (SceneManager.GetActiveScene().name == "Throw") {
			Bullseye.PlayerThrow.instance.CallShootAnim(finalHandPosX);
		}
	}

	private void HidePresentationSandbox(){
		Sandbox.GameUtils.GameManager.instance.isPlaying = true;
		TimeBar.instance.StartTimeBar();
		Sandbox.GameUtils.GameManager.Instance().SetGUIActive (true);
		GameObject.Find("Intro_Screen").GetComponent<Animation>().Play("HidePresentationScreen");
	}

	/// <summary>
	/// Adds the motion to list.
	/// </summary>
	/// <param name="gesture">Gesture.</param>
	/// <param name="jointIndex">Joint index.</param>
	/// <param name="axis">Axis.</param>
	private static void AddMotionToList(KinectGestures.Gestures gesture, int jointIndex, int axis){
		string id_player = PlayerPrefsManager.GetPlayerID().ToString();
		//string id_game = PlayerPrefsManager.GetCurrentGameID().ToString();
        int id_game = (int) GameManagerShare.instance.GetCurrentGame();
        string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();	
		string id_physio = PlayerPrefsManager.GetPhysioID().ToString();
		string id_clinic = PlayerPrefsManager.GetClinicID().ToString();

	//=========================================================================================================
		string desloc = "0";
		string desloc2 = "0";
		if(axis == 0)
			desloc = Math.Abs(KinectGestures.GetInitialJointPosition(jointIndex).x - KinectGestures.GetFinalJointPosition(jointIndex).x).ToString(FormatConfig.Nfi);
		if(axis == 1)
			desloc = Math.Abs(KinectGestures.GetInitialJointPosition(jointIndex).y - KinectGestures.GetFinalJointPosition(jointIndex).y).ToString(FormatConfig.Nfi);
		if (axis == 2)
			desloc = Math.Abs (KinectGestures.GetInitialJointPosition (jointIndex).z - KinectGestures.GetFinalJointPosition (jointIndex).z).ToString (FormatConfig.Nfi);
		if (axis == 3)
			desloc = lastAngle.ToString(FormatConfig.Nfi);

		if(gesture == KinectGestures.Gestures.MoveLeft || gesture == KinectGestures.Gestures.MoveRight){
			int j1 = (int)KinectWrapper.NuiSkeletonPositionIndex.AnkleLeft;
			int j2 = (int)KinectWrapper.NuiSkeletonPositionIndex.AnkleRight;
			desloc2 = Math.Abs(GenericKinectMethods.instance.GetJointsDistanceX(j1,j2,KinectManager.Instance.GetPlayer1ID())).ToString(FormatConfig.Nfi);
		}
	//==========================================================================================================
		string gestureTime = KinectGestures.GetGestureTime().ToString(FormatConfig.Nfi);
		string sb = "@" + desloc + "@"+ desloc2 + "@" + (int)gesture + "@" + gestureTime + "@" + id_player + "@" + id_game + "@" + current_match + "@" + id_physio + "@" + id_clinic;
	
		motionInserts.Add(sb);
	}




}

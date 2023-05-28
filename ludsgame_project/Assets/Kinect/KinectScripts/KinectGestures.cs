using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Share.KinectUtils;
using Share.Managers;
public class KinectGestures
{
	private static float auxTime;
	public static bool isInPoseStop; 
	//private bool startFinalPos;

	public interface GestureListenerInterface
	{
		// Invoked when a new user is detected and tracking starts
		// Here you can start gesture detection with KinectManager.DetectGesture()
		void UserDetected(uint userId, int userIndex);
		
		// Invoked when a user is lost
		// Gestures for this user are cleared automatically, but you can free the used resources
		void UserLost(uint userId, int userIndex);
		
		// Invoked when a gesture is in progress 
		void GestureInProgress(uint userId, int userIndex, Gestures gesture, float progress, 
		                       KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos);
		
		// Invoked if a gesture is completed.
		// Returns true, if the gesture detection must be restarted, false otherwise
		bool GestureCompleted(uint userId, int userIndex, Gestures gesture,
		                      KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos);
		
		// Invoked if a gesture is cancelled.
		// Returns true, if the gesture detection must be retarted, false otherwise
		bool GestureCancelled(uint userId, int userIndex, Gestures gesture, 
		                      KinectWrapper.NuiSkeletonPositionIndex joint);
	}

	
	public enum Gestures
	{
		None = 0,
		RaiseRightHand, //1
		RaiseLeftHand,//2
		Psi,//3
		Tpose,//4
		Stop,//5
		Wave,//6
		Click,//7
		SwipeLeft,//8
		SwipeRight,//9
		SwipeUp,//10
		SwipeDown,//11
		RightHandCursor,//12
		LeftHandCursor,//13
		ZoomOut,//14
		ZoomIn,//15
		Wheel,//16
		Jump,//17
		Squat,//18
		Push,//19
		Pull,//20
		ThrowLeft,//21
		ThrowRight,//22
		MoveLeft,//23
		MoveRight,//24
		RaiseRightKnee,//25
		RaiseLeftKnee,//26
		ThrowBothHandsFromRight,//27
		ThrowBothHandsFromLeft,//28
		ThrowOneHandRight,//29
		ThrowOneHandLeft,//30
		WheelFrontalLeftHand,//31
		WheelFrontalRightHand,//32
		WheelFrontal,//33
		PullLeft,//34
		PullRight,//35
		SquatRight,//36
		SquatLeft,//37
		RowLeft, //38
		RowRight, //39
		RowMid, //40
		RightHandStop, //41
		LeftHandStop, //42
		RightArmElevation, //43
		LeftArmElevation, //44
		TwoArmsUp, //45 elevaçao frontal
		RemarLeft, //46
		RemarRight, //47
		ThrowRightDiferentao, //48
		ThrowLeftDiferentao


	}
	
	
	public struct GestureData
	{
		public uint userId;
		public Gestures gesture;
		public int state;
		public float timestamp;
		public int joint;
		public Vector3 jointPos;
		public Vector3 screenPos;
		public float tagFloat;
		public Vector3 tagVector;
		public Vector3 tagVector2;
		public float progress;
		public bool complete;
		public bool cancelled;
		public List<Gestures> checkForGestures;
		public float startTrackingAtTime;
	}
	
	
	
	// Gesture related constants, variables and functions
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
	private const int rightAnkleIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.AnkleRight;
	private const int leftAnkleIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.AnkleLeft;

	private static int qtdWheelFrontal = 0;
	private static int qtdWheel = 0;

	private static void SetGestureJoint(ref GestureData gestureData, float timestamp, int joint, Vector3 jointPos)
	{
		//DistanceCalibrator.instance.angleText.text = "set gesture joint" +gestureData.gesture.ToString() ;

		gestureData.joint = joint;
		gestureData.jointPos = jointPos;
		gestureData.timestamp = timestamp;
		gestureData.state++;
	}
	
	private static void SetGestureCancelled(ref GestureData gestureData)
	{
		gestureData.state = 0;
		gestureData.progress = 0f;
		gestureData.cancelled = true;
	}
	
	private static void CheckPoseComplete(ref GestureData gestureData, float timestamp, Vector3 jointPos, bool isInPose, float durationToComplete)
	{
		if(isInPose)
		{
			float timeLeft = timestamp - gestureData.timestamp;
			gestureData.progress = durationToComplete > 0f ? Mathf.Clamp01(timeLeft / durationToComplete) : 1.0f;
			
			if(timeLeft >= durationToComplete)
			{
				gestureData.timestamp = timestamp;
				gestureData.jointPos = jointPos;
				gestureData.state++;
				gestureData.complete = true;
			}
		}
		else
		{
			SetGestureCancelled(ref gestureData);
		}
	}
	
	private static void SetScreenPos(uint userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
	{
		Vector3 handPos = jointsPos[rightHandIndex];
		//		Vector3 elbowPos = jointsPos[rightElbowIndex];
		//		Vector3 shoulderPos = jointsPos[rightShoulderIndex];
		bool calculateCoords = false;
		
		if(gestureData.joint == rightHandIndex)
		{
			if(jointsTracked[rightHandIndex] /**&& jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]*/)
			{
				calculateCoords = true;
			}
		}
		else if(gestureData.joint == leftHandIndex)
		{
			if(jointsTracked[leftHandIndex] /**&& jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex]*/)
			{
				handPos = jointsPos[leftHandIndex];
				//				elbowPos = jointsPos[leftElbowIndex];
				//				shoulderPos = jointsPos[leftShoulderIndex];
				
				calculateCoords = true;
			}
		}
		
		if(calculateCoords)
		{
			//			if(gestureData.tagFloat == 0f || gestureData.userId != userId)
			//			{
			//				// get length from shoulder to hand (screen range)
			//				Vector3 shoulderToElbow = elbowPos - shoulderPos;
			//				Vector3 elbowToHand = handPos - elbowPos;
			//				gestureData.tagFloat = (shoulderToElbow.magnitude + elbowToHand.magnitude);
			//			}
			
			if(jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && 
			   jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
			{
				Vector3 neckToHips = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
				Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];
				
				gestureData.tagVector2.x = rightToLeft.x; // * 1.2f;
				gestureData.tagVector2.y = neckToHips.y; // * 1.2f;
				
				if(gestureData.joint == rightHandIndex)
				{
					gestureData.tagVector.x = jointsPos[rightShoulderIndex].x - gestureData.tagVector2.x / 2;
					gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
				}
				else
				{
					gestureData.tagVector.x = jointsPos[leftShoulderIndex].x - gestureData.tagVector2.x / 2;
					gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
				}
			}
			
			//			Vector3 shoulderToHand = handPos - shoulderPos;
			//			gestureData.screenPos.x = Mathf.Clamp01((gestureData.tagFloat / 2 + shoulderToHand.x) / gestureData.tagFloat);
			//			gestureData.screenPos.y = Mathf.Clamp01((gestureData.tagFloat / 2 + shoulderToHand.y) / gestureData.tagFloat);
			
			if(gestureData.tagVector2.x != 0 && gestureData.tagVector2.y != 0)
			{
				Vector3 relHandPos = handPos - gestureData.tagVector;
				gestureData.screenPos.x = Mathf.Clamp01(relHandPos.x / gestureData.tagVector2.x);
				gestureData.screenPos.y = Mathf.Clamp01(relHandPos.y / gestureData.tagVector2.y);
			}
			
			//Debug.Log(string.Format("{0} - S: {1}, H: {2}, SH: {3}, L : {4}", gestureData.gesture, shoulderPos, handPos, shoulderToHand, gestureData.tagFloat));
		}
	}
	
	private static void SetZoomFactor(uint userId, ref GestureData gestureData, float initialZoom, ref Vector3[] jointsPos, ref bool[] jointsTracked)
	{
		Vector3 vectorZooming = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
		
		if(gestureData.tagFloat == 0f || gestureData.userId != userId)
		{
			gestureData.tagFloat = 0.5f; // this is 100%
		}
		
		float distZooming = vectorZooming.magnitude;
		gestureData.screenPos.z = initialZoom + (distZooming / gestureData.tagFloat);
	}
	
	//	private static void SetWheelRotation(uint userId, ref GestureData gestureData, Vector3 initialPos, Vector3 currentPos)
	//	{
	//		float angle = Vector3.Angle(initialPos, currentPos) * Mathf.Sign(currentPos.y - initialPos.y);
	//		gestureData.screenPos.z = angle;
	//	}
	
	// estimate the next state and completeness of the gesture
	public static void CheckForGesture(uint userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
	{
		if(gestureData.complete)
			return;
		
		float bandSize = (jointsPos[shoulderCenterIndex].y - jointsPos[hipCenterIndex].y);
		float gestureTop = jointsPos[shoulderCenterIndex].y + bandSize / 2;
		float gestureBottom = jointsPos[shoulderCenterIndex].y - bandSize;
		float gestureRight = jointsPos[rightHipIndex].x;
		float gestureLeft = jointsPos[leftHipIndex].x;

		switch (gestureData.gesture) 
		{
			// check for RaiseRightHand
		case Gestures.RaiseRightHand:
			switch (gestureData.state) {
			case 0:  // gesture detection
				if (jointsTracked [rightHandIndex] && jointsTracked [rightShoulderIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [rightShoulderIndex].y) > 0.1f) 
				{
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
				}
				break;
				
			case 1:  // gesture complete
				bool isInPose = jointsTracked [rightHandIndex] && jointsTracked [rightShoulderIndex] &&
					(jointsPos [rightHandIndex].y - jointsPos [rightShoulderIndex].y) > 0.1f;
				
				Vector3 jointPos = jointsPos [gestureData.joint];
				CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
				break;
			}
			break;
			
			// check for RaiseLeftHand
		case Gestures.RaiseLeftHand:
			switch (gestureData.state) {
			case 0:  // gesture detection
				if (jointsTracked [leftHandIndex] && jointsTracked [leftShoulderIndex] &&
				    (jointsPos [leftHandIndex].y - jointsPos [leftShoulderIndex].y) > 0.1f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
				}
				break;
				
			case 1:  // gesture complete
				bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [leftShoulderIndex] &&
					(jointsPos [leftHandIndex].y - jointsPos [leftShoulderIndex].y) > 0.1f;
				
				Vector3 jointPos = jointsPos [gestureData.joint];
				CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
				break;
			}
			break;
			
			// check for Psi
		case Gestures.Psi:
			switch (gestureData.state) {
			case 0:  // gesture detection
				if (jointsTracked [rightHandIndex] && jointsTracked [rightShoulderIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [rightShoulderIndex].y) > 0.1f &&
				    jointsTracked [leftHandIndex] && jointsTracked [leftShoulderIndex] &&
				    (jointsPos [leftHandIndex].y - jointsPos [leftShoulderIndex].y) > 0.1f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
				}
				break;
				
			case 1:  // gesture complete
				bool isInPose = jointsTracked [rightHandIndex] && jointsTracked [rightShoulderIndex] &&
					(jointsPos [rightHandIndex].y - jointsPos [rightShoulderIndex].y) > 0.1f &&
						jointsTracked [leftHandIndex] && jointsTracked [leftShoulderIndex] &&
						(jointsPos [leftHandIndex].y - jointsPos [leftShoulderIndex].y) > 0.1f;
				
				Vector3 jointPos = jointsPos [gestureData.joint];
				CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
				break;
			}
			break;
			
			// check for Tpose
		case Gestures.Tpose:
			switch (gestureData.state) {
			case 0:  // gesture detection
				if (jointsTracked [rightHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [rightShoulderIndex] &&
				    Mathf.Abs (jointsPos [rightElbowIndex].y - jointsPos [rightShoulderIndex].y) < 0.1f && // 0.07f
				    Mathf.Abs (jointsPos [rightHandIndex].y - jointsPos [rightShoulderIndex].y) < 0.1f && // 0.7f
				    jointsTracked [leftHandIndex] && jointsTracked [leftElbowIndex] && jointsTracked [leftShoulderIndex] &&
				    Mathf.Abs (jointsPos [leftElbowIndex].y - jointsPos [leftShoulderIndex].y) < 0.1f &&
				    Mathf.Abs (jointsPos [leftHandIndex].y - jointsPos [leftShoulderIndex].y) < 0.1f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
				}
				break;
				
			case 1:  // gesture complete
				bool isInPose = jointsTracked [rightHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [rightShoulderIndex] &&
					Mathf.Abs (jointsPos [rightElbowIndex].y - jointsPos [rightShoulderIndex].y) < 0.1f && // 0.7f
						Mathf.Abs (jointsPos [rightHandIndex].y - jointsPos [rightShoulderIndex].y) < 0.1f && // 0.7f
						jointsTracked [leftHandIndex] && jointsTracked [leftElbowIndex] && jointsTracked [leftShoulderIndex] &&
						Mathf.Abs (jointsPos [leftElbowIndex].y - jointsPos [leftShoulderIndex].y) < 0.1f &&
						Mathf.Abs (jointsPos [leftHandIndex].y - jointsPos [leftShoulderIndex].y) < 0.1f;
				
				Vector3 jointPos = jointsPos [gestureData.joint];
				CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
				break;
			}
			break;
			
			// check for Stop
		case Gestures.Stop:
			switch (gestureData.state) {
			case 0:  // gesture detection
				if (jointsTracked [rightHandIndex] && jointsTracked [rightHipIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [rightHipIndex].y) < 0.1f &&
				    (jointsPos [rightHandIndex].x - jointsPos [rightHipIndex].x) >= 0.4f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
				} else if (jointsTracked [leftHandIndex] && jointsTracked [leftHipIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [leftHipIndex].y) < 0.1f &&
				           (jointsPos [leftHandIndex].x - jointsPos [leftHipIndex].x) <= -0.4f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
				}
				break;
				
			case 1:  // gesture complete
				bool isInPose = (gestureData.joint == rightHandIndex) ?
					(jointsTracked [rightHandIndex] && jointsTracked [rightHipIndex] &&
					 (jointsPos [rightHandIndex].y - jointsPos [rightHipIndex].y) < 0.1f &&
					 (jointsPos [rightHandIndex].x - jointsPos [rightHipIndex].x) >= 0.4f) :
						(jointsTracked [leftHandIndex] && jointsTracked [leftHipIndex] &&
						 (jointsPos [leftHandIndex].y - jointsPos [leftHipIndex].y) < 0.1f &&
						 (jointsPos [leftHandIndex].x - jointsPos [leftHipIndex].x) <= -0.4f);
				
				Vector3 jointPos = jointsPos [gestureData.joint];
				CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.PoseCompleteDuration);
				break;
				
			}
			break;
			
			// check for Wave
		case Gestures.Wave:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHandIndex] && jointsTracked [rightElbowIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [rightElbowIndex].y) > 0.1f &&
				    (jointsPos [rightHandIndex].x - jointsPos [rightElbowIndex].x) > 0.05f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.3f;
				} else if (jointsTracked [leftHandIndex] && jointsTracked [leftElbowIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [leftElbowIndex].y) > 0.1f &&
				           (jointsPos [leftHandIndex].x - jointsPos [leftElbowIndex].x) < -0.05f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture - phase 2
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = gestureData.joint == rightHandIndex ?
						jointsTracked [rightHandIndex] && jointsTracked [rightElbowIndex] &&
							(jointsPos [rightHandIndex].y - jointsPos [rightElbowIndex].y) > 0.1f && 
							(jointsPos [rightHandIndex].x - jointsPos [rightElbowIndex].x) < -0.05f :
							jointsTracked [leftHandIndex] && jointsTracked [leftElbowIndex] &&
							(jointsPos [leftHandIndex].y - jointsPos [leftElbowIndex].y) > 0.1f &&
							(jointsPos [leftHandIndex].x - jointsPos [leftElbowIndex].x) > 0.05f;
					
					if (isInPose) {
						gestureData.timestamp = timestamp;
						gestureData.state++;
						gestureData.progress = 0.7f;
					}
				} 
				else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			case 2:  // gesture phase 3 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = gestureData.joint == rightHandIndex ?
						jointsTracked [rightHandIndex] && jointsTracked [rightElbowIndex] &&
							(jointsPos [rightHandIndex].y - jointsPos [rightElbowIndex].y) > 0.1f && 
							(jointsPos [rightHandIndex].x - jointsPos [rightElbowIndex].x) > 0.05f :
							jointsTracked [leftHandIndex] && jointsTracked [leftElbowIndex] &&
							(jointsPos [leftHandIndex].y - jointsPos [leftElbowIndex].y) > 0.1f &&
							(jointsPos [leftHandIndex].x - jointsPos [leftElbowIndex].x) < -0.05f;
					
					if (isInPose) {
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
			// check for Click
		case Gestures.Click:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHandIndex] && jointsTracked [rightElbowIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [rightElbowIndex].y) > -0.1f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.3f;
					
					// set screen position at the start, because this is the most accurate click position
					SetScreenPos (userId, ref gestureData, ref jointsPos, ref jointsTracked);
				} else if (jointsTracked [leftHandIndex] && jointsTracked [leftElbowIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [leftElbowIndex].y) > -0.1f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.3f;
					
					// set screen position at the start, because this is the most accurate click position
					SetScreenPos (userId, ref gestureData, ref jointsPos, ref jointsTracked);
				}
				break;
				
			case 1:  // gesture - phase 2
				//						if((timestamp - gestureData.timestamp) < 1.0f)
				//						{
				//							bool isInPose = gestureData.joint == rightHandIndex ?
				//								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
				//								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
				//								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
				//								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.05f :
				//								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
				//								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
				//								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
				//								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.05f;
				//				
				//							if(isInPose)
				//							{
				//								gestureData.timestamp = timestamp;
				//								gestureData.jointPos = jointsPos[gestureData.joint];
				//								gestureData.state++;
				//								gestureData.progress = 0.7f;
				//							}
				//							else
				//							{
				//								// check for stay-in-place
				//								Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
				//								isInPose = distVector.magnitude < 0.05f;
				//
				//								Vector3 jointPos = jointsPos[gestureData.joint];
				//								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.ClickStayDuration);
				//							}
				//						}
				//						else
			{
				// check for stay-in-place
				Vector3 distVector = jointsPos [gestureData.joint] - gestureData.jointPos;
				bool isInPose = distVector.magnitude < 0.05f;
				
				Vector3 jointPos = jointsPos [gestureData.joint];
				CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, KinectWrapper.Constants.ClickStayDuration);
				//							SetGestureCancelled(gestureData);
			}
				break;
				
				//					case 2:  // gesture phase 3 = complete
				//						if((timestamp - gestureData.timestamp) < 1.0f)
				//						{
				//							bool isInPose = gestureData.joint == rightHandIndex ?
				//								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
				//								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
				//								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
				//								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.05f :
				//								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
				//								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
				//								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
				//								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.05f;
				//
				//							if(isInPose)
				//							{
				//								Vector3 jointPos = jointsPos[gestureData.joint];
				//								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
				//							}
				//						}
				//						else
				//						{
				//							// cancel the gesture
				//							SetGestureCancelled(ref gestureData);
				//						}
				//						break;
			}
			break;
			
			// check for SwipeLeft
		case Gestures.SwipeLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				//						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
				//					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
				//					       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
				//						{
				//							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
				//							gestureData.progress = 0.5f;
				//						}
				
				if (jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].x <= gestureRight && jointsPos [rightHandIndex].x > gestureLeft) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.1f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					//							bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					//								Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
					//								Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f && 
					//								(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < -0.15f;
					
					bool isInPose = jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].x < gestureLeft;
					
					if (isInPose) {
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					} else if (jointsPos [rightHandIndex].x <= gestureRight) {
						float gestureSize = gestureRight - gestureLeft;
						gestureData.progress = gestureSize > 0.01f ? (gestureRight - jointsPos [rightHandIndex].x) / gestureSize : 0f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
			// check for SwipeRight
		case Gestures.SwipeRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				//						if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
				//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
				//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0f)
				//						{
				//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
				//							gestureData.progress = 0.5f;
				//						}
				
				if (jointsTracked [leftHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [leftHandIndex].x >= gestureLeft && jointsPos [leftHandIndex].x < gestureRight) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.1f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					//							bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					//								Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
					//								Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
					//								(jointsPos[leftHandIndex].x - gestureData.jointPos.x) > 0.15f;
					
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [leftHandIndex].x > gestureRight;
					
					if (isInPose) {
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					} else if (jointsPos [leftHandIndex].x >= gestureLeft) {
						float gestureSize = gestureRight - gestureLeft;
						gestureData.progress = gestureSize > 0.01f ? (jointsPos [leftHandIndex].x - gestureLeft) / gestureSize : 0f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
			// check for SwipeUp
		case Gestures.SwipeUp:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHandIndex] && jointsTracked [leftElbowIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) < 0.0f &&
				    (jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) > -0.15f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.5f;
				} else if (jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) < 0.0f &&
				           (jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) > -0.15f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = gestureData.joint == rightHandIndex ?
						jointsTracked [rightHandIndex] && jointsTracked [leftShoulderIndex] &&
							(jointsPos [rightHandIndex].y - jointsPos [leftShoulderIndex].y) > 0.05f && 
							Mathf.Abs (jointsPos [rightHandIndex].x - gestureData.jointPos.x) <= 0.1f :
							jointsTracked [leftHandIndex] && jointsTracked [rightShoulderIndex] &&
							(jointsPos [leftHandIndex].y - jointsPos [rightShoulderIndex].y) > 0.05f && 
							Mathf.Abs (jointsPos [leftHandIndex].x - gestureData.jointPos.x) <= 0.1f;
					
					if (isInPose) {
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
			// check for SwipeDown
		case Gestures.SwipeDown:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHandIndex] && jointsTracked [leftShoulderIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [leftShoulderIndex].y) > 0.05f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.5f;
				} else if (jointsTracked [leftHandIndex] && jointsTracked [rightShoulderIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [rightShoulderIndex].y) > 0.05f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = gestureData.joint == rightHandIndex ?
						jointsTracked [rightHandIndex] && jointsTracked [leftElbowIndex] &&
							(jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) < -0.15f && 
							Mathf.Abs (jointsPos [rightHandIndex].x - gestureData.jointPos.x) <= 0.1f :
							jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] &&
							(jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) < -0.15f &&
							Mathf.Abs (jointsPos [leftHandIndex].x - gestureData.jointPos.x) <= 0.1f;
					
					if (isInPose) {
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
			// check for RightHandCursor
		case Gestures.RightHandCursor:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1 (perpetual)
				if (jointsTracked [rightHandIndex] && jointsTracked [rightHipIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [rightHipIndex].y) > -0.1f) {
					gestureData.joint = rightHandIndex;
					gestureData.timestamp = timestamp;
					//gestureData.jointPos = jointsPos[rightHandIndex];
					SetScreenPos (userId, ref gestureData, ref jointsPos, ref jointsTracked);
					gestureData.progress = 0.7f;
				} else {
					// cancel the gesture
					//SetGestureCancelled(ref gestureData);
					gestureData.progress = 0f;
				}
				break;
				
			}
			break;
			
			// check for LeftHandCursor
		case Gestures.LeftHandCursor:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1 (perpetual)
				if (jointsTracked [leftHandIndex] && jointsTracked [leftHipIndex] &&
				    (jointsPos [leftHandIndex].y - jointsPos [leftHipIndex].y) > -0.1f) {
					gestureData.joint = leftHandIndex;
					gestureData.timestamp = timestamp;
					//gestureData.jointPos = jointsPos[leftHandIndex];
					SetScreenPos (userId, ref gestureData, ref jointsPos, ref jointsTracked);
					gestureData.progress = 0.7f;
				} else {
					// cancel the gesture
					//SetGestureCancelled(ref gestureData);
					gestureData.progress = 0f;
				}
				break;
				
			}
			break;
			
			// check for ZoomOut
		case Gestures.ZoomOut:
			Vector3 vectorZoomOut = (Vector3)jointsPos [rightHandIndex] - jointsPos [leftHandIndex];
			float distZoomOut = vectorZoomOut.magnitude;
			
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    distZoomOut < 0.3f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.tagVector = Vector3.right;
					gestureData.tagFloat = 0f;
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture phase 2 = zooming
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angleZoomOut = Vector3.Angle (gestureData.tagVector, vectorZoomOut) * Mathf.Sign (vectorZoomOut.y - gestureData.tagVector.y);
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							distZoomOut < 1.5f && Mathf.Abs (angleZoomOut) < 20f;
					
					if (isInPose) {
						SetZoomFactor (userId, ref gestureData, 1.0f, ref jointsPos, ref jointsTracked);
						gestureData.timestamp = timestamp;
						gestureData.progress = 0.7f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;
			
			// check for ZoomIn
		case Gestures.ZoomIn:
			Vector3 vectorZoomIn = (Vector3)jointsPos [rightHandIndex] - jointsPos [leftHandIndex];
			float distZoomIn = vectorZoomIn.magnitude;
			
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    distZoomIn >= 0.7f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.tagVector = Vector3.right;
					gestureData.tagFloat = distZoomIn;
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture phase 2 = zooming
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angleZoomIn = Vector3.Angle (gestureData.tagVector, vectorZoomIn) * Mathf.Sign (vectorZoomIn.y - gestureData.tagVector.y);
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							distZoomIn >= 0.2f && Mathf.Abs (angleZoomIn) < 20f;
					
					if (isInPose) {
						SetZoomFactor (userId, ref gestureData, 0.0f, ref jointsPos, ref jointsTracked);
						gestureData.timestamp = timestamp;
						gestureData.progress = 0.7f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;
			
			// check for Wheel
		case Gestures.Wheel:
			Vector3 vectorWheel = (Vector3)jointsPos [rightHandIndex] - jointsPos [leftHandIndex];
			float distWheel = vectorWheel.magnitude;
			
			//				Debug.Log(string.Format("{0}. Dist: {1:F1}, Tag: {2:F1}, Diff: {3:F1}", gestureData.state,
			//				                        distWheel, gestureData.tagFloat, Mathf.Abs(distWheel - gestureData.tagFloat)));
			
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    distWheel >= 0.3f && distWheel < 0.7f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.tagVector = Vector3.right;
					gestureData.tagFloat = distWheel;
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture phase 2 = turning wheel
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angle = Vector3.Angle (gestureData.tagVector, vectorWheel) * Mathf.Sign (vectorWheel.y - gestureData.tagVector.y);
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							distWheel >= 0.3f && distWheel < 0.7f && 
							Mathf.Abs (distWheel - gestureData.tagFloat) < 0.1f;
					
					if (isInPose) {
						//DistanceCalibrator.instance.angleText.text = "Wheel " + qtdWheel++;
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
						//SetWheelRotation(userId, ref gestureData, gestureData.tagVector, vectorWheel);
						gestureData.screenPos.z = angle;  // wheel angle
						gestureData.timestamp = timestamp;
						gestureData.tagFloat = distWheel;
						gestureData.progress = 0.7f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;

	
			// check for Wheel
		case Gestures.WheelFrontal:
			Vector3 vectorWheelFrontal = (Vector3)jointsPos [rightHandIndex] - jointsPos [leftHandIndex];
			float distWheelFrontal = vectorWheelFrontal.magnitude;
			
			//				Debug.Log(string.Format("{0}. Dist: {1:F1}, Tag: {2:F1}, Diff: {3:F1}", gestureData.state,
			//				                        distWheel, gestureData.tagFloat, Mathf.Abs(distWheel - gestureData.tagFloat)));
			
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    distWheelFrontal >= 0.3f && distWheelFrontal < 0.7f) {

					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.tagVector = Vector3.forward;
					gestureData.tagFloat = distWheelFrontal;
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture phase 2 = turning wheel
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angle = Vector3.Angle (gestureData.tagVector, vectorWheelFrontal) * Mathf.Sign (vectorWheelFrontal.z - gestureData.tagVector.z);
				
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							distWheelFrontal >= 0.3f && distWheelFrontal < 0.7f && 
							Mathf.Abs (distWheelFrontal - gestureData.tagFloat) < 0.15f;
					
					if (isInPose) {
						//DistanceCalibrator.instance.angleText.text = "WheelFrontal " + qtdWheelFrontal++;
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
						//SetWheelRotation(userId, ref gestureData, gestureData.tagVector, vectorWheel);
						gestureData.screenPos.z = angle;  // wheel angle
						gestureData.timestamp = timestamp;
						gestureData.tagFloat = distWheelFrontal;
						gestureData.progress = 0.7f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;


			// check for Jump
		case Gestures.Jump:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [hipCenterIndex] && 
				    (jointsPos [hipCenterIndex].y > 0.9f) && (jointsPos [hipCenterIndex].y < 1.3f)) {
					SetGestureJoint (ref gestureData, timestamp, hipCenterIndex, jointsPos [hipCenterIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [hipCenterIndex] &&
						(jointsPos [hipCenterIndex].y - gestureData.jointPos.y) > 0.15f && 
							Mathf.Abs (jointsPos [hipCenterIndex].x - gestureData.jointPos.x) < 0.2f;/* &&
							jointsPos [rightAnkleIndex].y  > 0.40f;*/
					
					if (isInPose) {
						Debug.Log("jump");
						SetInitialPos(gestureData.jointPos, hipCenterIndex);
						SetFinalPos(jointsPos[hipCenterIndex], hipCenterIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
		

			// check for Squat
		case Gestures.Squat:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightAnkleIndex] && jointsTracked [rightKneeIndex] && jointsTracked [rightHipIndex] 
				    && jointsTracked [leftAnkleIndex] && jointsTracked [leftKneeIndex] && jointsTracked [leftHipIndex] && 
				    Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightHipIndex, rightKneeIndex, rightAnkleIndex)) < 15 &&
				    Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftHipIndex, leftKneeIndex, leftAnkleIndex)) < 15) {
					SetGestureJoint (ref gestureData, timestamp, rightAnkleIndex, jointsPos [rightAnkleIndex]);
					gestureData.progress = 0.5f;
				}
				break;				
				
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [rightAnkleIndex] && jointsTracked [rightKneeIndex] && jointsTracked [rightHipIndex] 
					&& jointsTracked [leftAnkleIndex] && jointsTracked [leftKneeIndex] && jointsTracked [leftHipIndex] && 
						Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightHipIndex, rightKneeIndex, rightAnkleIndex)) > 20 &&
							Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftHipIndex, leftKneeIndex, leftAnkleIndex)) > 20;
							//&& jointsPos [rightAnkleIndex].y  < 0.2f && jointsPos [leftAnkleIndex].y  < 0.1f;
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightHipIndex);
						SetFinalPos(jointsPos[rightHipIndex], rightHipIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;
			/*switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [hipCenterIndex] && 
				    (jointsPos [hipCenterIndex].y <= 0.9f)) {
					SetGestureJoint (ref gestureData, timestamp, hipCenterIndex, jointsPos [hipCenterIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [hipCenterIndex] &&
						(jointsPos [hipCenterIndex].y - gestureData.jointPos.y) < -0.15f && 
							Mathf.Abs (jointsPos [hipCenterIndex].x - gestureData.jointPos.x) < 0.2f;
					
					if (isInPose) {
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);

						SetInitialPos(gestureData.jointPos, hipCenterIndex);
						SetFinalPos(jointsPos[hipCenterIndex], hipCenterIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;*/
			
			// check for Push
		case Gestures.Push:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHandIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightShoulderIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) > -0.1f &&
				    Mathf.Abs (jointsPos [rightHandIndex].x - jointsPos [rightShoulderIndex].x) < 0.2f &&
				    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.2f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.5f;
				} else if (jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftShoulderIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) > -0.1f &&
				           Mathf.Abs (jointsPos [leftHandIndex].x - jointsPos [leftShoulderIndex].x) < 0.2f &&
				           (jointsPos [leftHandIndex].z - jointsPos [rightElbowIndex].z) < -0.2f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = gestureData.joint == rightHandIndex ?
						jointsTracked [rightHandIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightShoulderIndex] &&
							(jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) > -0.1f &&
							Mathf.Abs (jointsPos [rightHandIndex].x - gestureData.jointPos.x) < 0.2f &&
							(jointsPos [rightHandIndex].z - gestureData.jointPos.z) < -0.1f :
							jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftShoulderIndex] &&
							(jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) > -0.1f &&
							Mathf.Abs (jointsPos [leftHandIndex].x - gestureData.jointPos.x) < 0.2f &&
							(jointsPos [leftHandIndex].z - gestureData.jointPos.z) < -0.1f;
					
					if (isInPose) {
						if(gestureData.joint == rightHandIndex){
							SetInitialPos(gestureData.jointPos, rightHandIndex);
							SetFinalPos(jointsPos[rightHandIndex], rightHandIndex);
							SetGestureTime(timestamp - gestureData.timestamp);
						}
						if(gestureData.joint == leftHandIndex){
							SetInitialPos(gestureData.jointPos, leftHandIndex);
							SetFinalPos(jointsPos[leftHandIndex], leftHandIndex);
							SetGestureTime(timestamp - gestureData.timestamp);
						}

						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
						//enviar a posiçao inicial e final das juntas
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
			// check for Pull
		case Gestures.Pull:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHandIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightShoulderIndex] &&
				    (jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) > -0.1f &&
				    //maos proximas
				    Mathf.Abs (jointsPos [rightHandIndex].x - jointsPos [rightShoulderIndex].x) < 0.2f &&
				    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.3f)
				{
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.progress = 0.5f;
				} else if (jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftShoulderIndex] &&
				           (jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) > -0.1f &&
				           Mathf.Abs (jointsPos [leftHandIndex].x - jointsPos [leftShoulderIndex].x) < 0.2f &&
				           (jointsPos [leftHandIndex].z - jointsPos [rightElbowIndex].z) < -0.3f) {
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = gestureData.joint == rightHandIndex ?
						jointsTracked [rightHandIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightShoulderIndex] &&
							(jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) > -0.1f &&
							Mathf.Abs (jointsPos [rightHandIndex].x - gestureData.jointPos.x) < 0.2f &&
							(jointsPos [rightHandIndex].z - gestureData.jointPos.z) > 0.1f :
							jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftShoulderIndex] &&
							(jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) > -0.1f &&
							Mathf.Abs (jointsPos [leftHandIndex].x - gestureData.jointPos.x) < 0.2f &&
							(jointsPos [leftHandIndex].z - gestureData.jointPos.z) > 0.1f;
					
					if (isInPose) {

						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
	
			/// 
		case Gestures.ThrowLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftElbowIndex] && jointsTracked [leftShoulderIndex] &&		
				    Mathf.Abs (jointsPos [leftHandIndex].x - jointsPos [leftShoulderIndex].x) < 0.2f &&
				    (jointsPos [leftHandIndex].z - jointsPos [rightElbowIndex].z) < -0.2f && 
				    Mathf.Abs (jointsPos [leftHandIndex].y - jointsPos [leftElbowIndex].y) > 0.15f)
				{
					SetGestureJoint (ref gestureData, timestamp, leftHandIndex, jointsPos [leftHandIndex]);
					gestureData.progress = 0.5f;
				} 
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftShoulderIndex] &&
						(jointsPos [leftHandIndex].y - jointsPos [rightElbowIndex].y) > -0.1f &&
							Mathf.Abs (jointsPos [leftHandIndex].x - gestureData.jointPos.x) < 0.2f &&
							(jointsPos [leftHandIndex].z - gestureData.jointPos.z) < -0.15f;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, leftHandIndex);
						SetFinalPos(jointsPos[leftHandIndex], leftHandIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
		
			//==========throwright e throwleft sao para o fishing================
		case Gestures.ThrowRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightElbowIndex] && jointsTracked [rightShoulderIndex] &&		
				    Mathf.Abs (jointsPos [rightWristIndex].x - jointsPos [rightShoulderIndex].x) < 0.25f &&
				    jointsPos[rightWristIndex].y <= gestureTop && jointsPos[rightWristIndex].y >= gestureBottom &&
				    //    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.2f && 
				    Mathf.Abs (jointsPos [rightWristIndex].y - jointsPos [rightElbowIndex].y) > 0.15f)
				{
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				} 
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f)  
				{
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightShoulderIndex]
					//	(jointsPos [rightHandIndex].y - jointsPos [leftElbowIndex].y) > -0.1f &&
					&& Mathf.Abs (jointsPos [rightWristIndex].x - gestureData.jointPos.x) < 0.2f
					&& (jointsPos [rightWristIndex].z - gestureData.jointPos.z) < -0.2f;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
		
			//throw diferentao para o throw arremeçar com 
		case Gestures.ThrowRightDiferentao:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightWristIndex] && jointsTracked [rightHipIndex] && jointsTracked [rightShoulderIndex] &&		
				    Mathf.Abs (jointsPos [rightWristIndex].x - jointsPos [rightShoulderIndex].x) < 0.15f &&
				    //    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.2f && 
				    Mathf.Abs (jointsPos [rightWristIndex].y - jointsPos [rightElbowIndex].y) < 0.15f &&
				    Mathf.Abs (jointsPos [rightWristIndex].z - jointsPos [rightShoulderIndex].z) < 0.2f &&
				    Mathf.Abs (jointsPos[rightHipIndex].x - jointsPos[rightWristIndex].x) > 0.15f &&
					jointsPos[rightElbowIndex].z > jointsPos[rightWristIndex].z )

				{
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				} 
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [rightHipIndex] && jointsTracked [rightShoulderIndex] &&
						Mathf.Abs (jointsPos [rightWristIndex].x - jointsPos [rightShoulderIndex].x) < 0.4f &&
					//    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.2f && 
							Mathf.Abs (jointsPos [rightWristIndex].y - jointsPos [rightElbowIndex].y) < 0.25f &&
							Mathf.Abs (jointsPos [rightWristIndex].z - jointsPos [rightShoulderIndex].z) < 0.25f &&
							Mathf.Abs (jointsPos[rightHipIndex].x - jointsPos[rightWristIndex].x) > 0.15f &&
							jointsPos[rightElbowIndex].z < jointsPos[rightWristIndex].z ;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;

		case Gestures.ThrowLeftDiferentao:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftWristIndex] && jointsTracked [leftHipIndex] && jointsTracked [leftShoulderIndex] &&		
				    Mathf.Abs (jointsPos [leftWristIndex].x - jointsPos [leftShoulderIndex].x) < 0.15f &&
				    //    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.2f && 
				    Mathf.Abs (jointsPos [leftWristIndex].y - jointsPos [leftElbowIndex].y) < 0.15f &&
				    Mathf.Abs (jointsPos [leftWristIndex].z - jointsPos [leftShoulderIndex].z) < 0.2f &&
				    Mathf.Abs (jointsPos[leftHipIndex].x - jointsPos[leftWristIndex].x) > 0.15f &&
				    jointsPos[leftElbowIndex].z > jointsPos[leftWristIndex].z )
					
				{
					SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
					gestureData.progress = 0.5f;
				} 
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [leftWristIndex] && jointsTracked [leftHipIndex] && jointsTracked [leftShoulderIndex] &&
					Mathf.Abs (jointsPos [leftWristIndex].x - jointsPos [leftShoulderIndex].x) < 0.4f &&
					//    (jointsPos [rightHandIndex].z - jointsPos [leftElbowIndex].z) < -0.2f && 
					Mathf.Abs (jointsPos [leftWristIndex].y - jointsPos [leftElbowIndex].y) < 0.25f &&
					Mathf.Abs (jointsPos [leftWristIndex].z - jointsPos [leftShoulderIndex].z) < 0.25f &&
					Mathf.Abs (jointsPos[leftHipIndex].x - jointsPos[leftWristIndex].x) > 0.15f &&
					jointsPos[leftElbowIndex].z < jointsPos[leftWristIndex].z ;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[leftWristIndex], leftWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			//=============================================

		case Gestures.MoveLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHipIndex]) 	

				{
					SetGestureJoint (ref gestureData, timestamp, leftHipIndex, jointsPos [leftHipIndex]);
					gestureData.progress = 0.5f;
				} 
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) 
				{
					bool isInPose = jointsTracked [leftHipIndex] &&
					//se moveu 0.2 para esquerda
					(jointsPos [leftHipIndex].x - gestureData.jointPos.x) < -0.2f
					//se nao teve variaçao maior que 0.2 em Z
					&& Mathf.Abs(jointsPos [leftHipIndex].z - gestureData.jointPos.z) < 0.2f;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, leftHipIndex);
						MovPositionController.instance.StartFinalPosition(leftHipIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
			
		case Gestures.MoveRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightHipIndex]) 	
					
				{
					SetGestureJoint (ref gestureData, timestamp, rightHipIndex, jointsPos [rightHipIndex]);
					gestureData.progress = 0.5f;
				} 
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [rightHipIndex] &&
						 (jointsPos [rightHipIndex].x - gestureData.jointPos.x) > 0.2f &&
							Mathf.Abs(jointsPos [rightHipIndex].z - gestureData.jointPos.z) < 0.2f;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightHipIndex);
						MovPositionController.instance.StartFinalPosition(rightHipIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;


		case Gestures.RaiseRightKnee:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [rightKneeIndex] && GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex,rightHipIndex,rightKneeIndex) <10) {
					SetGestureJoint (ref gestureData, timestamp, rightKneeIndex, jointsPos [rightKneeIndex]);
					gestureData.progress = 0.5f;
				}
				break;				
			

			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angleDiference = Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftShoulderIndex,leftHipIndex,leftKneeIndex)
					                                 -  (float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex,rightHipIndex,rightKneeIndex));
					//float inclination = Mathf.Abs(jointsPos [hipCenterIndex].z - jointsPos [shoulderCenterIndex].z); 
					bool isInPose = jointsTracked [rightKneeIndex] && GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex,rightHipIndex,rightKneeIndex) >30 &&
						Mathf.Abs(jointsPos [rightKneeIndex].y - gestureData.jointPos.y) > 0.1f && angleDiference > 18;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightKneeIndex);
						SetFinalPos(jointsPos[rightKneeIndex], rightKneeIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;

			}
			break;


		case Gestures.RaiseLeftKnee:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftKneeIndex] && GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftShoulderIndex, leftHipIndex, leftKneeIndex) <10) {
					SetGestureJoint (ref gestureData, timestamp, leftKneeIndex, jointsPos [leftKneeIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angleDiference = Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftShoulderIndex,leftHipIndex,leftKneeIndex)
					                                 -  (float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex,rightHipIndex,rightKneeIndex));
//					float inclination = Mathf.Abs(jointsPos [hipCenterIndex].z - jointsPos [shoulderCenterIndex].z); 
					bool isInPose = jointsTracked [leftKneeIndex] && GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftShoulderIndex,leftHipIndex,leftKneeIndex) >30 &&
						Mathf.Abs(jointsPos [leftKneeIndex].y - gestureData.jointPos.y) > 0.1f && angleDiference > 18;

					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, leftKneeIndex);
						SetFinalPos(jointsPos[leftKneeIndex], leftKneeIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;

		case Gestures.ThrowBothHandsFromLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if (jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex] && jointsTracked [rightShoulderIndex] && jointsTracked [hipCenterIndex] &&
				    //se as maos estao juntas
				    Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) <0.25f &&
				    //se estao na altura correta
				    jointsPos [leftWristIndex].y >= gestureBottom && jointsPos [leftWristIndex].y <= gestureTop &&
				    //se a maos estao do lado esquerdo do corpo
				    jointsPos [rightWristIndex].x < jointsPos [hipCenterIndex].x &&
				    jointsPos [leftWristIndex].x < jointsPos [hipCenterIndex].x) {
					SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex] && jointsTracked [rightShoulderIndex] && jointsTracked [hipCenterIndex] &&
						//se estao na altura correta
						jointsPos [leftWristIndex].y >= gestureBottom && jointsPos [leftWristIndex].y <= gestureTop && 
							//se a mao direita se afastou do corpo
							Mathf.Abs(jointsPos [leftWristIndex].z - gestureData.jointPos.z) > 0.15f &&
							//se a mao direita esta abaixo da posicao inicial
							jointsPos [leftWristIndex].y < gestureData.jointPos.y;
					
					if (isInPose) {
					//	DistanceCalibrator.instance.angleText.text = "isInPose! Distance Z: " + Mathf.Abs(jointsPos [leftWristIndex].z - gestureData.jointPos.z).ToString() ;
						SetInitialPos(gestureData.jointPos, leftWristIndex);
						SetFinalPos(jointsPos[leftWristIndex], leftWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
				
		case Gestures.ThrowBothHandsFromRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
			//	DistanceCalibrator.instance.angleText.text = "[ jointsPos [rightHandIndex].x - jointsPos [hipCenterIndex].x " + (jointsPos [leftWristIndex].x - jointsPos [hipCenterIndex].x).ToString();
				if (jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex] && jointsTracked [rightShoulderIndex] && jointsTracked [hipCenterIndex] &&
				    //se as maos estao juntas
				    Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) <0.25f &&
				    //se estao na altura correta
				    jointsPos [rightWristIndex].y >= gestureBottom && jointsPos [rightWristIndex].y <= gestureTop &&
				    //se a maos estao do lado direito do corpo
				    jointsPos [rightWristIndex].x > jointsPos [hipCenterIndex].x &&
				    jointsPos [leftWristIndex].x > jointsPos [hipCenterIndex].x) {
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex] && jointsTracked [rightShoulderIndex] && jointsTracked [hipCenterIndex] &&
						//se estao na altura correta
						jointsPos [rightWristIndex].y >= gestureBottom && jointsPos [rightWristIndex].y <= gestureTop && 
						//se a mao direita se afastou do corpo
						Mathf.Abs(jointsPos [rightWristIndex].z - gestureData.jointPos.z) > 0.15f &&
						//se a mao direita esta abaixo da posicao inicial
						jointsPos [rightWristIndex].y < gestureData.jointPos.y;
					
					if (isInPose) {
					//	DistanceCalibrator.instance.angleText.text = "isInPose! Distance Z: " + Mathf.Abs(jointsPos [rightWristIndex].z - gestureData.jointPos.z).ToString() ;
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;					
			}
			break;	

		case Gestures.ThrowOneHandRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if (jointsTracked [rightWristIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftWristIndex] && jointsTracked [hipCenterIndex] &&
				    //se estao na altura correta
				    jointsPos [rightWristIndex].y >= gestureBottom && jointsPos [rightWristIndex].y <= gestureTop &&
				    ///se as maos estiverem separadas
				    Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) > 0.45f &&
				    //se a mao estiver acima do cotovelo
				    jointsPos [rightWristIndex].y  > jointsPos [rightElbowIndex].y &&
				    //se a mao esta alinhada com o cotovelo
				    Mathf.Abs(jointsPos [rightWristIndex].x -jointsPos [rightElbowIndex].x) < 0.10f &&
				    //se a maos estao do lado direito do corpo
				    jointsPos [rightWristIndex].x > jointsPos [hipCenterIndex].x) {
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [rightElbowIndex] && jointsTracked [rightShoulderIndex] && jointsTracked [hipCenterIndex] &&
						//se estao na altura correta
						jointsPos [rightWristIndex].y >= gestureBottom && jointsPos [rightWristIndex].y <= gestureTop && 
						//se a mao direita se afastou do corpo
						Mathf.Abs(jointsPos [rightWristIndex].z - gestureData.jointPos.z) > 0.12f &&
						//	jointsPos [rightWristIndex].z - jointsPos[rightElbowIndex].z > 0.15f &&
						//se a mao direita esta abaixo da posicao inicial
						jointsPos [rightWristIndex].y < gestureData.jointPos.y;
					
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;					
			}
			break;	

		case Gestures.ThrowOneHandLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
			//	DistanceCalibrator.instance.angleText.text = "alinhamento mao cotovelo " +  Mathf.Abs(jointsPos [rightWristIndex].x -jointsPos [rightElbowIndex].x).ToString();
				if (jointsTracked [leftWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [leftShoulderIndex] && jointsTracked [hipCenterIndex] &&
				    //se estao na altura correta
				    jointsPos [leftWristIndex].y >= gestureBottom && jointsPos [leftWristIndex].y <= gestureTop &&
				    //se a mao estiver acima do cotovelo
				    jointsPos [leftWristIndex].y  > jointsPos [leftElbowIndex].y &&
				    //se as maos estiverem separadas
				    Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) > 0.45f &&
				    //se a mao esta alinhada com o cotovelo
				    Mathf.Abs(jointsPos [leftWristIndex].x - jointsPos [leftElbowIndex].x) < 0.10f &&
				    //se a maos estao do lado direito do corpo
				    jointsPos [leftWristIndex].x < jointsPos [hipCenterIndex].x) {
					SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [leftWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [leftShoulderIndex] && jointsTracked [hipCenterIndex] &&
						//se estao na altura correta
						jointsPos [leftWristIndex].y >= gestureBottom && jointsPos [leftWristIndex].y <= gestureTop && 
							//se a mao direita se afastou do corpo
							Mathf.Abs(jointsPos [leftWristIndex].z - gestureData.jointPos.z) > 0.12f &&
							//	jointsPos [rightWristIndex].z - jointsPos[rightElbowIndex].z > 0.15f &&
							//se a mao direita esta abaixo da posicao inicial
							jointsPos [leftWristIndex].y < gestureData.jointPos.y;
					
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, leftWristIndex);
						SetFinalPos(jointsPos[leftWristIndex], leftWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;	
			
		case Gestures.WheelFrontalLeftHand:		
			Vector3 vectorWheelLeft = (Vector3)jointsPos [rightHandIndex] - jointsPos [leftHandIndex];
			float distWheelLeft = vectorWheelLeft.magnitude;
			
			//				Debug.Log(string.Format("{0}. Dist: {1:F1}, Tag: {2:F1}, Diff: {3:F1}", gestureData.state,
			//				                        distWheel, gestureData.tagFloat, Mathf.Abs(distWheel - gestureData.tagFloat)));
			
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    distWheelLeft >= 0.3f && distWheelLeft < 0.7f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.tagVector = Vector3.right;
					gestureData.tagFloat = distWheelLeft;
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture phase 2 = turning wheel
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angle = Vector3.Angle (gestureData.tagVector, vectorWheelLeft) * Mathf.Sign (vectorWheelLeft.y - gestureData.tagVector.y);
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							distWheelLeft >= 0.3f && distWheelLeft < 0.7f && 
							Mathf.Abs (distWheelLeft - gestureData.tagFloat) < 0.1f;
					
					if (isInPose) {
						//SetWheelRotation(userId, ref gestureData, gestureData.tagVector, vectorWheelLeft);
						gestureData.screenPos.z = angle;  // wheel angle
						gestureData.timestamp = timestamp;
						gestureData.tagFloat = distWheelLeft;
						gestureData.progress = 0.7f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;

			
		case Gestures.WheelFrontalRightHand:
			Vector3 vectorWheelRight = (Vector3)jointsPos [rightHandIndex] - jointsPos [leftHandIndex];
			float distWheelRight = vectorWheelRight.magnitude;
			
			//				Debug.Log(string.Format("{0}. Dist: {1:F1}, Tag: {2:F1}, Diff: {3:F1}", gestureData.state,
			//				                        distWheel, gestureData.tagFloat, Mathf.Abs(distWheel - gestureData.tagFloat)));
			
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if (jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
				    jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
				    jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
				    distWheelRight >= 0.3f && distWheelRight < 0.7f) {
					SetGestureJoint (ref gestureData, timestamp, rightHandIndex, jointsPos [rightHandIndex]);
					gestureData.tagVector = Vector3.right;
					gestureData.tagFloat = distWheelRight;
					gestureData.progress = 0.3f;
				}
				break;
				
			case 1:  // gesture phase 2 = turning wheel
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					float angle = Vector3.Angle (gestureData.tagVector, vectorWheelRight) * Mathf.Sign (vectorWheelRight.y - gestureData.tagVector.y);
					bool isInPose = jointsTracked [leftHandIndex] && jointsTracked [rightHandIndex] && jointsTracked [hipCenterIndex] && jointsTracked [shoulderCenterIndex] && jointsTracked [leftHipIndex] && jointsTracked [rightHipIndex] &&
						jointsPos [leftHandIndex].y >= gestureBottom && jointsPos [leftHandIndex].y <= gestureTop &&
							jointsPos [rightHandIndex].y >= gestureBottom && jointsPos [rightHandIndex].y <= gestureTop &&
							distWheelRight >= 0.3f && distWheelRight < 0.7f && 
							Mathf.Abs (distWheelRight - gestureData.tagFloat) < 0.1f;
					
					if (isInPose) {
						//SetWheelRotation(userId, ref gestureData, gestureData.tagVector, vectorWheelLeft);
						gestureData.screenPos.z = angle;  // wheel angle
						gestureData.timestamp = timestamp;
						gestureData.tagFloat = distWheelRight;
						gestureData.progress = 0.7f;
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
				
			}
			break;

		case Gestures.PullLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				//	DistanceCalibrator.instance.angleText.text = "alinhamento mao cotovelo " +  Mathf.Abs(jointsPos [rightWristIndex].x -jointsPos [rightElbowIndex].x).ToString();
				if (jointsTracked [leftWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightWristIndex] && jointsTracked [leftShoulderIndex] && jointsTracked [hipCenterIndex] &&
				    //se estao na altura correta
				    jointsPos [leftWristIndex].y >= gestureBottom && jointsPos [leftWristIndex].y <= gestureTop &&
				    //se a mao estiver na mesma altura do cotovelo cotovelo
				    Mathf.Abs(jointsPos [leftWristIndex].y  - jointsPos [leftElbowIndex].y) < 0.1f &&
				    //se a mao estiver afastada para frente
				    jointsPos [leftWristIndex].z  - jointsPos [leftElbowIndex].z < 0.2f &&
				    //se as maos estiverem separadas
				    Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) > 0.45f &&
				    //se a mao esta alinhada com o cotovelo
				    Mathf.Abs(jointsPos [leftWristIndex].x - jointsPos [leftElbowIndex].x) < 0.10f &&
				    //se a maos estao do lado esquerdo do corpo
				    jointsPos [leftWristIndex].x < jointsPos [hipCenterIndex].x) {
					SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [leftWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [leftShoulderIndex] && jointsTracked [hipCenterIndex] &&
						//se estao na altura correta
						jointsPos [leftWristIndex].y >= gestureBottom && jointsPos [leftWristIndex].y <= gestureTop && 
							//se a mao esquerda se aproximou do corpo
							Mathf.Abs(jointsPos [leftWristIndex].z - gestureData.jointPos.z) > 0.12f &&
						
							//se a mao esquerda esta acima da posicao inicial
							jointsPos [leftWristIndex].y > gestureData.jointPos.y;
					
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, leftWristIndex);
						SetFinalPos(jointsPos[leftWristIndex], leftWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;	

		case Gestures.PullRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				//	DistanceCalibrator.instance.angleText.text = "alinhamento mao cotovelo " +  Mathf.Abs(jointsPos [rightWristIndex].x -jointsPos [rightElbowIndex].x).ToString();
				if (jointsTracked [rightWristIndex] && jointsTracked [rightElbowIndex] && jointsTracked [leftWristIndex] && 
				    jointsTracked [rightShoulderIndex] && jointsTracked [hipCenterIndex] &&
				    //se estao na altura correta
				    jointsPos [rightWristIndex].y >= gestureBottom && jointsPos [rightWristIndex].y <= gestureTop &&
				    //se a mao estiver na mesma altura do cotovelo cotovelo
				    Mathf.Abs(jointsPos [rightWristIndex].y - jointsPos [rightElbowIndex].y) < 0.1f &&
				    //se a mao estiver afastada para frente
				    jointsPos [rightWristIndex].z  - jointsPos [rightElbowIndex].z < 0.2f &&
				    //se as maos estiverem separadas
				    Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) > 0.45f &&
				    //se a mao esta alinhada com o cotovelo
				    Mathf.Abs(jointsPos [rightWristIndex].x - jointsPos [rightWristIndex].x) < 0.10f &&
				    //se a maos estao do lado direito do corpo
				    jointsPos [rightWristIndex].x > jointsPos [hipCenterIndex].x) {
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [leftWristIndex] && jointsTracked [leftElbowIndex] && jointsTracked [rightWristIndex] 
					&& jointsTracked [leftShoulderIndex] && jointsTracked [hipCenterIndex] &&
					//se estao na altura correta
					jointsPos [rightWristIndex].y >= gestureBottom && jointsPos [rightWristIndex].y <= gestureTop && 
					//se a mao direita se aproximou do corpo
					Mathf.Abs(jointsPos [rightWristIndex].z - gestureData.jointPos.z) > 0.12f &&
					
					//se a mao esquerda esta acima da posicao inicial
					jointsPos [rightWristIndex].y > gestureData.jointPos.y;
				
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;	

			//fishing sup? parece remada pra esquerda
		case Gestures.RowLeft:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if(
					jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[leftHipIndex] 
					//&& jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]
					//se pulso esquerdo acima que cotovelo esquerdo e pulso direito maior que cotovelo direito
					//&& (jointsPos[leftWristIndex].y > jointsPos[leftElbowIndex].y) && (jointsPos[rightWristIndex].y > jointsPos[rightElbowIndex].y)
					//se pulso esquerdo esta abaixo que pulso direito
					&& (jointsPos[leftWristIndex].y < jointsPos[rightWristIndex].y) 
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.5f)
					//se as maos estao depois do lado esquerdo da cintura
					&& (jointsPos [rightWristIndex].x < jointsPos[leftHipIndex].x)
					&& (jointsPos [leftWristIndex].x < jointsPos[leftHipIndex].x)
					)
					
				{
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				
				//falta bloquear outros movimentos enquanto estiver com o bait marker rolando
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex] && jointsTracked [hipCenterIndex]
					//&& jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]
					//se pulso esquerdo maior que cotovelo esquerdo e pulso direito maior que cotovelo direito
					//&& (jointsPos[leftWristIndex].y > jointsPos[leftElbowIndex].y) && (jointsPos[rightWristIndex].y > jointsPos[rightElbowIndex].y)
					//se pulso esquerdo esta acima que pulso direito
					&& (jointsPos[leftWristIndex].y > jointsPos[rightWristIndex].y) 
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.5f)
					//se as maos estao depois do lado direito da cintura
					&& (jointsPos [rightWristIndex].x > jointsPos[rightHipIndex].x)
					&& (jointsPos [leftWristIndex].x > jointsPos[rightHipIndex].x);
					
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
						Debug.Log("fisgando esquerda");
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;
			//fishing
		case Gestures.RowMid:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if(
					jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[hipCenterIndex] 
					//&& jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]
					//se pulso esquerdo acima que cotovelo esquerdo e pulso direito maior que cotovelo direito
					//&& (jointsPos[leftWristIndex].y > jointsPos[leftElbowIndex].y) && (jointsPos[rightWristIndex].y > jointsPos[rightElbowIndex].y)
					//se pulso esquerdo esta abaixo que pulso direito
					&& (jointsPos[leftWristIndex].y < jointsPos[rightWristIndex].y) 
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.5f)
					//se as maos estao proximas do meio
					&&(Vector3.Distance(jointsPos [rightWristIndex], jointsPos[hipCenterIndex]) < 1.0f)
					&&(Vector3.Distance(jointsPos [leftWristIndex], jointsPos[hipCenterIndex]) < 1.0f)
					)
					
				{
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				
				//falta bloquear outros movimentos enquanto estiver com o bait marker rolando
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex]
					&& jointsTracked [hipCenterIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]
					//se pulso esquerdo maior que cotovelo esquerdo e pulso direito maior que cotovelo direito
					//&& (jointsPos[leftWristIndex].y > jointsPos[leftElbowIndex].y) && (jointsPos[rightWristIndex].y > jointsPos[rightElbowIndex].y)
					//se pulso esquerdo esta acima que pulso direito
					&& (jointsPos[leftWristIndex].y > jointsPos[rightWristIndex].y) 
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.5f)
					//se as maos estao proximas do meio
					&& Vector3.Distance(jointsPos [rightWristIndex], jointsPos[hipCenterIndex]) < 1.0f
					&& Vector3.Distance(jointsPos [leftWristIndex], jointsPos[hipCenterIndex]) < 1.0f;
				
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
						//Debug.Log("fisgando meio");
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;	

			//fishing
		case Gestures.RowRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if(
					jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] &&
					jointsTracked[rightHipIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]
					//se pulso esquerdo acima que cotovelo esquerdo e pulso direito maior que cotovelo direito
					//&& (jointsPos[leftWristIndex].y > jointsPos[leftElbowIndex].y) && (jointsPos[rightWristIndex].y > jointsPos[rightElbowIndex].y)
					//se pulso esquerdo esta abaixo que pulso direito
					&& (jointsPos[leftWristIndex].y < jointsPos[rightWristIndex].y) 
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.5f)
					//se as maos estao depois do lado direito da cintura
					&& (jointsPos [rightWristIndex].x > jointsPos[rightHipIndex].x)
					&& (jointsPos [leftWristIndex].x > jointsPos[rightHipIndex].x)
					)
					
				{
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				
				//falta bloquear outros movimentos enquanto estiver com o bait marker rolando
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked [rightWristIndex] && jointsTracked [leftWristIndex]
					&& jointsTracked [hipCenterIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightElbowIndex]
					//se pulso esquerdo maior que cotovelo esquerdo e pulso direito maior que cotovelo direito
					//&& (jointsPos[leftWristIndex].y > jointsPos[leftElbowIndex].y) && (jointsPos[rightWristIndex].y > jointsPos[rightElbowIndex].y)
					//se pulso esquerdo esta acima que pulso direito
					&& (jointsPos[leftWristIndex].y > jointsPos[rightWristIndex].y) 
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.5f)
					//se as maos estao depois do lado direito da cintura
					&& (jointsPos [rightWristIndex].x > jointsPos[rightHipIndex].x)
					&& (jointsPos [leftWristIndex].x > jointsPos[rightHipIndex].x);
					
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;

				//remar no sup
			case Gestures.RemarLeft:
				switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if(
					jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[leftHipIndex]
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.4f)
					//se altura do pulso esquerdo menor que pulso direito
					&& ((jointsPos[rightWristIndex].y  - jointsPos[leftWristIndex].y) < 0.3f)
					//se mao direita.x > que mao esquerda.x
					&& (jointsPos[rightWristIndex].x > jointsPos[leftWristIndex].x)
					)
					
				{
					//Debug.Log("iniciando remada esquerda");
					SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[leftHipIndex]
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.3f)
					&&(Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) > 0.1f)
					//se maoE e hipE estiverem proximos
					&& (Vector3.Distance(jointsPos [leftWristIndex], jointsPos [leftHipIndex]) < 0.2f)
					//se maoE e hipD estiverem proximos
					//&& (Vector3.Distance(jointsPos [leftWristIndex], jointsPos [rightHipIndex]) < 0.1f)
					//se maoE.x > hipE.x
					&& (jointsPos [leftWristIndex].x < jointsPos [leftHipIndex].x)
					//se maoE.x > hipD.x
					//&& (jointsPos [leftWristIndex].x > jointsPos [rightHipIndex].x)
					;
					
					if (isInPose) {	
						Debug.Log("remou esquerda");
						SetInitialPos(gestureData.jointPos, leftWristIndex);
						SetFinalPos(jointsPos[leftWristIndex], leftWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;

		case Gestures.RemarRight:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				if(
					jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[rightHipIndex]
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.4f)
					//se altura do pulso esquerdo maior que pulso direito
					&& ((jointsPos[leftWristIndex].y  - jointsPos[rightWristIndex].y) < 0.3f)
					//se mao direita.x > que mao esquerda.x
					&& (jointsPos[rightWristIndex].x > jointsPos[leftWristIndex].x)
					)
				
				{
					//Debug.Log("iniciando remada direita");
					SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {			
					bool isInPose = jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[rightHipIndex]
					//se as maos estiverem proximas
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) < 0.3f)
						&&(Vector3.Distance(jointsPos [rightWristIndex], jointsPos [leftWristIndex]) > 0.1f)
					//se maoD e hipD estiverem proximos
					&& (Vector3.Distance(jointsPos [rightWristIndex], jointsPos [rightHipIndex]) < 0.2f)
					//se maoE e hipD estiverem proximos
					//&& (Vector3.Distance(jointsPos [leftWristIndex], jointsPos [rightHipIndex]) < 0.1f)
					//se maoD.x > hipD.x
					&& (jointsPos [rightWristIndex].x > jointsPos [rightHipIndex].x)
					//se maoE.x > hipD.x
					//&& (jointsPos [leftWristIndex].x > jointsPos [rightHipIndex].x)
					;
					
					if (isInPose) {	
						//Debug.Log("remou direita");
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;				
			}
			break;	


			//esticar o braço para pausar o jogo
		case Gestures.RightHandStop:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 
				//float angle = Vector3.Angle(jointsPos[rightElbowIndex].x, jointsPos[rightShoulderIndex].x) * Mathf.Sign(jointsPos[rightShoulderIndex].x - jointsPos[rightElbowIndex].x);
				if(jointsTracked[rightWristIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]
				    && jointsTracked[hipCenterIndex] && jointsTracked[rightHipIndex] && jointsTracked[leftElbowIndex]
				   && jointsTracked[leftWristIndex]
					//cotovelo abaixo do ombro e pulso e cotovelo do lado direito
				   	&& (jointsPos[rightElbowIndex].y < jointsPos[rightShoulderIndex].y)
				    && (jointsPos[rightWristIndex].x > jointsPos[rightHipIndex].x)
				   && (jointsPos[rightElbowIndex].x > jointsPos[rightHipIndex].x)
					//se cotovelo esta acima do pulso
					&& (jointsPos[rightElbowIndex].y > jointsPos[rightWristIndex].y)
					//se o pulso e cotovelo estao na mesma profundidade
					&& ((jointsPos[rightElbowIndex].z - jointsPos[rightWristIndex].z) <= 0.1f)

					//se cotovelo esquerdo estiver na mesma altura do pulso esquerdo
					&&(jointsPos[leftElbowIndex].y - jointsPos[leftWristIndex].y <= 0.1f)
					//se cotovelo e pulso esquerdo estao na mesma profundidade
					&& (jointsPos[leftElbowIndex].z - jointsPos[leftWristIndex].z <= 0.1f)
					//se ombro esquerdo maior que cotovelo esquerdo
					&&(jointsPos[leftShoulderIndex].y - jointsPos[leftElbowIndex].y <= 0.4f))

				{
					if(GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightElbowIndex,rightShoulderIndex,rightHipIndex) <= 140)
					{
						SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
						gestureData.progress = 0.5f;
					}
				}
				break;
				
			case 1:  // gesture phase 2 = complete
		
				//falta bloquear outros movimentos enquanto estiver com o bait marker rolando
				if ((timestamp - gestureData.timestamp) < 1.5f)
				{			
					//chamar apos 3s
					isInPoseStop = jointsTracked[rightWristIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]
					&& jointsTracked[hipCenterIndex] && jointsTracked[rightHipIndex]
					//cotovelo direito abaixo do ombro
					&& (jointsPos[rightElbowIndex].y < jointsPos[rightShoulderIndex].y)
					//se pulso e cotovelo direito estao do lado direito
					&& (jointsPos[rightWristIndex].x > jointsPos[rightHipIndex].x) 
					&& (jointsPos[rightElbowIndex].x > jointsPos[rightHipIndex].x)
					//cotovelo acima do pulso
					&& (jointsPos[rightElbowIndex].y > jointsPos[rightWristIndex].y)
					//se o pulso e cotovelo estao na mesma profundidade
					&& ((jointsPos[rightElbowIndex].z - jointsPos[rightWristIndex].z) <= 0.1f)
					&& (GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightElbowIndex,rightShoulderIndex,rightHipIndex) <= 140) == true;
					//Debug.Log("isinpose " + isInPoseStop);
					if (isInPoseStop){
						//calcula o tempo de espera
						if(GameManagerShare.one_stop) {	
							//Debug.Log("em pose");
							GameManagerShare.one_notInPose = true;
							GameManagerShare.instance.count_timer_stop += Time.deltaTime*10;
							if(GameManagerShare.instance.count_timer_stop > 20)
							{
								GameManagerShare.one_stop = false;
								GameManagerShare.instance.count_timer_stop = 0;
								Debug.Log("chamando pause");
								GameManagerShare.instance.ready_to_call_pause = true;

								SetInitialPos(gestureData.jointPos, rightWristIndex);
								SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
								SetGestureTime(timestamp - gestureData.timestamp);
								Vector3 jointPos = jointsPos [gestureData.joint];
								CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPoseStop, 0f);

							}
						}
					}else {
						if(GameManagerShare.one_notInPose){
							GameManagerShare.one_notInPose = false;
							GameManagerShare.one_stop = true;
							//timer = 0; 
							//Debug.Log("nao esta em pose");
						}
					}
				}
				else 
				{
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
		//pausar com mao esquerda
		case Gestures.LeftHandStop:
			switch (gestureData.state) 
			{
			case 0:  // gesture detection - phase 
				//float angle = Vector3.Angle(jointsPos[rightElbowIndex].x, jointsPos[rightShoulderIndex].x) * Mathf.Sign(jointsPos[rightShoulderIndex].x - jointsPos[rightElbowIndex].x);
				if(jointsTracked[leftWristIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex]
				   && jointsTracked[hipCenterIndex] && jointsTracked[leftHipIndex]
				   //cotovelo abaixo do ombro e pulso e cotovelo do lado direito
				   && (jointsPos[leftElbowIndex].y < jointsPos[leftShoulderIndex].y)
				   && (jointsPos[leftWristIndex].x < jointsPos[leftHipIndex].x)
				   && (jointsPos[leftElbowIndex].x < jointsPos[leftHipIndex].x)
				   //se cotovelo esta acima do pulso
				   && (jointsPos[leftElbowIndex].y > jointsPos[leftWristIndex].y)
				   //se o pulso e cotovelo estao na mesma profundidade
				   && ((jointsPos[leftElbowIndex].z - jointsPos[leftWristIndex].z) <= 0.1f))
				{
					if(GenericKinectMethods.instance.GetBodySegmentAngleLateral(leftElbowIndex,leftShoulderIndex,leftHipIndex) >= -140)
					//if(AngleTest.instance.ang_value >= 45 )
					{
						SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
						gestureData.progress = 0.5f;
					}
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				
				//falta bloquear outros movimentos enquanto estiver com o bait marker rolando
				if ((timestamp - gestureData.timestamp) < 1.5f)
				{			
					//chamar apos 3s
					isInPoseStop = jointsTracked[leftWristIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex]
					&& jointsTracked[hipCenterIndex] && jointsTracked[leftHipIndex]
					//cotovelo direito abaixo do ombro
					&& (jointsPos[leftElbowIndex].y < jointsPos[leftShoulderIndex].y)
					//se pulso e cotovelo direito estao do lado direito
					&& (jointsPos[leftWristIndex].x < jointsPos[leftHipIndex].x) 
					&& (jointsPos[leftElbowIndex].x < jointsPos[leftHipIndex].x)
					//cotovelo acima do pulso
					&& (jointsPos[leftElbowIndex].y > jointsPos[leftWristIndex].y)
					//se o pulso e cotovelo estao na mesma profundidade
					&& ((jointsPos[rightElbowIndex].z - jointsPos[rightWristIndex].z) <= 0.1f)
					&& (GenericKinectMethods.instance.GetBodySegmentAngleLateral(leftElbowIndex,leftShoulderIndex,leftHipIndex) <= 140) == true;
					//&& (AngleTest.instance.ang_value >= 45 );
					if (isInPoseStop){
						if(GameManagerShare.one_stop) {	
							GameManagerShare.one_notInPose = true;
							GameManagerShare.instance.count_timer_stop += Time.deltaTime*10;
							if(GameManagerShare.instance.count_timer_stop > 8)
							{
								GameManagerShare.one_stop = false;
								GameManagerShare.instance.count_timer_stop = 0;
								//Debug.Log("chamando pause");
								GameManagerShare.instance.ready_to_call_pause = true;
								
								SetInitialPos(gestureData.jointPos, leftWristIndex);
								SetFinalPos(jointsPos[leftWristIndex], leftWristIndex);
								SetGestureTime(timestamp - gestureData.timestamp);
								Vector3 jointPos = jointsPos [gestureData.joint];
								CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPoseStop, 0f);
								
							}
						}
						//						
					}else {
						if(GameManagerShare.one_notInPose){
							GameManagerShare.one_notInPose = false;
							GameManagerShare.one_stop = true;
							//timer = 0; 
							//Debug.Log("nao esta em pose");
						}
					}
				}
				else 
				{
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;

		case Gestures.RightArmElevation:
			switch (gestureData.state) 
			{
			case 0:  // gesture detection - phase 
				//float angle = Vector3.Angle(jointsPos[rightElbowIndex].x, jointsPos[rightShoulderIndex].x) * Mathf.Sign(jointsPos[rightShoulderIndex].x - jointsPos[rightElbowIndex].x);
				var angle2 = GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex);
				//Debug.Log("angulo: pre " + angle2);
				if(jointsTracked[rightWristIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]
					&& jointsTracked[rightHipIndex]
					//maoD proximo de HipD
					&& (Vector3.Distance (jointsPos[rightWristIndex], jointsPos[rightHipIndex]) <= 0.2f)
					//angulo lateral do pulso, ombro e cintura direita fazem >= -180
					&&(GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex) >= 155)
					&&(GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex) <= 170))
					{
						var angle = GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex);
					   // Debug.Log("abaixando braço ESQUERDO angulo: " + angle);
						SetGestureJoint (ref gestureData, timestamp, rightWristIndex, jointsPos [rightWristIndex]);
						gestureData.progress = 0.5f;
					}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f)
				{			
					//chamar apos 3s
					bool isInPose = jointsTracked[rightWristIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]
					&& jointsTracked[rightHipIndex]
					//cotovelo direito.x maior que ombro.x
					&& (jointsPos[rightElbowIndex].x > jointsPos[rightShoulderIndex].x)
					&& (jointsPos[rightWristIndex].x > jointsPos[rightElbowIndex].x)

					&& (GenericKinectMethods.instance.GetBodySegmentAngleLateral(leftElbowIndex,leftShoulderIndex,leftHipIndex) >= 90) == true
					&& (GenericKinectMethods.instance.GetBodySegmentAngleLateral(leftElbowIndex,leftShoulderIndex,leftHipIndex) <= 100) == true;

					//&& (AngleTest.instance.ang_value >= 45 );
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						Debug.Log("RightArmElevation completed");
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
			break;
			}break;

		case Gestures.LeftArmElevation:
			switch (gestureData.state) 
			{
			case 0:  // gesture detection - phase 
				//float angle = Vector3.Angle(jointsPos[rightElbowIndex].x, jointsPos[rightShoulderIndex].x) * Mathf.Sign(jointsPos[rightShoulderIndex].x - jointsPos[rightElbowIndex].x);
				var angle2 = GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex);
				//Debug.Log("angulo: pre " + angle2);
				if(jointsTracked[rightWristIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]
				   && jointsTracked[rightHipIndex]
				   //maoD.x > cotoveloD.x
				   && (jointsPos[rightWristIndex].x > jointsPos[rightElbowIndex].x)
				   //angulo lateral do pulso, ombro e cintura direita fazem >= -180
				   &&(GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex) >= 160))
				{
					var angle = GenericKinectMethods.instance.GetBodySegmentAngleLateral(rightWristIndex,rightShoulderIndex,rightHipIndex);
					//Debug.Log("angulo: a " + angle);
					SetGestureJoint (ref gestureData, timestamp, leftWristIndex, jointsPos [leftWristIndex]);
					gestureData.progress = 0.5f;
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f)
				{			
					//chamar apos 3s
					bool isInPose = jointsTracked[rightWristIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]
					&& jointsTracked[rightHipIndex]
					//cotovelo direito.x menor que ombro.x
					&& (jointsPos[leftElbowIndex].x > jointsPos[leftShoulderIndex].x)
						&& (jointsPos[leftWristIndex].x > jointsPos[leftElbowIndex].x)
							
							&& (GenericKinectMethods.instance.GetBodySegmentAngleLateral(leftElbowIndex,leftShoulderIndex,leftHipIndex) >= 80) == true;
					
					//&& (AngleTest.instance.ang_value >= 45 );
					if (isInPose) {					
						SetInitialPos(gestureData.jointPos, rightWristIndex);
						SetFinalPos(jointsPos[rightWristIndex], rightWristIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						Debug.Log("LeftArmElevation completed");
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}break;

		case Gestures.TwoArmsUp:
			switch (gestureData.state) 
			{
			case 0:  // gesture detection - phase 
				//float angle = Vector3.Angle(jointsPos[rightElbowIndex].x, jointsPos[rightShoulderIndex].x) * Mathf.Sign(jointsPos[rightShoulderIndex].x - jointsPos[rightElbowIndex].x);
				if(jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[rightShoulderIndex] 
				   && jointsTracked[leftShoulderIndex] && jointsTracked[rightHipIndex] && jointsTracked[leftHipIndex]
				   && jointsTracked[rightElbowIndex] && jointsTracked[leftElbowIndex]
				   //se altura da mao menor que altura do ombro
				   &&(jointsPos[rightWristIndex].y < jointsPos[rightShoulderIndex].y)
				   &&(jointsPos[leftWristIndex].y < jointsPos[leftShoulderIndex].y)
				   //se as maos estao abaixo do cotovelo
				   &&(jointsPos[rightWristIndex].y < jointsPos[rightElbowIndex].y)
				   &&(jointsPos[leftWristIndex].y < jointsPos[leftElbowIndex].y)
				   //se as maos estao com z perto dos ombros
				   &&Mathf.Abs(jointsPos[rightWristIndex].z - jointsPos[rightShoulderIndex].z ) < 0.2f
				   &&Mathf.Abs(jointsPos[leftWristIndex].z - jointsPos[leftShoulderIndex].z ) < 0.2f
				   //se x da mao eh proximo ao x do ombro
				   &&Mathf.Abs(jointsPos[rightWristIndex].x - jointsPos[rightShoulderIndex].x ) < 0.2f
				   &&Mathf.Abs(jointsPos[leftWristIndex].x - jointsPos[leftShoulderIndex].x ) < 0.2f
				   )	
				{
					//if((GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightShoulderIndex,rightWristIndex,rightHipIndex) <= 200)
					  // && (GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftShoulderIndex,leftWristIndex,leftHipIndex) <= 200))
						//entra)
					if(AngleTest.instance.CalculateAng(jointsPos[rightWristIndex],jointsPos[rightShoulderIndex],jointsPos[rightHipIndex],false) <= 40f
					   && AngleTest.instance.CalculateAng(jointsPos[leftWristIndex],jointsPos[leftShoulderIndex],jointsPos[leftHipIndex],false) <= 40f)
					{
							//if(AngleTest.instance.ang_value >= 45 )
						Debug.Log ("Inicio da elevaçao frontal:"); //entrou
						SetGestureJoint (ref gestureData, timestamp, rightElbowIndex, jointsPos [rightElbowIndex]);
						gestureData.progress = 0.5f;
					}
				}
				break;
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) 
				{			
					bool isInPose = jointsTracked[rightWristIndex] && jointsTracked[leftWristIndex] && jointsTracked[rightShoulderIndex] 
					&& jointsTracked[leftShoulderIndex] && jointsTracked[rightHipIndex] && jointsTracked[leftHipIndex]
					&& jointsTracked[rightElbowIndex] && jointsTracked[leftElbowIndex]
					//se altura da mao +-  altura do ombro
					//&&Mathf.Abs(jointsPos[rightWristIndex].y - jointsPos[rightShoulderIndex].y) < 0.2f
					//&&Mathf.Abs(jointsPos[leftWristIndex].y - jointsPos[leftShoulderIndex].y) < 0.2f;

					//se altura dos cotovelos +- altura dos ombros
					//&&Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.2f
					//&&Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.2f
					//se as maos estao com z longe dos ombros
					//&&Mathf.Abs(jointsPos[rightWristIndex].z - jointsPos[rightShoulderIndex].z ) > 0.2f
					//&&Mathf.Abs(jointsPos[leftWristIndex].z - jointsPos[leftShoulderIndex].z ) > 0.2f
					
					//se x da mao eh proximo ao x do ombro
					//&&Mathf.Abs(jointsPos[rightWristIndex].x - jointsPos[rightShoulderIndex].x ) < 0.25f
					//&&Mathf.Abs(jointsPos[leftWristIndex].x - jointsPos[leftShoulderIndex].x ) < 0.25f;
					&& (AngleTest.instance.CalculateAng(jointsPos[rightWristIndex],jointsPos[rightShoulderIndex],jointsPos[rightHipIndex],false) > 40) == true
					&& (AngleTest.instance.CalculateAng(jointsPos[leftWristIndex],jointsPos[leftShoulderIndex],jointsPos[leftHipIndex],false) > 40) == true;
								
					if (isInPose) {
						Debug.Log("elevaçao frontal complete: " + AngleTest.instance.CalculateAng(jointsPos[rightWristIndex],jointsPos[rightShoulderIndex],jointsPos[rightHipIndex],false));
						SetInitialPos(gestureData.jointPos, rightElbowIndex);
						SetFinalPos(jointsPos[rightElbowIndex], rightElbowIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;
		}
	}
			//==========================================================================

	/*	case Gestures.Squat:
			switch (gestureData.state) {
			case 0:  // gesture detection - phase 1
				if ( jointsTracked [rightAnkleIndex] && jointsTracked [rightKneeIndex] && 
				    Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightHipIndex, rightKneeIndex, rightAnkleIndex)) < 10) {
					SetGestureJoint (ref gestureData, timestamp, rightAnkleIndex, jointsPos [rightAnkleIndex]);
					gestureData.progress = 0.5f;
				}
				break;				
				
				
			case 1:  // gesture phase 2 = complete
				if ((timestamp - gestureData.timestamp) < 1.5f) {
					bool isInPose = jointsTracked [rightAnkleIndex] && jointsTracked [rightKneeIndex] && 
						Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightHipIndex, rightKneeIndex, rightAnkleIndex)) > 20 &&
						Mathf.Abs(jointsPos [rightAnkleIndex].y - gestureData.jointPos.y) < 0.08f;
					
					if (isInPose) {
						SetInitialPos(gestureData.jointPos, rightHipIndex);
						SetFinalPos(jointsPos[rightHipIndex], rightHipIndex);
						SetGestureTime(timestamp - gestureData.timestamp);
						Vector3 jointPos = jointsPos [gestureData.joint];
						CheckPoseComplete (ref gestureData, timestamp, jointPos, isInPose, 0f);
					}
				} else {
					// cancel the gesture
					SetGestureCancelled (ref gestureData);
				}
				break;
			}
			break;*/

			//fim dos cases dos gestures
			//}		
		
	private static int ctd;

	//metodos para retornar posicao inicial e final de uma joint em um gesto
	private static int skeletonJointsCount = (int)KinectWrapper.NuiSkeletonPositionIndex.Count;
	private static Vector3[] jointsInitialPosList = new Vector3[skeletonJointsCount];
	private static  Vector3[] jointsFinalPosList = new Vector3[skeletonJointsCount];
	private static  float initialPos, finalPos;
	private static float gestureTime;

	public static Vector3 GetInitialJointPosition (int jointindex){
		return jointsInitialPosList [jointindex];
	}

	private static void SetInitialPos(Vector3 pos, int index){
		jointsInitialPosList [index] = pos;
	}

	public static Vector3 GetFinalJointPosition (int jointindex){
		return jointsFinalPosList [jointindex];
	}

	public static void SetFinalPos(Vector3 pos, int index){
		jointsFinalPosList [index] = pos;
	}

	public static float GetGestureTime (){
		return gestureTime;
	}
	
	private static void SetGestureTime(float time){
		gestureTime = time;
	}
	



}

using UnityEngine;
using System.Collections;

using Share.KinectUtils;

namespace Sandbox.PlayerControl {
	public class HandController : MonoBehaviour {
		
		public static GameObject leftHand;
		public static GameObject rightHand;
		
		public static bool isRightHand;
		public static bool isLeftHand;
		private int rightHandJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight;
		private int leftHandJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
		
		private float CALIBRATION_X_RIGHT_HAND = 0;
		private float CALIBRATION_Y_RIGH_HAND = -15;
		
		private float CALIBRATION_X_LEFT_HAND = 0;
		private float CALIBRATION_Y_LEFT_HAND = -15;
		
		private float amount_x = 15;
		private float amount_y = 10;
		
		private float speed = 8;
		
		private float cameraPosX;
		private float cameraPosY;
		
		private KinectManager kinect;
		public bool usingKinect;

		// Use this for initialization
		void Awake () {
			leftHand = 	GameObject.Find("leftHand").gameObject;	
			rightHand = GameObject.Find("rightHand").gameObject;
			
			kinect = KinectManager.Instance;
		}
		

		void Update() {
			if(usingKinect){
				uint playerId = kinect.GetPlayer1ID ();
				if (kinect.IsPlayerCalibrated(playerId)) {
					cameraPosX = Camera.main.transform.position.x;
					cameraPosY = Camera.main.transform.position.y;
					
					float posX = Mathf.Lerp (leftHand.transform.position.x, (KinectManager.Instance.GetJointPosition (playerId, leftHandJoint).x * amount_x) + CALIBRATION_X_LEFT_HAND + cameraPosX, speed * Time.deltaTime);
					float posY = Mathf.Lerp (leftHand.transform.position.y, (KinectManager.Instance.GetJointPosition (playerId, leftHandJoint).y * amount_y) + CALIBRATION_Y_LEFT_HAND + cameraPosY, speed * Time.deltaTime);
					leftHand.transform.position = new Vector3 (posX, posY, -9);
					
					float posX_right = Mathf.Lerp (rightHand.transform.position.x, (KinectManager.Instance.GetJointPosition (playerId, rightHandJoint).x * amount_x) + CALIBRATION_X_RIGHT_HAND + cameraPosX, speed * Time.deltaTime);
					float posY_right = Mathf.Lerp (rightHand.transform.position.y, (KinectManager.Instance.GetJointPosition (playerId, rightHandJoint).y * amount_y) + CALIBRATION_Y_RIGH_HAND + cameraPosY, speed * Time.deltaTime);
					rightHand.transform.position = new Vector3 (posX_right, posY_right, -9);
				}
		}
		}
		
	}
}
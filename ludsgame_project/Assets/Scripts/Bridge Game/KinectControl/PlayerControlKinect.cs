using UnityEngine;
using System.Collections;
using BridgeGame.Player;
using Share.KinectUtils;
using Share.Managers;
using Assets.Scripts.Share.Kinect;

namespace BridgeGame.KinectControl {
	public class PlayerControlKinect : MonoBehaviour {
		
		public float amountRotation;
		public float w;		
		
		private float maxAngle;
		private static float distZ;
		
		private int neckJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
		private int spineJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.Spine;
		private uint playerId;
		private const float TORSO_INCLINATION_Z = -0.025f;
		private float firstDistBugFix;
		private bool boolfirstDistBugFix = false;
		private bool isBoostOn = false;
		
		//public int playerSensibilityRotationValue;	
		private float standarPlayerSensibilityRotationValue;
		
		public static PlayerControlKinect instance;
		
		void Awake (){
			instance = this;
		}
		
		void Start (){
			float value = PlayerPrefsManager.GetLateralSensibilit();
			if(value == 0)
			{
				value = 13;
			}else if(value == 1)
			{
				value =20;

			}else if(value == 2)
			{
				value = 27;
			}
			standarPlayerSensibilityRotationValue = value;
		}
		
		void Update () {
			if(GameManagerShare.instance.IsUsingKinect())
			{
				playerId = KinectManager.Instance.GetPlayer1ID ();
				ControlPlayerWithTorso ();
				AccelerateWithTorso ();
			}
		}

		public void ExecuteMovement(Movement movement)
		{
			switch (movement)
			{
			/*case Movement.StopHand:
				if(GameManagerShare.instance.ready_to_call_pause)
					GameManagerShare.instance.ExecuteActionOnHandStop();
				break;*/
			default:
				print("outro gesto " + movement.ToString());
				break;
			}
		}

		/*public void IncreasePlayerSensibilityRotationValue(int increment){
			playerSensibilityRotationValue = playerSensibilityRotationValue + increment;
		}
		public void DecreasePlayerSensibilityRotationToStandard(){
			//ficara zero se esse script nao estiver ativado, pois no start que a variavel recebe um valor
			if(standarPlayerSensibilityRotationValue == 0){
				standarPlayerSensibilityRotationValue = 25;
			}
			
			playerSensibilityRotationValue = standarPlayerSensibilityRotationValue;
		}*/
		
		public float GetPlayerSensibilityRotation(){
			return standarPlayerSensibilityRotationValue;
		}

		public bool IsBoostOn(){
			return isBoostOn;
		}
		
		void AccelerateWithTorso(){
			distZ = GenericKinectMethods.instance.GetJointsDistanceZ(neckJoint, spineJoint, playerId);
			if(KinectManager.Instance.IsPlayerCalibrated(playerId) && GameManagerShare.IsStarted() 
			   && !GameManagerShare.IsGameOver() && !GameManagerShare.IsPaused() &&!BridgeManager.instance.IsPlayerOnTheWater()){
				if(distZ < TORSO_INCLINATION_Z){
					PlayerControl.instance.playerSpeed =  PlayerControl.instance.GetDefaultPlayerSpeedBoost();
					isBoostOn = true;
				}else{
					PlayerControl.instance.playerSpeed =  PlayerControl.instance.GetDefaultPlayerSpeed();
					isBoostOn = false;
				}
			}			
		}

		
		void ControlPlayerWithTorso(){
			float dist = GenericKinectMethods.instance.GetJointsDistanceX(neckJoint, spineJoint, playerId);
			if(KinectManager.Instance.IsPlayerCalibrated(playerId) && GameManagerShare.IsStarted()
			   && !GameManagerShare.IsGameOver() && !GameManagerShare.IsPaused() && !BridgeManager.instance.IsPlayerOnTheWater()){
                if(!boolfirstDistBugFix){
					if(dist > 0){
						firstDistBugFix = dist;
					}else if(dist < 0){
						firstDistBugFix = -dist;
					}
					boolfirstDistBugFix = true;
				}else{
					this.transform.Rotate(-Vector3.forward *(dist*(amountRotation*standarPlayerSensibilityRotationValue)) *Time.deltaTime); //10//20
				}
			}
		}
		
		
		public static float GetDistZ(){
			return distZ;
		}
	}
}
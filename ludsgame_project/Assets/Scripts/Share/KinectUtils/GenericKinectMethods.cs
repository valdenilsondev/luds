using UnityEngine;
using System.Collections;
using System;
namespace Share.KinectUtils {
	public class GenericKinectMethods : MonoBehaviour {
		public static GenericKinectMethods instance;
		private static KinectManager kinect;
		private bool isInclinated;
		private bool isInclinated_left;
		private bool isInclinated_right;
		private int neckJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
		private int spineJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.Spine;
		private uint playerId;

		// Use this for initialization
		void Awake () {
			instance = this;
		}
		//

		void Start(){
			kinect = KinectManager.Instance;
		}

		void Update(){			
			if(Share.Managers.GameManagerShare.instance.IsUsingKinect()){
				playerId = KinectManager.Instance.GetPlayer1ID ();
				CheckInclination();
			}
		}
		

		/// <summary>
		/// Gets the joints distance x.
		/// </summary>
		/// <returns>The joints distance x.</returns>
		/// <param name="jointA">Joint a.</param>
		/// <param name="jointB">Joint b.</param>
		public float GetJointsDistanceX(int jointA, int jointB, uint playerId){
			float jointAPosX = KinectManager.Instance.GetJointPosition(playerId, jointA).x;
			float jointBPosX = KinectManager.Instance.GetJointPosition (playerId, jointB).x;
			//print ("junta " + jointAPosX.ToString());
			return jointAPosX - jointBPosX;
		}
		
		
		/// <summary>
		/// Gets the joints distance z.
		/// </summary>
		/// <returns>The joints distance z.</returns>
		/// <param name="jointA">Joint a.</param>
		/// <param name="jointB">Joint b.</param>
		/// <param name="playerId">Player identifier.</param>
		public float GetJointsDistanceZ(int jointA, int jointB, uint playerId){
			float jointAPosX = KinectManager.Instance.GetJointPosition(playerId, jointA).z;
			float jointBPosX = KinectManager.Instance.GetJointPosition (playerId, jointB).z;
			//print ("junta " + jointAPosX.ToString());
			return jointAPosX - jointBPosX;
		}
		
		/// <summary>
		/// Gets the joints distance. 
		/// </summary>
		/// <returns>The joints distance.</returns>
		/// <param name="jointA">Joint a.</param>
		/// <param name="jointB">Joint b.</param>
		/// <param name="playerId">Player identifier.</param>
		public float GetJointsDistance(int jointA, int jointB, uint playerId){
			return Vector3.Distance (KinectManager.Instance.GetJointPosition (playerId, jointA),  KinectManager.Instance.GetJointPosition (playerId, jointB));
		}
		
		
		
		/// <summary>
		/// Retorna o angulo do movimento lateral em relaçao ao eixo Y
		/// </summary>
		/// <returns>The joint sides angle to the top.</returns>
		/// <param name="joint">Joint.</param>
		/// <param name="playerId">Player identifier.</param>
		public int GetJointSidesAngleToTheTop(int joint, uint playerId){
			float jointPosX = KinectManager.Instance.GetJointPosition (playerId, joint).x;
			float angle = Vector3.Angle (new Vector3 (jointPosX, 0, 0), transform.up) - 90;
			return (int)angle;
		}
		
		
		
		/// <summary>
		/// Retorna o angulo do movimento de inclinaçao frontal (Z) em relaçao ao eixo Y
		/// </summary>
		/// <returns>The joint foward angle to the top.</returns>
		/// <param name="jointA">Joint a.</param>
		/// <param name="playerId">Player identifier.</param>
		public int GetJointFowardAngleToTheTop(int jointA, uint playerId){
			float jointAPosZ = KinectManager.Instance.GetJointPosition (playerId, jointA).z;
			float angle = Vector3.Angle (new Vector3(0,0,jointAPosZ) , transform.up);
			return (int)angle;
		}

		/// <summary>
		/// Gets the body segment angle lateral.
		/// </summary>
		/// <returns>The body segment angle lateral.</returns>
		/// <param name="top">Top.</param>
		/// <param name="middle">Middle.</param>
		/// <param name="bottom">Bottom.</param>
		public double GetBodySegmentAngleLateral(int top, int middle, int bottom)
		{
			Vector3 joint1 = kinect.GetJointPosition (kinect.GetPlayer1ID (), top);
			Vector3 joint2 = kinect.GetJointPosition (kinect.GetPlayer1ID (), middle);
			Vector3 joint3 =  kinect.GetJointPosition (kinect.GetPlayer1ID (), bottom);
			
			Vector3 vectorJoint1ToJoint2 = new Vector3(joint1.x - joint2.x, joint1.y - joint2.y, 0);
			Vector3 vectorJoint2ToJoint3 = new Vector3(joint2.x - joint3.x, joint2.y - joint3.y, 0);
			vectorJoint1ToJoint2.Normalize();
			vectorJoint2ToJoint3.Normalize();
			
			Vector3 crossProduct = Vector3.Cross(vectorJoint1ToJoint2, vectorJoint2ToJoint3);
			double crossProductLength = crossProduct.z;
			double dotProduct = Vector3.Dot(vectorJoint1ToJoint2, vectorJoint2ToJoint3);
			double segmentAngle = Math.Atan2(crossProductLength, dotProduct);
			
			// Convert the result to degrees.
			double degrees = segmentAngle * (180 / Math.PI);
			
		
			return degrees;
		}

		/// <summary>
		/// Gets the body segment angle frontal.
		/// </summary>
		/// <returns>The body segment angle frontal.</returns>
		/// <param name="top">Top.</param>
		/// <param name="middle">Middle.</param>
		/// <param name="bottom">Bottom.</param>
		public double GetBodySegmentAngleFrontal(int top, int middle, int bottom)
		{
			Vector3 joint1 = kinect.GetJointPosition (kinect.GetPlayer1ID(), top);
			Vector3 joint2 = kinect.GetJointPosition (kinect.GetPlayer1ID (), middle);
			Vector3 joint3 =  kinect.GetJointPosition (kinect.GetPlayer1ID (), bottom);
			
			Vector3 vectorJoint1ToJoint2 = new Vector3(0, joint1.y - joint2.y, joint1.z - joint2.z);
			Vector3 vectorJoint2ToJoint3 = new Vector3(0, joint2.y - joint3.y, joint2.z - joint3.z);
			vectorJoint1ToJoint2.Normalize();
			vectorJoint2ToJoint3.Normalize();
			
			Vector3 crossProduct = Vector3.Cross(vectorJoint1ToJoint2, vectorJoint2ToJoint3);
			double crossProductLength = crossProduct.x;
			double dotProduct = Vector3.Dot(vectorJoint1ToJoint2, vectorJoint2ToJoint3);
			double segmentAngle = Math.Atan2(crossProductLength, dotProduct);
			
			// Convert the result to degrees.
			double degrees = segmentAngle * (180 / Math.PI);
	
			
			return degrees;
		}

	
		private void CheckInclination(){
			//TODO: Verificar valores para colocar o correto.
			if(GetJointsDistanceX(neckJoint, spineJoint, playerId) > 0.045f || GetJointsDistanceX(neckJoint, spineJoint, playerId) < -0.045f){
				isInclinated = true;
				if(GetJointsDistanceX(neckJoint, spineJoint, playerId) > 0.045f)
				{
					isInclinated_left = true;
					isInclinated_right = false;
				}
				else if(GetJointsDistanceX(neckJoint, spineJoint, playerId) < -0.045f)
				{
					isInclinated_left = false;
					isInclinated_right = true;
				}
			}else{
				isInclinated = false;
			}
		}
		public bool IsInclinated(){
			return isInclinated;
		}
		public bool IsIncRight()
		{
			return isInclinated_right;
		}
		public bool IsIncLeft()
		{
			return isInclinated_left;
		}
	}
}
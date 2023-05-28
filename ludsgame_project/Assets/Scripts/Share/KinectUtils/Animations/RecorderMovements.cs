#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Share.KinectUtils;
using Share.KinectUtils.Record;
using Sandbox.GameUtils;
using UnityEngine.UI;

namespace Share.KinectUtils.Animations {
	public class RecorderMovements : MonoBehaviourSingleton<RecorderMovements> {

		private KinectManager kinect;
		private AnimationCreation animationCreation;
		private List<SkeletonFrameEureka> animation;

		private bool canRecord = false;

		public Transform[] skeleton;

		void Awake() {
			kinect = KinectManager.Instance;
			animationCreation = AnimationCreation.Instance;
			animation = new List<SkeletonFrameEureka>();
		}

		public bool StartRecording() {
			canRecord = true;
			if(animation.Count == 0)
				return true;
			return false;
		}

		public void StopRecording() {
			canRecord = false;
		}

		public void SaveRecording() {
			animationCreation.SetAnimation(animation);
			animation = new List<SkeletonFrameEureka>();
		}

		public void SetJointsPosition(SkeletonFrameEureka frame) {
			if (canRecord) {
				animation.Add (frame);
				for (int i = 0; i < skeleton.Length; i++) {
					if(skeleton[i] != null){
						skeleton[i].localPosition = frame.SkeletonPositions[i];
						skeleton[i].rotation = ConvertMatrixToQuat(frame.skeletonRotations[i], true);
					}
				}
			}
		}

		private Quaternion ConvertMatrixToQuat (Matrix4x4 mOrient, bool flip) {
			Vector4 vZ = mOrient.GetColumn (2);
			Vector4 vY = mOrient.GetColumn (1);
			
			if (!flip) {
				vZ.y = -vZ.y;
				vY.x = -vY.x;
				vY.z = -vY.z;
			} else {
				vZ.x = -vZ.x;
				vZ.y = -vZ.y;
				vY.z = -vY.z;
			}
			
			if (vZ.x != 0.0f || vZ.y != 0.0f || vZ.z != 0.0f)
				return Quaternion.LookRotation (vZ, vY);
			else
				return Quaternion.identity;
		}
	}
}
#endif

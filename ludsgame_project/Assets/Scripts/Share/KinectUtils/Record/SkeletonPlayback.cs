using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Share.KinectUtils.Record {
	public class SkeletonPlayback : MonoBehaviour {
		
		public Transform[] skeleton;
		
		private SkeletonRecorder skeletonRecorder;
		private bool isPlaying = false;
		private SkeletonFrameEureka tempSkeleton;
		private GestureEureka tempGesture;
		private uint currentFrame = 0;
		private KinectWrapper.NuiSkeletonPositionIndex[] ignoreJoints = new KinectWrapper.NuiSkeletonPositionIndex[] {KinectWrapper.NuiSkeletonPositionIndex.KneeLeft, KinectWrapper.NuiSkeletonPositionIndex.AnkleLeft,
			KinectWrapper.NuiSkeletonPositionIndex.FootLeft,KinectWrapper.NuiSkeletonPositionIndex.KneeRight,KinectWrapper.NuiSkeletonPositionIndex.AnkleRight,KinectWrapper.NuiSkeletonPositionIndex.FootRight};

		public Text stopReplayText;
		
		void Awake() {
			//skeletonRecorder = SkeletonRecorder.Instance;
		}
		
		void Update () {
			if(Input.GetKeyDown(KeyCode.Return)) {
				if (!isPlaying /*&& skeletonRecorder.StartPlaySkeletonRecord()*/) {
					isPlaying = true;
					
					Debug.Log ("Start Playback Skeleton");
				} else {
					Debug.Log (isPlaying);
				//	Debug.Log (skeletonRecorder.StartPlaySkeletonRecord());
				}
			}
			
			if(Input.GetKeyDown(KeyCode.Space)) {
				if (!isPlaying /* && skeletonRecorder.StartPlayGestureRecord()*/) {
					PlayGestures();
					isPlaying = true;
					
					Debug.Log ("Start Playback Gesture");
				}
			}
		}
		
		void FixedUpdate () {
		/*	if(isPlaying) {
				if(skeletonRecorder.CurrentState == SkeletonRecordState.PlayingSkeleton || skeletonRecorder.CurrentState == SkeletonRecordState.PlayingGesture) {
					Play (skeletonRecorder.CurrentState == SkeletonRecordState.PlayingGesture);
				}
			}*/
		}
		
		public void BackToLevelSelector() {
			SceneManager.LoadScene("LevelSelector");
		}

		private bool ContainsJoint(int jointIndex) {
			KinectWrapper.NuiSkeletonPositionIndex joint = (KinectWrapper.NuiSkeletonPositionIndex) jointIndex;

			for (int i = 0; i < ignoreJoints.Length; i++) {
				if(joint == ignoreJoints[i]) {
					return true;
				}
			}
			return false;
		}
		
		private void Play(bool gesture = false) {
			if(gesture) {
				if(currentFrame > tempGesture.finalFrame) {
					PlayGestures();
				}
				//tempSkeleton = skeletonRecorder.GetSkeletonFrame(currentFrame);
				currentFrame++;
			} else {
			//	tempSkeleton = skeletonRecorder.PlaySkeletonRecord();
			}
			
			if(tempSkeleton != null) {
				for (int i = 0; i < skeleton.Length; i++) {
					if(!ContainsJoint(i) && tempSkeleton.SkeletonPositions[(int) i] != Vector4.zero) {
						skeleton[i].localPosition = tempSkeleton.SkeletonPositions[(int)i];
					} /*else {
					Debug.Log ("Zero");
				}*/
				}
			} else {
				Stop();
			}
		}

		private void Stop() {
			isPlaying = false;
			if(stopReplayText != null) stopReplayText.gameObject.SetActive(true);
			Debug.Log ("Stop Playback");
		}

		private void PlayGestures() {
			//tempGesture = skeletonRecorder.PlayGestureRecord();
			if(tempGesture != null) {
				Debug.Log("Gesture: " + tempGesture.gesture.ToString());
				currentFrame = tempGesture.initialFrame;
			}
		}
		
		public void BackToGame() {
		//	Destroy (skeletonRecorder.gameObject);
			SceneManager.LoadScene("Intro");
		}
	}
}
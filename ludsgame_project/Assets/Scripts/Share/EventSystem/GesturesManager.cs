using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

//using Share.KinectUtils;
using Sandbox.UI;
namespace Share.EventSystem {
	public class GesturesManager : MonoBehaviour, KinectGestures.GestureListenerInterface {
		
		private GestureEvents gestureEvents;
		private KinectManager kinect;
		
		void Awake() {
			gestureEvents = GestureEvents.Instance;
			kinect = KinectManager.Instance;
		}

	
		private void CallTrackingScreen() {
			kinect.SetPlayerTracked(false);
			/*Camera.main.transform.position = new Vector3(-26.8f, 0, -10);
			MenuManager.instance.gameBG.gameObject.SetActive(false);*/

		}
		
		#region GestureListenerInterface implementation
		
		void KinectGestures.GestureListenerInterface.UserDetected (uint userId, int userIndex) {

		}
		
		void KinectGestures.GestureListenerInterface.UserLost (uint userId, int userIndex) {
			CallTrackingScreen();
		}
		
		void KinectGestures.GestureListenerInterface.GestureInProgress (uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
			
		}
		
		bool KinectGestures.GestureListenerInterface.GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
			gestureEvents.SetGestureCompleted(gesture, joint);

			return true;
		}
		
		bool KinectGestures.GestureListenerInterface.GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint) {
			return false;
		}
		
		#endregion
	}
}

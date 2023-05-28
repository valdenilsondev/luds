using UnityEngine;
using System.Collections;

using Sandbox.GameUtils;
using Share.KinectUtils;

namespace Share.EventSystem {
	public class EventsManager : MonoBehaviourSingleton<EventsManager>, KinectGestures.GestureListenerInterface {
		
		//private KinectManager kinectManager;
		private Events events;
		
		void Awake () {
			// Init instances
			//kinectManager = KinectManager.Instance;
			events = Events.instance;
		}
		
		public void UserDetected(uint userId, int userIndex) {
			//kinectManager.DetectGesture(userId, KinectGestures.Gestures.Push);
		}
		
		public void UserLost(uint userId, int userIndex){
			// TODO
		}
		
		public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos){
			// TODO
		}
		
		public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
			// Set the gesture completed
			//		Debug.Log(gesture);
			//		gameManager.SetGesture(gesture);
			//		events.Raise(typeof(EventTest));
			if(gesture == KinectGestures.Gestures.RaiseRightHand){
				
				Share.EventsSystem.Events.RaiseEvent<Share.EventsSystem.GameStart> ();
			}
			
			return true;
		}
		
		public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,  KinectWrapper.NuiSkeletonPositionIndex joint){
			// TODO
			return false;
		}
	}
}
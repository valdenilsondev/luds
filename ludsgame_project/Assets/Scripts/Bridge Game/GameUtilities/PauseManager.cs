using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using Share.EventSystem;
using Share.KinectUtils;

namespace BridgeGame.GameUtilities {
	public class PauseManager : MonoBehaviour {
		
		private Events events;
		
		void Awake () {
			events = Events.instance;
		}
		
		void OnEnable() {
			//events.AddListener<GestureEvents>(OnPush);
		}
		
		void OnDisable() {
			//events.RemoveListener<GestureEvents>(OnPush);
		}
		
		public void OnPush(GestureEvents e) {
//			if (e.GetGestureCompleted() == KinectGestures.Gestures.Push){
//				Debug.Log ("Pause Game");
//			}
		}
	}
}
using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using Sandbox.GameUtils;
using Sandbox.KinectControl;
using Share.KinectUtils;

namespace Share.EventSystem {
	public class GestureEvents {
		
		private Events events;
		
		private static GestureEvents instance;
		public static GestureEvents Instance {
			get {
				if(instance == null) {
					instance = new GestureEvents();
				}
				
				return instance;
			}
		}
		
		private static KinectGestures.Gestures gestureCompleted = KinectGestures.Gestures.None;
		private KinectWrapper.NuiSkeletonPositionIndex jointCompleted;
		
		public static KinectGestures.Gestures GestureCompleted {
			get {
				return gestureCompleted;
			}
			
			set {
				gestureCompleted = value;
			}
		}
		
		public KinectGestures.Gestures GetGestureCompleted() {
			return gestureCompleted;
		}
		
		public void SetGestureCompleted(KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint) {
			gestureCompleted = gesture;
			jointCompleted = joint;
			
			ExecuteAction();
		}
		
		private void ExecuteAction() {
			switch (gestureCompleted) {
			case KinectGestures.Gestures.Push:
				PushGesture();
				break;
			case KinectGestures.Gestures.RaiseRightHand:
				RaiseHandGesture();
				break;
			}
		}
		
		private void PushGesture() {
			if (jointCompleted == MenuManager.instance.GetHandJoint()) {
				if (MenuManager.isUserPlaying && !PauseManager.isPaused) {
					Events.instance.Raise (typeof(PushGestureGame));
				} else {
					Events.instance.Raise (typeof(PushGestureMenu));
				}
			}
		}
		
		// Gesto de calibração
		private void RaiseHandGesture() {
			// Testa se o jogador está a uma distância aceitável
			uint playerId = KinectManager.Instance.GetPlayer1ID ();
			if(DistanceCalibrator.instance.IsDistanceOk() && !KinectManager.Instance.IsPlayerCalibrated(playerId)) {
				KinectManager.Instance.SetPlayerTracked(true);
				// Determina se o jogador vai usar a mao esquerda ou direita
				MenuManager.instance.SetHandReference((jointCompleted == KinectWrapper.NuiSkeletonPositionIndex.HandRight));
			}
		}
	}
}

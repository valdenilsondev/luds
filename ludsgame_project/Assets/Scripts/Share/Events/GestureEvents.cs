using UnityEngine;
using System.Collections;
using Share.EventsSystem;

public class GestureEvents : MonoBehaviour {

	Events events;
	// Use this for initialization
	private static KinectGestures.Gestures gestureCompleted = KinectGestures.Gestures.None;
	private KinectWrapper.NuiSkeletonPositionIndex jointCompleted;
	private static GestureEvents instance;

	public static GestureEvents Instance() {			
		return instance;		
	}

	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetGestureCompleted(KinectGestures.Gestures gesture, KinectWrapper.NuiSkeletonPositionIndex joint) {
		gestureCompleted = gesture;
		jointCompleted = joint;
		print ("setgesturecompled");
		ExecuteAction();
	}

	private void ExecuteAction() {
		switch (gestureCompleted) {
		case KinectGestures.Gestures.RaiseLeftHand:
			RaiseLeftHandGesture();
			break;
		case KinectGestures.Gestures.RaiseRightHand:
			RaiseRightHandGesture();
			break;
		}
	}

	private void RaiseLeftHandGesture() {
		// Testa se o jogador está a uma distância aceitável
		if(DistanceCalibrator.instance.IsDistanceOk() && DistanceCalibrator.instance.IsLateralDistanceOk()) {
		//	KinectManager.Instance.SetPlayerTracked(true);
			DistanceCalibrator.instance.CallCalibratedTextAnim();

			// Determina se o jogador vai usar a mao esquerda ou direita
			//MenuManager.instance.SetHandReference((jointCompleted == KinectWrapper.SkeletonJoint.RIGHT_HAND));
		}
	}

	private void RaiseRightHandGesture() {
		// Testa se o jogador está a uma distância aceitável
		if(DistanceCalibrator.instance.IsDistanceOk() && DistanceCalibrator.instance.IsLateralDistanceOk()) {
		//	KinectManager.Instance.SetPlayerTracked(true);
			DistanceCalibrator.instance.CallCalibratedTextAnim();

			// Determina se o jogador vai usar a mao esquerda ou direita
			//MenuManager.instance.SetHandReference((jointCompleted == KinectWrapper.SkeletonJoint.RIGHT_HAND));
		}
	}

}

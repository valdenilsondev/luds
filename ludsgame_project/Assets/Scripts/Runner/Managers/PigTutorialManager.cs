using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share.Enums;
using Assets.Scripts.Share.Kinect;
using Share.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Share.EventsSystem;
using Assets.Scripts.Share.Managers;
using Share.Controllers;


public class PigTutorialManager : MonoBehaviour {
	public Dictionary<KinectGestures.Gestures, Movement> playerMovements;
	public static PigTutorialManager instance;

	// Use this for initialization
	void Start () {
		instance = this;

		playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();
		AddPlayerMovements ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExecuteGesture(KinectGestures.Gestures gesture){
		var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();
		
		if(movement != null)
			MovePigTutorial.instance.ExecuteMovement(movement);		
	}

	public void AddPlayerMovements(){
		KinectGestures.Gestures gJump1 = (KinectGestures.Gestures) PlayerPrefsManager.GetJump1 ();
		KinectGestures.Gestures gJump2 = (KinectGestures.Gestures) PlayerPrefsManager.GetJump2 ();
		KinectGestures.Gestures gSquat = (KinectGestures.Gestures) PlayerPrefsManager.GetSquat ();
		KinectGestures.Gestures gJMovLeft = (KinectGestures.Gestures) PlayerPrefsManager.GetMoveLeft ();
		KinectGestures.Gestures gJMovRight = (KinectGestures.Gestures) PlayerPrefsManager.GetMoveRight ();
		
		playerMovements.Add(gJump1, Movement.Jump);
		playerMovements.Add(gJump2, Movement.Jump);
		playerMovements.Add(gSquat, Movement.Roll);
		playerMovements.Add(gJMovLeft, Movement.MoveLeft);
		playerMovements.Add(gJMovRight, Movement.MoveRight);
	}
	
}

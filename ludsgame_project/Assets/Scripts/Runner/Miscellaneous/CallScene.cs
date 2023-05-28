using UnityEngine;
using System.Collections;

public class CallScene : MonoBehaviour {

	public void LoadGame(){
		print ("loadgame");
		DistanceCalibrator.instance.LoadGame();
	}
	
	public void LoadStartScreen(){
		print ("loadbar");
		DistanceCalibrator.instance.LoadStartScreen();
	}
}

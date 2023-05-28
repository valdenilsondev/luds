using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	//variaveis do tempo

	private bool timeRunning = false;
	private float timer;

	public static TimeManager instance;

	void Awake () {
		instance = this;
	}

	void Update () {
		if(timeRunning){
			timer = timer + Time.deltaTime;
		}
	}

	public void StartTimer(){
		timeRunning = true;
	}

	public float GetCurrentTime(){
		return timer;
	}
	public void StopTimer(){
		timeRunning = false;
		timer = 0;
		print ("time stop");
	}
//	public void ResetTimer(){
//		timer = 0;
//		print ("time reset");
//	}

//	public void PauseTimer(){
//		timeRunning = false;
//	}

//	public void ResumeTimer(){
//		timeRunning = true;
//	}

}

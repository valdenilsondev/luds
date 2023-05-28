using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BridgeGame.Player;

public class Counter : MonoBehaviour {

	private Text count;
	private float timer = 4;
	private int timerInt = 4;
	private bool startTimer = false;
	private bool timerComplete = false;
	private bool check = false;
	private GameObject cameraBridge;
	private GameObject pigBridge;
	private Color customColor;
	private Color customColorInvisible;
	 
	public static Counter instance;

	// Use this for initialization
	void Start () {
		count = this.GetComponent<Text>();
		customColor = new Color(1f, 1f, 0f, 1);
		customColorInvisible = new Color(1f, 1f, 0f, 0);
		count.color = customColorInvisible;
		cameraBridge = GameObject.Find("PlayerCamera");
		pigBridge = GameObject.Find("pig_bridge");
		instance = this;
	}

	public void StartTimer(){
		startTimer = true;
	}

	public bool IsTimerCompleted(){
		return timerComplete;
	}
	
	// Update is called once per frame
	void Update () {
		if(BridgeManager.instance.IsCameraOverviewAnimComplete() && startTimer && !check){
			if(count.color == customColorInvisible){
				count.color = customColor;
			}

			//camera moving on 3 2 1
			if(timer >= 3){
				cameraBridge.transform.position = new Vector3(90,50.5f,150);
				iTween.RotateTo(cameraBridge, new Vector3(15, 234, 0), 0.5f);
			}else if (timer >= 2){
				cameraBridge.transform.position = new Vector3(100,53,170);
				iTween.RotateTo(cameraBridge, new Vector3(0, 133, 0), 0.5f);
			}else if (timer >= 1){
				cameraBridge.transform.position = new Vector3(97,53,175);
				iTween.RotateTo(cameraBridge, new Vector3(0, 0, 0), 0.5f);
			}

			if(timer > 1){
				timer = timer - Time.deltaTime;
				timerInt = (int)timer;
				count.text = timerInt.ToString();
			}else if(timer < 1 && timer > 0){
				count.text = "VAI!";
				timer = timer - Time.deltaTime;
				if(!timerComplete){
					PlayerControl.instance.SetPigBridgeWalking();
				}
				timerComplete = true;
			}else{
				count.color = customColorInvisible;
				check = true;
			}
		}
	}
}


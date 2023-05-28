using UnityEngine;
using System.Collections;
using Assets.Scripts.Goalkeeper;
using Share.Managers;
using Goalkeeper.Managers;

public class CameraControl : MonoBehaviour {

	private Camera mainCamera;

	public static CameraControl instance;

	private void Awake(){
		instance = this;
		mainCamera = Camera.main;
	}

	void Update(){
	}

	public void MoveCameraTowards_GK(){
		Camera.main.GetComponent<Animator> ().SetTrigger ("moveToGk");
	}

	public void MoveCameraTowards_Kicker(){
        if (!GoalkeeperManager.Instance().playerLost)
		    Camera.main.GetComponent<Animator> ().SetTrigger ("moveToKicker");
	}

    public void MoveCameraTowards_KickerFinished()
    {
    }

	public void MoveCameraTowards_GK_Zoom(){
		Camera.main.GetComponent<Animator> ().SetTrigger ("zoom");
	}

	public void StartCountTime(){
		GoalkeeperManager.instance.isReady = true;
	}

    public void InitializeBallPosition()
    {
        Debug.Log("InitializeBallPosition");
        BallControl.instance.InitializeBallPosition();
    }

	public void StartCameraAnim(){
		this.gameObject.GetComponent<Animator>().SetTrigger("startAnim");
	}
}

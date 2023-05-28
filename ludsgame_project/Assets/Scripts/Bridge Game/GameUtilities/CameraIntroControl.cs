using UnityEngine;
using System.Collections;
using BridgeGame.Player;

public class CameraIntroControl : MonoBehaviour {

	public static CameraIntroControl instance;

	void Awake()
	{
		instance = this;
	}

	public void StartInitialCameraAnimation(){
		this.GetComponent<Animator>().SetTrigger("cameraIn");
	}

	//chamada do evento de fim da animacao da camera
	private void CallOverviewCamCompleted(){
		BridgeManager.instance.OverviewCameraCompleted();
		this.GetComponent<Animator>().SetTrigger("cameraOut");
	}
}

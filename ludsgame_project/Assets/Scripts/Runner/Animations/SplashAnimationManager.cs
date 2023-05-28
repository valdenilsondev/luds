using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashAnimationManager : MonoBehaviour {

	private GameObject logoSplash;
	private GameObject pigrunnerSplash;

	void Start(){
		logoSplash = GameObject.Find("LogoSplashScreen") as GameObject;
		pigrunnerSplash = GameObject.Find("PigRunnerSplashScreen").gameObject;
	}

	public void DeactivatelogoSplash(){
		logoSplash.SetActive(false);
	}

	public void DeactivatepigrunnerSplash(){
		pigrunnerSplash.SetActive(false);
	}

	public void LoadMenuScene(){
		SceneManager.LoadSceneAsync("Http");
	}
}

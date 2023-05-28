using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSelection : MonoBehaviour {

	public void SelectSandbox() {
		SceneManager.LoadScene("Intro");
	}

	public void SelectBridge() {
		SceneManager.LoadScene("StartupScene");
	}

	public void SelectGK() {
		SceneManager.LoadScene("Intro_Goalkeeper");
	}
}

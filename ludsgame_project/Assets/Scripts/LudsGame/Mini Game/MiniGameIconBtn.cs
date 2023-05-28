using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameIconBtn : MonoBehaviour {

	public int gameIndex;


	public void PlayMiniGame(){
	//	SceneManager.LoadScene (this.transform.GetComponentInChildren<Text> ().text);
		SceneManager.LoadScene ("PigRunner");

	}

	public void AddMiniGame(){ 
		if (this.transform.GetComponentInChildren<Toggle> ().isOn) {
			MiniGameScreenKinectController.instance.AddMiniGameToList (this.transform.GetComponentInChildren<Text> ().text, gameIndex);

		} else {
			MiniGameScreenKinectController.instance.RemoveMiniGameToList (this.transform.GetComponentInChildren<Text> ().text, gameIndex);
		}

	}

}

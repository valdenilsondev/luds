using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BridgeGame.GameUtilities {
	public class LevelSelector : MonoBehaviour {
		public static string levelToLoad;

		void Awake(){
			levelToLoad = "notSelected";
		}

		void OnGUI(){
			if(GUI.Button(new Rect (Screen.width/2 - (Screen.width/4 + (Screen.width/4)/2), Screen.height/4, Screen.width/4, Screen.height/2), "FASE 1") ){
				levelToLoad = "Bridge";
				SceneManager.LoadScene("Calibration_bridge");
			}
			if(GUI.Button(new Rect (Screen.width/2 + (Screen.width/4)/2, Screen.height/4, Screen.width/4, Screen.height/2), "FASE 2") ){
				levelToLoad = "fase2";
				SceneManager.LoadScene("Calibration_bridge");
			}
		}

		void Update(){
			if(Input.GetKeyDown(KeyCode.Space)){
				levelToLoad = "Bridge";
				SceneManager.LoadScene("Calibration_bridge");
			}
		}
	}
}
using UnityEngine;
using System.Collections;
using BridgeGame.Player;
using Share.Managers;
using Share.EventsSystem;
using UnityEngine.SceneManagement;

public class GameOverBridge : MonoBehaviour {

	public static bool isGameOverBridge = false;

	public static GameOverBridge instance;

	void Start(){
		instance = this;
	}

	/*void Update () {
		if(GameManagerShare.instance.IsUsingKinect()){
			CheckTagsAndLoad();
		}else if(Input.GetMouseButtonDown(0)){
			CheckTagsAndLoadClick();
		}
	}*/

	/*private void CheckTagsAndLoad(){
		if(HandCollider2D.handOnButtonTag == "button_restart"){
			//LoadingSelection.instance.StartLoadingSelection();
			//if(LoadingSelection.instance.IsTimerComplete()){
			isGameOverBridge = false;
            GameManagerShare.SetIsGameOver(false);
			HandCollider2D.handOnButtonTag = "nothing";
			SceneManager.LoadScene("Bridge");//"fase1"
            
			//}
		}else if(HandCollider2D.handOnButtonTag == "button_menu"){
			//LoadingSelection.instance.StartLoadingSelection();
			//if(LoadingSelection.instance.IsTimerComplete()){
			isGameOverBridge = false;
			HandCollider2D.handOnButtonTag = "nothing";
			MouseOnClickWall.goToMiniGames = false;
            GameManagerShare.SetIsGameOver(false);
			SceneManager.LoadScene("StartScreen");
			//}
		}else if(HandCollider2D.handOnButtonTag == "button_minigames"){
			//LoadingSelection.instance.StartLoadingSelection();
			//if(LoadingSelection.instance.IsTimerComplete()){
			isGameOverBridge = false;
			HandCollider2D.handOnButtonTag = "nothing";
			MouseOnClickWall.goToMiniGames = true;
            GameManagerShare.SetIsGameOver(false);
			SceneManager.LoadScene("StartScreen");
			//}
		}
	}*/

	private void CheckTagsAndLoadClick(){
		if(HandCollider2D.handOnButtonTag == "button_restart"){
			SceneManager.LoadScene("fase1");
		}else if(HandCollider2D.handOnButtonTag == "button_menu"){
			MouseOnClickWall.goToMiniGames = false;
			SceneManager.LoadScene("startScreenNew");
		}else if(HandCollider2D.handOnButtonTag == "button_minigames"){
			MouseOnClickWall.goToMiniGames = true;
			SceneManager.LoadScene("startScreenNew");
		}
	}
	
}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

using Share.EventsSystem;
using Share.Managers;
using Goalkeeper.Managers;
using UnityEngine.SceneManagement;

//acho que nao esta sendo usado
public class GameOver_Btn_Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print (this.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
        var btn = HandCollider2D.handOnButtonTag;

        switch (btn)
        {          
            case "button_menu":
				//SoundManager.Instance.ChangeSelection();
                Destroy (GoalkeeperManager.Instance().gameObject);
		        BarController.StopShooting ();
        		Camera.main.GetComponent<BlurOptimized> ().enabled = false;
                HandCollider2D.handOnButtonTag = "nothing";
                MouseOnClickWall.goToMiniGames = false;
                GameManagerShare.SetIsGameOver(false);
				SceneManager.LoadScene("startScreenNew");
                break;
            case "button_restart":
				//SoundManager.Instance.ChangeSelection();
                HandCollider2D.handOnButtonTag = "nothing";
				GameManagerShare.SetIsGameOver(false);
             /*   ScoreManager_GK.instance.playerWon = false;
		        ScoreManager_GK.instance.playerLost = false;
                ScoreManager_GK.instance.SetGoals(0);
                ScoreManager_GK.instance.SetSaves(0);
                ScoreManager_GK.instance.ReloadPenaltyScene();*/
                break;
            case "button_minigames":
				//SoundManager.Instance.ChangeSelection();
                HandCollider2D.handOnButtonTag = "nothing";
                MouseOnClickWall.goToMiniGames = true;
                Destroy (GoalkeeperManager.Instance().gameObject);
		        BarController.StopShooting ();
		        Camera.main.GetComponent<BlurOptimized> ().enabled = false;
				GameManagerShare.SetIsGameOver(false);
				SceneManager.LoadScene("startScreenNew");
                break;
        }
	}

	public void Restart(){
	}

	public void Menu(){
	}

	public void MiniJogos(){
	}
}

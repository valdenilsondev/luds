using UnityEngine;
using System.Collections;
using Assets.Scripts.GameOverScreen;
using System.Collections.Generic;
using UnityEngine.UI;
using Share.Managers;
using Assets.Scripts.Share.Controllers;
//atual
namespace Assets.Scripts.GameOverScreen
{
	public class MenuController : MonoBehaviour
    {
        public List<Button> btn;

        void Update()
        {
            var btn = HandCollider2D.handOnButtonTag;

            if(GameManagerShare.IsGameOver() && btn != string.Empty)
            {
                switch (btn)
                {
					//chamar pop-up do gameover
                    case "button_menu":
						print ("estado do pop-up: " + PauseMenu.instance.GetPop_up());
						if(PauseMenu.instance.GetPop_up() == false){
							//SoundManager.Instance.ChangeSelection();
	                        MouseOnClickWall.goToMiniGames = true;
	                        HandCollider2D.handOnButtonTag = "nothing";
							GameOverScreenController.instance.Disable();
						    PauseMenu.instance.SetPop_up(true); 
							GameManagerShare.instance.PopUp_On();
						//SceneManager.LoadScene("startScreenNew");
						}
                        break;
                    case "button_restart":
						//SoundManager.Instance.ChangeSelection();
                        GameManagerShare.instance.ResetVariablesController();
                        HandCollider2D.handOnButtonTag = "nothing";
                        GameManagerShare.instance.Restart();
                        break;
                    /*case "button_minigames":
						//SoundManager.Instance.ChangeSelection();
                        GameManagerShare.instance.ResetVariablesController();
                        MouseOnClickWall.goToMiniGames = true;
                        //LoadingScreen.instance.LoadScene("startScreenNew");
                        HandCollider2D.handOnButtonTag = "nothing";
                        SceneManager.LoadScene("startScreenNew");
                        break;*/
                }
            }
            
        }

        public void MenuOnClick (string btn)
        {
            switch (btn)
            {
                case "btnMenu":
					//SoundManager.Instance.ChangeSelection();
					MouseOnClickWall.goToMiniGames = true;
  					GameOverScreenController.instance.Disable();
					GameManagerShare.instance.PopUp_On();
                    break;
                case "btnReiniciar":
					//SoundManager.Instance.ChangeSelection();
					GameManagerShare.instance.ResetVariablesController();
                    GameManagerShare.instance.Restart();
                    break;
               /* case "btnMiniGames":
                * nao existe mais
					//SoundManager.Instance.ChangeSelection();
                    GameManagerShare.SetIsGameOver(false);
					GameManagerShare.SetIsStarted(false);
                    MouseOnClickWall.goToMiniGames = true;
                    //LoadingScreen.instance.LoadScene("StartScreen");
					SceneManager.LoadScene("StartScreen");
                    break;*/
                default:
                    break;
            }
        }

        public void MouseEnter(Button btn)
        {
            btn.Active();
        }

        public void MouseExit(Button btn)
        {
            btn.Normal();
        }
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;
using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;
    
	private bool pauseDown = false, pop_Ups = false;

    void Awake()
    {
        instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        var handKinect = HandCollider2D.handOnButtonTag;
     //   if (GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		//{
			if (handKinect != null && handKinect != string.Empty)
	            {
	                switch (handKinect)
	                {
	                    case "button_continue":
							if(GameManagerShare.instance.game == Game.Pig || GameManagerShare.instance.game == Game.Sup){
								if(!CountDownManager.instance.IsCounting())
								{
									handKinect = string.Empty;
									GameManagerShare.instance.PauseOff();									
									CountDownManager.instance.Initialize();
			                        //GameManagerShare.instance.UnPauseGame(); 
									//PowerUpManager.Instance.EnablePowerUp(PowerUps.Shield);
								}
							}else{							
								handKinect = string.Empty;
								GameManagerShare.instance.PauseOff();
								CountDownManager.instance.Initialize();
							}
							break;
						case "button_stop"://****************
							if(pop_Ups == false)
							{
								print("parar");
					            handKinect = string.Empty;
								GameManagerShare.instance.PauseOff();
								pop_Ups = true;
								GameManagerShare.instance.PopUp_On();
							}
							break;
						//pop-up
						case "button_accept":
							handKinect = string.Empty;
							GameManagerShare.instance.ResetVariablesController();
							if(SoundManager.Instance != null)
								SoundManager.Instance.StopBGmusic();
							if(GameManagerShare.instance.game == Game.Pig){
								BorgManager.instance.SendBorg();
							}
							SceneManager.LoadScene("startScreenNew");
							break;
						case "button_decline":
							if(pop_Ups == true){//*****************
								handKinect = string.Empty;
								pop_Ups = false;
								GameManagerShare.instance.PopUp_Off();
								if(GameManagerShare.IsGameOver()) //isGameOverMenu)
								{
									GameOverScreenController.instance.ShowMenuGameOver();
								}
								else{
									GameManagerShare.instance.PauseOn();
								}							
							}
							break;
	                }
	            } 
		pauseDown = false;
        if (BorgManager.instance.borgItemSelected && !pauseDown)
        {
            BorgItemClick(HandCollider2D.handOnButtonName);
        }
    }

	public bool GetPop_up()
	{
		return pop_Ups;
	}

	public void SetPop_up(bool state)
	{
		pop_Ups = state;
	}

    public void BorgItemClick(string id)
    {
        pauseDown = true;
    }


    public void BtnOnKinect()
	{

	}

    public void BtnOnClick(string btn)
    {
        switch (btn)
        {
            case "button_continue":
                GameManagerShare.instance.PauseOff();
				CountDownManager.instance.Initialize();
                //GameManagerShare.instance.UnPauseGame();
                break;
            case "button_stop":
                GameManagerShare.instance.PauseOff();
				GameManagerShare.instance.PopUp_On();
                //GameManagerShare.instance.GameOver();
                break;
			case "button_accept":
				GameManagerShare.instance.ResetVariablesController();
				if(SoundManager.Instance != null)
					SoundManager.Instance.StopBGmusic();
				if(GameManagerShare.instance.game == Game.Pig){
					BorgManager.instance.SendBorg();
				}
				SceneManager.LoadScene("startScreenNew");
				break;
			case "button_decline":
				Debug.Log("qualquer coisa");
				GameManagerShare.instance.PopUp_Off();
				if(GameManagerShare.IsGameOver()) //isGameOverMenu)
				{
					GameOverScreenController.instance.ShowMenuGameOver();
				}
				else{
					GameManagerShare.instance.PauseOn();
				}
				break;
			default:
				print ("none button");
			break;
        }
    }

    public void MouseEnter(Assets.Scripts.GameOverScreen.Button btn)
    {
        btn.Active();
    }

    public void MouseExit(Assets.Scripts.GameOverScreen.Button btn)
    {
        btn.Normal();
    }
}
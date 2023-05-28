using UnityEngine;
using System.Collections;

public class BarnBalloonsShowHide : MonoBehaviour {

	public static BarnBalloonsShowHide instance;

	void Awake () {
		instance = this;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Keypad1)){
			ShowCreditsBallon();
		}
		if(Input.GetKeyDown(KeyCode.Keypad2)){
			ShowRefreshBallon();
		}
		if(Input.GetKeyDown(KeyCode.Keypad3)){
			ShowGamesBallon();
		}
		if(Input.GetKeyDown(KeyCode.Keypad4)){
			HideCreditsBallon();
		}
		if(Input.GetKeyDown(KeyCode.Keypad5)){
			HideRefreshBallon();
		}
		if(Input.GetKeyDown(KeyCode.Keypad6)){
			HideGamesBallon();
		}
	}

	public void HideAllBalloons(){
		HideGamesBallon();
		HideRefreshBallon();
		HideCreditsBallon();
	}

    public void NewState()
    {
        this.GetComponent<Animator>().SetTrigger("NewState");
    }

	public void ShowCreditsBallon(){
		//this.GetComponent<Animator>().SetTrigger("ShowCredits");
        this.GetComponent<Animator>().SetBool("ShowBalloonCredits", true);
	}

	public void HideCreditsBallon(){
		//this.GetComponent<Animator>().SetTrigger("HideCredits");
        this.GetComponent<Animator>().SetBool("ShowBalloonCredits", false);
	}

	public void ShowRefreshBallon(){
		//this.GetComponent<Animator>().SetTrigger("ShowRefresh");
        this.GetComponent<Animator>().SetBool("ShowBalloonRefresh", true);
	}

	public void HideRefreshBallon(){
		//this.GetComponent<Animator>().SetTrigger("HideRefresh");
        this.GetComponent<Animator>().SetBool("ShowBalloonRefresh", false);
	}

	public void ShowGamesBallon(){
		//this.GetComponent<Animator>().SetTrigger("ShowGames");
        this.GetComponent<Animator>().SetBool("ShowBalloonGames", true);
	}

	public void HideGamesBallon(){
		//this.GetComponent<Animator>().SetTrigger("HideGames");
        this.GetComponent<Animator>().SetBool("ShowBalloonGames", false);
	}
}

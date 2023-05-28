using UnityEngine;
using UnityEngine.UI;
using Share.EventsSystem;
using System.Collections;
using Runner.Managers;
using Share.Managers;

public class LoadingSelection : MonoBehaviour {

	//public Text timerText;
	private float timer = 2;
	public Image img;
	private bool isLoading = false;
    private bool loadingComplete;
	private float timerInitial;
	private bool added;
	public static LoadingSelection instance;

	//public AudioSource confirmMenu, changeSelection;

	void Start(){
		DeactivateLoadingSelection();
		instance = this;
		timerInitial = timer;
		loadingComplete = false;
		if (PlayerPrefsManager.GetIsCircleOn () == 0) {
			this.GetComponent<CircleCollider2D> ().enabled = false;
		} else {
			this.GetComponent<CircleCollider2D> ().enabled = true;
		}
	}

	void Update(){
		if(isLoading){
			LoadingStart();
		}
		if (PlayerHandController.instance != null) {
			if (PlayerHandController.instance.isCursorActive ()) 
			{
				this.transform.position = new Vector3 (PlayerHandController.instance.GetPlayerCursor ().x, PlayerHandController.instance.GetPlayerCursor ().y, 
		    	                                   		PlayerHandController.instance.GetPlayerCursor ().z);
				/*if(PlayerHandRaycastUI.instance.GetButtonTag() == "button_restart" ||
				   PlayerHandRaycastUI.instance.GetButtonTag() == "button_menu" ||
				   PlayerHandRaycastUI.instance.GetButtonTag() == "button_minigames"){

				}*/
			}
		}

		if (Input.GetKeyDown (KeyCode.F3)) {
			//desativa o colisor do loading selection
			this.GetComponent<CircleCollider2D>().enabled = !this.GetComponent<CircleCollider2D>().enabled;
			if (this.GetComponent<CircleCollider2D> ().enabled) {
				PlayerPrefsManager.SetCircleOn (1);
			} else {
				PlayerPrefsManager.SetCircleOn (0);
			}
		}
	}

	public void DeactivateLoadingSelection(){		
		//desativa contador visual
		if(img != null)
		img.color = new Color (255,255,255,0);
	}

	private void ActivateLoadingSelection(){		
		//ativa contador visual
		img.color = new Color (255,255,255,255);
	}

	public void StartLoadingSelection(){
		if(!isLoading){
			ActivateLoadingSelection();
			if(SoundManager.Instance != null)
			{
				SoundManager.Instance.ConfirmMenu();
			}
			//checa se esta na posiçao de pause
			isLoading = true;
			print("isloading");
			loadingComplete = false;
			timer = 3;
		}
	}

	public void StopLoadingSelection(){
		//if (SceneManager.GetActiveScene().name == "startScreenNew" && BarnBalloonsShowHide.instance != null)
				//BarnBalloonsShowHide.instance.HideAllBalloons();

			DeactivateLoadingSelection();
			
			//SoundManager.Instance.ChangeSelection();
			ResetTimer();
			//img.fillAmount = 1;
			//timerText.text = ((int)timerInitial).ToString();
			isLoading = false;
			loadingComplete = false;
	}

	public void ResetTimer(){
		if(timer < timerInitial)
		timer = timerInitial;
	}

	public bool GetIsTimerComplete(){
		return loadingComplete;
	}

	public void SetIsTimerComplete(bool state)
	{
		loadingComplete = state;
	}

	//private void ApplySelection(){
	//if (SceneManager.GetActiveScene().name == "startScreenNew") {
			#region old barn code
			/*if (StartScreenKinectController.instance.miniGameStateStartScreen) {
				//DistanceCalibrator.instance.LoadGame();
				StartScreenKinectController.instance.miniGameStateStartScreen = false;
				StartScreenKinectController.instance.isMiniGameActived = true;
				StartScreenKinectController.instance.StartScreenOnMiniGamesEnter ();
				GameObject.Find ("box_minigames").GetComponent<MouseOnClickWall> ().ClickMiniGame ();

				//esconder btns que nao sao importantes para a tela de minigames
				SelectionController.instance.SendAwayNonMiniGamesBtns ();

				//
			} else if (StartScreenKinectController.instance.isMiniGameActived) {
				//
				if (StartScreenKinectController.instance.miniGamesBackBtnStateStartScreen) {
					StartScreenKinectController.instance.isMiniGameActived = false;
					StartScreenKinectController.instance.StartScreenOnMiniGamesExit ();
					MiniGameScreenController.instance.Hide ();
					//MenuController.instance.Hide();
					//CarouselBoxController.instance.Hide();
					GameObject.Find ("box_minigames").GetComponent<MouseOnClickWall> ().MiniGameBackButton ();
					//recuperar os btns
					SelectionController.instance.BringBackNonMiniGamesBtns ();
				}
				if (StartScreenKinectController.instance.miniGameSelectionAreaStateStartScreen) {
					MiniGameScreenController.instance.LoadBoxSelected ();
					//MiniGamesController.instance.LoadGameBasedOnSelected();
				}
				//
			} else if (StartScreenKinectController.instance.creditsStateStartScreen) {
				//
				StartScreenKinectController.instance.creditsStateStartScreen = false;
				StartScreenKinectController.instance.isCreditsActived = true;
				StartScreenKinectController.instance.StartScreenOnCreditsEnter ();
				GameObject.Find ("box_credits").GetComponent<MouseOnClickWall> ().ClickCredits ();
				//
			} else if (StartScreenKinectController.instance.isCreditsActived) {
				//
				if (StartScreenKinectController.instance.creditsBackBtnStateStartScreen) {
					StartScreenKinectController.instance.isCreditsActived = false;
					StartScreenKinectController.instance.StartScreenOnCreditsExit ();
					GameObject.Find ("box_credits").GetComponent<MouseOnClickWall> ().ConfigBackButton ();
				}
				//
			}*/
			#endregion

		//} 
	//}

	private void LoadingStart() {
		timer -= Time.deltaTime;

		//timerText.text = ((int)timer).ToString();
		img.fillAmount = (timer/timerInitial);
		if(timer <= 0) {
			loadingComplete = true;

            //if (SceneManager.GetActiveScene().name == "StartScreen")
			//	ApplySelection();
		}
	}

    public bool IsLoading()
    {
        return isLoading;
    }
}

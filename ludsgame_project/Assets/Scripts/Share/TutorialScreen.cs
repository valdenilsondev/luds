using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;
using Assets.Scripts.Share;
using Runner.Managers;
using Assets.Scripts.Share.Controllers;
using System.Collections.Generic;

public class TutorialScreen : MonoBehaviour {
	float elapsedTime;

	public float timeChangeScreen = 3;
	public GameObject[] screensPR_default;
	public GameObject[] screensPR_elevations;
	public GameObject[] screensGK;
	public GameObject[] screensBridge;
	public GameObject[] screensFishing;
	public GameObject[] screensSup_default;
	public GameObject[] screensSup_elevations;
	public GameObject[] screensThrow;

	public GameObject[] lamps;
	public GameObject[] texts;
	//private int qt_comandos;
	//guarda movimentos do tutorial
	private GameObject tutorialscreen;//tag
	private Game this_game;

	public Sprite enableLamp, disableLamp;
	public static TutorialScreen instance;
	public bool isTutorialOn;

	void Awake(){	
		isTutorialOn = true;
		instance = this;

		foreach(GameObject s in screensPR_default){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensPR_elevations){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensBridge){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensThrow){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensGK){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensFishing){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensSup_default){
			s.GetComponent<Image>().enabled = false;
		}foreach(GameObject s in screensSup_elevations){
			s.GetComponent<Image>().enabled = false;
		}
		//
		foreach(GameObject t in texts){
			t.GetComponent<Text>().enabled = false;
		}
		texts[0].gameObject.GetComponent<Text>().enabled = true;
		//if(GameManagerShare.instance.IsUsingKinect() && PlayerHandController.instance.isCursorActive() == false)
			//PlayerHandController.instance.ShowCursor();
	}

	void Start()
	{

		//ativar somente objetos de tutorial do jogo
		this_game = GameManagerShare.instance.game;
		tutorialscreen = GameObject.FindGameObjectWithTag("tutorial_screen");
		for(int i = 0; i < tutorialscreen.transform.childCount; i++)
		{
			tutorialscreen.transform.GetChild(i).gameObject.SetActive(false);
		}
		switch (this_game)
		{
			case Game.Pig://3,1,2,3,3,2
				if(PlayerPrefsManager.GetIdPerfilPigrunner() == 1){
					tutorialscreen.transform.GetChild(1).gameObject.SetActive(true);
					screensPR_elevations[0].gameObject.GetComponent<Image>().enabled = true;
					//qt_comandos = tutorialscreen.transform.GetChild(1).childCount;
				}
				else if(PlayerPrefsManager.GetIdPerfilPigrunner() == 0){
					tutorialscreen.transform.GetChild(0).gameObject.SetActive(true);
					screensPR_default[0].gameObject.GetComponent<Image>().enabled = true;
					//qt_comandos = tutorialscreen.transform.GetChild(0).childCount;
				}
				break;
			case Game.Goal_Keeper:
				tutorialscreen.transform.GetChild(2).gameObject.SetActive(true);
				screensGK[0].gameObject.GetComponent<Image>().enabled = true;
				//qt_comandos = tutorialscreen.transform.GetChild(2).childCount;
				break;
			case Game.Bridge:
				tutorialscreen.transform.GetChild(3).gameObject.SetActive(true);
				screensBridge[0].gameObject.GetComponent<Image>().enabled = true;
				//qt_comandos = tutorialscreen.transform.GetChild(3).childCount;
				break;
			case Game.Fishing:
				tutorialscreen.transform.GetChild(4).gameObject.SetActive(true);
				screensFishing[0].gameObject.GetComponent<Image>().enabled = true;
				//qt_comandos = tutorialscreen.transform.GetChild(4).childCount;
				break;
			case Game.Throw:
				tutorialscreen.transform.GetChild(5).gameObject.SetActive(true);
				screensThrow[0].gameObject.GetComponent<Image>().enabled = true;
				//qt_comandos = tutorialscreen.transform.GetChild(7).childCount;
				break;
			case Game.Sup:
				if(PlayerPrefsManager.GetIdPerfilSup() == 1){
					tutorialscreen.transform.GetChild(6).gameObject.SetActive(true);
					screensSup_elevations[0].gameObject.GetComponent<Image>().enabled = true;
				}
				else if(PlayerPrefsManager.GetIdPerfilSup() == 0){
					tutorialscreen.transform.GetChild(7).gameObject.SetActive(true);
					screensSup_default[0].gameObject.GetComponent<Image>().enabled = true;
				}
				//qt_comandos = tutorialscreen.transform.GetChild(5).childCount;
				break;

			default:
				break;
		}
		/*screensDefault = new GameObject[qt_comandos];
		screensElevations = new GameObject[qt_comandos];
		texts = new GameObject[qt_comandos];
		lamps = new GameObject[qt_comandos];*/
	}

	void Update () 
	{
	//	if(tutorial_cheked == false)
		//	CheckTutorialType();
		elapsedTime += Time.deltaTime;
		if(elapsedTime > timeChangeScreen){
			elapsedTime = 0;
			CallNextScreen();
		}
		if(isTutorialOn){
			if(GameManagerShare.instance.IsUsingKinect())
				OnHandOverSkipTutorial();
		}
	}

	int nextIndex = 0;
	GameObject[] screens;

	private void CallNextScreen(){
		//define a screen pelo jogo
		print ("index: " + nextIndex);
		switch (this_game){
			case Game.Pig:
				if(PlayerPrefsManager.GetIdPerfilPigrunner() == 1)
					screens = screensPR_elevations; 
				else{
					screens = screensPR_default;
				}
				break;
			case Game.Bridge:
				screens = screensBridge;
				break;
			case Game.Goal_Keeper:
				screens = screensGK;
				break;
			case Game.Throw:
				screens = screensThrow;
				break;
			case Game.Fishing:
				screens = screensFishing;
				break;
			case Game.Sup:
				if(PlayerPrefsManager.GetIdPerfilSup () == 1)
					screens = screensSup_elevations;
				else{
					screens = screensSup_default;
				}
				break;
		}

		foreach(GameObject t in texts){
			t.GetComponent<Text>().enabled = false;
		}
		foreach(GameObject s in screens){
			s.GetComponent<Image>().enabled = false;
		}
		lamps[nextIndex].GetComponent<Image>().sprite = disableLamp;
		nextIndex++;

		if(nextIndex< screens.Length)
		{
			screens[nextIndex].gameObject.GetComponent<Image>().enabled = true;
			texts[nextIndex].gameObject.GetComponent<Text>().enabled = true;
			lamps[nextIndex].GetComponent<Image>().sprite = enableLamp;
		}

		if (nextIndex >= screens.Length)
		{
			if(!GameManagerShare.instance.IsThisGameFirstTime(GameManagerShare.instance.game)){
				GameManagerShare.instance.StartGameCountDown();
			}else{
				print("showpresentation");
				Grandpa_Screen.instance.ShowPresentation();
			}		
			isTutorialOn = false;
			Destroy(this.gameObject);
		}
	}

	public void OnHandOverSkipTutorial(){
		if(HandCollider2D.current_handOnButtonTag == "exit_btn"){
			/*var obj = GameObject.Find(HandCollider2D.current_handOnButtonName).gameObject;
			obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			//resetar escalas
			for(int i = 0; i < StartScreenManager.instance.GetGameSelectionBoxes().Length; i++){
				if(StartScreenManager.instance.GetGameSelectionBoxes()[i].name != HandCollider2D.current_handOnButtonName){
					StartScreenManager.instance.GetGameSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
				}
			}*/
			//selecionar jogo
			if(LoadingSelection.instance.GetIsTimerComplete()){
				SkipTutorial();
			}
		}
	}

	public void SkipTutorial(){
		if(!GameManagerShare.instance.IsThisGameFirstTime(GameManagerShare.instance.game)){
			GameManagerShare.instance.StartGameCountDown();
		}else{print("showpresentation2");
			Grandpa_Screen.instance.ShowPresentation();
		}
		isTutorialOn = false;
		Destroy(this.gameObject);
	}
}

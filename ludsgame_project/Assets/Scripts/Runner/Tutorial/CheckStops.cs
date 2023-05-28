using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CheckStops : MonoBehaviour {

	public bool move;
	public int correctLane;
	public bool roll;
	public bool jump;
	public bool finish;

	private GameObject backBlackground;

	private Text helpText;

	public static CheckStops instance;

	void Start(){
		instance = this;
		helpText = GameObject.Find("HelpText").GetComponent<Text>();

		backBlackground = GameObject.Find("Black Background");

		StartCoroutine(CountDownForStart());
	}

	void OnTriggerEnter(Collider col){
		if(col.name == "pig"){
			print ("enter col stop");
			if(MovePigTutorial.GetCurrentPosition() == correctLane && move){
				//nothing
			}else{
				MoveTutorialMaps.instance.StopMaps();
				SetMessage();
			}
		}
	}

	void OnTriggerStay(Collider col){
		if(col.name == "pig"){
			if(MovePigTutorial.GetCurrentPosition() == correctLane && move){
				MoveTutorialMaps.instance.MoveMaps();
			}
			if(jump && MoveTutorialMaps.instance.GetJumped()){
				MoveTutorialMaps.instance.MoveMaps();
				print ("jumped");
			}
			if(roll && MoveTutorialMaps.instance.GetRolled()){
				MoveTutorialMaps.instance.MoveMaps();
				print ("rolled");
			}
		}
	}

	void OnTriggerExit(Collider col){
		if(col.name == "pig"){
			MoveTutorialMaps.instance.SetJumped(false);
			MoveTutorialMaps.instance.SetRolled(false);
			//onTrigger = false;
			print ("exit col stop");
		}
	}

	public void CheckIfJumpedOnTutorialStop(){
		print ("nome: "+MovePigTutorial.instance.name);
		print ("teste "+MoveTutorialMaps.instance.IsMapStopped()+" "+MovePigTutorial.mainStop.GetComponent<CheckStops>().jump);
		if(MoveTutorialMaps.instance.IsMapStopped() && MovePigTutorial.mainStop.GetComponent<CheckStops>().jump){
			MoveTutorialMaps.instance.SetJumped(true);
		}
	}

	public void CheckIfRolledOnTutorialStop(){
		print ("teste "+MoveTutorialMaps.instance.IsMapStopped()+" "+MovePigTutorial.mainStop.GetComponent<CheckStops>().roll);
		if(MoveTutorialMaps.instance.IsMapStopped() && MovePigTutorial.mainStop.GetComponent<CheckStops>().roll){
			MoveTutorialMaps.instance.SetRolled(true);
		}
	}

	public void BlackBackgroundOn(){
		GameObject.Find("CameraTutorial").GetComponent<BlurOptimized>().enabled = true;
		Color color = backBlackground.GetComponent<Image>().color;
		backBlackground.GetComponent<Image>().color =  new Color(color.r, color.g, color.b, 0.85f);
	}

	public void BlackBackgroundOff(){
		GameObject.Find("CameraTutorial").GetComponent<BlurOptimized>().enabled = false;
		Color color = backBlackground.GetComponent<Image>().color;
		backBlackground.GetComponent<Image>().color =  new Color(color.r, color.g, color.b, 0);
	}

	public void ClearHelpText(){
		helpText.text = "";
	}

	private void SetMessage(){
		if(move){
			if(MovePigTutorial.GetCurrentPosition() > correctLane){
				helpText.text = "Mova-se Para a Esquerda";
				GestureSprites.instance.WalkLeftOn();
			}else{
				helpText.text = "Mova-se Para a Direita";
				GestureSprites.instance.WalkRightOn();
			}
		}else if(roll){
			helpText.text = "Agache-se";
			GestureSprites.instance.SquatOn();
		}else if(jump){
			helpText.text = "Levante a Perna";
			GestureSprites.instance.RaiseLegOn();
		}else if(finish){
			helpText.text = "Fim";
			StartCoroutine(CountDownForLoad());
		}
	}

	IEnumerator CountDownForStart(){
		yield return new WaitForSeconds(0.5f);
		string standard = "Iniciando Tutorial em ";
		helpText.text = standard;;
		yield return new WaitForSeconds(0.5f);
		if(helpText.text == "Iniciando Tutorial em ")
		helpText.text = standard+"3";
		yield return new WaitForSeconds(1f);
		if(helpText.text == "Iniciando Tutorial em 3")
		helpText.text = standard+"2";
		yield return new WaitForSeconds(1f);
		if(helpText.text == "Iniciando Tutorial em 2")
		helpText.text = standard+"1";
		yield return new WaitForSeconds(1f);
		MoveTutorialMaps.instance.MoveMaps();
	}

	IEnumerator CountDownForLoad() {
		yield return new WaitForSeconds(2f);
		helpText.text = "Carregando";

		yield return new WaitForSeconds(0.25f);

		int counter = 0;
		for(int i = 0; i < 6; i++){

			yield return new WaitForSeconds(0.25f);
			if(counter == 3){
				counter = 0;
				helpText.text = "Carregando";
			}else{
				counter++;
				helpText.text = helpText.text+".";
			}

		}
		//colocar funcao de Saulo
		HttpController http = new HttpController();
		http.DisablePigTutorial();
		LoadGame();
	}


	private void LoadGame(){
		SceneManager.LoadScene("PigRunner");
	}
}

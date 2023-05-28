using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {

	public float minimalDistanceToObject;
	private float standardMinimalDistanceToObject;
	private float previousObjectDistToHand;
	private int previousObjectIndex;
	public List<GameObject> interfaceObjects = new List<GameObject>();
	public List<GameObject> interfaceObjectsForGameOver = new List<GameObject>();
	private bool forceMiniGameSelectionArea = false;

	public static SelectionController instance;

	public enum InterfacePauseEnum{
		PauseRestart,
		PauseMenu,
		PauseContinue,
		Nothing
	}

	public enum InterfaceGameOverEnum{
		GameOverRestart,
		GameOverMenu,
		GameOverMiniGame,
		Nothing
	}


	public enum InterfaceStartScreenEnum{
		StartScreenCredits,
		StartScreenMiniGame,
		StartScreenCreditsBackBtn,
		StartScreenMiniGamesBackBtn,
		StartScreenMiniGameSelection,
        StartScreenCategoriesSelection,
		Nothing
	}

	public InterfacePauseEnum selectedObjectInPause;
	public InterfaceGameOverEnum selectedObjectInGameOver;
	public InterfaceStartScreenEnum selectedObjectInStartScreen;
	
	void Start () {
		instance = this;
		standardMinimalDistanceToObject = minimalDistanceToObject;
		previousObjectDistToHand = minimalDistanceToObject;
	}

	public void SendAwayNonMiniGamesBtns(){
		//Credits Area x:-185 and y:0
		interfaceObjects[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000,0);

		//Game Area x:485 and y:-200
		interfaceObjects[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000,0);

		//Credits Exit Btn x: 350 and y: 350
		interfaceObjects[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000,0);
	}

	public void BringBackNonMiniGamesBtns(){
		//Credits Area
		interfaceObjects[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-185,0);
		
		//Game Area
		interfaceObjects[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(485,-200);
		
		//Credits Exit Btn
		interfaceObjects[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(350,350);
	}

	public void SetMinimalDistancetoObject(float value){
		minimalDistanceToObject = value;
	}

	public void ResetMinimalDistanceToObejct(){
		minimalDistanceToObject = standardMinimalDistanceToObject;
	}

	public void ForceMiniGameSelectionArea(){
		forceMiniGameSelectionArea = true;
	}

	public void ResetMiniGameSelectionArea(){
		forceMiniGameSelectionArea = false;
	}

	public GameObject ReturnCloserObjectToHandInPause(){
		Vector3 handPos = PlayerHandController.instance.GetPlayerCursor();
		for(int i = 0; i < interfaceObjects.Count; i++){
			if( Vector3.Distance(interfaceObjects[i].transform.position, handPos) < minimalDistanceToObject ){
				if(Vector3.Distance(interfaceObjects[i].transform.position, handPos) < previousObjectDistToHand || previousObjectIndex == i){
					previousObjectDistToHand = Vector3.Distance(interfaceObjects[i].transform.position, handPos);
					previousObjectIndex = i;
					selectedObjectInPause = (InterfacePauseEnum)i;
					return interfaceObjects[i];//.transform.position;
				}
			}
		}
		previousObjectIndex = -1;
		selectedObjectInPause = InterfacePauseEnum.Nothing;
		return null;//Vector3.zero;
	}

	public GameObject ReturnCloserObjectToHandInGameOver(){
		Vector3 handPos = PlayerHandController.instance.GetPlayerCursor();
		for(int i = 0; i < interfaceObjectsForGameOver.Count; i++){
			if( Vector3.Distance(interfaceObjectsForGameOver[i].transform.position, handPos) < minimalDistanceToObject ){
				if(Vector3.Distance(interfaceObjectsForGameOver[i].transform.position, handPos) < previousObjectDistToHand || previousObjectIndex == i){
					previousObjectDistToHand = Vector3.Distance(interfaceObjectsForGameOver[i].transform.position, handPos);
					previousObjectIndex = i;
					selectedObjectInGameOver = (InterfaceGameOverEnum)i;
					return interfaceObjectsForGameOver[i];
				}
			}
		}
		//previousObjectDistToHand = minimalDistanceToObject;
		previousObjectIndex = -1;
		selectedObjectInGameOver = InterfaceGameOverEnum.Nothing;
		return null;
	}

	public InterfaceStartScreenEnum ReturnCloserObjectToHandInStartScreen(){
		Vector3 handPos = PlayerHandController.instance.GetPlayerCursor();
		//Vector3 handPos = Input.mousePosition;

		if(forceMiniGameSelectionArea)
		{//forcar selecao de minigame
			selectedObjectInStartScreen = (InterfaceStartScreenEnum)4;
			return selectedObjectInStartScreen;
		}

		for(int i = 0; i < interfaceObjects.Count; i++){
			if( Vector3.Distance(interfaceObjects[i].transform.position, handPos ) < minimalDistanceToObject ){
				if(Vector3.Distance(interfaceObjects[i].transform.position, handPos) < previousObjectDistToHand || previousObjectIndex == i){
					previousObjectDistToHand = Vector3.Distance(interfaceObjects[i].transform.position, handPos);
					previousObjectIndex = i;
					selectedObjectInStartScreen = (InterfaceStartScreenEnum)i;
					return selectedObjectInStartScreen;
				}
			}
		}
		previousObjectIndex = -1;
		selectedObjectInStartScreen = InterfaceStartScreenEnum.Nothing;
		return selectedObjectInStartScreen;
	}
}

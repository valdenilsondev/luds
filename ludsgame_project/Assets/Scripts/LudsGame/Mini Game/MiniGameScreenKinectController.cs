using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameScreenKinectController : MonoBehaviour {

	private static List<string> miniGamesNameList = new List<string>();
	private static List<int> miniGamesIndexList = new List<int>();
	private static List<int> miniGamesIndexListFromPP = new List<int>();

	private static int activedButtons;
	private static  GameObject btn_play;
	public GameObject iconsParent;
	public GameObject selectedGamesIconPrefab;
	public Sprite[] selectedGamesIcon;
	private bool moveLeft, moveRight;

	public static MiniGameScreenKinectController instance;
	
	private List<GameObject> selectedIconList;
	// Use this for initialization
	void Awake () {
		selectedIconList = new List<GameObject> ();
		btn_play = GameObject.Find ("ButtonPlay");
		btn_play.SetActive (false);
		instance = this;
	}

	void Start(){
		CreateMiniIcons ();
		SelectIconsUsingPrefab ();
	}

	void Update(){
		if (moveLeft) {
			if(iconsParent.GetComponent<RectTransform>().anchoredPosition.x <= -1839){
				iconsParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1840,0);
				moveLeft = false;
			}else{
				
				iconsParent.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(iconsParent.GetComponent<RectTransform>().anchoredPosition, new Vector2(-1840,0), Time.deltaTime * 5f);
			}

		}

		if (moveRight) {
			if(iconsParent.GetComponent<RectTransform>().anchoredPosition.x >= -1){
				iconsParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
				moveLeft = false;
			}else{
				
				iconsParent.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(iconsParent.GetComponent<RectTransform>().anchoredPosition, new Vector2(0,0), Time.deltaTime * 5f);
			}
			
		}
	}

	public void Play(){
		string gamesString = string.Empty;
		string gamesIndex = string.Empty;

		for (int i = 0; i<miniGamesNameList.Count; i++) {
			gamesString = gamesString+miniGamesNameList[i] + "$";
		
		}
		print (miniGamesIndexList.Count);
		for (int i = 0; i<miniGamesIndexList.Count; i++) {

			gamesIndex = gamesIndex + miniGamesIndexList[i].ToString()+ "$";
		}

		if (gamesString != string.Empty) {
			PlayerPrefsManager.SetMiniGames (gamesString);
		}
		if (gamesIndex != string.Empty) {
			PlayerPrefsManager.SetMiniGamesIndex (gamesIndex);
		}
		print (PlayerPrefsManager.GetMiniGamesIndex());
		SceneManager.LoadScene ("PigRunner");
	}

	//miniIcons que indicam quais jogos estao selecionados
	public void CreateMiniIcons(){
		for (int i = 0; i < selectedGamesIcon.Length ; i++) {
			GameObject selecTedIcon = Instantiate(selectedGamesIconPrefab);
			selecTedIcon.gameObject.transform.SetParent (this.gameObject.transform);
			selecTedIcon.GetComponent<Image>().sprite = selectedGamesIcon[i];
			selecTedIcon.GetComponent<Image>().SetNativeSize();
			selecTedIcon.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			
			selecTedIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX,340);
			posX += 150;
			
			selectedIconList.Add (selecTedIcon);
		}
	}

	public void SelectIconsUsingPrefab(){
		//quebrando a string e jogando numa lista
		string indexList = PlayerPrefsManager.GetMiniGamesIndex ();
		string substring;
		string value = string.Empty;
		for(int i = 0; i<indexList.Length; i++){
			substring = indexList[i].ToString();
			if(substring != "$"){
				value += substring;
			}
			else{

				miniGamesIndexListFromPP.Add(int.Parse(value));
				value = string.Empty;
			}
		}
		//final da quebra
	

		//ativa os itens
		for (int i = 0; i < selectedIconList.Count; i++) {	
			if(miniGamesIndexListFromPP.Contains(i)){
				selectedIconList[i].GetComponent<Image>().color = new Color(1,1,1,1);
				CreateMiniGamesIcons.instance.miniGameIconsList[i].GetComponentInChildren<Toggle>().isOn = true;
			}else{			
				CreateMiniGamesIcons.instance.miniGameIconsList[i].GetComponentInChildren<Toggle>().isOn = false;
				selectedIconList[i].GetComponent<Image>().color = new Color(1,1,1,0.35f);
			}
		}
	}

	float posX = 300;
	public void AddMiniGameToList(string minigame, int index){
		//adiciona na lista de strings e index
		miniGamesIndexList.Add (index);
		miniGamesNameList.Add (minigame);

		selectedIconList [index].GetComponent<Image>().color = new Color(1,1,1,1);
		activedButtons++;
		if (activedButtons == 1) {
			btn_play.SetActive(true);
		}

	}

	
	public void RemoveMiniGameToList(string minigame, int index){
		posX -= 150;
		//remove da lista de strings e index
		miniGamesNameList.Remove (minigame);
		miniGamesIndexList.Remove (index);
		selectedIconList [index].GetComponent<Image>().color = new Color(1,1,1,0.35f);

		activedButtons--;
		if (activedButtons == 0) {
			btn_play.SetActive(false);	
		}
	}

	public void BackButton(){
		SceneManager.LoadScene ("startScreenNew");
	}

	public void MoveToLeft(){
		moveLeft = true;
		moveRight = false;
	}

	public void MoveToRight(){
		moveRight = true;
		moveLeft = false;
	}
}

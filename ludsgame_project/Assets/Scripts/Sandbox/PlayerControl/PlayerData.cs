using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;

using Share.Database;

public enum Action {
	Create,
	Update,
	Delete
}
namespace Sandbox.PlayerControl {
	public class PlayerData : MonoBehaviour {
		
		private Player player;
		private List<Toggle> allPlayers;
		private List<Toggle> allGameRecords;
		private List<string> playersName;
		private float delta = 50f;
		private Action currentAction = Action.Create;
		private string obs = " (Nao Exclua)";
		private Resolution initial;
		private string pathLastImage;
		
		public InputField nameInput;
		public Toggle maleToggle;
		public Toggle femaleToggle;
		public InputField ageInput;
		public ToggleGroup playersToggleGroup; 
		public ToggleGroup gameRecordsToggleGroup;
		public Toggle playerRowPrefab;
		public Toggle playerCell;
		public Toggle gameRecordRowPrefab;
		public Toggle gameRecordCell;
		public ToggleGroup playersSelectorGroup;
		public GameObject maxScore;
		
		public static List<PlayerGameRecord> gameReco = new List<PlayerGameRecord> ();
		
		public static PlayerData instance;
		
		private LayoutElement layoutElement;
		
		void Awake () {
			instance = this;
			
			allPlayers = new List<Toggle>();
			allGameRecords = new List<Toggle>();
			layoutElement = playersSelectorGroup.GetComponent<LayoutElement>();
			
			if(!nameInput || !maleToggle || !femaleToggle || !ageInput){
				Debug.LogError("Faltou arrastar o Game Object");
			}
			initial = Screen.currentResolution;
		}
		
		void Start() {
			SetGroupLayout();
			InitSelectorList();
			
			
		}
		
		
		void Update() {
			if(initial.Equals(Screen.currentResolution)){
				SetGroupLayout();
			}
		}
		
		private void SetGroupLayout() {
			float parentWidth = playersSelectorGroup.transform.parent.GetComponent<RectTransform>().rect.width;
			// set position = (0 ,0)
			playersSelectorGroup.transform.parent.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
			// Set minWidth = parent.width
			layoutElement.minWidth = parentWidth;
			// Set preferredwidth = parent.width
			layoutElement.preferredWidth = parentWidth;
			// set flexibleWidth = parent.width
			layoutElement.flexibleWidth = parentWidth;
		}
		
		private void InitPlayer() {
			string namePlayer = nameInput.text.ToString();
			char gender = ' ';
			int id_fisio = 1;
			
			if(maleToggle.isOn){
				gender = 'M';
			}else{
				gender = 'F';
			}
			
			int age = int.Parse(ageInput.text.ToString());
			
			Player.InitPlayer(namePlayer, gender, age, id_fisio);
		}
		
		private void CreateNewPlayer() {
			InitPlayer();
			
		//	MySQL.instance.InsertNewPlayer(Player.GetNamePlayer(), Player.GetAge(), Player.GetGender(), Player.GetIdFisio());
			
			//Refresh();
			ResetFields();
		}
		
		private void UpdatePlayer() {
			InitPlayer ();
			
		//	MySQL.instance.UpdatePlayer(Player.GetIdPlayer(), Player.GetNamePlayer(), Player.GetGender(), Player.GetAge(), Player.GetIdFisio());

			Refresh();
		}
		
		private void DeletePlayer() {
		//	MySQL.instance.DeletePlayer(Player.GetIdPlayer());
			
			Refresh();
			ResetFields();
		}
		
		public void InitSelectorList() {
			playersName = new List<string>();
		//	playersName = MySQL.instance.SelectAllPlayers();
			ClearPlayerToggles();
			
			for (int i = 0; i < playersName.Count; i++) {
				Toggle tempToggle = Instantiate(playerCell) as Toggle;
				
				tempToggle.isOn = false;
				tempToggle.group = playersSelectorGroup;
				tempToggle.transform.parent = playersSelectorGroup.transform;
				tempToggle.GetComponentInChildren<Text>().text = playersName[i];
				tempToggle.onValueChanged.AddListener( delegate { SelectPlayerToPlay(tempToggle); });
				
				allPlayers.Add(tempToggle);
			}
			
			allPlayers[0].isOn = true;
		}
		
		public void ClearPlayerToggles() {
			foreach (Toggle toggle in allPlayers) {
				playersToggleGroup.UnregisterToggle(toggle);
				Destroy(toggle.gameObject);
			}
			//allPlayers.Clear();
			allPlayers = new List<Toggle>();
		}
		
		public void ResetFields() {
			nameInput.text = "";
			maleToggle.isOn = true;
			femaleToggle.isOn = false;
			ageInput.text = "";
		}
		
		public void SelectPlayerToPlay(Toggle toggleSelected) {
			//MySQL.instance.SelectPlayer(toggleSelected.GetComponentInChildren<Text>().text.ToString ());
		}
		
		public void SelectPlayerToEdit(Toggle toggleSelected) {
			//MySQL.instance.SelectPlayer(toggleSelected.GetComponentInChildren<Text>().text.ToString ());
			
			nameInput.text = Player.GetNamePlayer();
			if(Player.GetGender().Equals('M')){
				maleToggle.isOn = true;
			}else{
				femaleToggle.isOn = true;
			}
			ageInput.text = Player.GetAge().ToString();
		}
		
		public void Refresh() {
			//List<string> players = MySQL.instance.SelectAllPlayers();
			ClearPlayerToggles();
			
	/*		for (int i = 0; i < players.Count; i++) {
				Toggle tempToggle = Instantiate(playerRowPrefab) as Toggle;
				
				tempToggle.isOn = false;
				tempToggle.group = playersToggleGroup;
				tempToggle.transform.parent = playersToggleGroup.transform;
				tempToggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-14, 71-(i * delta));
				tempToggle.GetComponentInChildren<Text>().text = players[i];
				
				tempToggle.onValueChanged.AddListener( delegate { SelectPlayerToEdit(tempToggle); });
				
				if(players[i].Equals("Teste")) {
					tempToggle.GetComponentInChildren<Text>().text += obs;
					tempToggle.interactable = false;
				}
				
				allPlayers.Add(tempToggle);
			}*/
		}
		
		public void ConsultPlayer(){
			
			//maior score do jogador (coins)
		/*	maxScore.GetComponent<Text>().text = "Max Score: " + MySQL.instance.GetPlayerMaxScore(Player.GetIdPlayer());
			
			//caminho para ultima imagem do jogador
			pathLastImage = MySQL.instance.GetPlayerLastImage (Player.GetIdPlayer ());
			
			//lista de todas as partidas do jogador
			gameReco = 	MySQL.instance.GetPlayerGameRecord (Player.GetIdPlayer ());
			int y = 0;
			foreach (PlayerGameRecord p in gameReco) {
				Toggle gr = Instantiate(gameRecordCell) as Toggle;
				
				gr.isOn = false;
				gr.group = gameRecordsToggleGroup;
				gr.transform.parent = gameRecordsToggleGroup.transform;
				
				gr.GetComponentInChildren<Text>().text = " Tresures: " + p.GetNumTreasure().ToString() + "  Coins: " + p.GetCoins().ToString() + 
					"  Data: " + p.GetDatePlayed().ToString() + "  Dificuldade: " + p.GetDifficultGamePreferences().ToString() + "  Posiçao: " + p.GetTreasurePosTpTreasurePosition().ToString();
				
				allGameRecords.Add(gr);
				
			*print("id gamerecord: " + p.GetIdGameRecord().ToString() + " Tresures: " + p.GetNumTreasure().ToString() + " Coins: " + p.GetCoins().ToString() + 
			      "data: " + p.GetDatePlayed().ToString() + " difficult: " + p.GetDifficultGamePreferences().ToString() + " treasure Pos: " + p.GetTreasurePosTpTreasurePosition().ToString());

			}*/
		}
		
		public void CloseGameRecod(){
			foreach (Toggle toggle in allGameRecords) {
				gameRecordsToggleGroup.UnregisterToggle(toggle);
				Destroy(toggle.gameObject);
			}
			//allPlayers.Clear();
			
			allGameRecords = new List<Toggle>();
			
		}
		
		public void SetActionCreate() {
			currentAction = Action.Create;
		}
		
		public void SetActionDelete() {
			currentAction = Action.Delete;
		}
		
		public void SetActionUpdate() {
			currentAction = Action.Update;
		}
		
		public void ExecuteAction() {
			switch (currentAction) {
			case Action.Create:
				CreateNewPlayer();
				break;
			case Action.Delete:
				DeletePlayer();
				break;
			case Action.Update:
				UpdatePlayer();
				break;
			}
		}
	}
}
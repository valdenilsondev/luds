using UnityEngine;
using System.Collections;
using Share.KinectUtils;
using BridgeGame.Player;
using Share.EventSystem;
using System.IO;
using System;
using Ludsgame;
using Share.Managers;

namespace Share.EventsSystem{
	public class GameStartClass : MonoBehaviour  {
		
		private Share.EventsSystem.Events events;
		private bool gameStarted = false;
		private PlayerControl playerControl;
		float gameTime;

		void Awake () {
			events = Events.instance;
			InstancesManager.AddInstance(this);
			playerControl = PlayerControl.GetInstance();
		}

		void Update(){
			if (Input.GetKeyDown (KeyCode.I)) {
			}
			if (gameStarted) {
				gameTime += Time.deltaTime;
			}
		}
		
		void OnEnable () {
		}
		
		void OnDisable () {
		}
		
		public void StartGame() {
			gameStarted = true;
		}
		
		public bool IsGameStarted() {
			return gameStarted;
		}

		public void SetGameStartedToFalse() {
			IncreaseCurrentMatch ();
			SendRoundTime ();
			gameStarted = false;
		}

		public void SetGameStartedToTrue() {
			gameStarted = true;
		}

		private void IncreaseCurrentMatch()
		{
			var currentMatch = PlayerPrefsManager.GetCurrentMatch();
			PlayerPrefsManager.SetCurrentMatch(currentMatch + 1);
		}

		private void SendRoundTime(){
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedRound\" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Hour +
				"-" + System.DateTime.Today.Minute + ".txt";
			
			string roundInfo;
			string id_player = PlayerPrefsManager.GetPlayerID().ToString();
			int id_game = (int) GameManagerShare.instance.GetCurrentGame();
			string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();	
			
			roundInfo = "@" + ((int)gameTime).ToString(FormatConfig.Nfi) + "@" + id_player + "@" + id_game + "@" + current_match;
			System.IO.File.WriteAllText(folder, roundInfo);
			
			new	HttpController().InsertRoundTime(folder);
		}

	}

}
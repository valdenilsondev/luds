using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System;
using UnityEngine.UI;
using Sandbox.PlayerControl;
using Share.Database;
using Share.KinectUtils;

namespace Sandbox.GameUtils {
	public class GameManager : MonoBehaviour {
		
		private static int idGame = 1;
		
		public Text scoreText, relicsText, congratsText;
		public GameObject uiScreen;
		private GameObject userImagePanel;
		public string pathToSaveSS;
		public string pathAndFileName;
		public static GameManager instance;
		public bool isPlaying;

		// Use this for initialization
		void Awake () {
			instance = this;
			userImagePanel = GameObject.Find ("userImagePanel");
		}
		
		void Start () {
			congratsText.text = "Parabéns " + Player.GetNamePlayer ();
			
			
			string playerName;
			if (Player.GetNamePlayer () == " ") {
				playerName = "Teste";		
			} else {
				playerName = Player.GetNamePlayer();
			}
			//Cria uma pasta com o nome do jogador dentro da pasta do jogo em arquivos de programas
			//pathToSaveSS = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)+ "/SandBox Treasure/Screenshots/" + Player.GetIdPlayer() +"/";
			//Directory.CreateDirectory (pathToSaveSS);
			
			//imprime o maior score do player
//			int maxScore = MySQL.instance.GetPlayerMaxScore (Player.GetIdPlayer ());
			//print ("maxScore " + maxScore);
			
			//Kinect.Instance.skeletonRecorder.StartRecording ();
			//Kinect.Instance.canRecord = true;
		}
		
		// Update is called once per frame
		void Update () {
			scoreText.text = ScoreManager.GetGold ().ToString();
			relicsText.text = ScoreManager.GetRelics ().ToString();
		}
		
		public static int GetIdGame() {
			return idGame;
		}
		
		/// <summary>
		/// Prints the user.
		/// </summary>
		public void PrintUser(){
			Texture2D userImage = UserMap.Instance ().GetUsersClrTex (); // userImagePanel.GetComponent<Renderer> ().material.mainTexture as Texture2D;
			string hora = System.DateTime.Now.Hour.ToString ();
			string min = System.DateTime.Now.Minute.ToString ();
			string seg = System.DateTime.Now.Second.ToString ();
			string ms = System.DateTime.Now.Millisecond.ToString ();
			
			string fileName = System.DateTime.Today.Year.ToString () + "-" + System.DateTime.Today.Month.ToString () + "-" +
				System.DateTime.Today.Day.ToString () + "_" + hora + "_" + min + "_" + seg + "_" + ms + "_" + Player.GetIdPlayer();
			
			SaveTextureToFile(userImage, fileName + ".jpg");
		}
		
		/// <summary>
		/// Saves the texture to file.
		/// </summary>
		/// <param name="texture">Texture.</param>
		/// <param name="filename">Filename.</param>
		private void SaveTextureToFile(Texture2D texture , string filename)
		{
			var bytes=texture.EncodeToJPG();
			
			string path = pathToSaveSS + filename;
			pathAndFileName = path;
			FileStream file = File.Open(path, FileMode.Create);
			var binary= new BinaryWriter(file);
			binary.Write(bytes);
			
			file.Close();
		}
		
		public string GetPathToSS(){
			return pathAndFileName;
		}
		
		
		public void SetGUIActive(bool active){
			if (active) {
				uiScreen.gameObject.GetComponent<Animation> ().Play ("ShowScore");
			} else {
				uiScreen.gameObject.GetComponent<Animation> ().Play ("HideScore");
			}
		}
		
		public static GameManager Instance(){
			return instance;
		}

		public void CallPauseScreen(){

		}
	}
}
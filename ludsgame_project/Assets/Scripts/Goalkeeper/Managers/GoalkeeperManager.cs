using UnityEngine;
using System.Collections;
using Share.KinectUtils;
using Share.KinectUtils.Record;
using Share.EventSystem;
using Share.EventsSystem;
using UnityEngine.UI;
using Share.Managers;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using Share.Controllers;
using Assets.Scripts.Share.Enums;
using UnityStandardAssets.ImageEffects;
using Assets.Scripts.Share.Managers;
using System.Linq;
using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share.Kinect;
using Ludsgame;

namespace Goalkeeper.Managers
{
    public class GoalkeeperManager : MonoBehaviour
    {

        public static GoalkeeperManager instance;
        private KinectGestures.Gestures currentGesture;
        private static GameStartClass g;
        private static bool isGameStarted;
        private static float gameTime;
        //  public static bool isGameOverGoalKeeper = false; qual a diferença entre isGameOver?
        public List<ScoreItem> scoreItems;

		public Sprite gameover_board;

       // private int Goals;
		//private int Saves;
		[SerializeField]
        private int GoalsLimit;
		[SerializeField]
        private int SavesLimit;
		//public int difficult;
		private bool showedGameOver;
        public bool playerLost;
        public bool playerWon;

		public bool isReady = false;
		//dados para relatorio
		public int vitoria, derrota;

		public Dictionary<KinectGestures.Gestures, Movement> playerMovements;

        void Awake()
        {
            instance = this;
            g = GameObject.Find("GameManager_Goalkeeper").GetComponent<GameStartClass>();

            //define o peso de cada score Item
            scoreItems = new List<ScoreItem>();
            scoreItems.Add(new ScoreItem { multiplyingFactor = -1, type = ScoreItemsType.Goal });
            scoreItems.Add(new ScoreItem { multiplyingFactor = 8, type = ScoreItemsType.Defense });

            Camera.main.GetComponent<BlurOptimized>().enabled = false;

			playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();
			AddPlayerMovements();
			GoalsLimit = PlayerPrefsManager.GetGoalsLimit();
			SavesLimit = PlayerPrefsManager.GetDefenseLimit();
        }
		void Start()
		{
			GoalKeeperSoundManager.Instance.PlayEnviroment();
			if(SoundManager.Instance != null)
				SoundManager.Instance.StopStartscreenSound();
			/*if(difficult == 1)
			{
				PlayerPrefsManager.SetGoalsLimit(8);
				PlayerPrefsManager.SetDefenseLimit(3) ;
			}
			else if(difficult == 2)
			{
				PlayerPrefsManager.SetGoalsLimit(5);
				PlayerPrefsManager.SetDefenseLimit(3);
			}
			else if (difficult == 3)
			{
				PlayerPrefsManager.SetGoalsLimit(3);
				PlayerPrefsManager.SetDefenseLimit(3);
			}*/
		}

        void Update()
        {
           
            if (!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && (VerifyPlayerWin() || VerifyPlayerLose()) && 
			    GameManagerShare.IsStarted())
            {
                GameManagerShare.instance.PauseGame();
            }

			else if(GameManagerShare.instance.IsUsingKinect())
			{
				if(DistanceCalibrator.instance.CheckOutOfScreen() && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()
				   && GameManagerShare.IsStarted() && !CountDownManager.instance.IsCounting())
				{
					GameManagerShare.instance.PauseGame();
					PauseWithoutBorg();
				}
			}

            if (GameManagerShare.IsStarted() && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
            {
                if (AnimationControllerGoalkeeper.instance.gkAnimationCompleted && (!VerifyPlayerWin() || !VerifyPlayerLose()))
                {
                    AnimationControllerGoalkeeper.instance.gkAnimationCompleted = false;
                    CameraControl.instance.MoveCameraTowards_Kicker();
                    BallControl.instance.GetComponent<Rigidbody>().isKinematic = true;
                    BallControl.instance.InitializeBallPosition();

                    if (!GoalkeeperManager.Instance().VerifyPlayerLose() && !GoalkeeperManager.Instance().VerifyPlayerWin())
                        BarController.StopShooting();  
                }
            }

            if (BorgManager.instance.borgItemSelected && !showedGameOver)
            {
				showedGameOver = true;
				SetVictoryNdefeat();
                GameManagerShare.instance.GameOver();
            }
        }

        public void StartGame()
        {
            BarController.StartShooting();
        }

        public void SetGesture(KinectGestures.Gestures gesture)
        {
            currentGesture = gesture;
        }

        public KinectGestures.Gestures GetGesture()
        {
            return currentGesture;
        }

		public Sprite GetGameOverBoard()
		{
			return gameover_board;
		}

		public int GetVictory(){
			return vitoria;
		}
		public void SetVictory(int value){
			vitoria = value;
		}
		public int GetDefeat(){
			return derrota;
		}
		public void SetDefeat(int value){
			derrota = value;
		}

        /// <summary>
        /// Returns GameManager instance
        /// </summary>
        public static GoalkeeperManager Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GoalkeeperManager>();
                if (instance == null)
                {
                    Debug.LogError("Nenhum GameObject do tipo GameManager foi encontrado");
                }
            }
            return instance;
        }

         public static void UnpauseGame()
        {
           
        }

        private void ShowRaiseHandMsg()
        {
            if (GameObject.Find("Lateral_Dist_Text") != null)
                GameObject.Find("Lateral_Dist_Text").GetComponent<Text>().text = "Levante a mao";
        }

        private void HideRaiseHandMsg()
        {
            if (GameObject.Find("Lateral_Dist_Text") != null)
                GameObject.Find("Lateral_Dist_Text").GetComponent<Text>().text = "Volte pro jogo";
        }

        private static void IncreaseCurrentMatch()
        {
            var currentMatch = PlayerPrefsManager.GetCurrentMatch();
            PlayerPrefsManager.SetCurrentMatch(currentMatch + 1);
        }

        private static void SendRoundTime()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedRound\" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Hour +
                "-" + System.DateTime.Today.Minute + ".txt";

            string roundInfo;
            string id_player = PlayerPrefsManager.GetPlayerID().ToString();
            int id_game = (int)GameManagerShare.instance.GetCurrentGame();
            string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();

            roundInfo = "@" + ((int)gameTime).ToString(FormatConfig.Nfi) + "@" + id_player + "@" + id_game + "@" + current_match;
            System.IO.File.WriteAllText(folder, roundInfo);

            new HttpController().InsertRoundTime(folder);
        }

       /* public int GetGoalsLimit()
        {
            return GoalsLimit;
        }

		public int GetSavesLimit()
        {
            return SavesLimit;
        }

		public void SetGoalsLimit(int value)
        {
            GoalsLimit = value;
        }

		public void SetSavesLimit(int value)
        {
            SavesLimit = value;
        }*/

        public void ShowGameOverScreen()
        {

           // BarController.HideBar();
            //GameManagerShare.instance.GameOver();
            Camera.main.GetComponent<BlurOptimized>().enabled = true;
        }


        private void Lose()
        {
            //play animation lose	
            //AnimationControllerGoalkeeper.instance.PlayAnimation_GK_Sad();
            //GameManagerShare.instance.GameOver();
            //CameraControl.instance.MoveCameraTowards_GK_Zoom();
            //BorgManager.instance.ShowBorgScreen();
            
            playerLost = true;
            playerWon = false;
        }

        private void Win()
        {
            //play animation Win
            //AnimationControllerGoalkeeper.instance.PlayAnimation_GK_Celebrate();
            //GameManagerShare.instance.GameOver();

            playerWon = true;
            playerLost = false;
        }

        public void Pause()
        {

			if(GameManagerShare.instance.IsUsingKinect() == true)
			{
				if(GameManagerShare.instance.stopGesture == true){
					PauseWithoutBorg();
					print ("pause do gesto");
				}
				else if((DistanceCalibrator.instance != null) && (DistanceCalibrator.instance.CheckOutOfScreen()))
				{
					PauseWithoutBorg();
					print ("pause saiu do kinect");
				}
				else if(GameManagerShare.instance.stopGesture == false)
					BorgManager.instance.ShowBorgScreen();
			}
			else{
				CameraControl.instance.MoveCameraTowards_GK_Zoom();
				Camera.main.GetComponent<BlurOptimized> ().enabled = true;
           		BorgManager.instance.ShowBorgScreen();
			}
		}

		
		private void PauseWithoutBorg()
		{
			GameManagerShare.SetIsPaused(true);
			GameManagerShare.instance.PauseOn();
			Camera.main.GetComponent<BlurOptimized> ().enabled = true;
			//print (PlayerHandController.instance.isCursorActive());
			if (PlayerHandController.instance.isCursorActive() == false && GameManagerShare.instance.IsUsingKinect())
			{
				PlayerHandController.instance.ShowCursor();
				LoadingSelection.instance.StartLoadingSelection();
			}
			Camera.main.GetComponent<Animator> ().speed = 0;
			AnimationControllerKicker.instance.GetComponent<Animator> ().speed = 0;

		}

		public void UnPause()
		{		
			Camera.main.GetComponent<BlurOptimized> ().enabled = false;
			/*if (CountDownManager.instance.IsCounting() == false && !BarController.IsShooting()) {	
				BarController.StartShooting ();
			}*/
			//BarController.ShowBar();		
			Assets.Scripts.Share.Controllers.GUIController.instance.Enable ();
			Camera.main.GetComponent<Animator> ().speed = 1;
			AnimationControllerKicker.instance.GetComponent<Animator> ().speed = 1;
		} 

        private void OnGameOver()
        {
            //GameManagerShare.instance.PauseGame();
            //BorgManager.instance.ShowBorgScreen();
        }

		public void Restart(){
			BarController.SetShooting(true);
		}

        public bool VerifyPlayerLose()
        {
            var amount = ScoreManager.instance.scoreItemList.Where(x => x.type == ScoreItemsType.Goal).FirstOrDefault();
            return amount.GetValue() >= GoalsLimit;
        }

        public bool VerifyPlayerWin()
        {
            var amount = ScoreManager.instance.scoreItemList.Where(x => x.type == ScoreItemsType.Defense).FirstOrDefault();
			return amount.GetValue() >= SavesLimit;
        }

		private void SetVictoryNdefeat()
		{
			if(VerifyPlayerWin() == true){
				SetVictory(1);
				SetDefeat(0);
				//PlayerPrefsManager.SetVictory(GetVictory());
				//PlayerPrefsManager.SetDefeated(GetDefeat());
			}else{
				SetVictory(0);
				SetDefeat(1);
				//PlayerPrefsManager.SetVictory(GetVictory());
				//PlayerPrefsManager.SetDefeated(GetDefeat());
			}
		}

        //não ta usando
        public void GameOver()
        {
			BorgManager.instance.borgItemSelected = false;
			BorgManager.instance.SendBorg();
			if(SoundManager.Instance != null){
				SoundManager.Instance.StopBGmusic();
				if(GoalKeeperSoundManager.Instance != null){
					GoalKeeperSoundManager.Instance.StopEnviroment();
				}
				SoundManager.Instance.PlayGameTransition();
			}

			GameOverScreenController.instance.Enable();
            //IncreaseCurrentMatch();
            //SendRoundTime();
        }

        public bool IsReady()
        {
			return isReady;
        }

		public void AddPlayerMovements(){
			KinectGestures.Gestures gStopR = (KinectGestures.Gestures) PlayerPrefsManager.GetStopRightHand ();
			KinectGestures.Gestures gStopL = (KinectGestures.Gestures) PlayerPrefsManager.GetStopLeftHand ();
			
			playerMovements.Add(gStopR, Movement.StopHand);
			playerMovements.Add(gStopL, Movement.StopHand);
		}

		public void ExecuteGesture(KinectGestures.Gestures gesture){
			var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();
			
			if (movement != null)
				BarController.instance.ExecuteMovement (movement);
		}
    }
}
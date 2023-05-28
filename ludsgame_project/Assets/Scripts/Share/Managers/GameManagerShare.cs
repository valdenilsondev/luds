using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Share.EventsSystem;
using Assets.Scripts.Share;
using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share.Enums;
using UnityStandardAssets.ImageEffects;
using Runner.Managers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Share.Managers;
using Share.Controllers;
using Goalkeeper.Managers;
using System.Linq;
using Ludsgame;
using UnityEngine.SceneManagement;

namespace Share.Managers
{
    public class GameManagerShare : MonoBehaviour
    {
        private static bool isStarted, isGameOver, isPaused;
		private GameObject menuContinueStop;

        public bool resetTime = false;
        public static GameManagerShare instance;
		public bool tutorialOff;
        public bool usingKinect;
		private int kinect_situation;
		public Game game;
		private int gesture_choice;//setup de alteraçao de gestos
		private int qtdtrofeus = 0;
		private List<ScoreItem> scoreItemList;
		//gatilho para borg do pigrunner
		[HideInInspector]
		public bool pwborg_enter = false;
		//estado da mao do pause, na pose ou nao
		//public bool stopPosix;
		[HideInInspector]
		public bool press_Stop;
		[HideInInspector]
		public bool ready_to_call_pause = false;
        //saber se executou o gesto
		public bool stopGesture;
		//trigger para entrar so uma vez em NotInPose
		[HideInInspector]
		public static bool one_notInPose;
		//trigger para entrar so uma vez InPose
		[HideInInspector]
		public static bool one_stop = true;
		//contador para pause da mao
		public float count_timer_stop;

		private SpeechManager speechManager;
		private GameObject pauseScreen;

		private void OnEnable()
        {
	        SceneManager.sceneLoaded += OnSceneLoaded;
        }

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			//pig runner
			string gameName = scene.name;
			if (gameName == "PigRunner")
			{
				//print ("recarregou runner");
				if (isStarted)
				{
					isStarted = false;
				}
			}
		}

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
				PauseButton();	              
            }

			if (Input.GetKeyDown (KeyCode.Y)) {
				Time.timeScale = 5;
			}
			if (Input.GetKeyDown (KeyCode.U)) {
				Time.timeScale = 1;
			}
		
			switch (game)
            {
                case Game.Pig:
                   if (PigRunnerManager.instance.IsReady())
                    {				
                        PigRunnerManager.instance.cameraMovementAroundTheWorldCompleted = false;
                        CountDownManager.instance.Initialize();
                    }
                    break;
                case Game.Goal_Keeper:
                    if (GoalkeeperManager.instance.IsReady())
                    {	
						GoalkeeperManager.instance.isReady = false;
	                    CountDownManager.instance.Initialize();
                    }
                    break;
                case Game.Bridge:
					if(BridgeManager.instance.IsReady())	
					{						
						//BridgeManager.instance.HideGUIStartToPlay();
						BridgeManager.instance.SetIsReady(false); 
						CountDownManager.instance.Initialize();	
					}
                    break;
                case Game.Throw:
                    break;
				case Game.Sup:
					break;
				case Game.Fishing:
					break;
                default:
                    break;
            }
        }

        void FixedUpdate()
		{
			SpeechRecognize ();		
		}


        void Awake()
        {
            instance = this;
			kinect_situation = PlayerPrefsManager.GetUsingKinect();

			if(kinect_situation == 0)
				usingKinect = false;
			else
				usingKinect = true;
			if (SceneManager.GetActiveScene().name != "startScreenNew" && SceneManager.GetActiveScene().name != "Calibration")
				menuContinueStop = GameObject.Find("PauseScreen").transform.Find("Menu").gameObject;

        }

        private void Start()
        {
	        gameObject.AddComponent<HttpController>();
	        
	        scoreItemList = GameManagerShare.instance.GetScoreItems();
			speechManager = SpeechManager.Instance;
			stopGesture = false;
			press_Stop = false;
			count_timer_stop = 0;
			//countTimer = 0;
			
			SendPendencyToDatabase ();

			if (SoundManager.Instance != null)
			{
				SoundManager.Instance.PlayBGmusic();
			}
        }

        #region [Gets]

        public static bool IsGameOver()
        {
            return isGameOver;
        }

        public static bool IsPaused()
        {
            return isPaused;
        }

        public static bool IsStarted()
        {
            return isStarted;
        }

        public Game GetCurrentGame()
        {
            return game;
        }
	
        #endregion

        #region [Sets]

        public static void SetIsStarted(bool value)
        {
			print ("isStarted " + value);
            isStarted = value;
        }

        public static void SetIsGameOver(bool value)
        {
            isGameOver = value;
        }

        public static void SetIsPaused(bool value)
        {
            isPaused = value;
        }


        #endregion

        public void OnGameOver()
        {
            isGameOver = true;
        }

        public bool IsUsingKinect()
        {
            return usingKinect;
        }

        public void StartGame()
        {
            isStarted = true;
            isPaused = false;
            isGameOver = false;
			SetGamesFirstTime(game);
            switch (game)
            {
                case Game.Pig:
                    PigRunnerManager.instance.StartGame();
                    break;
                case Game.Goal_Keeper:
					GoalkeeperManager.Instance().StartGame();
                    break;
                case Game.Bridge:
					BridgeManager.instance.StartGame();
                    break;
                case Game.Throw:
					ThrowManager.Instance.StartGame();
                    break;
				case Game.Sup:
					SupManager.instance.StartGame();
					break;
				case Game.Fishing:
					FishingManager.instance.StartGame();
					break;
				default:
                    break;
            }
        }

		public void PauseOn()
		{
			pauseScreen = GameObject.Find("PauseScreen");
			pauseScreen.GetComponent<Animator>().SetTrigger("PauseDown");
			GUIController.instance.Disable();
		}
		
		public void PauseOff()
		{
			pauseScreen = GameObject.Find("PauseScreen");
			pauseScreen.GetComponent<Animator>().SetTrigger("PauseUp");
		}
		public void PopUp_On()
		{
			pauseScreen = GameObject.Find("Pop-upScreen");
			pauseScreen.GetComponent<Animator>().SetTrigger("PauseDown");
		}
		public void PopUp_Off()
		{
			pauseScreen = GameObject.Find("Pop-upScreen");
			pauseScreen.GetComponent<Animator>().SetTrigger("PauseUp");
		}

        public void PauseGame()
        {
            isStarted = true;
            isPaused = true;
            isGameOver = false;
            //startBorgCounter = false;
			GUIController.instance.Disable();
			Camera.main.GetComponent<BlurOptimized>().enabled = true;
            
            switch (game)
            {
                case Game.Pig:
				//precisa pois o pig n entra mais no gameover
					IncreaseCurrentMatch ();
					SendRoundToDatabase ();
					SendMotionToDatabase ();
                    PigRunnerManager.instance.Pause();
                    break;
                case Game.Goal_Keeper:
                    GoalkeeperManager.instance.Pause();
                    break;
                case Game.Bridge:
                    BridgeManager.instance.Pause();
                    break;
                case Game.Throw:
					ThrowManager.Instance.Pause();
                    break;
				case Game.Sup:
					SupManager.instance.Pause();
					break;
				case Game.Fishing:
					FishingManager.instance.Pause();
					break;
                default:
                    break;
            }
        }
		private void PauseButton()
		{
			isStarted = true;
			isPaused = true;
			isGameOver = false;
			//startBorgCounter = false;
			GUIController.instance.Disable();
			Camera.main.GetComponent<BlurOptimized>().enabled = true;
			switch (game)
			{
			case Game.Pig:
				PigRunnerController.instance.Iddle();
				FloorMovementControl.instance.Pause();
				PauseOn();
				break;
			case Game.Goal_Keeper:
				Camera.main.GetComponent<BlurOptimized> ().enabled = true;
				PauseOn();
				break;
			case Game.Bridge:
				BridgeManager.instance.StopPiranhas();
				PauseOn();
				break;
			case Game.Throw:
				ThrowManager.Instance.StopTargets();
				PauseOn();
				break;
			case Game.Sup:
				PauseOn();
				break;
			case Game.Fishing:
				PauseOn();
				break;
			default:
				break;

			}

		}
        public void UnPauseGame()
        {
            isPaused = false;
            //startBorgCounter = true;
            BorgManager.instance.borgItemSelected = false;
			if(GUIController.instance == null){
				print ("is null");
			}
            GUIController.instance.Enable();

            switch (game)
            {
                case Game.Pig:
                    isStarted = true;
					print ("isStarted true");
                    PigRunnerManager.instance.UnPause();
                    break;
                case Game.Goal_Keeper:
					GoalkeeperManager.instance.UnPause();
                    break;
                case Game.Bridge:
                    BridgeManager.instance.UnPause();
                    break;
                case Game.Throw:
					ThrowManager.Instance.UnPause();
                    break;
				case Game.Fishing:
					FishingManager.instance.UnPause();
					break;
                default:
                    break;
            }
        }

        public void GameOver()
        {
			print("gameover");
            isStarted = true;
			//print ("isStarted true");
            isPaused = false;
            isGameOver = true;

			IncreaseCurrentMatch ();
			SendRoundToDatabase ();
			SendMotionToDatabase ();
			Camera.main.GetComponent<BlurOptimized>().enabled = true;
            
            switch (game)
            {
                case Game.Pig:
					PigRunnerManager.instance.GameOver();
                    break;
                case Game.Goal_Keeper:
                    GoalkeeperManager.instance.GameOver();
                    break;
                case Game.Bridge:
					BridgeManager.instance.GameOver();
                    break;
				case Game.Sup:					
					SupManager.instance.GameOver();
					break;
				case Game.Throw:
					ThrowManager.Instance.GameOver();
                    break;
				case Game.Fishing:
					FishingManager.instance.GameOver();
					break;
                default:
                    break;
            }

            GUIController.instance.ShowGameOverScreen();
        }

        public void Restart()
        {
            isStarted = true;
            //print ("isStarted true");
            isPaused = false;
            isGameOver = false;

            switch (game)
            {
                case Game.Pig:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case Game.Goal_Keeper:
					isStarted = false;
					GoalkeeperManager.instance.Restart();
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case Game.Bridge:
					isStarted = false;
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case Game.Throw:
					isStarted = false;
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					break;
				case Game.Sup:
					isStarted = false;
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					break;
				case Game.Fishing:
					isStarted = false;
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					break;
                default:
                    break;
            }
        }

        public void Reset()
        {
            //Camera.main.GetComponent<BlurOptimized>().enabled = true;
            //isStarted = false;
            isGameOver = false;
            resetTime = false;

            switch (game)
            {
                case Game.Pig:
                    PigRunnerManager.instance.Reset();
                    break;
                case Game.Goal_Keeper:
                    break;
                case Game.Bridge:
					BridgeManager.instance.Reset();
                    break;
                case Game.Throw:
                    break;
                default:
                    break;
            }
        }
		public void SetActiveBlur(bool act){
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = act;
		}
		private void SpeechRecognize(){
			// get the speech manager instance
			if (speechManager == null) {
				speechManager = SpeechManager.Instance;
			}
			
			if (speechManager != null && speechManager.IsSapiInitialized ()) {
				if (speechManager.IsPhraseRecognized ()) {
					string sPhraseTag = speechManager.GetPhraseTagRecognized ();
					switch (sPhraseTag) {
					case "STOP":			
						//Chamando a tela de pause que vem apos a borg como pedido por Michel
						//PauseManagerShare.instance.PauseOutOfScreen();
						break;
					}
					
					speechManager.ClearPhraseRecognized();
				}
			}

		}

        public void ExecuteKinectGesture(KinectGestures.Gestures gesture)
        {
            switch (game)
            {
                case Game.Pig:
                    PigRunnerManager.instance.ExecuteGesture(gesture);
                    break;
                case Game.Goal_Keeper:
					GoalkeeperManager.instance.ExecuteGesture(gesture);
                    break;
                case Game.Bridge:
					BridgeManager.instance.ExecuteGesture(gesture);
                    break;    
                case Game.Throw:
					ThrowManager.Instance.ExecuteGesture(gesture); //Bullseye.Player.instance.CallShootAnim(finalHandPosX);
                    break;
					break;
				case Game.Sup:
					SupManager.instance.ExecuteGesture(gesture);
					break;
				case Game.Fishing:
					FishingManager.instance.ExecuteGesture(gesture);
					break;
			default:
				break;
            }
        }

		/*public void ExecuteActionOnHandStop()
		{
				ready_to_call_pause = false;
				if(IsStarted () && !IsPaused() && !IsGameOver() && !CountDownManager.instance.IsCounting())
				{
					stopGesture = true;
					if(KinectGestures.isInPoseStop)
					{
						print ("posicao: " + KinectGestures.isInPoseStop);

						PauseGame();
					}
				}
		}*/

        public void LifeLost()
        {
            GUIController.instance.LifeLost();
        }

		public void IncreaseScore(ScoreItemsType item, float amount, float amount2)//amount e um valor inteiro em segundos
        {
            GUIController.instance.IncreaseScore(item, amount, amount2);
        }


		public int GetQtdTrofeus(){
			if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Pig) {
				return 3;
			}
			if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Goal_Keeper) {
				if (GoalkeeperManager.Instance().VerifyPlayerWin()) {
					var goals = scoreItemList.Where(x => x.type == ScoreItemsType.Goal).FirstOrDefault(); 
					if (goals.GetValue() == 0) {
						return 3;
					}
					if (goals.GetValue() == 1) {
						return 2;
					}
					if (goals.GetValue() == 2) {
						return 1;
					}
				}
				if (GoalkeeperManager.Instance().VerifyPlayerLose()) {
					return 0;
				}
			}
			
			if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Bridge) {
				if(LifesManager.instance.GetCurrentNumberOfHearts() == 3){
					return 3;
				}
				if(LifesManager.instance.GetCurrentNumberOfHearts() == 2){
					return 2;
				}
				if(LifesManager.instance.GetCurrentNumberOfHearts() == 1){
					return 1;
				}
				if(LifesManager.instance.GetCurrentNumberOfHearts() == 0){
					return 3;
				}
				
			}
			
			if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Sup) {
				
				if(SupManager.instance.TotalTimeScore >= 10 && SupManager.instance.TotalAppleScore >= 30){
					print ("blink 3");
					return 3;
				}
				else if(SupManager.instance.TotalAppleScore >= 30){
					print ("blink 2");
					return 2;
				}
				else if(SupManager.instance.TotalAppleScore >= 10 && SupManager.instance.TotalAppleScore < 30){
					print ("blink 1");
					return 1;
				}
				else if(SupManager.instance.TotalAppleScore == 0)
					
				{
					print ("blink 0");
					return 1;
				}
				
			}
			if(GameManagerShare.instance.game == Assets.Scripts.Share.Game.Throw)
			{
				//var apple = scoreItemList.Where(x => x.type == ScoreItemsType.Apple).FirstOrDefault();
				if(ThrowManager.Instance.GetHits() >= 15){
					return 3;
				}
				else if(ThrowManager.Instance.GetHits() >= 9 && ThrowManager.Instance.GetHits() < 15){
					return 2;
				}
				else if(ThrowManager.Instance.GetHits() >= 3 && ThrowManager.Instance.GetHits() < 8 ){
					return 1;
				}
				else if(ThrowManager.Instance.GetHits() == 0)
				{
					return 0;
				}
			}
			
			if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Fishing) {
				if( Assets.Scripts.Share.Managers.ScoreManager.instance.GetScore() > 300){
					return 3;
				}else{
					return 2;
				}
			}
			else{
				return 0;
			}
		}


        public void BlinkStar(int qtt)
        {
            if (qtt == 0)
            {
            }
            if (qtt == 1)
            {
                GameObject.Find("GameOverScreen2").GetComponent<Animator>().SetTrigger("blink1star");
            }
            if (qtt == 2)
            {
				GameObject.Find("GameOverScreen2").GetComponent<Animator>().SetTrigger("blink2star");
            }
            if (qtt == 3)
            {
				GameObject.Find("GameOverScreen2").GetComponent<Animator>().SetTrigger("blink3star");
            }
        }
		//criar os diretórios
		public void SendPendencyToDatabase(){
			string folderMotion = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + @"\insertedMotion\";
			string folderRound = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedRound\";
			string folderBorg = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedBorg\";
			
			//HttpController http =  new HttpController();
			var http = gameObject.GetComponent<HttpController>();
			
			//insert do Motion
			if (Directory.Exists (folderMotion)) {
				//caminho dos arquivos
				string [] filesMotion = Directory.GetFiles (folderMotion);
				for (int i = 0; i < filesMotion.Length; i++) {
					//tenta inserir a string
					//tenta inserir e retorna o resultado
					http.InsertMotion(filesMotion[i]);
				}
			}

			//insert do Round
			if (Directory.Exists (folderRound)) {
				string [] filesRound = Directory.GetFiles (folderRound);	
				for (int i = 0; i < filesRound.Length; i++) {				
					//tenta inserir a string				
					http.InsertRoundTime(filesRound[i]);							
				}
			}

			//insert do borg
			if (Directory.Exists (folderBorg)) {
				string [] filesBorg = Directory.GetFiles (folderBorg);	
				for (int i = 0; i < filesBorg.Length; i++) {
					//tenta inserir a string				
					http.InserBorg(filesBorg[i]);							
				}
			}

		}

		public void SendMotionToDatabase()
		{
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + @"\insertedMotion\";
			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedMotion\" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Now.Hour +
				"-" + System.DateTime.Now.Minute + ".txt";
		
			//varre a lista de gestos e gera uma string para ser inserida
			StringBuilder sb = new StringBuilder();
			if (GestureListener.motionInserts != null) {
				foreach (string st in GestureListener.motionInserts) {
					sb.AppendLine (st);
				
				}			
				//se o diretoria nao exitir sera criado
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				//salva no arquivo texto os dados
				System.IO.File.WriteAllText (path, sb.ToString ());
			
			//	HttpController http =  new HttpController();
				this.gameObject.GetComponent<HttpController>().InsertMotion(path);
			
				//limpa a lista de gestos
				GestureListener.motionInserts.Clear ();
			}
		}
		
		public void IncreaseCurrentMatch()
		{
			var currentMatch = PlayerPrefsManager.GetCurrentMatch();
			PlayerPrefsManager.SetCurrentMatch(currentMatch + 1);
		}
		
		/// <summary>
		/// Envia informaçoes sobre o round jogado
		/// </summary>
		public void SendRoundToDatabase(){
			string folder = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + @"\insertedRound\";

			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedRound\" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Hour +
				"-" + System.DateTime.Today.Minute + ".txt";

			//==== DADOS COMUNS ENTRE OS ROUNDS ==========================
			string roundInfo;
			string id_player = PlayerPrefsManager.GetPlayerID().ToString();
			int id_game = (int)GetCurrentGame ();
			string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();	
			string gameTime = ((int)(Time.timeSinceLevelLoad)).ToString(FormatConfig.Nfi);
			string score = ScoreManager.instance.GetScore ().ToString();
			//=============================================================

			//========================DADOS EXCLUSIVOS DE JOGOS ==========================================
			string applesColetadas = "0";
			string qtdQuedas = "0";
			string tempoInclinado = "0";
			string colisoesPiranha = "0";
			string qtdGoals = "0";
			string qtdDefenses = "0";
			string distanceSup = "0";
			string arremessos = "0";
			string acertos_alvo = "0";
			string erros = "0";
			string qtdCardume = "0";
			string pescados = "0";

			string data_secao = System.DateTime.Today.Month + "-" + System.DateTime.Today.Day + "-" +System.DateTime.Today.Year;
			string tempo_inc_esquerda = "0"; 
			string tempo_inc_direita = "0"; 
			string trofeus = GetQtdTrofeus().ToString();
			string vitorias = "0"; 
			string derrotas = "0";
			string vidas_perdidas = "0";



			if(this.game == Game.Pig){
				vidas_perdidas = PigRunnerManager.instance.GetLifelost().ToString();
				applesColetadas = ScoreManager.instance.GetScoreOfType(ScoreItemsType.Apple).ToString();
			}			
			if(this.game == Game.Bridge){
				qtdQuedas = BridgeManager.instance.GetQtdFalls().ToString();
				colisoesPiranha = BridgeManager.instance.GetQtdPiranhasHits().ToString(); 
				tempoInclinado = BridgeManager.instance.GetInclinationTime().ToString();
				tempo_inc_esquerda = BridgeManager.instance.GetTempoIncLeft().ToString();
				tempo_inc_direita = BridgeManager.instance.GetTempoIncRight().ToString();
				vitorias = BridgeManager.instance.GetVictory().ToString();
				derrotas = BridgeManager.instance.GetDefeat().ToString();
			}
			if(this.game == Game.Goal_Keeper){
				qtdGoals = ScoreManager.instance.GetScoreOfType(ScoreItemsType.Goal).ToString();
				qtdDefenses = ScoreManager.instance.GetScoreOfType(ScoreItemsType.Defense).ToString();
				vitorias = GoalkeeperManager.instance.GetVictory().ToString();
				derrotas = GoalkeeperManager.instance.GetDefeat().ToString();
			}
			if(this.game == Game.Sup){
				applesColetadas = ScoreManager.instance.GetScoreOfType(ScoreItemsType.Apple).ToString();
				distanceSup = "1";
			}
			if(this.game == Game.Throw){
				arremessos = ScoreManager.instance.GetScoreOfType(ScoreItemsType.Apple).ToString();
				acertos_alvo = ScoreManager.instance.GetScoreOfType(ScoreItemsType.Hits).ToString();
				erros = (ScoreManager.instance.GetScoreOfType(ScoreItemsType.Apple) - ScoreManager.instance.GetScoreOfType(ScoreItemsType.Hits)).ToString();
				print("score throw arremessos" + arremessos+ " acertos " + acertos_alvo + " erros " + erros);
			}
			if(this.game == Game.Fishing){
				qtdCardume = FishingManager.instance.GetNumberofFishBySpot().ToString();
				pescados = (ScoreManager.instance.GetScoreOfType(ScoreItemsType.Fish1) + 
				            ScoreManager.instance.GetScoreOfType(ScoreItemsType.Fish2) + 
				            ScoreManager.instance.GetScoreOfType(ScoreItemsType.Fish3)).ToString();
			}
			//===============================================================================================

			// concatenacao de todos os dados exclusivos
			string allExclusiveData =  tempoInclinado + "@" +applesColetadas + "@" + qtdQuedas + 
				"@" + colisoesPiranha + "@" + qtdGoals + "@" + qtdDefenses + "@" + distanceSup +
				"@" + arremessos + "@" + acertos_alvo + "@" + erros + "@" + qtdCardume + "@" + pescados +
				"@" + data_secao + "@" + tempo_inc_esquerda + "@" + tempo_inc_direita + "@" + trofeus
				
			+ "@" + vitorias + "@" + derrotas + "@" + vidas_perdidas;

			roundInfo = "@" + current_match + "@" + id_game + "@" + id_player + "@" + score + "@" + gameTime + "@" + allExclusiveData;
			//print(roundInfo);
			//se o diretoria nao exitir sera criado
			if (!Directory.Exists (folder)) {
				Directory.CreateDirectory (folder);
			}

			File.WriteAllText(path, roundInfo);

			this.gameObject.GetComponent<HttpController>().InsertRoundTime (path);
		}

        public List<ScoreItem> GetScoreItems()
        {
            var list = new List<ScoreItem>();

            switch (game)
            {
                case Game.Pig:
                    list = PigRunnerManager.instance.scoreItems;
                    break;
                case Game.Goal_Keeper:
					list = GoalkeeperManager.Instance().scoreItems;
                    break;
                case Game.Bridge:
					list = BridgeManager.instance.scoreItems;
                    break;
                case Game.Throw:
					list = ThrowManager.Instance.scoreItems;
                    break;
				case Game.Sup:
					list = SupManager.instance.scoreItems;
					break;
				case Game.Fishing:
					list = FishingManager.instance.scoreItems;
					break;
                default:
                    break;
            }

            return list;
        }
    
        public void ResetVariablesController()
        {
            isStarted = false;
            isPaused = false;
            isGameOver = false;
        }		

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			
			if(usingKinect)
				LoadingSelection.instance.StopLoadingSelection();
		}

		public void StartGameCountDown(){
			switch(game){
			case Game.Pig:
				PigRunnerManager.instance.StartCameraAnim();
				break;
			case Game.Goal_Keeper:
				CameraControl.instance.StartCameraAnim();
				break;
			case Game.Bridge:
				CameraIntroControl.instance.StartInitialCameraAnimation();
				GUIController.instance.Disable();
				break;
			case Game.Throw:
				ThrowManager.Instance.StartCount();
				break;
			case Game.Sup:
				SupManager.instance.StartCount();
				break;
			case Game.Fishing:
				FishingManager.instance.StartCount();
				break;
			default:
				print("default: StartGameCountDown  ");
				break;
			}
		}

		char delimiterChar = '@';
		string gamesFirstTimeString;
		string[] gamesFirstTimeList;
		public void SetGamesFirstTime(Game g){
			gamesFirstTimeString = PlayerPrefsManager.GetGamesFirstTime();
			gamesFirstTimeList = gamesFirstTimeString.Split(delimiterChar);
			//print(gamesFirstTimeString);
			switch(game){
			case Game.Pig:
				gamesFirstTimeList[0] = "0";
				break;
			case Game.Goal_Keeper:
				gamesFirstTimeList[1] = "0";	
				break;
			case Game.Bridge:
				gamesFirstTimeList[2] = "0";
				break;
			case Game.Throw:
				gamesFirstTimeList[3] = "0";
				break;
			case Game.Sup:
				gamesFirstTimeList[4] = "0";
				break;
			case Game.Fishing:
				gamesFirstTimeList[5] = "0";
				break;
			default:
				print("setgamesfirstgame");
				break;
			}
			gamesFirstTimeString = "";
			for(int i =0; i<6; i++){
				if(i==5){
					gamesFirstTimeString += gamesFirstTimeList[i];
				}else{
					gamesFirstTimeString += gamesFirstTimeList[i]+"@";
				}
			}
			PlayerPrefsManager.SetGamesFirstTime(gamesFirstTimeString);
		//	print(gamesFirstTimeString);

		}


		public bool IsThisGameFirstTime(Game g){
			gamesFirstTimeString = PlayerPrefsManager.GetGamesFirstTime();
			gamesFirstTimeList = gamesFirstTimeString.Split(delimiterChar);
			string situation = "1";
			switch(g){
			case Game.Pig:
				situation = gamesFirstTimeList[0];
				break;
			case Game.Goal_Keeper:
				situation = gamesFirstTimeList[1];
				break;
			case Game.Bridge:
				situation = gamesFirstTimeList[2];
				break;
			case Game.Throw:
				situation = gamesFirstTimeList[3];
				break;
			case Game.Sup:
				situation = gamesFirstTimeList[4];
				break;
			case Game.Fishing:
				situation = gamesFirstTimeList[5];
				break;
			}
			if(situation == "1"){
				return true;
			}else{
				return false;
			}
		}
    }
}
using Assets.Scripts.Share.Controllers;
using Assets.Scripts.Share.Enums;
using Assets.Scripts.Share.Kinect;
using Share.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine ;
using Share.EventsSystem;
using Assets.Scripts.Share.Managers;
using Share.Controllers;

namespace Runner.Managers
{
    public class PigRunnerManager : MonoBehaviour
    {
        public static PigRunnerManager instance;
        public Dictionary<KinectGestures.Gestures, Movement> playerMovements;
        public List<ScoreItem> scoreItems;
        private int currentPigLevelGroup;
		private int currentPigGroupType;

		public bool cameraMovementAroundTheWorldCompleted = false;
		public GameObject particleDropApples;
        private float time = 10;
        private float elapsedTime = 0;
		[SerializeField]
		private bool move_default;
		private int using_hands;
		//dados relatorio
		private int lifelost;//como ele pega as vidas?

        void Awake()
        {
		
            instance = this;

            scoreItems = new List<ScoreItem>();
            scoreItems.Add(new ScoreItem { multiplyingFactor = 2, type = ScoreItemsType.Apple });
            scoreItems.Add(new ScoreItem { multiplyingFactor = 2, type = ScoreItemsType.Distance });
            //Carregar parametrização feita pela fisioterapeuta. Ex: Levantar a mão faz com que o Pig execute a ação de pular
            playerMovements = new Dictionary<KinectGestures.Gestures, Movement>();
			    
        }

        void Start()
        {
            SetCurrentGroupType();
			SetCurrentLevelGroup();
			GameManagerShare.instance.SetActiveBlur(false);
			if(SoundManager.Instance != null)
				SoundManager.Instance.StopStartscreenSound();
			using_hands =  PlayerPrefsManager.GetIdPerfilPigrunner();//GetPigRunnerSetupMovement();
			AddPlayerMovements ();
			lifelost = 0;
        }

        public void Initialize()
        {
            GUIController.instance.Initialize();
        }

        public void StartGame()
        {
			time = PlayerPrefsManager.GetBorgTime();
			CountMetersRan.instance.Initialize();
			FloorMovementControl.instance.MoveFloor();

			PigRunnerController.instance.Run ();

        }

		public int GetLifelost()
		{
			return lifelost;
		}
		public void IncLifelost(){
			lifelost += 1;
		}
        //pause para o Borg
        public void Pause()
        {
			PigRunnerController.instance.Iddle();
			FloorMovementControl.instance.Pause();
			if(GameManagerShare.instance.IsUsingKinect() == true)
			{
				/*if(GameManagerShare.instance.stopGesture == true){
					PauseWithoutBorg();
					print ("pause do gesto");
				}*/
				if((DistanceCalibrator.instance != null))
				{
					if(DistanceCalibrator.instance.CheckOutOfScreen()){
						PauseWithoutBorg();
					}
					else //if(!KinectGestures.isInPoseStop)
					{					
						BorgManager.instance.ShowBorgScreen();
					}
				}
			}
			else{		
				BorgManager.instance.ShowBorgScreen();
			}
			GameManagerShare.instance.SetActiveBlur(true);
        }

		private void PauseWithoutBorg()
		{
			//gatilho do pigrunner para nao chamar tela de pause repetido apos BORG
			if(GameManagerShare.instance.pwborg_enter == false){
				GameManagerShare.instance.pwborg_enter = true;
				PigRunnerController.instance.Iddle();
				FloorMovementControl.instance.Pause();
				GameManagerShare.instance.stopGesture = false;
				GameManagerShare.instance.press_Stop = false;
				//BorgManager.instance.borgItemSelected = false;//true
				GameManagerShare.instance.PauseOn();
			}
		}

		public void UnPause()
		{
			if (!CountDownManager.instance.IsCounting ()) {
				PigRunnerController.instance.Run ();
			}
            FloorMovementControl.instance.UnPauseGame();
			GameManagerShare.instance.SetActiveBlur(false);
		} 

        //Recomeçar a corrida após perder todas as vidas
        public void Reset()
        {
			PigRunnerController.instance.GetComponent<Animator>().SetTrigger("hitDown");
            StartCoroutine(FloorMovementControl.instance.Slow());
            CameraRunnerController.instance.Shake();

            GUIController.instance.Reset();
        }

        //Reiniciar jogo pelo menu reiniciar
        public void Restart()
        {
            //
        }

        void Update()
		{
            if (LifesManager.instance.IsDead())
            {
				DropApples();
                GameManagerShare.instance.Reset();
            }
			if (Input.GetKeyDown (KeyCode.D)) {
				DropApples();
			}

			if (GameManagerShare.IsStarted() && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
            {
				if(GameManagerShare.instance.IsUsingKinect() && DistanceCalibrator.instance.CheckOutOfScreen())
				{
					GameManagerShare.instance.PauseGame();
				}
                if(elapsedTime >= time)
                {
                    elapsedTime = 0;				
                    GameManagerShare.instance.PauseGame();
                }
                else
                {
                    elapsedTime += Time.deltaTime;
                }
			}
			else if(GameManagerShare.IsStarted() && GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
			{
				if(BorgManager.instance.borgItemSelected){
					print ("borg clicado: :" + BorgManager.instance.borgItemSelected);
					PauseWithoutBorg();
				}
			}
		}

		//Todo jogo deve ter uma checagem  para dar inicio ao countdown, está é a função principal e deve ser usada no gamemanagershared
		public bool IsReady()		
		{
			return cameraMovementAroundTheWorldCompleted;
		}

        public void GameOver()
        {
			BorgManager.instance.borgItemSelected = false;
			BorgManager.instance.SendBorg();
			if(SoundManager.Instance != null){
				SoundManager.Instance.StopBGmusic();
				SoundManager.Instance.PlayGameTransition();
			}
			GameOverScreenController.instance.Enable();
           
        }

        public void ExecuteGesture(KinectGestures.Gestures gesture)
        {
            var movement = playerMovements.Where(item => item.Key == gesture).Select(item => item.Value).FirstOrDefault();

            if(movement != null)
				PigRunnerController.instance.ExecuteMovement(movement);
        }
    
        public void AddScore()
        {
            //GameManagerShare.instance.IncreaseScore(ScoreItems.Apple);
        }

        public int GetCurrentGroupType()
        {
			return currentPigGroupType;
        }

		private void SetCurrentGroupType()
		{
			int value = PlayerPrefsManager.GetPigRunnerGroupType();
			if(value == 0){
				value = 1;
			}
			else if(value == 1){
				value = 2;
			}
			else if (value == 2){
				value = 3;
			}
			currentPigGroupType = value;
		}

		public int GetCurrentLevelGroup()
		{
			return currentPigLevelGroup;
		}

		public void SetCurrentLevelGroup()
		{
			int value= PlayerPrefsManager.GetPigRunnerLevelGroup();
			//mapear para o enum: 0 => 1 ou 2 | 1 => 3 | 2 => 4 ou 5
			if(value==0){
				value= UnityEngine.Random.Range(1,3);
			}
			else if(value == 1){
				value= 3;
			}
			else if(value==2){
				value = UnityEngine.Random.Range(4,6);
			}
			currentPigLevelGroup = value;
		}

		//ninguem usa
		public void LifeLost()
        {
            GameManagerShare.instance.LifeLost();
        }
		
		public void DropApples(){
			//particleDropApples.SetActive (true);
			Vector3 pos = new Vector3 (PigRunnerController.instance.transform.position.x, PigRunnerController.instance.transform.position.y + 0.5f, PigRunnerController.instance.transform.position.z);
			GameObject p = Instantiate (particleDropApples, pos, Quaternion.identity) as GameObject;	
			Destroy (p, 1);
		}

		public void AddPlayerMovements(){
			if(using_hands == 0)
			{
				KinectGestures.Gestures gJump1 = (KinectGestures.Gestures) PlayerPrefsManager.GetJump1 ();
				KinectGestures.Gestures gJump2 = (KinectGestures.Gestures) PlayerPrefsManager.GetJump2 ();
				KinectGestures.Gestures gSquat = (KinectGestures.Gestures) PlayerPrefsManager.GetSquat ();
				KinectGestures.Gestures gJMovLeft = (KinectGestures.Gestures) PlayerPrefsManager.GetMoveLeft ();
				KinectGestures.Gestures gJMovRight = (KinectGestures.Gestures) PlayerPrefsManager.GetMoveRight ();
				playerMovements.Add(gJump1, Movement.Jump);
				playerMovements.Add(gJump2, Movement.Jump);
				playerMovements.Add(gSquat, Movement.Roll);
				playerMovements.Add(gJMovLeft, Movement.MoveLeft);
				playerMovements.Add(gJMovRight, Movement.MoveRight);
			}
			else if (using_hands == 1){
				KinectGestures.Gestures gElevationF = (KinectGestures.Gestures) PlayerPrefsManager.GetTwoArmsUp ();
				playerMovements.Add(gElevationF, Movement.Jump);
				print ("pig com as maos");

			//KinectGestures.Gestures gStopR = (KinectGestures.Gestures) PlayerPrefsManager.GetStopRightHand ();
			//KinectGestures.Gestures gStopL = (KinectGestures.Gestures) PlayerPrefsManager.GetStopLeftHand ();
			//KinectGestures.Gestures gElevationR = (KinectGestures.Gestures) PlayerPrefsManager.GetRightArmElevation ();
			//playerMovements.Add(gStopR, Movement.StopHand);
			//playerMovements.Add(gStopL, Movement.StopHand);
			//playerMovements.Add(gElevationR, Movement.MoveRight);

			}
		}

		public void StartCameraAnim(){
			Camera.main.GetComponent<Animator>().SetTrigger("EnterCameraAnim");
		}

    }
}

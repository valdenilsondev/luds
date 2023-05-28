using UnityEngine;
using System.Collections;

using Sandbox.GameUtils;
using Sandbox.PlayerControl;
using Sandbox.UI;
using Share.EventsSystem;
using Share.KinectUtils.Record;

namespace Sandbox.Level {
	public class LevelManager : MonoBehaviour {
		
		public static LevelManager instance;
		
		public GameObject x;
		
		Sprite[] iconsSprites;
		
		public TimeManagerSandbox timeManager;
		
		private int level;
		private int configLevel;
		private int maxLevel = 5;
		private int rndRelic = 0;
		public bool isGameStarted = false;
		
		//private SoundManager soundManager;
		private Events events;
		private SkeletonRecorder skeletonRecorder;
		
		void Awake() {
			instance = this;
			
			//soundManager = SoundManager_Sandbox.Instance();
			events = Events.instance;
		///	skeletonRecorder = SkeletonRecorder.Instance;
			timeManager = new TimeManagerSandbox();
		}
		
		// Use this for initialization
		void Start () {
			iconsSprites =  Resources.LoadAll<Sprite>("Sandbox/Icons");
			print (iconsSprites.Length);
		}
		int index = -1;

		private void HidePresentationSandbox(){
			Sandbox.GameUtils.GameManager.instance.isPlaying = true;
			TimeBar.instance.StartTimeBar();
			Sandbox.GameUtils.GameManager.Instance().SetGUIActive (true);
			GameObject.Find("Intro_Screen").GetComponent<Animation>().Play("HidePresentationScreen");
		}

		void Update () {
			if(Input.GetKeyDown(KeyCode.K)){
				HidePresentationSandbox();
				Events.RaiseEvent<PushGestureGame>();
			}

			if(PauseManager.isPaused) {
				return;
			}
			if(GameManager.instance.isPlaying&& !isGameStarted) {
				if(Input.GetKeyDown(KeyCode.Space)){
					StartGame();
				}
			}else if(GameManager.instance.isPlaying && isGameStarted){
				
				if(Input.GetKeyDown(KeyCode.Space)){				
					//soundManager.Play(Sounds.Digging);
					//referencia para o x do level creator
					x = LevelCreator.instance.ReturnLevelX();		

					if(x.transform.GetChild(CheckClosestItem.instance.closest).gameObject.activeSelf == true && 
					   x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite.Equals(iconsSprites[0])){
						
						if(MatrixManager.instance.TestIfItsTreasure()){
							//visual treasure
							MenuManager.isUserPlaying = false;
							x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[MatrixManager.instance.ReturnValueInEnum()];
							Invoke("CallNextLevel", 2f);
							DontPauseOnInvoke();
							TimeBar.instance.PauseTimeBar(true);
						}
						
						switch(MatrixManager.instance.ReturnNameInEnum()){
							
						case SceneObjects.Treasure:	
							break;
							
						case SceneObjects.Relic:
							//metodo
							DefineRelicTexture();
							break;
							
						case SceneObjects.Enemy:
							//soundManager.Play(Sounds.EnemyLaugh);
							x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[MatrixManager.instance.ReturnValueInEnum()];
							break;
							
						case SceneObjects.None:
							x.transform.GetChild(CheckClosestItem.instance.closest).gameObject.SetActive(false);
							break;
							
						default:
							x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[MatrixManager.instance.ReturnValueInEnum()];
							break;
						}
						
						ScoreManager.AddScore(MatrixManager.instance.ReturnNameInEnum());
					} 				
				}
			}
			if(!MenuManager.isUserPlaying && !LevelManager.instance.isGameStarted){
				DetectorAnimationController.instance.SetHandSprite();
			}else if(MenuManager.isUserPlaying){
				DetectorAnimationController.instance.SetDetectorSprite();
			}
		}
		
		private void OnEnable() {
			Events.AddListener<PushGestureGame> (LevelManagerPush);
							 
			//events.AddListener<PushGestureGame>(LevelManagerPush);
		}
		
		private void OnDisable() {
			//events.RemoveListener<PushGestureGame>(LevelManagerPush);
			Events.RemoveListener<PushGestureGame> (LevelManagerPush);
		}
		
		private void DontPauseOnInvoke() {
			//metodo criado para evitar pausar durante a chamada do invoke (bug: o array so era destruido na chamada do CallNextLevel());
			for(int i = 0; i < x.transform.childCount; i++){
				if(x.transform.GetChild(i).tag == "pauseCollider"){
					x.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
		}
		
		private void DefineRelicTexture(){
			if(rndRelic == 0){
				rndRelic = Random.Range(5,9);
			}
			
			x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[rndRelic];
		}
		
		private void LevelManagerPush() {
			if(PauseManager.isPaused) {
				return;
			}
			
			if(GameManager.instance.isPlaying && !isGameStarted){
				StartGame();
			}else if(GameManager.instance.isPlaying && isGameStarted){
				//soundManager.Play(Sounds.Digging);
				
				//referencia para o x do level creator
				x = LevelCreator.instance.ReturnLevelX();
				
				if(x.transform.GetChild(CheckClosestItem.instance.closest).gameObject.activeSelf == true && 
				   x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite == iconsSprites[0]){
					
					//se achar o treasure
					if(MatrixManager.instance.TestIfItsTreasure()){
						//visual treasure
						MenuManager.isUserPlaying = false;
						x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[MatrixManager.instance.ReturnValueInEnum()];
						Invoke("CallNextLevel", 2f);
						DontPauseOnInvoke();
						TimeBar.instance.PauseTimeBar(true);
					}
					
					switch(MatrixManager.instance.ReturnNameInEnum()){
						
					case SceneObjects.Treasure:	
						break;
						
					case SceneObjects.Relic:
						//metodo
						DefineRelicTexture();
						break;
						
					case SceneObjects.Enemy:
						//soundManager.Play(Sounds.EnemyLaugh);
						x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[MatrixManager.instance.ReturnValueInEnum()];
						break;
						
					case SceneObjects.None:
						x.transform.GetChild(CheckClosestItem.instance.closest).gameObject.SetActive(false);
						break;
						
					default:
						x.transform.GetChild(CheckClosestItem.instance.closest).GetComponent<SpriteRenderer>().sprite = iconsSprites[MatrixManager.instance.ReturnValueInEnum()];
						break;
					}
					
					ScoreManager.AddScore(MatrixManager.instance.ReturnNameInEnum());
				}
			}
			if(!GameManager.instance.isPlaying&& !LevelManager.instance.isGameStarted){
				DetectorAnimationController.instance.SetHandSprite();
			}else if(GameManager.instance.isPlaying){
				DetectorAnimationController.instance.SetDetectorSprite();
			}
		}
		
		public void StartGame() {
			timeManager.StartTimer();
			//Tira print do jogador ao iniciar o jogo
			//GameManager.instance.PrintUser ();
			
			GameStartedOnComplete ();
			TimeBar.instance.StartTimeBar();
			
			//chamar start do level creator
			LevelCreator.instance.StartGame();
			
			//iniciar matriz logica
			MatrixManager.instance.InitMatrix();

			Share.EventsSystem.Events.RaiseEvent<Share.EventsSystem.GameStart> ();
		}
		
		private void GameStartedOnComplete() {
			isGameStarted = true;
		}
		
		private void CallNextLevel() {
			GameObject.Destroy(x);
			
			MenuManager.isUserPlaying = true;
			
			//valor rnd da reliquia
			rndRelic = 0;
			
			//level creator chamar proximo level
			LevelCreator.instance.CreateNextLevel();
			
			//iniciar matriz logica
			MatrixManager.instance.InitMatrix();
			
			//resetar barra tempo
			TimeBar.instance.ResetTimeBar();
			TimeBar.instance.PauseTimeBar(false);
		}
		
		public int GetCurrentLevel() {
			return level;
		}
		
		public int GetCurrentConfigLevel() {
			return configLevel;
		}
		
		public void SetCurrentConfigLevel(int value) {
			configLevel = value;
			level = value;
		}
		
		public void NextLevel() {
			if(ScoreManager.GetRelics()%2 == 0){
				if(level < maxLevel){
					level++;
				}
			}
		}
		
		public bool IncrementLevel() {
			if(level < maxLevel){
				level++;
				configLevel++;
				return true;
			}
			return false;
		}
		
		public bool DecrementLevel(){
			if(level > 0){
				level--;
				configLevel--;
				return true;
			}
			return false;
		}
		
		public void ResetLevel() {
			level = configLevel;
		}
		
		public int GetPieceIndex(Transform piece){
			//referencia para o x do level creator
			x = LevelCreator.instance.ReturnLevelX();
			
			if(x == null || x.transform.childCount <=0){
				return -1;
			}
			
			for(int i =0; i < x.transform.childCount; i++){
				if(x.transform.GetChild(i) == piece){
					return i;
				}
			}
			return -1;
		}
	}
}
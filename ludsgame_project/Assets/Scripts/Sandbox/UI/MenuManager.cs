using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using Sandbox.GameUtils;
using Sandbox.Level;
using Sandbox.PlayerControl;
using Sandbox.UI;
using Share.Database;
using Share.EventSystem;

public class MenuManager : MonoBehaviour {

	//menu inicial
	private bool jogar = false;
	private bool config = false;
	private bool sair = false;


	//referencias para os pais dos objetos pushable
	private GameObject initialMenuParent;


	//extras
	private Transform shovel;
	public static Transform hand;
	private static Transform presentationBG;
	public Transform gameBG;

	private bool pushed = false;
	public bool usingMouse = false;
	public int smaller = 0;
	private Transform masterTransform;

	private float itweenSpeed = 0.5f;

	//estaticos
	private static bool inGame = false;
	private static bool check = false;
	public static bool isUserPlaying = false;
	private static bool backFromGame = false;
	private static bool isGameOver = false;
	private static bool isOnGamePresentation = false;

	public static MenuManager instance;
	
	//private SoundManager soundManager;
	private Events events;
	private KinectWrapper.NuiSkeletonPositionIndex handJoint;

	void Awake(){
		//soundManager = SoundManager_Sandbox.Instance();
		events = Events.instance;
		//dica OP
		masterTransform = this.transform;
	
		instance = this;

	}

	// Use this for initialization
	void Start () {
		//ao iniciar onMenu verdadeiro
	

		//referencia para a pa
 		shovel = GameObject.Find("Shovel").transform;

		if(usingMouse){
			GameObject.Find("leftHand").transform.gameObject.SetActive(false);
			hand = GameObject.Find("rightHand").transform;
			CallPresentation();
			check = true;
			DetectorAnimationController.instance.SetHandDetector(hand);
			//ao comentar esse trecho lembrar de comentar segunda linha do metodo GetWhatMouseIsOver(), onde a pos do cubo recebe a pos do mouse
			//e verificar se o GameObject Zig esta desativado
		}
	}	

	// Update is called once per frame
	void Update () {
		if (check) {
			GetWhatMouseIsOver();

			GoToCloserPushableObject();	
		}
	}

	void OnEnable() {
		events.AddListener<PushGestureMenu>(PushGesture);
	}

	void OnDisable() {
		events.RemoveListener<PushGestureMenu>(PushGesture);
	}

	private void PushGesture(PushGestureMenu pushGesture) {
		Push();
	}

	private bool ItweenStop(){
		if(iTween.tweens.Count == 0){
			iTween.Stop();
			return true;
		}
		// Na verdade era false
		// Retornando TRUE para resolver bug do push. Push nao funcionava quando o jogador ia pra cena de REPLAY
		return true;
	}

	public static void BackFromGame() {
		backFromGame = true;
	}

	public static void SetCheck(bool value){
		check = value;
	}

	private void OnlyThisBoolTrue(){
		//gerencia dos booleanos dos menus feita aqui
	
	}
	//public Text t;
	//private int cont = 0;
	private void Push() {
		//cont++;
		//t.text = cont.ToString();
		//chamar push aqui
		//soundManager.Play(Sounds.BubblePop);
		pushed = true;
		OnlyThisBoolTrue();
	}

	private void GoToCloserPushableObject(){
		//chamar push aqui
		//if(!LevelManager.instance.isGameStarted && Input.GetKeyDown(KeyCode.Space)){
		if(Input.GetKeyDown(KeyCode.Space)){
			//soundManager.Play(Sounds.BubblePop);
			pushed = true;

			OnlyThisBoolTrue();
		}
	}
	bool hasSeenLogo =false;
	public void SetHandReference(bool isRight){
		if (isRight) {
			handJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
			hand = GameObject.Find("rightHand").transform;	
			GameObject.Find("leftHand").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("rightHand").GetComponent<SpriteRenderer>().enabled = true;
		} else {
			handJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
			GameObject.Find("leftHand").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("rightHand").GetComponent<SpriteRenderer>().enabled = false;
			hand = GameObject.Find("leftHand").transform;
		}
		CheckClosestItem.instance.SetHand (hand);
		
		if(!LevelManager.instance.isGameStarted){
			if(!hasSeenLogo){
				hasSeenLogo = true;
				CallPresentation ();
			}else{
				CallIntro();
			}

			inGame = true;
		}else{
			CallGame();
		}
	}

	public KinectWrapper.NuiSkeletonPositionIndex GetHandJoint() {
		return handJoint;
	}

	private void CallPresentation(){
		//if(!check){

		check = true;

		SetBoolPlaceOnIntro(8);
		//}
	}

	private void CallIntro(){
		//if(!check){
			iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("x", 0, "time", itweenSpeed));
			presentationBG.gameObject.SetActive (true);
			presentationBG.transform.position = new Vector3(presentationBG.transform.position.x, presentationBG.transform.position.y, 1.58611f);	
			check = true;
			
			gameBG.gameObject.SetActive (true);
			SetBoolPlaceOnIntro(1);
		//}
	}

	private static void CallGame(){
		Camera.main.transform.position = new Vector3 (0, 12, -10);
		presentationBG.gameObject.SetActive (false);	
		MenuManager.instance.SetBoolPlaceOnIntro (5);
	}

	public void CallIntroFromEndGame(){
		//if(!check){
		presentationBG.gameObject.SetActive (true);
			iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("y", 0, "time", itweenSpeed));
		//	check = true;
			SetBoolPlaceOnIntro(1);
			LevelManager.instance.isGameStarted = false;
			//destruir jogo
			if(GameObject.Find("Levels").transform.childCount > 0){
				//GameObject.Destroy( GameObject.Find("Levels").transform.GetChild(0).gameObject);
			}
		//}
		DetectorAnimationController.instance.SetHandSprite();
	}

	private void GetWhatMouseIsOver(){
		//chamar tracking da mao aqui
		if(usingMouse){
			hand.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}

		if(hand != null){
			hand.position = new Vector3(hand.transform.position.x, hand.transform.position.y, -1);
		}else{		
			hand = GameObject.Find("rightHand").transform;
		}			

	
	}

	public void SetBoolPlaceOnIntro(int value){

	}

	public void MoveGUIToCenter(){
	/*	iTween.MoveTo (GameManager.instance.score.gameObject, iTween.Hash ("x", 0.5f, "y", 0.2f));
		iTween.MoveTo (GameManager.instance.scoreText.gameObject, iTween.Hash ("x", 0.5f, "y", 0.2f));
		
		iTween.MoveTo (GameManager.instance.relics.gameObject, iTween.Hash ("x", 0.5f, "y", 0.2f));
		iTween.MoveTo (GameManager.instance.relicsText.gameObject, iTween.Hash ("x", 0.5f, "y", 0.2f));*/
	}


	public void MoveGUIToTopDesactive(){
	/*	iTween.MoveTo (GameManager.instance.score.gameObject, iTween.Hash ("x", 0f, "y", 1, "onComplete", "OnCompleteSetScoreFalse", "oncompletetarget", this.gameObject));
		iTween.MoveTo (GameManager.instance.scoreText.gameObject, iTween.Hash ("x", 0f, "y", 1, "onComplete", "OnCompleteSetScoreTextFalse", "oncompletetarget", this.gameObject));
		
		iTween.MoveTo (GameManager.instance.relics.gameObject, iTween.Hash ("x", 1, "y", 1, "onComplete", "OnCompleteRelicsFalse", "oncompletetarget", this.gameObject));
		iTween.MoveTo (GameManager.instance.relicsText.gameObject, iTween.Hash ("x", 1, "y", 1, "onComplete", "OnCompleteRelicsTextFalse", "oncompletetarget", this.gameObject));*/
	}

	public void MoveGUIToTopActive(){
	/*	iTween.MoveTo (GameManager.instance.score.gameObject, iTween.Hash ("x", 0f, "y", 1));
		iTween.MoveTo (GameManager.instance.scoreText.gameObject, iTween.Hash ("x", 0f, "y", 1));
		
		iTween.MoveTo (GameManager.instance.relics.gameObject, iTween.Hash ("x", 1, "y", 1));
		iTween.MoveTo (GameManager.instance.relicsText.gameObject, iTween.Hash ("x", 1, "y", 1));*/
	}

	void OnCompleteSetScoreFalse(){
		//GameManager.instance.score.gameObject.SetActive (false);
	}

	void OnCompleteSetScoreTextFalse(){
		GameManager.instance.scoreText.gameObject.SetActive (false);
	}

	void OnCompleteRelicsFalse(){
	//	GameManager.instance.relics.gameObject.SetActive (false);
	}

	void OnCompleteRelicsTextFalse(){
		GameManager.instance.relicsText.gameObject.SetActive (false);
	}


	private void PrepareGame(){
		SetBoolPlaceOnIntro(0);//jogar
		isUserPlaying = true;
		TimeBar.instance.StartTimeBar();
		TimeBar.instance.PauseTimeBar(true);
		GameManager.instance.SetGUIActive (true);
	}

	private void DoTransition(){

	}























}

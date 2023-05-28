using UnityEngine;
using System.Collections;
using Assets.Scripts.Share.Kinect;
using System.Collections.Generic;
using Runner.Pool;
using Runner.Managers;
using Assets.Scripts.Share.Managers;
using Share.Managers;
using Assets.Scripts.Share.Enums;
using Share.EventsSystem;
using Assets.Scripts.Share.Controllers;

public class Batata_Fishing_Control : MonoBehaviour {

	public static Batata_Fishing_Control instance;
	public GameObject[] batata_e_barco;

	private GameObject batata_obj;
	//spot de pesca
	private int indexRdSpot;
	[HideInInspector]
	public Animator batata_fishing_animator;

	//checa se continua na posiçao do gesto
	//public KinectGestures gestoStop;

	// Use this for initialization
	void Awake () {
		instance =  this;
		foreach(GameObject respawn in batata_e_barco){
			respawn.gameObject.SetActive(false);
		}
		//indexRdSpot = PlayerPrefsManager.GetFishingDifficult();
		//random
		indexRdSpot = UnityEngine.Random.Range(0,6);
		//indexRdSpot = 5;
		RespawnFishing();
	}

	void Start()
	{
		//RespawnFishing();
		BatataFishAnimatorController.instance.PlayBatataIdle();
	}
	

	private void RespawnFishing()
	{
		//sao 6 spots, so ativa o menino do spot setado na dificuldade do playerprefs
		//o animator controler eh buscado apenas no menino ativo
		if(indexRdSpot == 0){
			batata_e_barco[0].gameObject.SetActive(true);
			//referencia para achar os peixes
			batata_obj = batata_e_barco[0];
			batata_fishing_animator = GameObject.Find("1Barco e Menino").transform.Find("batata_fishing").GetComponent<Animator>();

		}
		else if(indexRdSpot == 1){
			batata_e_barco[1].gameObject.SetActive(true);
			batata_obj = batata_e_barco[1];
			batata_fishing_animator = GameObject.Find("2Barco e Menino").transform.Find("batata_fishing").GetComponent<Animator>();
		}
		else if(indexRdSpot == 2){
			batata_e_barco[2].gameObject.SetActive(true);
			batata_obj = batata_e_barco[2];
			batata_fishing_animator = GameObject.Find("3Barco e Menino").transform.Find("batata_fishing").GetComponent<Animator>();
		}
		else if(indexRdSpot == 3){
			batata_e_barco[3].gameObject.SetActive(true);
			batata_obj = batata_e_barco[3];
			batata_fishing_animator = GameObject.Find("4Barco e Menino").transform.Find("batata_fishing").GetComponent<Animator>();
		}
		else if(indexRdSpot == 4){
			batata_e_barco[4].gameObject.SetActive(true);
			batata_obj = batata_e_barco[4];
			batata_fishing_animator = GameObject.Find("5Barco e Menino").transform.Find("batata_fishing").GetComponent<Animator>();
		}
		else if(indexRdSpot == 5){
			batata_e_barco[5].gameObject.SetActive(true);
			batata_obj = batata_e_barco[5];
			batata_fishing_animator = GameObject.Find("6Barco e Menino").transform.Find("batata_fishing").GetComponent<Animator>();
		}
	}

	public GameObject GetBarco(){
		return batata_obj;
	}

	public void ExecuteMovement(Movement movement)
	{
		switch (movement)
		{
		case Movement.Pull:
			ExecuteActionOnPull();
			break;
		case Movement.Throw:
			ExecuteActionOnThrow();
			break;
		case Movement.BigPullR:
			//puxao forte
			ExecuteActionOnBigPull_Right();
			break;
		case Movement.BigPullL:
			ExecuteActionOnBigPull_Left();
			break;
		/*case Movement.StopHand://testar
			if(GameManagerShare.instance.ready_to_call_pause)
				GameManagerShare.instance.ExecuteActionOnHandStop();
			break;*/
		default:
			print("outro gesto " + movement.ToString());
			break;
		}
	}
	//girar carretel
	private void ExecuteActionOnPull(){
		if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting())
		{
			//girando carretel
			PullBaitControl.instance.Pull();
		}
	}
	//puxao forte
	private void ExecuteActionOnBigPull_Right(){
		if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting())
		{
			PullBaitControl.instance.BigPullRight();
		}
	}
	private void ExecuteActionOnBigPull_Left(){
		if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting())
		{
			PullBaitControl.instance.BigPullLeft();
		}
	}
	//arremeçar isca
	private void ExecuteActionOnThrow(){
		print("executethrow");
		if(!GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting()
		   && (!PullBaitControl.instance.pullingRodHope || !PullBaitControl.instance.bigRodHopePull))
		{
			if(!PullBaitControl.instance.IsBaitThrow()){
				PullBaitControl.instance.ThrowBait();
				GameManagerShare.instance.IncreaseScore(Assets.Scripts.Share.Enums.ScoreItemsType.Bait, 1,0);
			}
		}
	}

	public int GetIndexRdSpot()
	{
		return indexRdSpot;
	}
	public void SetIndexRdSpot(int index)
	{
		indexRdSpot = index;
	}
	/*private void ExecuteActionToCancelStop()
	{
		LoadingSelection.instance.StopLoadingSelection();
		LoadingSelection.instance.SetIsTimerComplete(false);
	}*/
}

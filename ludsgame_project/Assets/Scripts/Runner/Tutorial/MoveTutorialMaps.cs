using UnityEngine;
using System.Collections;

public class MoveTutorialMaps : MonoBehaviour {

	//bools
	private bool jumped = false;
	private bool rolled = false;

	//var publicas
	public float speed;
	public float distFactor;

	//estados
	private bool ableToMove = false;

	//ref. porco
	private GameObject pig;

	//estados
	private bool isMapStopped = true;

	public static MoveTutorialMaps instance;

	void Awake(){
		instance = this;
	}

	void Start () {
		print ("para iniciar/parar mov. dos mapas do tutorial aperte barra de espaco");

		//ref porco
		pig = GameObject.Find("pig").gameObject;

		//pig anim iddle
		pig.GetComponent<Animator>().SetTrigger("iddle");
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(ableToMove){
				StopMaps();

			}else{
				MoveMaps();
			}
		}
		if(ableToMove)
		StartMoving();
	}

	public bool IsMapStopped(){
		return isMapStopped;
	}

	public bool GetJumped(){
		return jumped;
	}

	public void SetJumped(bool var){
		jumped = var;
	}

	public bool GetRolled(){
		return rolled;
	}

	public void SetRolled(bool var){
		rolled = var;
	}

	public void MoveMaps(){
		ableToMove = true;
		pig.GetComponent<Animator>().SetTrigger("running");
		isMapStopped = false;

		CheckStops.instance.ClearHelpText();
		GestureSprites.instance.AllSpritesOff();

		CheckStops.instance.BlackBackgroundOff();
	}

	public void StopMaps(){
		ableToMove = false;
		pig.GetComponent<Animator>().SetTrigger("iddle");
		isMapStopped = true;

		CheckStops.instance.BlackBackgroundOn();
	}

	public void StartMoving(){
		Vector3 actual = this.transform.position;
		this.transform.position = Vector3.MoveTowards(actual, new Vector3(actual.x, actual.y, actual.z - distFactor), speed * Time.deltaTime);
	}
}

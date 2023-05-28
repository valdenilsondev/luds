using UnityEngine;
using System.Collections;
using VacuumShaders.CurvedWorld; 
using Runner.Managers;
using Share.Managers;
using UnityEngine.SceneManagement;

public class PlayerTrailMovement : MonoBehaviour {

	private GameObject myPlayer;

	//0 == left, 1 == middle, 2 == right
	private static int currentPosition;

	public float playerY;
	public float playerZ;

	public float leftPosition;
	public float middlePosition;
	public float rightPosition;

	public float playerMovementSpeed;
	public float cameraMovementSpeed;

	private Camera mainCamera;

	private static bool floor3Ways;
	public static PlayerTrailMovement instance;

	RaycastHit hit;

	void Awake(){
		instance = this;
	}

	void Start () {
		mainCamera = Camera.main;
		currentPosition = 1;
		myPlayer = this.gameObject;
		myPlayer.transform.position = new Vector3(middlePosition,playerY,playerZ);
		//this.GetComponent<Animator>().SetTrigger("running");
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			GoLeft();
		}else if(Input.GetKeyDown(KeyCode.RightArrow)){
			GoRight();
		}else if(Input.GetKeyDown(KeyCode.UpArrow)){
			PlayerJumpSlideControl.instance.Jump();
			//this.GetComponent<Animator>().SetTrigger("jumping");

		}else if(Input.GetKeyDown(KeyCode.DownArrow)){
			PlayerJumpSlideControl.instance.Roll();
			//this.GetComponent<Animator>().SetTrigger("slide");
		}

		//necessario para saber se o player pode se mover ou nao (bifurcacao)
		if (SceneManager.GetActiveScene().name == "PigRunner" && FloorMovementControl.instance.GetPermisionToMove ()) {
			IfCurrentFloorTag ();
		}
		//
		ControlPlayerAnimation();

		if(currentPosition == 0){
			MoveLeft();
		}else if(currentPosition == 1){
			MoveMiddle();
		}else if(currentPosition == 2){
			MoveRight();
		}
	}

	public static int GetCurrentPosition(){
		return currentPosition;
	}

	public static void SetCurrentPosition(int value){
		currentPosition = value;
	}

	public void GoLeft(){
		//print("goleft");
		//print(currentPosition);
		//print("floor3Ways "+ floor3Ways);
		//print("FloorMovementControl.instance.IsFloorMoving() " + FloorMovementControl.instance.IsFloorMoving());

		if(currentPosition > 0 && floor3Ways && FloorMovementControl.instance.IsFloorMoving()){
			//print("goleft entrou no if");
			currentPosition--;
			PigRunnerSoundManager.Instance.PlayChangeLane();
			this.GetComponent<Animator>().SetTrigger("goLeftLane");
		}
	}

	public void GoRight(){
		//instance.GetComponent<Animator>().SetTrigger("lane");
		//instance.GetComponent<Animator>().SetBool("running", false);
		if(currentPosition < 2 && floor3Ways && FloorMovementControl.instance.IsFloorMoving()){	
			currentPosition++;
			PigRunnerSoundManager.Instance.PlayChangeLane();
			this.GetComponent<Animator>().SetTrigger("goRightLane");
		}
	}

	public float GetCameraMovementSpeed(){
		return cameraMovementSpeed;
	}

	private void ControlPlayerAnimation(){
		if(FloorMovementControl.instance.IsFloorMoving() == false && GameManagerShare.IsPaused() == true){//PauseManager.IsPaused() == true){
			if(myPlayer.GetComponent<Animator>().enabled == true)
			myPlayer.GetComponent<Animator>().enabled = false;
		}else if(FloorMovementControl.instance.IsFloorMoving() == true && GameManagerShare.IsPaused() == false){// PauseManager.IsPaused() == false){
			if(myPlayer.GetComponent<Animator>().enabled == false)
			myPlayer.GetComponent<Animator>().enabled = true;
		}

	}

	private void IfCurrentFloorTag(){
		if (Physics.Raycast(this.transform.Find("Raycast").transform.position, Vector3.up, out hit)){
			if(hit.collider.gameObject.tag == "Floor3Ways" || hit.collider.gameObject.tag == "TutorialMap"  /*retirar depois */){
				floor3Ways = true;
				//mover camera para posicao padrao
				MoveCameraToStandardPosition();
			}else{
				floor3Ways = false;
				//mover camera para perto do player
				MoveCameraCloseToPlayer();
			}
		}else{
			
		}
	}

	public static bool IsFloor3Ways() {
		return floor3Ways;
	}

	private void MoveCameraToStandardPosition(){
        //if (PlayerCollision.movingToMiniGame == false) {
        //    mainCamera.transform.position = Vector3.Lerp (mainCamera.transform.position,
        //                                             new Vector3 (this.transform.position.x, 5, -4), cameraMovementSpeed * Time.deltaTime);
        //    CurvedWorld_Controller.get._V_CW_Bend_Y = Mathf.Lerp(CurvedWorld_Controller.get._V_CW_Bend_Y, 
        //                                                                           FloorMovementControl.instance.GetRandomYCurveFloor(), 0.5f * Time.deltaTime);
        //}

	}

	private void MoveCameraCloseToPlayer(){
        //if (PlayerCollision.movingToMiniGame == false) {
        //    mainCamera.transform.position = Vector3.Lerp (mainCamera.transform.position,
        //                                                 new Vector3 (this.transform.position.x, 2, -4), cameraMovementSpeed * Time.deltaTime);
        //    if(PlayerTrailMovement.currentPosition>0)
        //        CurvedWorld_Controller.get._V_CW_Bend_Y = Mathf.Lerp(CurvedWorld_Controller.get._V_CW_Bend_Y, 20, 0.5f * Time.deltaTime);
        //    else
        //        CurvedWorld_Controller.get._V_CW_Bend_Y = Mathf.Lerp(CurvedWorld_Controller.get._V_CW_Bend_Y, -20, 0.5f * Time.deltaTime);
        //}
	}

	private void MoveCameraToCurrentLane(){
        //if (PlayerCollision.movingToMiniGame == false) {
        //    mainCamera.transform.position = Vector3.Lerp (mainCamera.transform.position,
        //                                                  new Vector3 (this.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z), cameraMovementSpeed * Time.deltaTime);
        //}
	}

	private void MoveLeft(){
		myPlayer.transform.position = Vector3.Lerp(myPlayer.transform.position,
		                                           new Vector3(leftPosition, /*myPlayer.transform.position.y*/-1, myPlayer.transform.position.z),
		                                           playerMovementSpeed * Time.deltaTime );
	}

	private void MoveMiddle(){
		myPlayer.transform.position = Vector3.Lerp(myPlayer.transform.position,
		                                           new Vector3(middlePosition, /*myPlayer.transform.position.y*/-1, myPlayer.transform.position.z),
		                                           playerMovementSpeed * Time.deltaTime );
	}

	private void MoveRight(){
		myPlayer.transform.position = Vector3.Lerp(myPlayer.transform.position,
		                                           new Vector3(rightPosition, /*myPlayer.transform.position.y*/-1, myPlayer.transform.position.z),
		                                           playerMovementSpeed * Time.deltaTime );
	}





}

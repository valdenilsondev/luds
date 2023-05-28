using UnityEngine;
using System.Collections;
using Assets.Scripts.Share.Kinect;

public class MovePigTutorial : MonoBehaviour {
	private KinectManager kinect;
	private static int currentPosition;
	private GameObject myPlayer;

	public float playerY;
	public float playerZ;
	
	public float leftPosition;
	public float middlePosition;
	public float rightPosition;
	public float playerMovementSpeed;
	public float cameraMovementSpeed;

	private float movingSideAmount;	//marcha lateral
	public GameObject simpleHitParticle, hitStarsParticle;
	public static GameObject mainStop;

	public static MovePigTutorial instance;
	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		kinect = KinectManager.Instance;
		currentPosition = 1;
		myPlayer = this.gameObject;
		myPlayer.transform.position = new Vector3(middlePosition,playerY,playerZ);

		movingSideAmount = PlayerPrefsManager.GetMovingSideAmount ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			GoLeft();
		}else if(Input.GetKeyDown(KeyCode.RightArrow)){
			GoRight();
		}

		if(kinect != null)
			MovingToSides ();

		if(currentPosition == 0){
			MoveLeft();
		}else if(currentPosition == 1){
			MoveMiddle();
		}else if(currentPosition == 2){
			MoveRight();
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.name == "Stop"){
			mainStop = col.gameObject;
		}else if(col.name == "Apple(Clone)"){
			GameObject.Destroy(col.gameObject);
		}else{
			GameObject.Destroy(col.gameObject);
			InstantiateHitParticlesCopy();
		}
	}

	private void InstantiateHitParticlesCopy()
	{
		Instantiate(simpleHitParticle, this.transform.position, Quaternion.identity);
		
		Transform posToInstantiate = GameObject.Find("hair_01").transform;
		GameObject particleStar = Instantiate(hitStarsParticle, new Vector3(posToInstantiate.transform.position.x + 0.45f,
		                                                                    posToInstantiate.transform.position.y + 1,
		                                                                    posToInstantiate.transform.position.z), Quaternion.identity) as GameObject;
		particleStar.transform.SetParent(posToInstantiate);
	}


	private void MovingToSides(){
		if(AllowKinectToMovePlayerIf()){	
			float playerPos = kinect.GetUserPosition(kinect.GetPlayer1ID()).x;		
			if (playerPos <= movingSideAmount && playerPos >= -movingSideAmount) {
				SetCurrentPosition(1);
			}
			if(playerPos > movingSideAmount){		
				SetCurrentPosition(2);
			}
			if(playerPos < -movingSideAmount){			
				SetCurrentPosition(0);
			}
		}
	}

	private bool AllowKinectToMovePlayerIf(){
		if(currentPosition>= 0 /*&& !MoveTutorialMaps.instance.IsMapStopped()*/){
			return true;
		}
		return false;
	}

	public static int GetCurrentPosition(){
		return currentPosition;
	}

	public void GoLeft(){
		if(currentPosition > 0){
			//SetCurrentPosition(currentPosition-1);
			currentPosition--;
			this.GetComponent<Animator>().SetTrigger("goLeftLane");
		}
	}
	
	public void GoRight(){	
		if(currentPosition < 2){	
			currentPosition++;	
			this.GetComponent<Animator>().SetTrigger("goRightLane");
		}
	}


	private static int posAnt;
	public static void SetCurrentPosition(int value){
		posAnt = currentPosition;
		currentPosition = value;
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

	public void ExecuteMovement(Movement movement)
	{
		switch (movement)
		{
		case Movement.Jump:
			Jump();
			break;
		case Movement.Roll:
			Roll();
			break;
		case Movement.MoveLeft:
			break;
		case Movement.MoveRight:
			break;
		default:
			//print("outro gesto " + movement.ToString() );
			break;
		}
	}


	public void Jump(){
		this.GetComponent<Animator>().SetTrigger("jumping");				
		//soundManager.PlayJump();
		CheckStops.instance.CheckIfJumpedOnTutorialStop();
		print ("pulou");
	}
	
	public void Roll(){
		this.GetComponent<Animator>().SetTrigger("slide");		
	//	soundManager.PlayRoll();
		CheckStops.instance.CheckIfRolledOnTutorialStop();
		print ("agachou");
	}
	
}

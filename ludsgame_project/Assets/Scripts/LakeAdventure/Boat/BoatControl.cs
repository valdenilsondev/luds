using UnityEngine;
using System.Collections;
using Share.Managers;

//fishing respaw control
public class BoatControl : MonoBehaviour {

	//public int boatSpeed;
	public int distToFinish;
	public int cameraSpeed;

	private bool moveBoat = false;
	private GameObject boatAndBoy;
	private GameObject fishingSpotReference;
	private Vector3 targetToLookAt;

	public static BoatControl instance;

	void Awake(){
		instance = this;

	}

	void Start(){
		FishingManager.instance.FillFishList();	
		RopeManager.instance.DestroyRope();
		/*if(boatSpeed == 0){
			boatSpeed = 3;
		}*/
		if(distToFinish == 0){
			distToFinish = 1;
		}
		if(cameraSpeed == 0){
			cameraSpeed = 2;
		}
		boatAndBoy = this.gameObject;
		//cria a linha
		MoveCameraToSpot();
	}

	void Update(){
		/*if(moveBoat){
			//mover barco em direcao ao ponto de pesca
			boatAndBoy.transform.position = Vector3.MoveTowards(boatAndBoy.transform.position, new Vector3(fishingSpotReference.transform.position.x,
			                                                                                        boatAndBoy.transform.position.y,
			                                                                                        fishingSpotReference.transform.position.z), boatSpeed * Time.deltaTime);
			FishingSoundManager.Instance.PlayBoatMoving();

			if(Vector3.Distance(boatAndBoy.transform.position, fishingSpotReference.transform.position) < distToFinish){
				//quando o barco estiver proximo, a camera se move em direcao ao ponto
				MoveItToFishingSpotComplete();
			}

			if(Vector3.Distance(boatAndBoy.transform.position, fishingSpotReference.transform.position) < distToFinish){
				//barco ira parar quando estiver bem proximo do ponto
				moveBoat = false;
				//CountDownManager.instance.Initialize();
				print ("boat movement completed");
				//barco ira olhar para target
				BoatLookAtTarget();
			}
		}*/
	}

	public void MoveItToFishingSpot(GameObject fishingSpot){
		if(fishingSpot == null){
			return;
		}
		//movimentando o barco para o destino de pesca
		fishingSpotReference = fishingSpot;
		iTween.LookTo(boatAndBoy, iTween.Hash("looktarget", fishingSpotReference.transform.position, "time", 1, "easytype", iTween.EaseType.linear) );
		moveBoat = true;
		FishingManager.instance.FillFishList();

		RopeManager.instance.DestroyRope();
	}

	/*private void BoatLookAtTarget(){
		targetToLookAt = fishingSpotReference.GetComponent<FishingSpot>().GetTargetToLookAtboy();
		iTween.LookTo(boatAndBoy, iTween.Hash("looktarget", new Vector3(targetToLookAt.x, boatAndBoy.transform.position.y, targetToLookAt.z),"oncomplete", "BoatMovementComplete",
		                                      "oncompletetarget", this.gameObject, "time", 1));
	}*/

	/*private void MoveItToFishingSpotComplete(){
		CameraMovementControlFish.instance.SendCameraToFishingSpot(fishingSpotReference, cameraSpeed);
		FishingSoundManager.Instance.StopBoatMoving();
		//FishingSoundManager.Instance.PlayBoatIddle();
	}*/

	//rotacao e movimento
	private void MoveCameraToSpot(){
		//fishingSpotReference = obj spot
		//CameraMovementControlFish.instance.SendCameraToFishingSpot(fishingSpotReference, cameraSpeed);
		//CameraMovementControlFish.instance.ChooseCameraSPot();
		FishingSoundManager.Instance.PlayBoatIddle();
		//RopeManager.instance.CreateRopeLine();
		//RopeManager.instance.RopeToRestingPoint();
	}
}

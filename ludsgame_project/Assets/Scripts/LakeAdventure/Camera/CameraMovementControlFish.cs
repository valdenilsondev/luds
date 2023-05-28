using UnityEngine;
using System.Collections;

public class CameraMovementControlFish : MonoBehaviour {

	//variaveis publicas
	public int distanceToWater;

	//camera
	private GameObject cameraFishing;

	//fishing spot
	//private GameObject fishingSpotReference;
	//private Vector3 fishingSpotReference;

	//posicoes do target e destination
	private Vector3 targetToLookOnComplete;
	private Vector3 destination;
	private Vector3 targetToLook;

	//posicao inicial da camera
	private Vector3 cameraInitialPos;
	private Vector3 cameraInitialRot;

	//gatilho que verifca se a camera esta no ponto de pesca
	private bool isCameraOnFishingSpot = false;

	//gatilho que verica se completou itweens
	private bool sendCameraToFishingSpotCompleted = false;
	private bool sendCameraToStandardPosCompleted = false;

	//variavel que verifica se a camera ja esta proxima o suficiente do ponto de pesca para olhar para o target
	private bool cameraCloseEnoughToSpot = false;
	//camera destination spots
	[SerializeField]
	public GameObject[] cameraDestination;
	private Transform cam_parent;
	private int spot;

	public static CameraMovementControlFish instance;

	void Awake(){
		instance = this;
		//spot = PlayerPrefsManager.GetFishingDifficult();
		//spot = Batata_Fishing_Control.instance.GetIndexRdSpot();
	}

	void Start () {
		spot = Batata_Fishing_Control.instance.GetIndexRdSpot();
		//referencia para a camera principal do jogo de pesca
		cameraFishing = GameObject.Find("CameraFishing").gameObject;
		//pegar referencia do batata ativo
		cam_parent = Batata_Fishing_Control.instance.GetBarco().transform;
		cameraFishing.transform.SetParent(cam_parent);
		//desativando animator da cam - workaround bug itween x animator

		if(distanceToWater == 0){
			distanceToWater = 15;
		}
		//desativar
		//cameraFishing.GetComponent<Animator>().enabled = false;

		//referencia para posicao e rotacao inicial da camera
		//cameraInitialPos = cameraFishing.transform.position;
		//cameraInitialRot = new Vector3 (90, 90, 0);
		cameraFishing.transform.position = cameraDestination[spot].transform.position;
		//nao usa mais
		if(spot == 0){
			cameraFishing.transform.rotation = Quaternion.Euler(0f,18.47f,0f);
		}else if(spot == 1){
			cameraFishing.transform.rotation = Quaternion.Euler(0f,334.75f,0f);
		}else if(spot == 2){
			cameraFishing.transform.rotation = Quaternion.Euler(0f,39.29f,0f);
		}else if(spot == 3){
			cameraFishing.transform.rotation = Quaternion.Euler(0f,88.97f,0f);
		}else if(spot == 4){
			cameraFishing.transform.rotation = Quaternion.Euler(0f,124.94f,0f);
		}else if(spot == 5){
			cameraFishing.transform.rotation = Quaternion.Euler(14.46f,256.5f,359f);
		}
		//SendCameraToStandardPosition();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.W)){
			print ("key code w");
			//PlayIniticalCamAnim();
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			print ("key code q");
			//SendCameraToStandardPosition();
		}
	}

	private void PlayIniticalCamAnim(){
		//if(cameraFishing.GetComponent<Animator>() == null){
			cameraFishing.GetComponent<Animator>().enabled = true;
		//}
		cameraFishing.GetComponent<Animator>().SetTrigger("initialCamAnim");
	}

	public void DeactivateCamAnimator(){
		cameraFishing.GetComponent<Animator>().enabled = false;
		//SendCameraToStandardPosition();
	}

	public bool IsCameraOnFishingSpot(){
		return isCameraOnFishingSpot;
	}		                                  

	public GameObject GetCameraFishing(){
		return cameraFishing;
	}

	//nao rpecisa
	/*public void SendCameraToStandardPosition(){
		//movemento da camera para a posicao inical
		isCameraOnFishingSpot = false;
		iTween.MoveTo(cameraFishing, iTween.Hash("position", cameraInitialPos, "time", 2));
		//iTween.RotateTo(cameraFishing, cameraInitialRot, 3);
		//desativar UI's
		UIHandler.instance.DeactivateAll();
	}*/

	/*public void SendCameraToFishingSpot(GameObject fishingSpot, float time){
		//metodo que envia a camera para um ponto proximo do ponto de pesca
		if(fishingSpot == null){
			//return se spot eh nulo
			return;
		}
		fishingSpotReference = fishingSpot.GetComponent<FishingSpot>().GetFishingSpotPosition() ; 
		destination = fishingSpot.GetComponent<FishingSpot>().GetCameraFishingSpotDestination();
		targetToLook = fishingSpot.GetComponent<FishingSpot>().GetTargetToLookAtboy();

		//move to da camera para o ponto de pesca
		destination = new Vector3(destination.x, destination.y + distanceToWater, destination.z);
		//targetToLookOnComplete = targetToLook;
		iTween.MoveTo(cameraFishing, iTween.Hash("position", destination, "time", time, "oncomplete", "SendCameraToFishingSpotCompleted", "oncompletetarget", this.gameObject, "easytype", iTween.EaseType.easeInQuad));
		iTween.RotateTo(cameraFishing, iTween.Hash("rotation", new Vector3(45, 90, 0), "time", time));
		iTween.LookTo(cameraFishing, iTween.Hash("looktarget", targetToLook, "time", time, "easytype", iTween.EaseType.linear) );
	}*/

	/*private void SendCameraToFishingSpotCompleted(){
		isCameraOnFishingSpot = true;

		iTween.LookTo(cameraFishing, iTween.Hash("looktarget", fishingSpotReference.GetComponent<FishingSpot>().GetTargetToLookAtboy(), "time", 2f));

		//ao terminar o movimento de movimento da camera, trazer UI's
		UIHandler.instance.ActivateAll();
	}*/
	//camera inicial de cada spot
	public void PlayFishStartCam_1(){
		CamFish_spot1();
	}

	private void CamFish_spot1(){
		cameraFishing.GetComponent<Animator>(). SetTrigger("StartGame1");
	}
	//camera iddle de cada spot
}

using UnityEngine;
using System.Collections;

public class FishingSpot : MonoBehaviour {

	//public int difficulty;

	private Vector3 fishingSpotPosition;
	private Vector3 targetToLook;
	private Vector3 cameraDestination;

	public static FishingSpot instance;

	void Awake(){
		instance = this;
	}

	void Start () {
		//referencia para a posicao inicial do ponto pesca, usado para a movimentacao da camera
		fishingSpotPosition = this.transform.position;

		//referencia para onde a camera deve ir ao selecionar o ponto de pesca
		cameraDestination = this.transform.Find("cameraDestination").transform.position;
		//referencia para a onde a camera deve olhar ao selecionar o ponto de pesca
		targetToLook = this.transform.Find("targetToLook").transform.position;
	}

	public Vector3 GetFishingSpotPosition(){
		return fishingSpotPosition;
	}

	public Vector3 GetCameraFishingSpotDestination(){
		return cameraDestination;
	}

	public Vector3 GetTargetToLookAtboy(){
		return targetToLook;
	}
}

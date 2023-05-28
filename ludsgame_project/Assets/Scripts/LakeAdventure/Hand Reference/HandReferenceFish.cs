using UnityEngine;
using System.Collections;

public class HandReferenceFish : MonoBehaviour {

	private GameObject handReference;
	private GameObject handReferenceToWorld;
	private Vector3 handReferencePosToWorld;

	private GameObject fishingSpot;

	public static HandReferenceFish instance;

	void Awake(){
		instance = this;
	}

	void Start(){		
		//referencia para o cursor da mao
		handReference = GameObject.Find("handReference").gameObject;
		//referencia para obj que ira representar cursor da mao no mundo
		handReferenceToWorld = GameObject.Find("handReferenceToWorld").gameObject;
	}

	void Update () {
		//cursor recebe posiçao do mouse e mais uma pos z
		handReference.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 400);
		//convertendo posicao do cursor da mao para uma posicao no mundo
		handReferencePosToWorld = CameraMovementControlFish.instance.GetCameraFishing().GetComponent<Camera>().ScreenToWorldPoint(handReference.transform.position);		
		//obj que representa a mao receba a posicao dela convertida
		handReferenceToWorld.transform.position = handReferencePosToWorld;

		if(Input.GetMouseButtonDown(0)){
			print ("click");
			//BoatControl.instance.MoveItToFishingSpot(fishingSpot);
		}
	}
	
	void OnTriggerStay(Collider col){
		if(col.name == "fishing_spot"){
			print ("fishing spot found");
			fishingSpot = col.gameObject;
		}else{
			fishingSpot = null;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.name == "fishing_spot"){
			print ("fishing spot exit");
			fishingSpot = null;
		}
	}
	
	public FishingSpot GetFishingSpot(){
		if(fishingSpot == null){
			print ("nenhum ponto de pesca escolhido");
			return null;
		}
		return fishingSpot.GetComponent<FishingSpot>();
	}
}










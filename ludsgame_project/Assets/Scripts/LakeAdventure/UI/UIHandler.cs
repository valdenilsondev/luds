using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	//esta classe vai manter referencia para todas as UI's do jogo
	private GameObject backToMapBtn;
	private GameObject baitMarkerBG;
	private GameObject baitMarkerCenter;
	private GameObject baitMarkerGreenBar;
	//private GameObject baitMarkerYellowBar;
	//private GameObject baitMarkerRedBar;
	private GameObject bucket;
	private GameObject bucketText;
	private GameObject fish1;
	private GameObject fish1Text;
	private GameObject fish2;
	private GameObject fish2Text;
	private GameObject fish3;
	private GameObject fish3Text;

	//altura da barra
	public float barHeight;

	//lista de todas as UI's
	private List<GameObject> UIsList = new List<GameObject>();
	private GameObject UIsParent;

	public static UIHandler instance;

	void Awake(){
		instance = this;

		backToMapBtn = GameObject.Find("BackToMapBtn").gameObject;
		baitMarkerBG = GameObject.Find("BaitMarkerBG").gameObject;
		baitMarkerCenter = GameObject.Find("BaitMarker").gameObject;
		baitMarkerGreenBar = GameObject.Find("green_bar").gameObject;
		//baitMarkerYellowBar = GameObject.Find("yellow_bar").gameObject;
		//baitMarkerRedBar = GameObject.Find("red_bar").gameObject;
		bucket = GameObject.Find("bucket").gameObject;
		bucketText = GameObject.Find("bucket_text").gameObject;
		fish1 = GameObject.Find("Fish1").gameObject;
		fish1Text = GameObject.Find("Fish1_text").gameObject;
		fish2 = GameObject.Find("Fish2").gameObject;
		fish2Text = GameObject.Find("Fish2_text").gameObject;
		fish3 = GameObject.Find("Fish3").gameObject;
		fish3Text = GameObject.Find("Fish3_text").gameObject;
		barHeight = GetGreenBar().GetComponent<RectTransform>().sizeDelta.y;
		baitMarkerGreenBar.GetComponent<Image>().fillAmount = 0.5f;
	}

	void Start () {
		UIsParent = GameObject.Find("GUIController"); //UI's
		//armazendo todas as ui's em uma lista
		for(int i = 0; i < UIsParent.transform.childCount; i++){
			UIsList.Add(UIsParent.transform.GetChild(i).gameObject);
		}	
		DeactivateAll();
	}

	public void DeactivateAll(){
		for(int i = 0; i < UIsList.Count; i++){
			UIsList[i].SetActive(false);
		}
	}

	public void ActivateAll(){
		for(int i = 0; i < UIsList.Count; i++){
			UIsList[i].SetActive(true);
		}
	}

	//back to map btn
	public void ActivateBackToMapBtn(){
		backToMapBtn.SetActive(true);
	}
	public void DeactivateBackToMapBtn(){
		backToMapBtn.SetActive(false);
	}
	public GameObject GetBackToMapBtn(){
		if(backToMapBtn.activeSelf == false){
			print ("tentando acessar back to map btn desativado");
			return null;
		}
		return backToMapBtn;
	}

	//bait marker - all
	public void ActivateBaitMarkerBG(){
		baitMarkerBG.SetActive(true);
	}	
	public void DeactivateBaitMarkerBG(){
		baitMarkerBG.SetActive(false);
	}
	public GameObject GetBaitMarkerBG(){
		if(baitMarkerBG.activeSelf == false){
			print ("tentando acessar bait marker BG desativado");
			return null;
		}
		return baitMarkerBG;
	}

	//bait marker - bars
	public void ActivateBaitMarkerGreenBar(){
		baitMarkerGreenBar.SetActive(true);
	}
	public void DeactivateBaitMarkerGreenBar(){
		baitMarkerGreenBar.SetActive(false);
	}
	public GameObject GetGreenBar(){
		return baitMarkerGreenBar;
	}
	/*public void ActivateBaitMarkerRedBar(){
		baitMarkerRedBar.SetActive(true);
	}
	public void DeactivateBaitMarkerRedBar(){
		baitMarkerRedBar.SetActive(false);
	}
	public GameObject GetRedBar(){
		return baitMarkerRedBar;
	}
	public void ActivateBaitMarkerYellowBar(){
		baitMarkerYellowBar.SetActive(true);
	}
	public void DeactivateBaitMarkerYellowBar(){
		baitMarkerYellowBar.SetActive(false);
	}
	public GameObject GetYellowBar(){
		return baitMarkerYellowBar;
	}*/

	//bait marker - center
	public void ActivateBaitMarkerCenter(){
		baitMarkerCenter.SetActive(true);
	}	
	public void DeactivateBaitMarkerCenter(){
		baitMarkerCenter.SetActive(false);
	}
	public GameObject GetBaiMarkerCenter(){
		if(baitMarkerCenter.activeSelf == false){
			print ("tentando acessar bait marker center desativado");
			return null;
		}
		return baitMarkerCenter;
	}

	//baits bucket
	public void ActivateBaitsBucket(){
		bucket.SetActive(true);
	}
	public void DeactivateBaitsBucket(){
		bucket.SetActive(false);
	}
	public GameObject GetBaitsBucket(){
		return bucket;
	}
	public int GetBaitsBucketQuantity(){
		return int.Parse(bucketText.GetComponent<Text>().text);
	}
	public void SetBaitsBucketQuantity(int quantity){
		bucketText.GetComponent<Text>().text = quantity.ToString();
	}

}

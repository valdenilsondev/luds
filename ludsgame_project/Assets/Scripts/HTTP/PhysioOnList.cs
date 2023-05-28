using UnityEngine;
using System.Collections;


public class PhysioOnList : MonoBehaviour {
	public string thisPhysioUser;
	GameObject btn;

	public void Awake(){
		btn = GameObject.Find("SelectBtn_Physio").gameObject;
		btn.SetActive(false);
	}

	public void SetIndex(){
		HttpController.SelectPhysioFromList(thisPhysioUser);
		btn.gameObject.SetActive(true);
	}

}

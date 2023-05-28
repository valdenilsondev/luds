using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComputerOnList : MonoBehaviour {
	public int thisPcIndex;
	//public int thisPcNumberOnList;

	// Use this for initialization
	void Start () {
		//this.gameObject.transform.FindChild("pc_number").GetComponent<Text>().text = thisPcNumberOnList.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetIndex(){
		HttpController.SelectComputerFromList(thisPcIndex);
	}
}

using UnityEngine;
using System.Collections;

public class Arrow_Control : MonoBehaviour {
	public GameObject land_pos;
	public GameObject boy_pos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (land_pos.transform.position);
		//this.transform.position = new Vector3 (boy_pos.transform.position.x, boy_pos.transform.position.y - 4, boy_pos.transform.position.z);
	}
}

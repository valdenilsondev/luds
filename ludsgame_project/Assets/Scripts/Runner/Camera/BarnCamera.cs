using UnityEngine;
using System.Collections;

public class BarnCamera : MonoBehaviour {

	public Camera cam;
	public bool cam_mov=true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		/*
		if (Input.GetKeyDown (KeyCode.T)) {
			cam_mov = true;
		}
		if (cam_mov) {
			Vector3 position_vector = new Vector3 ( 0,1,3);
			Vector3 rotate_vector = new Vector3 (15,9,0);
			MoveCam(position_vector, rotate_vector, 3);
			//MoveCam (0,1,3,3f);

		} */
	
	}
	public void MoveCam (Vector3 position, Vector3 rotation, float time) {
		bool cam_mov = true;
		Vector3 position_final = position;
		int lerp = 0;
		while (Vector3.Distance (this.transform.position, position_final) > 0.03) {
			print("distance" + Vector3.Distance (this.transform.position, position_final).ToString());
			this.transform.position = Vector3.Lerp (this.transform.position, position_final, time * Time.deltaTime);
			print ("lerp = " + lerp);
			lerp++;
		}

				print ("aproximou");
				cam_mov = false;
				this.transform.position = position_final;
				this.transform.Rotate (rotation * Time.deltaTime);


		print (Vector3.Distance (this.transform.position, position_final));
		cam_mov=false;
			


	}
	/*
	void MoveCam(float x, float y, float z, float time) {
		//cam = Camera.main;
		Vector3 position_final = new Vector3 (x, y, z);
		this.transform.position = Vector3.Lerp (this.transform.position, position_final, time*Time.deltaTime);
		//this.transform.rotation = 
		if (Vector3.Distance (this.transform.position, position_final) < 0.03) {
			cam_mov = false;
			this.transform.position = position_final;
		} 
		else {
			print (Vector3.Distance (this.transform.position, position_final));

	}
	*/
}

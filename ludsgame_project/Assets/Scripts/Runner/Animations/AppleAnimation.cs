using UnityEngine;
using System.Collections;
using Runner.Pool;
//using Runner.PlayerControl;
public class AppleAnimation : MonoBehaviour {

	Vector3 target, targetClose;
	// Use this for initialization
	void Start () {
		// -3  4  thi.z
		// 2  2,5
		target = new Vector3 (-7, 3.35f, 15f);

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = Vector3.Lerp (this.transform.position, target, Time.deltaTime * 10);	
		if (Vector3.Distance (target, this.transform.position) < 0.02f) {
			//Instantiate(ScoreManager.instance.particle, this.transform.position, Quaternion.identity);
			Destroy(this.gameObject);
		}
	}
}

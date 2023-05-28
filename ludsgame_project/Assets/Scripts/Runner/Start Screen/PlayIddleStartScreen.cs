using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayIddleStartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene().name == "Barn"){
			this.GetComponent<Animator>().SetTrigger("iddle");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

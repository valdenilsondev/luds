using UnityEngine;
using System.Collections;
using BridgeGame.Player;
using System.Collections.Generic;
using Share.Managers;

public class PiranhaControl : MonoBehaviour {

	private int rndTime;
	private int rndPos;
	private float timer;
	private bool check = false;
	private bool collideOnce = false;
	private GameObject player;
	private GameObject pigHeadReference;
	private static List<int> numbers = new List<int>{1,2,3,4,5,6};
	public AnimationClip[] piranhaAnimations;
	public GameObject splashPrefab;

	// Use this for initialization
	void Start () {

		player = GameObject.Find("azeitona").gameObject;
		rndPos = Random.Range(0,numbers.Count);
		rndTime = numbers[rndPos];
		numbers.RemoveAt(rndPos);
	}
	
	// Update is called once per frame
	void Update () {
		if(numbers.Count == 0){
			RepopulateNumbers();
		}
		if(timer < rndTime){
			timer = Time.deltaTime + timer;
		}else if(timer > rndTime && !check){
			this.GetComponent<Animator>().SetBool("piranhaMoving", true);			
			check = true;
		}
	
		if (collideOnce == true){
			this.transform.position = new Vector3 (pigHeadReference.transform.position.x, pigHeadReference.transform.position.y +2, pigHeadReference.transform.position.z);
		}
	}

	private static void RepopulateNumbers(){
		numbers = new List<int>{1,2,3,4,5,6};
	}

	void OnTriggerEnter(Collider col){
		if(col.name == "joint15_head" && collideOnce == false){
			BridgeSoundManager.Instance.PlayPiranha_Bite();
			PlayerControl.instance.PiranhaHitPlayer(PlayerControl.instance.GetPiranhaOnPigReference());
			//this.GetComponent<Animator>().enabled = false;
			this.gameObject.SetActive(false);
			collideOnce = true;
			pigHeadReference = col.gameObject;
			BridgeManager.instance.IncrementQtdPiranhaHits();
			//this.transform.parent.transform.parent = pigHeadReference.transform;
		}
		if (col.tag == "Ocean") {	
			BridgeSoundManager.Instance.PlayPiranha_Splash();
			GameObject p =	Instantiate(splashPrefab, new Vector3(this.transform.position.x, col.transform.position.y, this.transform.position.z), Quaternion.identity) as GameObject;
			Destroy(p, p.GetComponent<ParticleSystem>().duration);
		}
	}
}


using UnityEngine;
using System.Collections;

public class GestureSprites : MonoBehaviour {

	private GameObject raiseLeg;
	private GameObject squat;
	private GameObject walk;
	private GameObject walk_left;

	public static GestureSprites instance;

	void Awake(){
		instance = this;
	}

	void Start () {
		//ref
		raiseLeg = GameObject.Find("RaiseLeg").gameObject;
		squat = GameObject.Find("Squat").gameObject;
		walk = GameObject.Find("Walk_Right").gameObject;
		walk_left = GameObject.Find("Walk_Left").gameObject;

		//deactivate
		raiseLeg.SetActive(false);
		squat.SetActive(false);
		walk.SetActive(false);
		walk_left.SetActive(false);
	}

	public void RaiseLegOn(){
		if(raiseLeg.activeSelf == false){
			raiseLeg.SetActive(true);
		}
		raiseLeg.GetComponent<Animator>().SetTrigger("raise_leg_on");
	}

	private void RaiseLegOff(){
		if(raiseLeg.activeSelf == true){
			raiseLeg.SetActive(false);
		}
	}

	public void SquatOn(){
		if(squat.activeSelf == false){
			squat.SetActive(true);
		}
		squat.GetComponent<Animator>().SetTrigger("squat_on");
	}

	private void SquatOff(){
		if(squat.activeSelf == true){
			squat.SetActive(false);
		}
	}

	public void WalkRightOn(){
		if(walk.activeSelf == false){
			walk.SetActive(true);
		}
		walk.GetComponent<Animator>().SetTrigger("walk_on");
	}
	
	private void WalkRightOff(){
		if(walk.activeSelf == true){
			walk.SetActive(false);
		}
	}

	public void WalkLeftOn(){
		if(walk_left.activeSelf == false){
			walk_left.SetActive(true);
		}
		walk_left.GetComponent<Animator>().SetTrigger("walk_left_on");
	}

	private void WalkLeftOff(){
		if(walk_left.activeSelf == true){
			walk_left.SetActive(false);
		}
	}

	public void AllSpritesOff(){
		RaiseLegOff();
		SquatOff();
		WalkRightOff();
		WalkLeftOff();
	}
}

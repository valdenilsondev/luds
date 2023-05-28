using UnityEngine;
using System.Collections;
using Share.Managers;

public class PlayerJumpSlideControl : MonoBehaviour {

	private GameObject player;

	public static PlayerJumpSlideControl instance;

	void Awake(){
		instance = this;
			}

	void Start(){
		player = this.gameObject;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow)){	
		//	Jump ();
			print ("jump pig");
		}

		/*if(Input.GetKeyDown(KeyCode.DownArrow)){	
			Roll();
		}*/
	}

	public void Jump(){
		if(FloorMovementControl.instance.IsFloorMoving()){
			if(this.name == "azeitona"){
				this.GetComponent<Animator>().SetTrigger("jumping");
				this.GetComponent<Animator>().SetBool("running", false);
			}
			if(PigRunnerSoundManager.Instance != null)
			{
				PigRunnerSoundManager.Instance.StopRunSound();
				//acho q n ta usando
				PigRunnerSoundManager.Instance.PlayJump();
				//print ("pulando");
			}
		}
	}

	public void Roll(){
		if(FloorMovementControl.instance.IsFloorMoving()){
		//	if(this.name == "pig"){
			this.GetComponent<Animator>().SetTrigger("slide");
		//	}
			//this.GetComponent<Animator>().SetBool("running", false);
			/*if(this.name == "Robot"){
				this.GetComponent<Animator>().SetTrigger("RobotRoll");
			}*/
			PigRunnerSoundManager.Instance.PlaySlide();
		}
	}

	public void SetRunTrue(){
		//this.GetComponent<Animator>().SetBool("running", true);
		//this.GetComponent<Animator>().SetTrigger("iddle");
	}


}

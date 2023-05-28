using UnityEngine;
using System.Collections;
using Assets.Scripts.Goalkeeper;

public class AnimationControllerKicker : MonoBehaviour {

    public static ShootDirection ChosenSideToKick = ShootDirection.Middle;

	public static AnimationControllerKicker instance;

	private bool kicking;
	private Vector3 origin;
	public GameObject target; 

	void Awake(){
		instance = this;
		origin = this.transform.localPosition;
	}

	void Start(){
		//GetComponent<Animation>().Play("Kicker_Iddle");
	}

	// Update is called once per frame

	void Update () {
		if(Input.GetKeyDown(KeyCode.K)){
			GoalKeeperSoundManager.Instance.PlayRun();
			PlayAnimation_Kicker_kick1();
		}
		if(kicking)
			this.transform.position = Vector3.Lerp (this.transform.localPosition, target.transform.position, 1f * Time.deltaTime);
	}

	public void PlayAnimation_Kicker_kick1(){
		kicking = true;
		GetComponent<Animator>().SetTrigger ("kick");
	}

	public void KickerActionCompleted(){
		kicking = false;
		this.transform.localPosition = origin;
        BallControl.instance.GetComponent<Rigidbody>().isKinematic = false;
        if (ChosenSideToKick == ShootDirection.ExtremeLeft){
            BallControl.instance.KickBallToLeft(new Vector3(710, 210, -210));
			/*if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){			
				BallControl.instance.KickBallToLeft2_Save();
			}else{
				BallControl.instance.KickBallToLeft_Goal();
			}*/
		}
        if (ChosenSideToKick == ShootDirection.Left){
            BallControl.instance.KickBallToLeft(new Vector3(710, 115 , -125));
			/*if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){
				BallControl.instance.KickBallToLeft1_Save();
			}else{
				BallControl.instance.KickBallToLeft_Goal();
			}*/
		}

        if (ChosenSideToKick == ShootDirection.Middle){
            BallControl.instance.KickBallToMiddle(new Vector3(780, 120, 0));
			/*if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){
				BallControl.instance.KickBallToMid_Save();
			}else{
				BallControl.instance.KickBallToMid_Goal();
			}*/
		}

        if (ChosenSideToKick == ShootDirection.Right){
            BallControl.instance.KickBallToRight(new Vector3(700, 125, 120));
			/*if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){
				BallControl.instance.KickBallToRight1_Save();
			}else{
				BallControl.instance.KickBallToRight_Goal();
			}*/
		}

        if (ChosenSideToKick == ShootDirection.ExtremeRight){
            BallControl.instance.KickBallToRight(new Vector3(710, 190, 210));
			/*if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){
				BallControl.instance.KickBallToRight2_Save();
			}else{
				BallControl.instance.KickBallToRight_Goal();
			}*/
		}

		CameraControl.instance.MoveCameraTowards_GK();
		GoalKeeperSoundManager.Instance.PlayFeetKick();
		//GetComponent<AudioSource>().PlayOneShot(shoot);
        AnimationControllerGoalkeeper.instance.PlayChosen_GK_AnimationSave();
	}
}

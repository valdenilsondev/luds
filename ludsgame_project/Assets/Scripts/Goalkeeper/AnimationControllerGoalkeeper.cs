using UnityEngine;
using System.Collections;
using Assets.Scripts.Goalkeeper;
using Goalkeeper.Managers;

public class AnimationControllerGoalkeeper : MonoBehaviour {

    public ShootDirection ChosenSideToSave;

	public static AnimationControllerGoalkeeper instance;

	public GameObject Animation_GK;

    public bool gkAnimationCompleted = false;

    private Vector3 startPosition;
    private Vector3 lastPosition;

    public bool animation_started;

    public bool animation_finished;

    private Animator animator;

	void Awake () {
		instance = this;
        startPosition = this.transform.position;
        animator = this.GetComponent<Animator>();
	}

	void Start () {
		GK_Iddle();
	}

    void Update()
    {
        if (gkAnimationCompleted && animation_started)
        {
            lastPosition = this.transform.Find("esqueleto").gameObject.transform.localPosition;

            /*if (AnimationControllerGoalkeeper.instance.ChosenSideToSave == BarController.instance.chosenSide)
                PlayAnimation_GK_Celebrate();
            else
                PlayAnimation_GK_Sad();*/
        }
        
        var stateInfo = animator.GetCurrentAnimatorClipInfo(0);

        if(stateInfo.Length > 0)
        {
            if (animation_started && stateInfo[0].clip.name == "boy_glkp_vitoria")
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, lastPosition.x);
                animation_started = false;
            }

            if (stateInfo[0].clip.name == "boy_glkp_iddle")
            {
                this.transform.position = startPosition;
            }
        }
    }

	public void PlayChosen_GK_AnimationSave()
	{
		if(ChosenSideToSave == ShootDirection.ExtremeRight){
			if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){			
				PlayAnimation_GK_Save_Right2();
			}else{
				PlayAnimation_GK_Miss_Right2();
			}
			GoalKeeperSoundManager.Instance.PlayFeetKick();
		}

        if (ChosenSideToSave == ShootDirection.Right){
			if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){			
				PlayAnimation_GK_Save_Right1();
			}else{
				PlayAnimation_GK_Miss_Right1();
			}
			GoalKeeperSoundManager.Instance.PlayFeetKick();
		}

        if (ChosenSideToSave == ShootDirection.Middle){
			if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){			
				PlayAnimation_GK_Save_Mid();	
			}else{
				PlayAnimation_GK_Miss_Mid();
			}
			GoalKeeperSoundManager.Instance.PlayFeetKick();
		}

        if (ChosenSideToSave == ShootDirection.Left){
			if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){			
				PlayAnimation_GK_Save_Left1();
			}else{
				PlayAnimation_GK_Miss_Left1();
			}
			GoalKeeperSoundManager.Instance.PlayFeetKick();
		}

        if (ChosenSideToSave == ShootDirection.ExtremeLeft){
			if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){			
				PlayAnimation_GK_Save_Left2();	
			}else{
				PlayAnimation_GK_Miss_Left2();
			}
			GoalKeeperSoundManager.Instance.PlayFeetKick();
		}

		/*if((ShootDirection) BarController.instance.chosenSide == BarController.instance.GetHandPosX()){
			//save
			BallControl.instance.AddSaveToScore();
		}else{
			//goal
			BallControl.instance.AddGoalToScore();
		}*/
	}

	public void GK_AnimationCompleted(){
        gkAnimationCompleted = true;
	}

	public void GK_Iddle(){
		GetComponent<Animator> ().SetBool ("Gk_Iddle", true);
	}

	public void PlayAnimation_GK_Save_Left2(){
        StartedAnimation();
        GetComponent<Animator> ().SetTrigger ("victory_def_left_2");
	}

	public void PlayAnimation_GK_Save_Left1(){
        StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("victory_def_left_1");
	}

	public void PlayAnimation_GK_Save_Mid(){
        StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("victory_def_mid");
	}

	public void PlayAnimation_GK_Save_Right1(){
        StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("victory_def_right_1");
	}

	public void PlayAnimation_GK_Save_Right2(){
        StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("victory_def_right_2");
	}

	public void PlayAnimation_GK_Miss_Left2(){
		StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("defeat_def_left_2");
	}

	public void PlayAnimation_GK_Miss_Left1(){
		StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("defeat_def_left_1");
	}

	public void PlayAnimation_GK_Miss_Mid(){
		StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("defeat_def_mid");
	}

	public void PlayAnimation_GK_Miss_Right1(){
		StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("defeat_def_right_1");
	}

	public void PlayAnimation_GK_Miss_Right2(){
		StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("defeat_def_right_2");
	}

	public void PlayAnimation_GK_Sad(){
        StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("isDefeat");
	}

	public void PlayAnimation_GK_Celebrate(){
        StartedAnimation();
		GetComponent<Animator> ().SetTrigger ("isVictory");
	}

    public void StartedAnimation()
    {
        animation_started = true;
        animation_finished = false;
        gkAnimationCompleted = false;
    }
}

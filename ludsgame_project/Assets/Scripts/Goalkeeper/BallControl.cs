using UnityEngine;
using System.Collections;
using Share.Managers;
using Goalkeeper.Managers;
using UnityEngine.UI;

public class BallControl : MonoBehaviour {

	public static BallControl instance;
	public static int rnd;

	//public AudioClip ohSound, clapCrowd;

	public Text userFeedback;

    private float noMovementThreshold = 0.005f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;

    private Vector3 startPosition;
    private Quaternion startRotation;

	void Awake(){
		instance = this;
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
        startRotation = this.transform.rotation;
	}

	void Update (){
        CheckBallIsMoving();
	}

    void CheckBallIsMoving()
    {
        //Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = this.transform.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold){
                isMoving = true;
            }
            else{
                isMoving = false;
                break;
            }
        }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    public void InitializeBallPosition()
    {
        //this.GetComponent<Rigidbody>().freezeRotation = true;
        this.transform.position = new Vector3(-0.4279993f, 0.5f, 0f);
        this.transform.rotation = startRotation;        
    }
	//desativa o texto de resposta de usuario
	private void HideFeedback()
	{
		userFeedback.text = string.Empty;
	}
	public void AddGoalToScore(){
		GoalKeeperSoundManager.Instance.PlayOhSound();
		//GetComponent<AudioSource>().PlayOneShot (ohSound);
		GameManagerShare.instance.IncreaseScore (Assets.Scripts.Share.Enums.ScoreItemsType.Goal, 1, 0);
		userFeedback.text = "ERROU!";
		Invoke("HideFeedback",1);
		GoalKeeperSoundManager.Instance.PlayWallKick();
	}

	public void AddSaveToScore(){
		GoalKeeperSoundManager.Instance.PlayClaps();
		//GetComponent<AudioSource>().PlayOneShot (clapCrowd);
		//ScoreManager_GK.instance.Saved();
		GameManagerShare.instance.IncreaseScore (Assets.Scripts.Share.Enums.ScoreItemsType.Defense, 1, 0);
		userFeedback.text = "DEFENDEU!";
		Invoke("HideFeedback",1);
		GoalKeeperSoundManager.Instance.PlayFeetKick();
	}

	public void KickBallToLeft(Vector3 force)
	{
		this.GetComponent<Rigidbody>().freezeRotation = false;
		this.GetComponent<Rigidbody>().AddForce(force);
	}

	public void KickBallToMiddle(Vector3 force)
	{
		this.GetComponent<Rigidbody>().freezeRotation = false;
		this.GetComponent<Rigidbody>().AddForce(force);
	}

	public void KickBallToRight(Vector3 force)
	{
		this.GetComponent<Rigidbody>().freezeRotation = false;
		this.GetComponent<Rigidbody>().AddForce(force);
	}

	public void KickBallToLeft2_Save(){
		this.GetComponent<Animator>().SetTrigger("ball_def_l2");
	}

	public void KickBallToLeft1_Save(){
		this.GetComponent<Animator>().SetTrigger("ball_def_l1");
	}

	public void KickBallToMid_Save(){
		this.GetComponent<Animator>().SetTrigger("ball_def_mid");
	}

	public void KickBallToRight1_Save(){
		this.GetComponent<Animator>().SetTrigger("ball_def_r1");
	}

	public void KickBallToRight2_Save(){
		this.GetComponent<Animator>().SetTrigger("ball_def_r2");
	}

	public void KickBallToLeft_Goal(){
		this.GetComponent<Animator>().SetTrigger("ball_goal_l");
	}

	public void KickBallToMid_Goal(){
		this.GetComponent<Animator>().SetTrigger("ball_goal_mid");
	}

	public void KickBallToRight_Goal(){
		this.GetComponent<Animator>().SetTrigger("ball_goal_r");
	}

	/*public void GK_Saved_FurtherLeft(){
		this.GetComponent<Animator>().SetBool("goalLeft2", false);
	}

	public void GK_Saved_CloserLeft(){
		this.GetComponent<Animator>().SetBool("goalLeft1", false);
	}

	public void GK_Saved_Middle(){
		this.GetComponent<Animator>().SetBool("goalCenter", false);
	}    

	public void GK_Saved_CloserRight(){
		this.GetComponent<Animator>().SetBool("goalRight1", false);
	}

	public void GK_Saved_FurtherRight(){
		this.GetComponent<Animator>().SetBool("goalRight2", false);
	}*/

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.name == "blockade" && !GKLimit.instance.firstTime)
        {
            AddSaveToScore();

        }
        else if (col.collider.name == "goal" && !GKLimit.instance.firstTime) 
        {
            AddGoalToScore();
        }
		//GoalKeeperSoundManager.Instance.PlayWallKick();
        GKLimit.instance.firstTime = true;
    }

    void OnBecameInvisible()
    {
        if (GameManagerShare.IsStarted())
        {
            //this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            InitializeBallPosition();            
        }
    }
}

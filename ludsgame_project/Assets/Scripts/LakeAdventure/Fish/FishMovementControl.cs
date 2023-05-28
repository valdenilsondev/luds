using UnityEngine;
using System.Collections;

public class FishMovementControl : MonoBehaviour {

	public float fish_min_dist;
	public float fish_max_dist;
	public float fish_min_time;
	public float fish_max_time;
	private Animator fishA_animator_control;
	private Animator fishB_animator_control;
	private Animator fishC_animator_control;

	private GameObject fish;
	private GameObject fish_parent;

	public static FishMovementControl instance;

	void Awake () {
		instance = this;
	}
	void Start()
	{
		fishA_animator_control = Batata_Fishing_Control.instance.GetBarco().transform.Find("peixe_A_fishing").GetComponent<Animator>();
		fishB_animator_control = Batata_Fishing_Control.instance.GetBarco().transform.Find("peixe_B_fishing").GetComponent<Animator>();
		fishC_animator_control = Batata_Fishing_Control.instance.GetBarco().transform.Find("peixe_C_fishing").GetComponent<Animator>();
			//GameObject.Find("Barco e Menino").transform.FindChild("peixe_C_fishing").GetComponent<Animator>();

	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.A)){
			print ("key code a");
			MoveFishAway();
		}
	}

	public void CreateFishAndHandleMov(){
		if(fish != null){
			GameObject.Destroy(fish);
		}
		//primitive objs
		fish = GameObject.CreatePrimitive(PrimitiveType.Cube);
		fish_parent = GameObject.CreatePrimitive(PrimitiveType.Cube);
		fish.GetComponent<MeshRenderer>().enabled = false;
		fish_parent.GetComponent<MeshRenderer>().enabled = false;

		//setting positions
		var pos = RopeManager.instance.bait_throw_pos2[Batata_Fishing_Control.instance.GetIndexRdSpot()].transform.position;
		fish.transform.position = new Vector3(pos.x, pos.y - 0.5f, pos.z);
		fish_parent.transform.position = fish.transform.position;

		//parenting
		RopeManager.instance.GetBait().transform.parent = fish.transform;
		fish.transform.parent = fish_parent.transform;

		//mover fish
		Invoke("MoveFish", 1);
	}

	public void DestroyFish(){
		GameObject.Destroy(fish);
		GameObject.Destroy(fish_parent);
	}

	private void MoveFishLeft(){
		if(PullBaitControl.instance.IsFishBaited()){
			var pos = fish.transform.localPosition;
			var chosen_time = Random.Range(fish_min_time, fish_max_time);
			var chosen_dist = Random.Range(fish_min_dist, fish_max_dist);
			iTween.MoveTo(fish, iTween.Hash("x", pos.x - chosen_dist, "islocal", true, "oncomplete", "MoveFishRight", "oncompletetarget", this.gameObject, "time", chosen_time, "easetype", iTween.EaseType.linear));
		}
	}

	private void MoveFishRight(){
		if(PullBaitControl.instance.IsFishBaited()){
			var pos = fish.transform.localPosition;
			var chosen_time = Random.Range(fish_min_time, fish_max_time);
			var chosen_dist = Random.Range(fish_min_dist, fish_max_dist);
			iTween.MoveTo(fish, iTween.Hash("x", pos.x + chosen_dist, "islocal", true, "oncomplete", "MoveFishLeft", "oncompletetarget", this.gameObject, "time", chosen_time, "easetype", iTween.EaseType.linear));
		}
	}

	public void MoveFishAway(){
		if(PullBaitControl.instance.IsFishBaited()){
			var pos = fish_parent.transform.position;
			var chosen_time = Random.Range(fish_min_time, fish_max_time);
			var chosen_dist = Random.Range(fish_min_dist, fish_max_dist);
			iTween.MoveTo(fish_parent, iTween.Hash("z", pos.z + chosen_dist, "oncomplete", "MoveFishCloser", "oncompletetarget", this.gameObject, "time", chosen_time, "easetype", iTween.EaseType.linear));
		}
	}

	public void MoveFishCloser(){
		if(PullBaitControl.instance.IsFishBaited()){
			var pos = fish_parent.transform.position;
			var chosen_time = Random.Range(fish_min_time, fish_max_time);
			var chosen_dist = Random.Range(fish_min_dist, fish_max_dist);
			iTween.MoveTo(fish_parent, iTween.Hash("z", pos.z - chosen_dist, "oncomplete", "MoveFishAway", "oncompletetarget", this.gameObject, "time", chosen_time, "easetype", iTween.EaseType.linear));
		}
	}

	private void MoveFish(){
		//int rnd = Random.Range(0,2);
		//if(rnd == 0){
			MoveFishLeft();
			MoveFishAway();
		//}else if(rnd == 1){
		//	MoveFishRight();
		//}
	}

	public void PlayBlueFishJump(){
		BlueFishJump();
	}
	public void PlayYellowFishJump(){
		YellowFishJump();
	}
	public void PlayBrowFishJump(){
		BrownFishJump();
	}

	public void YellowFishJump(){
		fishA_animator_control.SetTrigger("yellow_jump");
	}
	public void BlueFishJump(){
		fishB_animator_control.SetTrigger("blue_jump");
	}
	public void BrownFishJump(){
		fishC_animator_control.SetTrigger("brown_jump");
	}














}
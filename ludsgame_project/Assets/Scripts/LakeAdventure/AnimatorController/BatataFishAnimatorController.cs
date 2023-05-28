using UnityEngine;
using System.Collections;

public class BatataFishAnimatorController : MonoBehaviour {

	private Animator[] batata_fish_animator;//boy_fish_animator
	private Animator batata_animator;

	private string batata_fish_idle = "batata_fish_idle";
	private string batata_fish_throw= "batata_fish_throw";
	private string batata_fish_force= "batata_fish_force";
	private string batata_fish_pull= "batata_fish_pull";
	private string batata_fish_roll = "batata_fish_roll";
	private string batata_fish_win= "batata_fish_win";
	private string batata_fish_lose= "batata_fish_lose";
	/*private string boy_iddle_2h = "boy_iddle_2h";

	//private string boy_iddle_lh = "boy_iddle_lh";
	//private string boy_iddle_rh = "boy_iddle_rh";

	private string boy_throw_2h = "boy_throw_2h";
	//private string boy_throw_lh = "boy_throw_lh";
	// string boy_throw_rh = "boy_throw_rh";

	//private string boy_pull_2h_l = "boy_pull_2h_l";
	//private string boy_pull_2h_mid = "boy_pull_2h_mid";
	//private string boy_pull_2h_r = "boy_pull_2h_r";

	private string boy_pull_rh_l = "boy_pull_rh_l";
	private string boy_pull_rh_r = "boy_pull_rh_r";

	private string boy_pull_lh_l = "boy_pull_lh_l";
	private string boy_pull_lh_r = "boy_pull_lh_r";

	private string boy_roll_2h = "boy_roll_2h";
	//private string boy_roll_rh = "boy_roll_rh";
	//private string boy_roll_lh = "boy_roll_lh";*/

	public static BatataFishAnimatorController instance;

	void Awake(){
		instance = this;
	}
	
	void Start () {

		//batata_fish_animator[1] = GameObject.Find(i"Barco e Menino").transform.FindChild("batata_fishing").GetComponent<Animator>();
		//batata_fish_animator[2] = GameObject.Find("3Barco e Menino").transform.FindChild("batata_fishing").GetComponent<Animator>();
		//batata_fish_animator[3] = GameObject.Find("4Barco e Menino").transform.FindChild("batata_fishing").GetComponent<Animator>();
		//batata_fish_animator[4] = GameObject.Find("5Barco e Menino").transform.FindChild("batata_fishing").GetComponent<Animator>();
		//batata_fish_animator[5] = GameObject.Find("6Barco e Menino").transform.FindChild("batata_fishing").GetComponent<Animator>();

		//batata_fish_animator = Batata_Fishing_Control.instance.batata_obj.GetComponent<Animator>();
		//batata_fish_animator = GameObject.Find("1Barco e Menino").transform.FindChild("batata_fishing").GetComponent<Animator>();	
		//animacao padrao
		//BatataFishIdle();
	}

	void Update(){
		//inputs de teste
		if(Input.GetKeyDown(KeyCode.Keypad1)){
			BatataFishThrow();
		}else if(Input.GetKeyDown(KeyCode.Keypad2)){
			BatataFishForce();
		}else if(Input.GetKeyDown(KeyCode.Keypad3)){
			BatataFishIdle();
		}else if(Input.GetKeyDown(KeyCode.Keypad4)){
			BatataFishLose();
		}else if(Input.GetKeyDown(KeyCode.Keypad5)){
			BatataFishWin();
		}else if(Input.GetKeyDown(KeyCode.Keypad6)){
			BatataFishPull();
		}else if(Input.GetKeyDown(KeyCode.Keypad7)){
			BatataFishRoll();
		}
	}

	//resumo -> 2hands, right, left
	public void PlayBatataIdle()
	{
		BatataFishIdle();
	}
	public void PlayBatataThrow()//
	{
		BatataFishThrow();
	}
	public void PlayBatataFishForce()//n usa ainda
	{
		BatataFishForce();
	}
	public void PlayBatataLose()//n usa
	{
		BatataFishLose();
	}
	public void PlayBatataWin()//n usa
	{
		BatataFishWin ();
	}
	public void PlayBatataPull()//
	{
		BatataFishPull();
	}
	public void PlayBatataRoll()//
	{
		BatataFishRoll();
	}


	//=====================================
	//resumo -> 2hands, right, left
	/*public void PlayThrowAnim(string choseThrowAnim){
		if(choseThrowAnim == "2hands"){
			BatataFishIdle();
		}else if(choseThrowAnim == "right"){
			BoyThrowLeft();
		}else if(choseThrowAnim == "left"){
			BoyThrowRight();
		}
	}

	//resumo -> right, left
	/*public void PlayPull2hAnim(string chosePullAnim){
		if(chosePullAnim == "right"){
			BoyPull2hRight();
		}else if(chosePullAnim == "left"){
			BoyPull2hLeft();
		}
	}

	//resumo -> right, left
	public void PlayPullRightAnim(string chosePullAnim){
		if(chosePullAnim == "right"){
			BoyPullRightToRight1h();
		}else if(chosePullAnim == "left"){
			BoyPullRightToLeft1h();
		}
	}

	//resumo -> right, left
	/*public void PlayPullLeftAnim(string chosePullAnim){
		if(chosePullAnim == "right"){
			BoyPullLeftToRight1h();
		}else if(chosePullAnim == "left"){
			BoyPullLeftToLeft1h();
		}
	}

	//resumo -> 2hands, right, left
	public void PlayRollAnim(string choseRollAnim){
		if(choseRollAnim == "right"){
			BoyRollRight();
		}else if(choseRollAnim == "left"){
			BoyRollLeft();
		}else if(choseRollAnim == "2hands"){
			BoyRoll2Hands();
		}
	}

	//catch fish
	/*private void BoyPullFish(){
		boy_fish_animator.SetTrigger(boy_pull_2h_mid);
	}*/

	//iddles
	private void BatataFishIdle(){
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_idle");
	}
	private void BatataFishThrow()
	{
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_throw");
	}
	private void BatataFishForce()
	{
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_force");
	}
	private void BatataFishLose()
	{
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_lose");
	}

	private void BatataFishWin()
	{
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_victory");
	}
	private void BatataFishPull()
	{
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_pull");
	}
	private void BatataFishRoll()
	{
		Batata_Fishing_Control.instance.batata_fishing_animator.SetTrigger("fish_roll");
	}

	/*private void BoyIddleLeft(){
		boy_fish_animator.SetTrigger(boy_iddle_lh);
	}
	private void BoyIddleRight(){
		boy_fish_animator.SetTrigger(boy_iddle_rh);
	}*/

	//throws
	/*private void BoyThrow2h(){
		boy_fish_animator.SetTrigger(boy_throw_2h);
	}
	private void BoyThrowLeft(){
		boy_fish_animator.SetTrigger(boy_throw_lh);
	}
	private void BoyThrowRight(){
		boy_fish_animator.SetTrigger(boy_throw_rh);
	}

	//pulls 2h
	private void BoyPull2hLeft(){
		boy_fish_animator.SetTrigger(boy_pull_2h_l);
	}
	private void BoyPull2hRight(){
		boy_fish_animator.SetTrigger(boy_pull_2h_r);
	}*/

	//pulls left
	/*private void BoyPullRightToLeft1h(){
		boy_fish_animator.SetTrigger(boy_pull_rh_l);
	}
	private void BoyPullRightToRight1h(){
		boy_fish_animator.SetTrigger(boy_pull_rh_r);
	}

	//pulls right
	private void BoyPullLeftToRight1h(){
		boy_fish_animator.SetTrigger(boy_pull_lh_r);
	}
	private void BoyPullLeftToLeft1h(){
		boy_fish_animator.SetTrigger(boy_pull_lh_l);
	}

	//rolls
	private void BoyRoll2Hands(){
		boy_fish_animator.SetTrigger(boy_roll_2h);
	}
	private void BoyRollLeft(){
		boy_fish_animator.SetTrigger(boy_roll_lh);
	}
	private void BoyRollRight(){
		boy_fish_animator.SetTrigger(boy_roll_rh);
	}*/

}

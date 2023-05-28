using UnityEngine;
using System.Collections;

using Assets.Scripts.Goalkeeper;
using Share.Managers;
using Goalkeeper.Managers;
using UnityEngine.UI;
using  Assets.Scripts.Share.Kinect;

public class BarController : MonoBehaviour {

	private float barDeltaX, barDeltaY;
	private float minValue = -280;
	private float maxValue = 280f;
	private GameObject point;
	private GameObject handCursor;
	private bool movingRight = true;
	private static  bool handLocked;
	public ShootDirection chosenSide;
	//public float timeToChoose;
	private float elapsedTime;
	private float barSpeed;
	private float barSpeedIncrement;
	private int timeToShoot;
	public bool usingKinect;
	private static bool shooting;
	public static BarController instance;

	private static Animation shot_indicator;
    private float handCursorPosX;
    private float handCursorPosY;
    private float handCursorPosZ;
	//movimento configurado
	private int movement_choice;


	// Use this for initialization
	void Awake () {
		instance = this;
	//	barSpeed = 1;
		handCursor = GameObject.Find("hand_cursor");
		point = GameObject.Find ("point");
		timeToShoot = PlayerPrefsManager.GetTimeToShoot();
		movement_choice = PlayerPrefsManager.GetGKSetupMovement();
		if(movement_choice == 0){

		}
		else if(movement_choice == 1)
		{

		}
	}

	void Start(){
		shot_indicator = this.gameObject.GetComponent<Animation> ();
		/*if(GoalkeeperManager.instance.difficult == 1)
		{
			PlayerPrefsManager.SetBarSpeed(40);
		}
		else if(GoalkeeperManager.instance.difficult == 2)
		{
			PlayerPrefsManager.SetBarSpeed(80);
		}
		else if(GoalkeeperManager.instance.difficult == 3)
		{
			PlayerPrefsManager.SetBarSpeed(120);
		}*/

		int value = PlayerPrefsManager.GetBarSpeed();
		if(value == 0)
		{
			value = 75;
		}
		else if(value == 1)
		{
			value = 100;
		}
		else if(value == 2)
		{
			value = 120;
		}
		barSpeed = value;
		barSpeedIncrement = PlayerPrefsManager.GetBarSpeedIncrement ();

	}
	// Update is called once per frame
	void Update () {

        handCursorPosX = handCursor.transform.GetComponent<RectTransform>().transform.localPosition.x;
        handCursorPosY = handCursor.transform.GetComponent<RectTransform>().transform.localPosition.y;
        handCursorPosZ = handCursor.transform.GetComponent<RectTransform>().transform.localPosition.z;

        if (Input.GetKey(KeyCode.RightArrow))
            handCursor.transform.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(handCursor.transform.GetComponent<RectTransform>().transform.localPosition,
                                                                                                      new Vector3(30 * (Time.deltaTime + 10),
                        handCursorPosY,
                        handCursorPosZ), 2);
        if (Input.GetKey(KeyCode.LeftArrow))
            handCursor.transform.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(handCursor.transform.GetComponent<RectTransform>().transform.localPosition,
                                                                                                      new Vector3(-30 * (Time.deltaTime + 10),
                        handCursorPosY,
                        handCursorPosZ), 2);

        if (Input.GetKey(KeyCode.UpArrow))
            handCursor.transform.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(handCursor.transform.GetComponent<RectTransform>().transform.localPosition,
                                                                                                      new Vector3(15 * (Time.deltaTime + 10),
                        handCursorPosY,
                        handCursorPosZ), 2);
        if (Input.GetKey(KeyCode.DownArrow))
            handCursor.transform.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(handCursor.transform.GetComponent<RectTransform>().transform.localPosition,
                                                                                                      new Vector3(-15 * (Time.deltaTime + 10),
                        handCursorPosY,
                        handCursorPosZ), 2);

        if (Input.GetKey(KeyCode.M))
            handCursor.transform.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(handCursor.transform.GetComponent<RectTransform>().transform.localPosition,
                                                                                                      new Vector3(0,
                        handCursorPosY,
                        handCursorPosZ), 2);

        if (GameManagerShare.IsStarted())
        {
			if(!handLocked)
				HandsVariation();
			if(!shooting && !GameManagerShare.IsPaused() && !CountDownManager.instance.IsCounting()){
				BallVariation (); 
				elapsedTime += Time.deltaTime;
		
				if (elapsedTime > timeToShoot) {
					StartCoroutine(Shoot());
				}
			}
		}
	}

	public static void SetShooting(bool _shoting){
		shooting = _shoting;
	}

	public static bool IsShooting(){
		return shooting;
	}
	public bool IsUsingKinect(){
		if(usingKinect){
			return true;
		}
		return false;
	}

	IEnumerator Shoot(){
	//chamar numero de 0 a 4
		float pointXPos = point.transform.GetComponent<RectTransform> ().transform.localPosition.x;
		shooting = true;
		if (pointXPos< -210f) {
            chosenSide = ShootDirection.ExtremeRight;
		}
		if (pointXPos> -210f && pointXPos<= -70f) {
            chosenSide = ShootDirection.Right;
		}

		//centro
		if (pointXPos > -70f && pointXPos <= 70f) {
            chosenSide = ShootDirection.Middle;
		}

		if (pointXPos > 70f && pointXPos <= 210f) {
            chosenSide = ShootDirection.Left;
		}
		if (pointXPos > 210f) {
            chosenSide = ShootDirection.ExtremeLeft;
		}
		//print (point.transform.position.x);
		yield return new WaitForSeconds(2);
		handLocked = true;
		AnimationControllerKicker.ChosenSideToKick = chosenSide;
	
		//Atribui um valor de 0 a 4 
		//TODO: Verificar a posicao do jogador e retornar um valor de 0 a 4 para ser atribuido ao SideToSave
		//
		AnimationControllerGoalkeeper.instance.ChosenSideToSave = GetHandPosX ();
		GoalKeeperSoundManager.Instance.PlayRun();
		AnimationControllerKicker.instance.PlayAnimation_Kicker_kick1();
		barSpeed += 0.2f;
		elapsedTime = 0;
		//timeToShoot = Random.Range (3, 8); o mesmo ja setado no sistema
		//HideBar ();	
		Assets.Scripts.Share.Controllers.GUIController.instance.Disable ();
	}

	public static void HideBar()
	{
        iTween.MoveTo(instance.gameObject, iTween.Hash("y", -100));
	}

    public static void ShowBar()
    {
        iTween.MoveTo(instance.gameObject, iTween.Hash("y", 120));
	}

	private void BallVariation (){
		print ("ball variation");
		if(movingRight){	
			barDeltaX += Time.deltaTime * barSpeed;
			point.transform.GetComponent<RectTransform>().transform.localPosition = new Vector3(barDeltaX, point.transform.GetComponent<RectTransform>().transform.localPosition.y, point.transform.position.z);
		}
		if(!movingRight){	
			barDeltaX -= Time.deltaTime * barSpeed;
			point.transform.GetComponent<RectTransform>().transform.localPosition = new Vector3(barDeltaX, point.transform.GetComponent<RectTransform>().transform.localPosition.y, point.transform.position.z);
		}

		if (barDeltaX <= minValue) {		
			barDeltaX = minValue;
			movingRight = true;			
		}
		if (barDeltaX >= maxValue) {	
			barDeltaX = maxValue;		
			movingRight = false;
		}
	}

	private void HandsVariation(){
		
		if(GameManagerShare.instance.IsUsingKinect() && DistanceCalibrator.instance != null){

            var playerId = DistanceCalibrator.instance.kinect.GetPlayer1ID();
            Vector4 distance = DistanceCalibrator.instance.kinect.GetUserPosition(playerId);

			if(distance.x * 1000 < 280 && distance.x * 1000 > -280){
			handCursor.transform.GetComponent<RectTransform>().transform.localPosition = Vector3.Lerp(handCursor.transform.GetComponent<RectTransform>().transform.localPosition ,
                                                                                                      new Vector3(distance.x* 1000, handCursorPosY,handCursorPosZ),2);
			}
		}
	}

    public ShootDirection GetHandPosX()
    {
		var posXHandCursor = handCursor.transform.GetComponent<RectTransform> ().transform.localPosition.x;

		if (Vector3.Distance (handCursor.transform.GetComponent<RectTransform>().transform.localPosition, point.transform.GetComponent<RectTransform>().transform.localPosition) < 0.9) {		
			return chosenSide;		
		} else {
            //canto superior direito do gol
			if (posXHandCursor < -210) {
                return ShootDirection.ExtremeRight;
			}
			//direita do gol
			if (posXHandCursor > -210 && posXHandCursor <= -70) {
                return ShootDirection.Right;		
			}

			//centro
			if (posXHandCursor> -70 && posXHandCursor <= 70) {
                return ShootDirection.Middle;			
			}
			//esquerda do gol
			if (posXHandCursor > 70 && posXHandCursor <= 210) {
                return ShootDirection.Left;		
			}
			//canto esquerdo do gol
			if (posXHandCursor> 210) {
                return ShootDirection.ExtremeLeft;		
			}
            return ShootDirection.Middle;
		}
	}

	public static void StopShooting(){
		shooting = false;
		handLocked = false;
		Assets.Scripts.Share.Controllers.GUIController.instance.Enable ();
		//ShowBar ();

	}

	public static void StartShooting(){	
		shooting = false;
		handLocked =  false;
		//HideBar();
	}

	public float GetBarDeltaX(){
		return barDeltaX;
	}

	public float GetBarDeltaY(){
		return barDeltaY;
	}

	public void ExecuteMovement(Movement movement)
	{
		switch (movement)
		{
		/*case Movement.StopHand:
			if(GameManagerShare.instance.ready_to_call_pause)
				GameManagerShare.instance.ExecuteActionOnHandStop();
			break;*/
		default:
			print("outro gesto " + movement.ToString());
			break;
		}
	}
}

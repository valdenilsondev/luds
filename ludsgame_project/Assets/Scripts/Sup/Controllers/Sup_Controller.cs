using UnityEngine;
using System.Collections;
using Assets.Scripts.Share.Kinect;
using Share.Managers;
using Assets.Scripts.Share.Enums;
using Share.KinectUtils;
using Assets.Scripts.Share.Managers;
using UnityEngine.UI;
using System.Linq;

public class Sup_Controller : MonoBehaviour {

	public GameObject front_left_engine;
	public GameObject front_right_engine;
	//criando engine pra frente
	public GameObject front_engine;
	public GameObject back_engine;
	//força pra frente localizada no fundo da prancha
	public GameObject front_back_engine;
 
	public GameObject left_splash;
	public GameObject right_splash;
	public GameObject splashPrefab;
	
	public float strength;
	public float stabilityAngleLimit;
	public float timeStabilishBalance;

	private bool front_left_engine_force;
	private bool front_right_engine_force;
	//criando impulso pra frente
	private bool front_engine_force;

	private bool lock_engine;
	private float standardDensity;
	private GameObject prancha;
	private float timer;
	private bool triggerBridge = false;

	private int neckJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
	private int spineJoint = (int)KinectWrapper.NuiSkeletonPositionIndex.Spine;
	private uint playerId;

	public static Sup_Controller instance;
	private Animator boy_anim;
	private float timer_toSayRow = 10f, elapsedTime = 0f;
	private bool remando = false, neverRow = true;
	public Text user_Feedback;

	void Start()
	{
		prancha = this.gameObject;
		instance = this;
		//standardDensity = prancha.GetComponent<Buoyancy>().density;
		boy_anim = this.GetComponentInChildren<Animator> ();
	}

	void Update ()
	{
		if(elapsedTime >= timer_toSayRow && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			elapsedTime = 0;
			user_Feedback.text = "REME!";
			Invoke("HideFeedback", 1);
		}
		else if((elapsedTime <  timer_toSayRow || neverRow == true) &&  GameManagerShare.IsStarted())
		{
			elapsedTime += Time.deltaTime;
		}
		//inputs de teste, remada
		if(Input.GetKeyDown(KeyCode.LeftArrow) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			LeftEngineImpulse();
		}
		if(Input.GetKeyDown(KeyCode.RightArrow) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			RightEngineImpulse();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			FrontEngineImpulse();
		}

		//remada atraves do kinect (cube man)
		if(CubeManRowManager.IsRowLeft() && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			LeftEngineImpulse();
			CubeManRowManager.ResetRows();
		}
		if(CubeManRowManager.IsRowRight() && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			RightEngineImpulse();
			CubeManRowManager.ResetRows();
		}

		//inputs de teste, parada
		if(Input.GetKey(KeyCode.R))
		{
			ClearForces(prancha);
			ClearForces(front_left_engine);
			ClearForces(front_right_engine);
			ClearForces(front_engine);
			ClearForces(back_engine);
		}

		RestabilishBalance();
		RestabilishHeight();
		if(GameManagerShare.instance.IsUsingKinect())
		{
// 			playerId = KinectManager.Instance.GetPlayer1ID ();
			//ControlPlayerWithTorso ();
		}
		if(GameManagerShare.IsStarted() && GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{
			ClearForces(prancha);
			ClearForces(front_left_engine);
			ClearForces(front_right_engine);
			ClearForces(front_engine);
			ClearForces(back_engine);
			//impulso pra frente vindo de tras da prancha
			ClearForces(front_back_engine);

		}
	}

	public void LeftEngineImpulse()
	{
		if(front_left_engine_force)
		{
			front_left_engine.GetComponent<Rigidbody>().AddForce(front_left_engine.transform.forward * strength, ForceMode.Impulse);
			front_right_engine.GetComponent<Rigidbody>().AddForce(-front_right_engine.transform.forward * (strength*0.45f), ForceMode.Impulse);
			back_engine.GetComponent<Rigidbody>().AddForce(-back_engine.transform.forward * (strength*0.70f), ForceMode.Impulse);
		}else{
			front_left_engine.GetComponent<Rigidbody>().AddForce(front_left_engine.transform.forward * (strength), ForceMode.Impulse);
		}
		front_left_engine_force = true;
		front_right_engine_force = false;
		boy_anim.SetTrigger ("row_L");
		elapsedTime = 0;
		neverRow = false;
		SupSoundManager.Instance.PlayRow();
	}

	//empurra pra frente + empurra 0.25 pra esquerda
	public void RightEngineImpulse()
	{
		if(front_right_engine_force)
		{
			front_right_engine.GetComponent<Rigidbody>().AddForce(front_right_engine.transform.forward * strength, ForceMode.Impulse);
			front_left_engine.GetComponent<Rigidbody>().AddForce(-front_left_engine.transform.forward * (strength*0.45f), ForceMode.Impulse);
			back_engine.GetComponent<Rigidbody>().AddForce(-back_engine.transform.forward * (strength*0.70f), ForceMode.Impulse);
		}else{
			front_right_engine.GetComponent<Rigidbody>().AddForce(front_right_engine.transform.forward * (strength), ForceMode.Impulse);
		}
		front_left_engine_force = false;
		front_right_engine_force = true;
		boy_anim.SetTrigger ("row_R");
		elapsedTime = 0;
		neverRow = false;
		SupSoundManager.Instance.PlayRow();
	}

	public void FrontEngineImpulse()
	{
		if(front_engine_force)
		{
			front_engine.GetComponent<Rigidbody>().AddForce(front_engine.transform.forward * (strength/2), ForceMode.Impulse);
			front_back_engine.GetComponent<Rigidbody>().AddForce(front_back_engine.transform.forward * (strength/2), ForceMode.Impulse);
		}
		front_left_engine_force = false;
		front_right_engine_force = false;
		front_engine_force = true;
		//boy_anim.SetTrigger ("row_R");
		neverRow = false;
		elapsedTime = 0;
	}

	private void ClearForces(GameObject go)
	{
		timer = Time.deltaTime + timer;
		if(timer > 0.5f){
			go.GetComponent<Rigidbody>().velocity = Vector3.zero;
			timer = 0;

		}
	}

	private void RestabilishBalance(){
		Vector3 pranchaEulerAng = prancha.transform.rotation.eulerAngles;
		if((pranchaEulerAng.x > stabilityAngleLimit && pranchaEulerAng.x < 90) ||
		   (pranchaEulerAng.x < 360-stabilityAngleLimit && pranchaEulerAng.x > 270)){
			prancha.transform.rotation = new Quaternion(Mathf.Lerp(prancha.transform.rotation.x, 0, timeStabilishBalance*Time.deltaTime),
			                                            prancha.transform.rotation.y, prancha.transform.rotation.z, prancha.transform.rotation.w);
			//ClearForces(prancha);
		}
		if((pranchaEulerAng.z > stabilityAngleLimit && pranchaEulerAng.z < 90) ||
		   (pranchaEulerAng.z < 360-stabilityAngleLimit && pranchaEulerAng.z > 270)){
			prancha.transform.rotation = new Quaternion(prancha.transform.rotation.x, prancha.transform.rotation.y,
			                                            Mathf.Lerp(prancha.transform.rotation.z, 0, timeStabilishBalance*Time.deltaTime), prancha.transform.rotation.w);
			//ClearForces(prancha);
		}
	}

	private void RestabilishHeight(){
		if(prancha.transform.position.y > 0.5f){
			prancha.GetComponent<Rigidbody>().mass = 4;
			//ClearForces(prancha);
		}else if(prancha.transform.position.y < -0.5f){
			prancha.GetComponent<Rigidbody>().mass = 0.1f;
			ClearForces(prancha);
		}else{
			prancha.GetComponent<Rigidbody>().mass = 1;
		}
	}

	private void HideFeedback()
	{
		user_Feedback.text = string.Empty;
	}
	public void Splash(Vector3 pos)
	{
		GameObject p =	Instantiate(splashPrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity) as GameObject;
		//SoundManager.Instance.PlaySPlash();
		Destroy(p, p.GetComponent<ParticleSystem>().duration);
	}

	public void SplashLeft()
	{
		GameObject p =	Instantiate(splashPrefab,left_splash.transform.position, Quaternion.identity) as GameObject;
		if(SoundManager.Instance != null){
			//SoundManager.Instance.PlaySPlash();
		}
		Destroy(p, p.GetComponent<ParticleSystem>().duration);
	}

	public void SplashRight()
	{
		GameObject p =	Instantiate(splashPrefab, right_splash.transform.position, Quaternion.identity) as GameObject;
		if(SoundManager.Instance != null){
			//SoundManager.Instance.PlaySPlash();
		}
		Destroy(p, p.GetComponent<ParticleSystem>().duration);
	}

	public bool GetTriggerBridge()
	{
		return triggerBridge;
	}

	void OnCollisionEnter(Collision col)
	{
		if(col.transform.name == "Sup_Terrain")
		{
			/*Splash(col.transform.position);
			Vector3 contactPt = col.contacts[0].point;
			ClearForces(prancha);
			var currentVelocityX = prancha.GetComponent<Rigidbody>().velocity.x;
			var currentVelocityY = prancha.GetComponent<Rigidbody>().velocity.y;
			var currentVelocityZ = prancha.GetComponent<Rigidbody>().velocity.z;
			prancha.GetComponent<Rigidbody>().AddExplosionForce((currentVelocityX+currentVelocityY+currentVelocityZ)*50, contactPt, 50);*/
			Vector3 contactPt = col.contacts[0].point;
			contactPt = -contactPt.normalized;
			prancha.GetComponent<Rigidbody>().AddForce(contactPt*100);
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.transform.name == "Bridge")
		{
			triggerBridge = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.transform.name == "Bridge" )
		{
			triggerBridge = true;
		}
		if(SupManager.instance.Get_ReadyToBackToIsland() == false)
		{
			if(col.transform.parent == null){
				print (col.name);
			}
			if(col.transform.parent.name == "Apples")
			{
				HandleApples.instance.CollectApple(col.gameObject);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple,3,0);
				if(SoundManager.Instance != null){
					SoundManager.Instance.selectionSounds[0].Play ();
				}
			}
			if (col.transform.parent.name == "Chests") 
			{
				Destroy(col.gameObject);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple,30,0);
				SupManager.instance.AddTime(30);
				if(SoundManager.Instance != null){
					SoundManager.Instance.selectionSounds[0].Play ();
				}
			}

		}
		else
		{
			if(col.transform.parent.name == "Apples")
			{
				HandleApples.instance.CollectApple(col.gameObject);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple,1,0);
				if(SoundManager.Instance != null){
					SoundManager.Instance.selectionSounds[0].Play ();
				}
			}
			if (col.transform.parent.name == "Chests") 
			{
				print ("Redy to go to island");
				Destroy(col.gameObject);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple,10,0);
				SupManager.instance.AddTime(10);
				if(SoundManager.Instance != null){
					SoundManager.Instance.selectionSounds[0].Play ();
				}
			}

		}
		if(col.transform.name == "ZoomOutArea")
		{
			CameraZoomControl.instance.HandleCameraZoom();
		}

	}
	
	public void ExecuteMovement(Movement movement)
	{	
		switch (movement)
		{
		case Movement.RemarLeft:
			if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()){
				LeftEngineImpulse();
				print ("remar pra esquerda");
			}
			break;
		case Movement.RemarRight:
			if(CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()){
				RightEngineImpulse();
				print ("remar pra direita");
			}
			break;
		/*case Movement.StopHand:
			if(GameManagerShare.instance.ready_to_call_pause)
				GameManagerShare.instance.ExecuteActionOnHandStop();
			break;*/
		case Movement.ArmsUp:
			print ("impulso pra frente");
			FrontEngineImpulse();
			break;
		default:
			print("outro gesto " + movement.ToString());
			break;
		}
	}

	/*void ControlPlayerWithTorso(){
		float dist = GenericKinectMethods.instance.GetJointsDistanceX(neckJoint, spineJoint, playerId);
		if(KinectManager.Instance.IsPlayerCalibrated(playerId) && GameManagerShare.IsStarted()
		   && !GameManagerShare.IsGameOver() && !GameManagerShare.IsPaused()){
			if(dist > 0.1f || dist < -0.1f){
				GameManagerShare.instance.PauseGame();
			}
		}
	}*/

}


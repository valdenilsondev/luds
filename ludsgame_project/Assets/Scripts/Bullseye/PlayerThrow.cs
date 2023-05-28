using UnityEngine;
using System.Collections;
using Assets.Scripts.Share.Kinect;
using Share.Managers;
using Assets.Scripts.Share.Enums;
using UnityEngine.UI;
using Share.EventsSystem;
using Assets.Scripts.Share.Managers;
using Share.Managers;

namespace Bullseye {
	public class PlayerThrow : MonoBehaviour {

		public Transform target;
		public float power = 80;
		public float sensitivity = 3;
		public Text debug;

		private bool shoot = false;
		private float initialPos = -1.22f, finalPos = 1.22f, deltaPos = 0f;
		private float initialRot = -22f, finalRot = 22f, deltaRot = 0f;
		private float mousePosX = 0;
		public bool handFull;
		Rigidbody  apple;
		public static PlayerThrow instance;
		private float moveSideAmount;

		void Start(){
			instance = this;
			apple = ApplesPool.Instantiate();
			apple.gameObject.SetActive(true);
			appleP.gameObject.SetActive (false);
			appleP.GetComponent<Rigidbody>().useGravity = false;
			appleP.GetComponent<Rigidbody>().isKinematic = true;
			//	apple.transform.localPosition = new Vector3(0, 0, 0);
			initialVec3 = appleP.transform.position;
			handFull = true;
			moveSideAmount = PlayerPrefsManager.GetMovingSideAmount();
		}

		void Update () {
			if(GameManagerShare.IsPaused() || !GameManagerShare.IsStarted())return;// isPlaying) return;

			mousePosX = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
			deltaPos = initialPos + ((finalPos - initialPos) * mousePosX);
			deltaPos *= sensitivity;
			deltaRot = initialRot + ((finalRot - initialRot) * mousePosX);
			deltaRot *= sensitivity;

			//transform.position = new Vector3(deltaPos, transform.position.y, transform.position.z);
			//transform.eulerAngles = new Vector3(transform.eulerAngles.x, deltaRot, transform.eulerAngles.z);

			if(Input.GetMouseButtonDown(0) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()) {
				//ShootApple();
				CallShootAnim(0.2f);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, 1, 0);
			}

			if(Input.GetKeyDown(KeyCode.LeftArrow) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()) {
				if(transform.position.x > initialPos) {
					transform.position = new Vector3(transform.position.x - deltaPos, transform.position.y, transform.position.z);
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - deltaRot, transform.eulerAngles.z);
				}
				CallShootAnim(-2.0f);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, 1, 0);
			}

			if(Input.GetKeyDown(KeyCode.RightArrow) && CountDownManager.instance.IsCounting() == false && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()) {
				if(transform.position.x < finalPos) {
					transform.position = new Vector3(transform.position.x + deltaPos, transform.position.y, transform.position.z);
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + deltaRot, transform.eulerAngles.z);
				}
				CallShootAnim(2.0f);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, 1, 0);
			}
			if(GameManagerShare.instance.IsUsingKinect()){
				UpdatePigPos();
			}
		}

		float hipPos;
		public void UpdatePigPos(){
			hipPos = KinectManager.Instance.GetJointPosition (KinectManager.Instance.GetPlayer1ID (), 
			                                         (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).x;
		/*	this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(hipPos*10, this.transform.position.y, this.transform.position.z), Time.deltaTime *1);*/

			if(hipPos< moveSideAmount && hipPos > -moveSideAmount){
				//centro
				this.transform.position = new Vector3(0.49f, -3.5f,-2.24f);
			}
			if(hipPos < -moveSideAmount){
				//esquerda
				this.transform.position = new Vector3(-4.70f, -3.5f,-2.24f);
			}
			if(hipPos > moveSideAmount){
				//direita
				this.transform.position = new Vector3(3.40f, -3.5f,-2.24f);
			}
		}

		float finalHandPos;
		public void CallShootAnim(float finalHandPos){
			GameObject.Find ("handDebug").GetComponent<Text> ().text = "hand pos " + finalHandPos;
			this.gameObject.GetComponent<Animator> ().SetTrigger ("isThrowing");
			ThrowSoundManager.Instance.PlayThrowSfx();
			this.finalHandPos = finalHandPos;

		}

		//ninguem chama
		private void ShootApple() 
		{
			appleP.transform.SetParent(null);
			appleP.gameObject.SetActive (true);
			appleP.GetComponent<Rigidbody>().useGravity = true;
			appleP.GetComponent<Rigidbody>().isKinematic = false;
			apple.AddForce(this.transform.forward.normalized * power, ForceMode.Impulse);
			apple.AddForce(this.transform.right * (finalHandPos*50), ForceMode.Impulse);
			AppleBullet.instace.PlayerAnimationBullet (finalHandPos);

			handFull = false;
		}
		//referencia da mao
		public GameObject r_hand_pig;
		private Vector3 initialVec3;
		public void PrepareApple(){
			if (!handFull) {
				//apple = ApplesPool.Instantiate ();
				appleP.transform.position = initialVec3;
				Transform applesContainer = GameObject.Find ("Apple_Hand").transform;
				appleP.transform.SetParent(applesContainer);
				appleP.GetComponent<Rigidbody>().useGravity = false;
				appleP.GetComponent<Rigidbody>().isKinematic = true;
				appleP.gameObject.SetActive (false);
				handFull = true;
			}
		}
		//maça a ser jogada
		public GameObject appleP;

		public void ExecuteMovement(Movement movement)
		{
			switch (movement)
			{
			case Movement.Throw:
			//	CallShootAnim(Aim_controller.instance.targetPosix);
				CallShootAnim(this.transform.position.x);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, 1, 0);
				break;
			case Movement.RaiseHand:
				//	CallShootAnim(Aim_controller.instance.targetPosix);
				CallShootAnim(this.transform.position.x);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, 1, 0);
				break;
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

}
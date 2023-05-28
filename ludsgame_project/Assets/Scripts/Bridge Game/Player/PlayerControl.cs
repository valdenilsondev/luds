using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using BridgeGame.Bridge;
using Share.KinectUtils.Record;
using Share.Database;
using BridgeGame.UI;
using BridgeGame.GameUtilities;
using BridgeGame.KinectControl;
using Share.KinectUtils;
using Share.Managers;
using Assets.Scripts.Share.Managers;
using Assets.Scripts.Share.Enums;

namespace BridgeGame.Player {
	public class PlayerControl : MonoBehaviour {
		
		//referencia para sabermos se o player esta indo para frente ou para tras
		public bool toggleWalkForward = true;
		
		//referencia para o player
		private static GameObject player;
		private GameObject piranhaOnPig;
			
		//definir qual pieces esta mais proxima do player
		private int closest = 0;
		
		//variaveis de controle do jogo
		private bool playerLost;
		private bool playerWon;
		private bool isBoostOn = false;
		private bool pig_walking = false;
		//gui
		private float angle;
		public GameObject placa;
		public Sprite placaFeliz, placaTristeL, placaTristeR;
		
		public int playerSpeed;
		public int playerRotationSpeed;
		public float rotationLimit;
		public float camerInitialIncrementZ;
		public GameObject splashPrefab;
		private bool splashed;

		private int defaultPlayerSpeed;
		private int defaultPlayerSpeedBoost;
		private RigidbodyConstraints constraints;
		private Vector3 playerInitialPosition;
		public static float balanceBarRedPartWidth;

		private bool spacePressedOrHandRaisedStartGame = false;	
		public static PlayerControl instance;		
		private Animator pigAnimatorController;
		
		private bool check = false; // usado no restart
		private bool checkIfLost = false;
		
		void Awake(){
			instance = this;
			
			//player
			player = this.gameObject;
			constraints = player.GetComponent<Rigidbody>().constraints;

			//positions e rotations
			playerInitialPosition = this.transform.position;
			
			//speed and boost speed
			defaultPlayerSpeed = playerSpeed;
			//recebe por padrao do sistema chaveado 0, 1 ou 2
			int value = PlayerPrefsManager.GetBridgeBoost();
			if(value == 0)
			{
				value = 2;
			}else if(value == 1)
			{
				value = 3;
			}else if(value == 2)
			{
				value = 4;
			}
			defaultPlayerSpeedBoost = playerSpeed * value;
		}
		
		// Use this for initialization
		void Start () {
			if (!GameManagerShare.instance.IsUsingKinect())
			{
				GameObject.Find("azeitona").GetComponent<PlayerControlKinect>().enabled = false;
			}
			
			//animator controller
			pigAnimatorController = player.GetComponent<Animator>();
			SetPigBridgeIddle();
			
			//referencia para o pig na piranha
			piranhaOnPig = GameObject.Find("piranhaOnPig").gameObject;
			piranhaOnPig.SetActive(false);
			//BridgeManager.instance.playerBar.gameObject.GetComponent<Image>();
			//GUIplayer_bar
			balanceBarRedPartWidth = (BridgeManager.instance.GetBalanceBarRedPart().GetComponent<RectTransform>().rect.width - 
			                          (BridgeManager.instance.GetBalanceBarRedPart().GetComponent<RectTransform>().rect.width * 0.35f));

		}
		void Update()
		{	
			//angle = angulo do porco
			if(angle <= 135 && angle >= -135)
			{
				//criar 2 sprites publicos e arrastar o sprite para o objeto
				placa.gameObject.GetComponent<Image>().sprite = placaFeliz;
			}
			else if(angle < -135)
			{
				placa.gameObject.GetComponent<Image>().sprite = placaTristeL;
			}
			else if(angle > 135)
			{
				placa.gameObject.GetComponent<Image>().sprite = placaTristeR;
			}

		}

		public GameObject GetPlayerBar()
		{
			return placa;
		}
		public Animator GetPigAnimtorController()
		{
			return pigAnimatorController;
		}
		
		public int GetDefaultPlayerSpeed(){
			return defaultPlayerSpeed;
		}
		
		public int GetDefaultPlayerSpeedBoost(){
			return defaultPlayerSpeedBoost;
		}
		
		public void SetPigBridgeWalking(){
			pigAnimatorController.SetTrigger("pigBridgeWalking");
			pig_walking = true;
			BridgeSoundManager.Instance.PlayBridgeNoise();
			//BridgeSoundManager.Instance.PlaySteps();
		}
		public bool GetPigWalking()
		{
			return pig_walking;
		}
		public void SetPigBridgeIddle(){
			pigAnimatorController.SetTrigger("pigBridgeIddle");
			pig_walking = false;
			BridgeSoundManager.Instance.StopBridgeNoise();
		}

		
		public GameObject GetPlayer(){
			return player;
		}

		public void MovePlayerBar()
		{
			if(GameManagerShare.IsStarted()){
				if(player.transform.eulerAngles.z >= (360-rotationLimit) && player.transform.eulerAngles.z <= 360){
					GetPlayerBar().GetComponent<RectTransform>().anchoredPosition = 
						new Vector2(  ( (rotationLimit-(player.transform.eulerAngles.z-(360-rotationLimit))) / rotationLimit) * (balanceBarRedPartWidth/2) , GetPlayerBar().GetComponent<RectTransform>().anchoredPosition.y);
					angle = ((rotationLimit-(player.transform.eulerAngles.z-(360-rotationLimit))) / rotationLimit) * (balanceBarRedPartWidth/2);
				}
				else if(player.transform.eulerAngles.z >= 0 && player.transform.eulerAngles.z <= rotationLimit){
					GetPlayerBar().GetComponent<RectTransform>().anchoredPosition = 
						new Vector3( -(((player.transform.eulerAngles.z) / rotationLimit) * (balanceBarRedPartWidth/2)), GetPlayerBar().GetComponent<RectTransform>().anchoredPosition.y);
					angle = -(((player.transform.eulerAngles.z) / rotationLimit) * (balanceBarRedPartWidth/2));
				}
			}
		}

		public void Walking()
		{			
            //acelerar
			if (Input.GetKey(KeyCode.UpArrow))
			{
				PlayerControl.instance.playerSpeed = PlayerControl.instance.GetDefaultPlayerSpeedBoost();
				isBoostOn = true;
				BridgeSoundManager.Instance.PlaySpeedSteps();
			}
			else {
				PlayerControl.instance.playerSpeed = PlayerControl.instance.GetDefaultPlayerSpeed();
				isBoostOn = false;
				BridgeSoundManager.Instance.PlaySteps();
			}
			
			//indo para frente
			if (toggleWalkForward && playerLost == false && playerWon == false && !PlayerControlKinect.instance.IsBoostOn()) {
				BridgeManager.instance.GetPlayerCamera().transform.position -= BridgeManager.instance.GetPlayerCamera().transform.forward * Time.deltaTime * playerSpeed;
				player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, BridgeManager.instance.GetPlayerCamera().transform.position.z-3);
				//anim speed normal
				GetPigAnimtorController().speed = 1;

			}
			else if(toggleWalkForward && playerLost == false && playerWon == false && PlayerControlKinect.instance.IsBoostOn())
			{
				BridgeManager.instance.GetPlayerCamera().transform.position -= BridgeManager.instance.GetPlayerCamera().transform.forward * Time.deltaTime * defaultPlayerSpeedBoost;
				player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, BridgeManager.instance.GetPlayerCamera().transform.position.z-3);
				//acelerar anim
				GetPigAnimtorController().speed = 2;
			}
			
			//rotacionando para esquerda
			if (Input.GetKey (KeyCode.LeftArrow))
			{
				player.transform.Rotate (Vector3.forward * PlayerControlKinect.instance.GetPlayerSensibilityRotation()/*playerRotationSpeed*/ * Time.deltaTime);
			}
			//rotacionando para direita
			else if (Input.GetKey (KeyCode.RightArrow))
			{
				player.transform.Rotate (-Vector3.forward * PlayerControlKinect.instance.GetPlayerSensibilityRotation()/*playerRotationSpeed*/ * Time.deltaTime);
			}

		}
		
		public bool CheckIfPlayerFelt()
		{
			//aplicando queda ao player
			if ( (player.transform.GetChild (0).transform.rotation.eulerAngles.z > rotationLimit && player.transform.GetChild (0).transform.rotation.eulerAngles.z < 360 - rotationLimit) &&
			     ( !BridgeManager.instance.IsPlayerOnTheWater() ) ) 
			{
				//pausa o jogo
				BridgeManager.instance.SetPlayerOnTheWater(true);

				//player perde uma vida
				LifesManager.instance.LifeLost();

				if(!LifesManager.instance.IsDead())
				{
					//player retorna para a ponte, com pos e rot ajustadas
					Invoke("GetPlayerBackToBridge", 2);
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public void PlayPigFallingAnimation()
		{
			if(player.transform.GetChild (0).transform.rotation.eulerAngles.z > 0 && player.transform.GetChild (0).transform.rotation.eulerAngles.z < rotationLimit+1)
			{
				player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
				pigAnimatorController.SetTrigger("pigBridgeLeftFall");
				Invoke("MoveItToBottomLeftOfOcean", 0.5f);
			}
			else
			{
				player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
				pigAnimatorController.SetTrigger("pigBridgeRightFall");
				Invoke("MoveItToBottomRightOfOcean", 0.5f);
			}
		}

		private void GetPlayerBackToBridge()
		{
			checkIfLost = false;
			BridgeManager.instance.ResetSphereBar();
			player.GetComponent<Rigidbody>().useGravity = false;
			SetPigBridgeIddle();
			//if(BridgeManager.instance.difficult != 3){
			player.transform.position = new Vector3(97, 60, player.transform.position.z);
			//}
			/*else{
				player.transform.position = new Vector3(97, 60, 169);
				BridgeManager.instance.GetPlayerCamera().transform.position = new Vector3(97, 55, 172);
			}*/
			ResetPlayerRotation();
			player.GetComponent<Rigidbody>().useGravity = true;
			player.GetComponent<Rigidbody>().constraints = constraints;
			CountDownManager.instance.Initialize();
		}

		public void ResetPlayerRotation(){
			player.transform.rotation = new Quaternion(0, 180, 0, 0);
		}
		
		private void MoveItToBottomLeftOfOcean()
		{
			if(checkIfLost == false)
			{
				player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

				iTween.MoveTo(player, iTween.Hash("x", 99, "time", 0.5f, "easytype", iTween.EaseType.linear));
				checkIfLost = true;
			}
		}
		
		private void MoveItToBottomRightOfOcean()
		{
			if(checkIfLost == false)
			{
				player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

				iTween.MoveTo(player, iTween.Hash("x", 95, "time", 0.5f, "easytype", iTween.EaseType.linear));
				checkIfLost = true;
			}
		}
		
		public GameObject GetPiranhaOnPigReference()
		{
			return piranhaOnPig;
		}
		
		public void ActivatePiranha()
		{
			piranhaOnPig.SetActive(true);
			piranhaOnPig.GetComponent<Animator>().SetBool("piranhaHit", true);
		}
		
		public void DeactivatePiranha()
		{
			piranhaOnPig.GetComponent<Animator>().SetBool("piranhaHit", false);
			piranhaOnPig.SetActive(false);
		}
		
		public void PiranhaHitPlayer(GameObject piranha)
		{
			LifesManager.instance.LifeLost();
			ActivatePiranha();
			if (LifesManager.instance.IsDead()) 
			{
				pigAnimatorController.SetTrigger ("pigBridgeLeftFall");
				Invoke ("MoveItToBottomLeftOfOcean", 0.5f);
			} 
			else 
			{
				StartCoroutine(WaitToDeactivate(piranha));
			}
		}
		
		IEnumerator WaitToDeactivate(GameObject piranha) 
		{ 		
			yield return new WaitForSeconds (3f);
			DeactivatePiranha();
		}
		
		public Vector3 GetPlayerCameraInitialPosition(){return  new Vector3 (playerInitialPosition.x, 60, playerInitialPosition.z+camerInitialIncrementZ);}
		
		public static PlayerControl GetInstance() {
			if(instance == null) {
				instance = FindObjectOfType<PlayerControl>();
			}
			return instance;
		}		

		public void OnTriggerEnter(Collider col){
			if (col.tag == "Ocean") {
				if(!splashed){
					Instantiate(splashPrefab, new Vector3(this.transform.position.x, col.transform.position.y, this.transform.position.z), Quaternion.identity);
					splashed = true;
				}
			}
		}
	}
}
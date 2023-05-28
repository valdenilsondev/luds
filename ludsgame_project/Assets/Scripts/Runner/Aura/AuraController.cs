using UnityEngine;
using System.Collections;

public class AuraController : MonoBehaviour {

	private GameObject auraGreen;
	private GameObject auraBlue;
	private GameObject auraYellow;
	private GameObject auraRed;

	private bool checkChanges = false;
	private GameObject powerUpsDisplay;
	public Transform player;

	public static AuraController instance;

	void Start () {
		instance = this;

		//referencias
		powerUpsDisplay = GameObject.Find ("PowerUpsDisplay").gameObject;
		auraGreen = GameObject.Find("AuraGreen").gameObject	;
		auraBlue = GameObject.Find("AuraBlue").gameObject	;
		auraYellow = GameObject.Find("AuraYellow").gameObject	;
		auraRed = GameObject.Find("AuraRed").gameObject	;

		auraGreen.SetActive(false);
		auraBlue.SetActive(false);
		auraYellow.SetActive(false);
		auraRed.SetActive(false);
	}

	void Update () {
		//atualizando posicao
        this.transform.position = new Vector3(player.position.x, -0.5f, player.position.z);

		//verificar se tem algum power up ativo
		CheckActivatedPowerUp();

		if(Input.GetKeyDown(KeyCode.A)){
			ActivateRedAura();
		}

		if(Input.GetKeyDown(KeyCode.B)){
			DeactivateAll();
		}
	}

	public void CheckActivatedPowerUp(){
		if(powerUpsDisplay.activeSelf == false){
			DeactivateAll();
		}
	}

	public void ActivateRedAura(){
		DeactivateAll();
		auraRed.SetActive(true);
		//this.GetComponent<Animator>().SetTrigger("AuraRed");
		powerUpsDisplay.SetActive(true);
	}

	public void ActivateGreenAura(){
		DeactivateAll();
		auraGreen.SetActive(true);
		//this.GetComponent<Animator>().SetTrigger("AuraGreen");
		powerUpsDisplay.SetActive(true);
	}

	public void ActivateBlueAura(){
		DeactivateAll();
		auraBlue.SetActive(true);
		//this.GetComponent<Animator>().SetTrigger("AuraBlue");
		powerUpsDisplay.SetActive(true);
	}

	public void ActivateYellowAura(){
		DeactivateAll();
		auraYellow.SetActive(true);
		//this.GetComponent<Animator>().SetTrigger("AuraYellow");
		powerUpsDisplay.SetActive(true);
	}

	public void DeactivateAll(){
		auraGreen.SetActive(false);
		auraBlue.SetActive(false);
		auraYellow.SetActive(false);
		auraRed.SetActive(false);
	}























}

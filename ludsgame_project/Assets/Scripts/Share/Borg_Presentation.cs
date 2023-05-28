using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;
using UnityStandardAssets.ImageEffects;

public class Borg_Presentation : MonoBehaviour {
	public GameObject texto_explicativo_go;
	public GameObject banner_LevelToLoad;
	private float letterPause = 0.05f;
	public static Borg_Presentation instance;
	//strings
	private string message;
	public bool isGrandpaScreenOn;

	void Awake(){
		instance = this;
	}
	// Use this for initialization
	void Start () {
		this.transform.localScale = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowPresentation(){
		isGrandpaScreenOn = true;
		Camera.main.GetComponent<BlurOptimized>().enabled = true;
		this.transform.localScale = new Vector3(1,1,1);
		message = this.transform.GetComponentInChildren<Text>().text;
	//	texto_explicativo_go //GameObject.Find("texto_explicativo");
		texto_explicativo_go.GetComponent<Text>().text = "";
		StartCoroutine(TypeText ());
	}

	
	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) {
			//GetComponent<GUIText>().text += letter;	
			texto_explicativo_go.GetComponent<Text>().text += letter;			
			if(texto_explicativo_go.GetComponent<Text>().text == message){
				yield return new WaitForSeconds (5);
				HidePresentation();
			}
			/*if (sound)
				audio.PlayOneShot (sound);*/
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}      
	}

	private void HidePresentation(){
		Camera.main.GetComponent<BlurOptimized>().enabled = false;
		GameManagerShare.instance.StartGameCountDown();
		if(BorgManager.instance.countingBorgTime){			
			BorgManager.instance.borg_bg.SetActive(true);
			BorgManager.instance.BorgList.SetActive(true);
		}
		isGrandpaScreenOn = false;
		Destroy(this.gameObject);
	}

}

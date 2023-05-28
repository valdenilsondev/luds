using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;
using UnityStandardAssets.ImageEffects;

public class GameOver_Presentation : MonoBehaviour {
	public GameObject texto_explicativo_go;
	public GameObject banner_LevelToLoad;
	private float letterPause = 0.05f;
	public static GameOver_Presentation instance;
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
		print("showpresentation");
		isGrandpaScreenOn = true;
		Camera.main.GetComponent<BlurOptimized>().enabled = true;
		this.transform.parent.localScale = new Vector3(1,1,1);
		this.transform.localScale = new Vector3(1,1,1);
		message = this.transform.GetComponentInChildren<Text>().text;
	//	texto_explicativo_go //GameObject.Find("texto_explicativo");
		//texto_explicativo_go.GetComponent<Text>().text = "";
		StartCoroutine(TypeText ());
	}

	
	IEnumerator TypeText () {
		/*foreach (char letter in message.ToCharArray()) {
			texto_explicativo_go.GetComponent<Text>().text += letter;			
			if(texto_explicativo_go.GetComponent<Text>().text == message){
				yield return new WaitForSeconds (5);
				HidePresentation();
			}		
			//yield return 0;
			yield return new WaitForSeconds (letterPause);
		}  */    
		yield return new WaitForSeconds (10);
		HidePresentation();
	}

	private void HidePresentation(){
		Camera.main.GetComponent<BlurOptimized>().enabled = false;
		//GameManagerShare.instance.StartGameCountDown();
		print("hidepresentation");
		Assets.Scripts.Share.Controllers.GameOverScreenController.instance.Show();
		Assets.Scripts.Share.Controllers.GameOverScreenController.instance.ScoreResult();
		isGrandpaScreenOn = false;
		Destroy(this.gameObject);
	}

}

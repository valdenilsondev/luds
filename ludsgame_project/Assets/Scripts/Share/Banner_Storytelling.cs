using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;

public class Banner_Storytelling : MonoBehaviour {
	public Sprite[] story_banner_list;
	private float elapsedTime;
	public float timeChangeScreen = 3;
	private GameObject tutorialScreen;
	bool isGameFirstTime;
	public bool isBannerOn;
	public static Banner_Storytelling instance;

	void Awake(){
		isBannerOn = true;
		instance = this;
	}

	// Use this for initialization
	void Start () {
		tutorialScreen = GameObject.Find("TutorialScreen").gameObject;
		//comentado para que va direto para o tutorial
		/*if(GameManagerShare.instance.IsThisGameFirstTime(GameManagerShare.instance.game)){
			print("hidetutorial");
			HideTutorial();
		}else{
			//print("showtutorial");
			ShowTutorial();
			Destroy(this.gameObject);
		}*/
		ShowTutorial();
		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if(elapsedTime > timeChangeScreen){
			elapsedTime = 0;
			CallNextScreen();
		}
	}

	int nextIndex;
	private void CallNextScreen(){
		nextIndex++;
		if(nextIndex< story_banner_list.Length){
			this.gameObject.GetComponent<Image>().sprite = story_banner_list[nextIndex];
		}else{
			ShowTutorial();
		}
	}

	private void HideTutorial(){
		tutorialScreen.GetComponent<TutorialScreen>().enabled = false;
		tutorialScreen.SetActive(false);
	}

	private void ShowTutorial(){
		tutorialScreen.GetComponent<TutorialScreen>().enabled = true;
		tutorialScreen.SetActive(true);
		isBannerOn = false;
		Destroy(this.gameObject);
	}

}

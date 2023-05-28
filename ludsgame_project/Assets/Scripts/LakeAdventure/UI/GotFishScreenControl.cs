using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using Share.Managers;

public class GotFishScreenControl : MonoBehaviour {

	public Sprite[] fishSprites; 
	public Image fishImage;
	public static GotFishScreenControl instance;
	public GameObject gotFish;
	public GameObject bigPull;
	public GameObject fish_text;

	private Text gotFish_text, pullTime_text;
	private Animator gotFish_anim;
	// Use this for initialization

	void Awake(){
		instance = this;
		gotFish_text = fish_text.gameObject.GetComponent<Text>();
		gotFish_anim = gotFish.gameObject.GetComponent<Animator>();
		pullTime_text = bigPull.gameObject.GetComponent<Text>();
	}
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ShowGotFishScreen(int index){			
		fishImage.sprite = fishSprites[index-1];	
		gotFish_text.text = "FISGOU!";// + FishingManager.instance.GetCurrenFish().GetFishType().ToString();
		BatataFishAnimatorController.instance.PlayBatataWin();
		fishImage.SetNativeSize();
		gotFish_anim.SetTrigger("showFishScreen");
		FishingManager.instance.EncreaseCurrentFish();

		FishingSoundManager.Instance.StopBoatIddle();
		FishingSoundManager.Instance.StopWatter_Environment();
		if(SoundManager.Instance != null)
			SoundManager.Instance.StopBGmusic();
		FishingSoundManager.Instance.PlayYouGot();
	}

	public void ShowFishScapeScreen(){
		BatataFishAnimatorController.instance.PlayBatataPull();
		gotFish_text.text = "O PEIXE ESCAPOU";
		gotFish_anim.SetTrigger("showFishScapeScreen");

		FishingSoundManager.Instance.StopBoatIddle();
		FishingSoundManager.Instance.StopWatter_Environment();
		if(SoundManager.Instance != null)
			SoundManager.Instance.StopBGmusic();
		FishingSoundManager.Instance.PlayYouLost();
	}

	public void ShowTimeToPull(){
		pullTime_text.text = "PUXE!";
		Invoke("HideFeedback",1);
	}

	private void HideFeedback()
	{
		pullTime_text.text = string.Empty;
	}
}

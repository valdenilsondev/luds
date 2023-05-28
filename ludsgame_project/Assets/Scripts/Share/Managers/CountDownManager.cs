using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Share.Managers;
using System.Linq;
using Runner.Managers;
using UnityEngine.SceneManagement;

public class CountDownManager : MonoBehaviour {
	
	//private Text countDown;
	private bool countDownOver, countDownIsStarded = false;
	//[SerializeField]
	//private Sprite count1, count2, count3, countVai;
	public Text countDown;
	//public GameObject countDown_GO;
	private bool isCounting = false;

	public AudioClip[] audios;
	private AudioSource bipCountdown, music;

	public static CountDownManager instance;

	void Awake(){
		instance = this;
		bipCountdown = this.GetComponent<AudioSource>();
        music = this.GetComponent<AudioSource>();
        countDown = this.gameObject.GetComponent<Text>();
		//countDown_GO.gameObject.GetComponent<Image>().sprite = countVai;
		//countDown_GO.SetActive(false);
	}
	
	public void Initialize()
	{
		countDownIsStarded = true;
//		print ("initialize / start countdown,3");
		StartCoroutine("StartCountdown", 3);
		isCounting = true;
	}

	public void SetCountDownOver(bool isOver){
		countDownOver = isOver;
	}

    public bool IsCounting() {
		return isCounting;
    }

	private IEnumerator StartCountdown(int time)
	{
		if (SceneManager.GetActiveScene().name == "PigRunner")
		{
			FloorMovementControl.instance.startGame = false;
		}

		while (time >= 0)
		{		
			countDownOver = false;
			switch (time.ToString())
			{
			case "3":
				//countDown.gameObject.GetComponent<Image>().sprite = count3;
				countDown.text = time.ToString();
				bipCountdown.clip = audios[0];
				bipCountdown.Play();
				break;
			case "2":
				countDown.text = time.ToString();
				bipCountdown.clip = audios[0];
				bipCountdown.Play();
				break;
			case "1":
				countDown.text = time.ToString();
				bipCountdown.clip = audios[0];
				bipCountdown.Play();
				break;
			case "0":
				countDown.text = "COMEÇOU!";
				bipCountdown.clip = audios[1];
				bipCountdown.Play();    
				music.Play();
				if(GameManagerShare.IsPaused())
				{	
					GameManagerShare.instance.UnPauseGame();
				}
				GameManagerShare.instance.SetActiveBlur(false);
				GameManagerShare.instance.StartGame();
				break;
			}

			//if(!GameManagerShare.IsPaused()){
			time--;			
			//}
			yield return new WaitForSeconds(1);
		}
		isCounting = false;
		countDownOver = true;
		GameManagerShare.instance.pwborg_enter = false;
		countDown.text = string.Empty;
	}

}

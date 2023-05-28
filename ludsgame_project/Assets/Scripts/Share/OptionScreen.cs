using UnityEngine;
using System.Collections;
using Share.Managers;

public class OptionScreen : MonoBehaviour {
	private GameObject musicObj, fxObj;
	public static OptionScreen instance;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		musicObj = GameObject.Find("music_icon");
		fxObj = GameObject.Find("soundfx_icon");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.J)){
			ShowScreen();
		}
	}

	public void TurnMusic(){
		SoundManager.Instance.SwitchMusicSituation();
	} 

	public void TurnOnSoundFX(){
		SoundManager.Instance.SwitchSoundFXSituation();
	}

	public void HideScreen(){
		this.gameObject.GetComponent<Animator>().SetTrigger("ScreenUp");
	}

	public void ShowScreen(){
		this.gameObject.GetComponent<Animator>().SetTrigger("ScreenDown");
	}
}

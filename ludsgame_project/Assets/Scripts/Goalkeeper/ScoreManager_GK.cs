using UnityEngine;
using System.Collections;
using Share.KinectUtils;
using Share.KinectUtils.Record;
//using Goalkeeper.GameUtilities;
using Share.EventSystem;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using Share.Managers;
using Share.EventsSystem;

public class ScoreManager_GK : MonoBehaviour {

	private int Goals;
	public int GoalsLimit;
	private int Saves;
	public int SavesLimit;

	private Text GoalsValue;
	private Text SavesValue;

	private Text goals_go;
	private Text saves_go;
	private Text score_text;


	public static ScoreManager_GK instance;
	public GameObject continueBtn, scores_screen;
	public GameObject  pause_bg;
	public GameObject  gloves_gui, ball_gui;
	
	public bool playerLost;
	public bool playerWon;

	void Awake(){
		playerLost = false;
		playerWon = false;
		instance = this;	
	}

	void Start(){
		GoalsValue = GameObject.Find("GoalsValue").GetComponent<Text>();
		SavesValue = GameObject.Find("SavesValue").GetComponent<Text>();

		continueBtn = GameObject.Find ("Continue_Button");
		//gameOverScreen = GameObject.Find ("gameOverScreen");
		//scores_screen = GameObject.Find ("Scores_screen");
		pause_bg = GameObject.Find ("pause_bg");
		gloves_gui = GameObject.Find ("gloves_GUI");
		ball_gui = GameObject.Find ("ball_GUI");

//		goals_go = GameObject.Find("Item1").transform.FindChild("value").GetComponent<Text>();
	//	saves_go = GameObject.Find("Item2").transform.FindChild("value").GetComponent<Text>();
		score_text = GameObject.Find("Score_description_text").transform.parent.GetComponent<Text>();

	}


}


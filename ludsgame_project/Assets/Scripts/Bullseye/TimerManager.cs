using UnityEngine;
using System.Collections;
using Share.EventsSystem;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Net;
using System;
using Share.Managers;

//Game Throw
public class TimerManager : MonoBehaviour {

	public float maxTime = 30f;
	public Text timer;

	private float time = 0;
	private bool isPaused = false;
	private bool isPlaying = false;
	private KinectManager manager;
	private bool isGameOver;

	void Awake() {
		time = maxTime;
	}

	void Start(){
		manager  = KinectManager.Instance;
		if(manager != null)
			manager.DeleteGesture (manager.GetPlayer1ID (), KinectGestures.Gestures.Push);
	}

	void Update () {
		if(!isPaused && isPlaying) {
			time -= Time.deltaTime;
			timer.text = ((int)time).ToString();
		}

		if(time <= 0) {
			if(!isGameOver)
				GameOver();
		}
	}

	void GameOver() {
		isPlaying = false;
		if (GameManagerShare.instance.IsUsingKinect ()) {
			//SendMotionToDatabase ();
		}
		//Events.RaiseEvent<GameOverEvent>();
	}

	void OnEnable() {
		Events.AddListener<PauseEvent>(OnPause);
		Events.AddListener<UnPauseEvent>(OnPause);
		Events.AddListener<GameStart>(OnGameStarted);
	}

	void OnDisable() {
		Events.RemoveListener<PauseEvent>(OnPause);
		Events.RemoveListener<UnPauseEvent>(OnPause);
		Events.RemoveListener<GameStart>(OnGameStarted);
	}

	private void OnPause() {
		isPaused = !isPaused;
	}

	private void OnGameStarted() {
		isPlaying = true;
	}


	public void SendMotionToDatabase()
	{
		string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedMotion" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Hour +
			"-" + System.DateTime.Today.Minute + ".txt";
		
		StringBuilder sb = new StringBuilder();
		foreach (string st in GestureListener.motionInserts)
		{
			sb.AppendLine(st);
			
		}
		print (sb.ToString ());
		System.IO.File.WriteAllText(folder, sb.ToString());
	//	HttpController.InsertMotion(sb.ToString());
		GestureListener.motionInserts.Clear ();

	}
}

using UnityEngine;
using System.Collections;
using Share.EventsSystem;
using Share.Managers;
using Assets.Scripts.Share.Enums;

public class CountMetersRan : MonoBehaviour {

	private float timer;
	private float mapSpeed;
	private float meters;
	private bool countTime;

	public static CountMetersRan instance; 

	void Awake () {
		instance = this;
	}

    public void Initialize()
    {
        StartCounting();
    }

	void Update () {
		mapSpeed = FloorMovementControl.instance.GetMapSpeed();
		ConvertTimeAndSpeedOnMeters();
	}

	private void ConvertTimeAndSpeedOnMeters(){
		if(countTime){
			timer = timer + Time.deltaTime;
			if(timer > 0.2f){
				timer = 0;
				//velocidade esta em m/s -> calculo da distancia percorrida esta a cada 1/5 de seg
				meters = ( meters + mapSpeed*0.2f);
                GameManagerShare.instance.IncreaseScore(ScoreItemsType.Distance, Mathf.Ceil(CountMetersRan.instance.GetMeters()), 0);
			}
		}
	}

	public void PauseGame(){
		StopCounting();
	}
	
    public void GameOver(){
		StopCounting();
	}

	public void UnPauseGame(){
		StartCounting();
	}

	void OnEnable(){
		Events.AddListener<PauseEvent>(OnPause);
		Events.AddListener<UnPauseEvent>(OnUnPause);
		Events.AddListener<GameOverEvent> (OnGameOver);

	}
	
	void OnDisable(){
		Events.RemoveListener<PauseEvent>(OnPause);
		Events.RemoveListener<PauseEvent>(OnUnPause);
	}
	
	private void OnUnPause(){
		UnPauseGame();	
	}
	
	private void OnPause(){
		PauseGame();	
	}

	private void OnGameOver() {
		StopCounting ();
	}

	public void StartCounting(){
		countTime = true;
	}

	public void StopCounting(){
		countTime = false;
	}

	public float GetMeters(){
		return meters;
	}

    public void Reset()
    {
        meters = 0;
    }
}

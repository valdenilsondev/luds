using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.EventsSystem;
//using Runner.PlayerControl;
using Assets.Scripts.Share.Managers;

public class GameOverScreen : MonoBehaviour {
	

	public static GameOverScreen instance;
	private Text apple_score_gameover_text;
	private Text ray_text_gameover_text;
	private Text score_gameover_text;
	
	void Awake()
    {
        instance = this;
    }
    
    void Start () {
        
	}

	public bool IsGameOverScreenActive(){
        return this.gameObject.activeSelf;
	}

	public void ActivateGameOverScreen(){
        iTween.MoveTo(this.gameObject, iTween.Hash("y", 540, "islocal", true));
	}

	public void FillGameOverScreen(){
		ray_text_gameover_text.text = Mathf.Ceil(CountMetersRan.instance.GetMeters()).ToString();
        apple_score_gameover_text.text = ScoreManager.instance.GetScore().ToString();
        score_gameover_text.text = (ScoreManager.instance.GetScore() * 2 + Mathf.Ceil(CountMetersRan.instance.GetMeters()) * 2).ToString();
	}


}





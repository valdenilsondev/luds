using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.EventsSystem;
using Goalkeeper.Managers;


public class CountToStart : MonoBehaviour {
	public static Text countText;
	private int time = 3; 
	public static bool startCount;
	private	float elapsedTime;


	// Use this for initialization
	void Start () {
		countText = this.gameObject.GetComponent<Text> ();
		countText.text = time.ToString();
		countText.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if(startCount){
            
			elapsedTime += Time.deltaTime;
			if (time >= 1) { 
				time = 3 - (int)elapsedTime;
				countText.text = time.ToString ();
			}

			if (elapsedTime >= 3) {
                GoalkeeperManager.Instance().StartGame();
				countText.text = "VAI";
			}
			if (elapsedTime >= 4) {
				Events.RaiseEvent<GameStart>();
				Events.RaiseEvent<UnPauseEvent>();
				countText.gameObject.SetActive(false);
			}
		}
	}
}

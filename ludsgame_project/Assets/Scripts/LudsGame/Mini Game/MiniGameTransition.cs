using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameTransition : MonoBehaviour {

	public Text timerText;
	public float timer = 3;
	public Image img;

	void Update () {
		timer -= Time.deltaTime;
		timerText.text = ((int)timer).ToString();
		img.fillAmount = (timer/3);

		if(timer <= 0) {
			timerText.text = "Go!";
			SceneManager.LoadScene("PigRunner");
		}
	}
}

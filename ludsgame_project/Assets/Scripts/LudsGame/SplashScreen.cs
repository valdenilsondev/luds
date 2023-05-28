using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {
	float elapsedTime;
	//public GameObject partner;
	//public GameObject banner_historia;
	//private Image eureka;
	private float timeChangeScreen = 3f;
	public GameObject[] screens;
	private Color fullAlpha = new Color (1f, 1f, 1f, 1f);
	private Color emptyAlpha = new Color (1f, 1f, 1f, 0f);

	void Awake(){
		/*foreach(GameObject s in screens){
			s.GetComponent<Image>().enabled = false;
		}*/
		foreach(GameObject img in screens)
		{
			img.GetComponent<Image>().color = emptyAlpha;
		}
		screens[0].gameObject.GetComponent<Image>().enabled = true;
		screens[0].gameObject.GetComponent<Image>().color = fullAlpha;
	}

	void Update () {
		elapsedTime += Time.deltaTime;

		if(elapsedTime > timeChangeScreen){
			elapsedTime = 0;
			CallNextScreen();
		}
		else if((nextIndex + 1) < screens.Length){
			screens[nextIndex].gameObject.GetComponent<Image>().color = Color.Lerp(fullAlpha, emptyAlpha, elapsedTime/timeChangeScreen);
			screens[nextIndex + 1].gameObject.GetComponent<Image>().color = Color.Lerp(emptyAlpha, fullAlpha, elapsedTime/timeChangeScreen);
		}
		else if(screens.Length == (nextIndex +1)){
		//	screens[nextIndex].gameObject.GetComponent<Image>().color = Color.Lerp(fullAlpha, emptyAlpha, elapsedTime/timeChangeScreen);
		}
	}

	int nextIndex = 0;
	private void CallNextScreen(){
		/*foreach(GameObject s in screens){
			s.GetComponent<Image>().enabled = false;
		}*/
		nextIndex++;
		//if(nextIndex< screens.Length){
		//	screens[nextIndex].gameObject.GetComponent<Image>().enabled = true;
		
		//}else{
		if(nextIndex > screens.Length){
			//LoadingScreen.instance.LoadScene("Calibration");
			SceneManager.LoadScene("Http");
		}
	}
}

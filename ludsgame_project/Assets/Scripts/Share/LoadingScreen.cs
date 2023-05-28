using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
	public Image background; 
	public Text text;
	public Image progressBar;
	public string levelToLoad;
	private int loadProgress = 0;
	public static LoadingScreen instance;
	// Use this for initialization

    void Awake()
    {
        instance = this;
    }

	void Start () {
		background.gameObject.SetActive (false);
		text.gameObject.SetActive (false);
		progressBar.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			StartCoroutine(DisplayerLoadingScreen(levelToLoad));
		}
	}

	public void LoadScene(string _levelToLoad){
		StartCoroutine(DisplayerLoadingScreen(_levelToLoad));
	}

	IEnumerator DisplayerLoadingScreen(string level){
		background.gameObject.SetActive (true);
		text.gameObject.SetActive (true);
		progressBar.gameObject.SetActive (true);

		progressBar.transform.localScale = new Vector3 (loadProgress, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

		text.text = "Loading Progress " + loadProgress + "%";

		AsyncOperation async = SceneManager.LoadSceneAsync (level);
		async.allowSceneActivation= false;
		while (!async.isDone) {
			//print(async.allowSceneActivation);
			loadProgress = (int)(async.progress * 100);
			if(async.progress > 0.89f){
				async.allowSceneActivation= true;
			}
			text.text = "Loading Progress " + loadProgress + "%";
			progressBar.transform.localScale = new Vector3 (async.progress, progressBar.transform.localScale.y,  progressBar.transform.localScale.z);

			yield return null;
		}
		Destroy (this.gameObject);

	}
}

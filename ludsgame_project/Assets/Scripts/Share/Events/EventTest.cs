using UnityEngine;
using System.Collections;
using Share.EventsSystem;
using UnityEngine.SceneManagement;

public class EventTest : MonoBehaviour {

	void Awake() {
		this.transform.position = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Events.RaiseEvent<PauseEvent>();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			Events.RemoveListener<PauseEvent>(Pause);
		}

		if(Input.GetKeyDown(KeyCode.Backspace)) {
			SceneManager.LoadScene("EventsTest");
		}
	}

	void OnEnable() {
		Events.AddListener<PauseEvent>(Pause);
		Events.AddListener<PauseEvent>(Hey);
	}

	void OnDisable() {
		Events.RemoveAllListeners<PauseEvent>();
	}

	void Pause() {
		Debug.Log ("Pause");
	}

	void Hey() {
		Debug.Log ("Hey");
	}
}

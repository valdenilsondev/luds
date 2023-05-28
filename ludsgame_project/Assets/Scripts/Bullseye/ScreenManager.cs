using UnityEngine;
using System.Collections;
using Share.EventsSystem;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

namespace Bullseye {
	public class ScreenManager : MonoBehaviour {

		public GameObject pauseScreen;
		public GameObject gameOverScreen;
		public GameObject initialMenu;
		public BlurOptimized blur;
		private KinectManager manager;
		private bool isPaused = false;


		void Start(){
			StartGame ();
		}

		void OnEnable () {
			Events.AddListener<PauseEvent>(OnPause);
			Events.AddListener<UnPauseEvent>(OnUnpause);
			Events.AddListener<GameOverEvent>(OnGameOver);
		}
		
		void OnDisable () {
			Events.RemoveListener<PauseEvent>(OnPause);
			Events.RemoveListener<UnPauseEvent>(OnUnpause);
			Events.RemoveListener<GameOverEvent>(OnGameOver);
		}

		private void OnPause() {
			isPaused = true;
			blur.enabled = true;
			pauseScreen.SetActive(true);
		}

		private void OnUnpause() {
			isPaused = false;
			blur.enabled = false;
			pauseScreen.SetActive(false);
		}

		public void UnpauseGame() {
			Events.RaiseEvent<UnPauseEvent>();
		}

	
		public void LoadStartScreen() {
			manager  = KinectManager.Instance;
			SceneManager.LoadScene("startScreenNew");
		}

		public void Restart() {
			SceneManager.LoadScene("Throw");
		}

		public void StartGame() {
//			initialMenu.SetActive(false);
			blur.enabled = false;
			Events.RaiseEvent<GameStart>();
		}

		private void OnGameOver() {
			if(!isPaused) {
				blur.enabled = true;
				gameOverScreen.gameObject.GetComponent<Animator>().SetTrigger("gameOverIn");
			}
		}
	}
}

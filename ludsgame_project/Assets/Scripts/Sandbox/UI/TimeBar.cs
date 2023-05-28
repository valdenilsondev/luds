using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using Sandbox.GameUtils;
using Sandbox.Level;
using Share.KinectUtils.Record;
using UnityEngine.SceneManagement;

namespace Sandbox.UI {
	public class TimeBar : MonoBehaviour {
		
		private SpriteRenderer mySprite;
		private Transform myTransform;
		private bool isPaused = true;
		private bool gameOver = false;
		private float sizeX = 1;
		private float time = 0;
		//[SerializeField]
		private float maxTime = 30;
		private float maxSize = 7.42f;
		private float initialBarTipPosition;
		private Color initialColor;
		private Color targetColor;
		
		private float barTime = 0;
		private float barSize = 1;
		private float barFix = 1;
		
		public static TimeBar instance;
		public Transform barTip;
		private SpriteRenderer barTipSprite;
		
		private SkeletonRecorder skeletonRecorder;
		
		public Text replayText;
		GameObject go_screen;

		void Awake () {
			instance = this;
			
			mySprite = this.GetComponent<SpriteRenderer>();
			myTransform = this.GetComponent<Transform>();
			barTipSprite = barTip.GetComponent<SpriteRenderer>();
			
			time = 1/maxTime;
			barTime = 1/(maxTime/2);
			
			initialBarTipPosition = barTip.position.x;
			initialColor = mySprite.color;
			targetColor = Color.yellow;
			
			//skeletonRecorder = SkeletonRecorder.Instance;
			go_screen = GameObject.Find ("GameOverScreen");
		}
		
		void Update () {
			KeyDown();
			
			if(isPaused || IsGameOver() || !LevelManager.instance.isGameStarted){
				return;
			}
			
			sizeX -= Time.smoothDeltaTime * time;
			myTransform.localScale = new Vector3(sizeX, 1, 1);
			barTip.localPosition = new Vector3(initialBarTipPosition - (maxSize * (1 - sizeX)), barTip.localPosition.y, barTip.localPosition.z);
			
			SetBarColor();
			
			if(sizeX <= 0){
				myTransform.localScale = new Vector3(0, 1, 1);
				isPaused = true;
				GameOver();
				barTip.gameObject.SetActive(false);
			}
		}
		
		public void SetNewTime(float newTime){
			maxTime = newTime;
			time = 1/maxTime;
			barTime = 1/(maxTime/2);
		}
		
		public void DecreaseTime(){
			if(maxTime <= 15){
				return;
			}
			maxTime = maxTime - 5;
			SetNewTime(maxTime);
		}
		
		public void IncreaseTime(){
			if(maxTime >= 60){
				return;
			}
			maxTime = maxTime + 5;
			SetNewTime(maxTime);
		}
		
		public int GetCurrentMaxTime(){
			return (int) maxTime;
		}
		
		private void SetBarColor() {
			barSize -= Time.deltaTime * barTime;
			
			if(barSize <= 0){
				targetColor = Color.red;
				initialColor = Color.yellow;
				barFix = 0f;
			}
			
			mySprite.color = Color.Lerp(initialColor, targetColor, (barFix - barSize));
			barTipSprite.color = mySprite.color;
		}
		
		private void KeyDown() {
			if(Input.GetKeyDown(KeyCode.Return)) {
				StartTimeBar();
			}
			
			if(Input.GetKeyDown(KeyCode.Alpha1)) {
				ResetTimeBar();
			}
			if(Input.GetKeyDown(KeyCode.Alpha2)) {
				IncreaseTime(30);
			}
			if(Input.GetKeyDown(KeyCode.Alpha3)) {
				DecreaseTime(30);
			}
			
			if(Input.GetKeyDown(KeyCode.Backspace)) {
				PauseManager.PauseGame(true);
				MenuManager.BackFromGame();
			}
			
			if(Input.GetKeyDown(KeyCode.R) && gameOver) {
				SceneManager.LoadScene("Skeleton");
			}
		}
		
		public void GameOver() {
			// TODO: GameOver
			print("game over");

			go_screen.GetComponent<Animation> ().Play ("ShowPresentationScreen");
			gameOver = true;
			//replayText.gameObject.SetActive(true);
			MenuManager.isUserPlaying = false;
			MenuManager.instance.SetBoolPlaceOnIntro (7);
			LevelCreator.instance.GameOver();
			DetectorAnimationController.instance.SetHandSprite();
		//	skeletonRecorder.StopRecording(true);
		}
		
		public void StartTimeBar() {
			ResetTimeBar();
			PauseTimeBar(false);
		}
		
		public void ResetTimeBar() {
			barTip.gameObject.SetActive(true);
			sizeX = 1;
			barSize = 1;
			barFix = 1;
			
			barTip.localPosition = new Vector3(initialBarTipPosition, barTip.localPosition.y, barTip.localPosition.z);
			myTransform.localScale = new Vector3(1, 1, 1);
			
			mySprite.color = Color.green;
			barTipSprite.color = Color.green;
			initialColor = mySprite.color;
			targetColor = Color.yellow;
			
			gameOver = false;
		}
		
		public void PauseTimeBar(bool pause) {
			isPaused = pause;
		}
		
		public void IncreaseTime(float percent) {
			float newScale = (percent / 100) + myTransform.localScale.x;
			
			if(newScale > 1)
				newScale = 1;
			
			if(sizeX + (percent/100) > 1)
				sizeX = 1;
			else
				sizeX += (percent/100);
			
			myTransform.localScale = new Vector2(newScale, 1);
		}
		
		public void DecreaseTime(float percent) {
			float newScale = myTransform.localScale.x - (percent / 100);
			sizeX -= (percent/100);
			
			if(newScale < 0)
				newScale = 0;
			
			if(sizeX + (percent/100) < 0)
				sizeX = 0;
			else
				sizeX -= (percent/100);
			
			myTransform.localScale = new Vector2(newScale, 1);
		}
		
		public bool IsGameOver() {
			return gameOver;
		}
	}
}

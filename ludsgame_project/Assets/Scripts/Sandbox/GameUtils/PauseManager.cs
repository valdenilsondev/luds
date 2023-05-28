using UnityEngine;
using System.Collections;

using Sandbox.UI;
using Share.EventSystem;

namespace Sandbox.GameUtils {
	public class PauseManager: MonoBehaviour {
		
		public static bool isPaused = false;
		public static bool pauseOnCollide;
		
		public static PauseManager instance;

		private SpeechManager speechManager;
		private Events events;
		
		void Awake(){
			instance = this;
			events = Events.instance;
		//	speechManager = GameObject.Find("Kinect Extras").GetComponent<SpeechManager>();
		}

		void Update(){
			//PauseRecognize ();
		}

		private void PauseRecognize(){
			if(speechManager != null && speechManager.IsSapiInitialized())
			{
				
				if(speechManager.IsPhraseRecognized())
				{
					
					string sPhraseTag = speechManager.GetPhraseTagRecognized();
					
					switch(sPhraseTag)
					{
					case "PAUSE":					
						PauseGame(true);
						break;
					case "POUNZEE":
						PauseGame(true); 
						break;
					}
					speechManager.ClearPhraseRecognized();
				}
			}
		}

		void OnEnable() {
			events.AddListener<SpeechEvents>(PauseEvent);
		}
		
		void OnDisable() {
			events.RemoveListener<SpeechEvents>(PauseEvent);
		}
		
		public void PauseEvent(SpeechEvents se) {
			PauseGame(true); 
		}
		
		public static void PauseGame(bool pause) {		
			isPaused = pause;
			TimeBar.instance.PauseTimeBar(pause);
			if(pause){
				pauseOnCollide = true;
				DetectorAnimationController.instance.SetHandSprite();
			}
		}
	}
}
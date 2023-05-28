using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using Sandbox.GameUtils;
using Sandbox.Level;
using Share.KinectUtils;


namespace Sandbox.KinectControl {
	public class DistanceCalibrator : MonoBehaviour {
		
		public static DistanceCalibrator instance;
		
		public float minDistance = 1.5f;
		public float warningDistance = 1f;
		
		public Sprite aguardando;
		public Sprite naoDefinida;
		public Sprite ideal;
		
		public Text distanceText;
		
		private SpriteRenderer mySpriteRenderer;
		private bool isDistanceOk = false;
		private float distance;
		private KinectManager kinect;
		private Vector4 initialVector = new Vector4(-1, -1, -1, -1);
		
		void Awake () {
			instance = this;
			
			mySpriteRenderer = this.GetComponent<SpriteRenderer>();
			kinect = KinectManager.Instance;
		}
		
		void Update() {
			uint playerId = kinect.GetPlayer1ID ();
			if(kinect.IsPlayerCalibrated(playerId)) {
				Vector4 vec = kinect.GetUserPosition (playerId);
					if(vec != initialVector)
						CheckDistance(vec.z);
				
			}
		}
		
		private void CheckDistance(float distance) {
			if(distance == 0) {
				return;
			}
			
			// Ideal
			if(distance >= minDistance) {
				mySpriteRenderer.sprite = ideal;
				isDistanceOk = true;

				return;
			} else {
				mySpriteRenderer.sprite = naoDefinida;
				isDistanceOk = false;
			}
			
			// Aviso
			if(distance >= warningDistance && distance < minDistance){
				distanceText.enabled = true;
				
				return;
			} else{
				distanceText.enabled = false;
			}
			
			// Pausa o jogo
			if(distance < warningDistance && LevelManager.instance.isGameStarted) {
				PauseManager.PauseGame(true);
				return;
			}
		}
		
		public bool IsDistanceOk() {
			return isDistanceOk;
		}
	}
}
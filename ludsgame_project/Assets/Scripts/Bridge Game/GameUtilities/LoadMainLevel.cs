using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Share.KinectUtils {
	public class LoadMainLevel : MonoBehaviour {
		
		private bool levelLoaded = false;
		KinectManager mng;

		void Awake() {
			mng = KinectManager.Instance;
		}

		void Update() {						
			if(!levelLoaded && mng && KinectManager.IsKinectInitialized()) {
				levelLoaded = true;
				SceneManager.LoadScene("LevelSelector");
			}
			
		}
		
	}
}
using UnityEngine;
using System.Collections;

using System.Collections.Generic;

namespace Bullseye {
	public class ApplesPool : MonoBehaviour {

		public Rigidbody applePrefab;
		public int poolCount = 5;
		private static List<Rigidbody> applesPool;
		private static Transform applesContainer;

		void Awake () {
			InitPool();
		}

		private void InitPool() {
			applesPool = new List<Rigidbody>();
			applesContainer = GameObject.Find ("Apple_Hand").transform;
			for (int i = 0; i < poolCount; i++) {
				applesPool.Insert(applesPool.Count, Instantiate<Rigidbody>(applePrefab));
				applesPool[i].transform.SetParent(applesContainer);
				applesPool[i].gameObject.SetActive(false);
			}
		}
		
		private static int GetAvaiableAppleIndex() {
			for (int i = 0; i < applesPool.Count; i++) {
				if(!applesPool[i].gameObject.activeSelf) {
					applesPool[i].transform.SetParent(applesContainer);
					return i;
				}
			}

			Rigidbody newApple = Instantiate<Rigidbody>(applesPool[0]);
			newApple.transform.SetParent(applesContainer);
			newApple.gameObject.SetActive(false);
			applesPool.Insert(applesPool.Count, newApple);

			return applesPool.Count - 1;
		}

		public static Rigidbody Instantiate() {
			return applesPool[GetAvaiableAppleIndex()];
		}

		public static void Destroy(GameObject apple) {
			apple.SetActive(false);
			apple.transform.SetParent(applesContainer);
			apple.GetComponent<Rigidbody>().velocity = Vector3.zero;
			apple.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}

}
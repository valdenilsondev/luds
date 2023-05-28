using UnityEngine;
using System.Collections;

namespace Bullseye {
	public class Target : MonoBehaviour {

		public void TargetUp() {
			ThrowManager.Instance.TargetUp();
		}

		public void TargetDown() {
			ThrowManager.Instance.TargetDown();
		}
	}

}
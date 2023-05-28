using UnityEngine;
using System.Collections;

public class InitialAnimFix : MonoBehaviour {
	private void DeactivateCamAnimator(){
		CameraMovementControlFish.instance.DeactivateCamAnimator();
	}
}

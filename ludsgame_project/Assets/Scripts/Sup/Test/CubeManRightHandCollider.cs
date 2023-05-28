using UnityEngine;
using System.Collections;

public class CubeManRightHandCollider : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		if(CubeManRowManager.handsClose){
			if(col.gameObject == CubeManRowManager.instance.GetTopPlane())
			{
				CubeManRowManager.touchedTop = true;
				CubeManRowManager.touchedBottom = false;
			}
			if(col.gameObject == CubeManRowManager.instance.GetBottomPlane() && CubeManRowManager.touchedTop)
			{
				CubeManRowManager.touchedBottom = true;
				CubeManRowManager.touchedTop = false;
			}
		}
	}
}

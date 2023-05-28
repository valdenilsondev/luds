using UnityEngine;
using System.Collections;

public class RotateObjects : MonoBehaviour {

	void Update () {
		this.transform.Rotate(new Vector3(0,1,0), 2);
	}
}

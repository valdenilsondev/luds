using UnityEngine;
using System.Collections;

public class MovPositionController : MonoBehaviour {
	public static MovPositionController instance;
	private KinectManager kinect;
	private int jointIndex;
	private bool startPos;
	private float elapsedTime;

	// Use this for initialization
	void Awake () {
		instance = this;
		kinect = KinectManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;

		if (startPos && elapsedTime > 0.5f) {
			Vector3 position = kinect.GetJointPosition (kinect.GetPlayer1ID (),jointIndex);
			KinectGestures.SetFinalPos(position, jointIndex);
			//print("Delta X " + Mathf.Abs(KinectGestures.GetInitialJointPosition(jointIndex).x - KinectGestures.GetFinalJointPosition(jointIndex).x));
			startPos = false;
		}
	}

	public void StartFinalPosition(int j){
		jointIndex = j;
		startPos = true;
		elapsedTime = 0;
	}
}

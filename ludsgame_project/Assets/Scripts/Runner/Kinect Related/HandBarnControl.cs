using UnityEngine;
using System.Collections;

public class HandBarnControl : MonoBehaviour {

	public GameObject leftHand, rightHand;
	private KinectManager kinect;

	// Use this for initialization
	void Start () {
		kinect = KinectManager.Instance;
		if (PlayerHandController.GetHand () == PlayerHandController.UsedHand.Left) {
			rightHand.GetComponent<SpriteRenderer> ().enabled = false;
			rightHand.GetComponent<Collider> ().enabled = false;

		} else {
			leftHand.GetComponent<SpriteRenderer> ().enabled = false;
			leftHand.GetComponent<Collider> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float leftHandX = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft).x;
		float leftHandY = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft).y;
		float righttHandX = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x;
		float rightHandY = kinect.GetJointPosition (kinect.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y;

		Vector3 leftHandTarget = new Vector3 (leftHandX, leftHandY, 0);
		Vector3 rightHandTarget = new Vector3 (righttHandX*2, rightHandY, 0);

		leftHand.transform.position = Vector3.Lerp (leftHand.transform.position, leftHandTarget, Time.deltaTime * 25);
		rightHand.transform.position = Vector3.Lerp (rightHand.transform.position, rightHandTarget, Time.deltaTime * 25);
	}
}

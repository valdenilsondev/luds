using UnityEngine;
using System.Collections;
using Share.Managers;

public class Aim_controller : MonoBehaviour {
	public static Aim_controller instance;
	public float targetPosix;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	/*	this.transform.position = new Vector3 (KinectManager.Instance.GetJointPosition (KinectManager.Instance.GetPlayer1ID (), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x,
		                                       this.transform.position.y, this.transform.position.z);*/
		if(GameManagerShare.instance.IsUsingKinect()){
			this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (KinectManager.Instance.GetJointPosition (KinectManager.Instance.GetPlayer1ID (), 
			                                                                                                                       (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).x*15, //NuiSkeletonPositionIndex.HandRight
			                                                                              this.transform.position.y, this.transform.position.z), 3);
			targetPosix = this.transform.position.x;

		}
	}
}

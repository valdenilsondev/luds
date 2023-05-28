#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using Share.EventsSystem;
using Share.Kinect.Record;
public class SkeletonReplay : MonoBehaviour {

	private SkeletonRecorder skeletonRecorder;
	private bool isReplaying = false;
	private SkeletonFrame currentFrame;
	public Transform[] skeleton;
	
	void Awake () {
		skeletonRecorder = SkeletonRecorder.Instance;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			Events.RaiseEvent<PlayRecordingEvent>();
			isReplaying = true;
		}
	}

	void FixedUpdate () {
		if (isReplaying) {
			currentFrame = skeletonRecorder.GetNextSkeletonFrame();

			if(currentFrame == null) {
				Events.RaiseEvent<PauseRecordingEvent>();
				isReplaying = false;
				return;
			}

			for (int i = 0; i < skeleton.Length; i++) {
				skeleton[i].localPosition = currentFrame.SkeletonPositions[i];
			}
		}
	}

	void OnEnable() {
		Events.AddListener<PlayRecordingEvent>(OnPlaying);
	}

	void OnDisable() {
		Events.RemoveListener<PlayRecordingEvent>(OnPlaying);
	}

	void OnPlaying() {
		isReplaying = true;
	}
}
#endif
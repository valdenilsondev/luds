#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Share.KinectUtils;
using Share.KinectUtils.Record;
using Sandbox.GameUtils;
using UnityEngine.UI;
using System;
using KinectInte;

[RequireComponent(typeof(Animation))]
public class AnimationCreation : MonoBehaviourSingleton<AnimationCreation> {

	private AnimationClip clip;
	private string currentAnimation;
	private List<SkeletonFrameEureka> newAnimation;

	private Keyframe[] xPositions;
	private Keyframe[] yPositions;
	private Keyframe[] zPositions;

	private Keyframe[] xRotations;
	private Keyframe[] yRotations;
	private Keyframe[] zRotations;
	private Keyframe[] wRotations;

	private float deltaTimeAnimation = 0;

	public GameObject[] skeleton;

	void Awake() {
		clip = new AnimationClip();
		clip.name = "CharacterAnimation";
		clip.frameRate = 30f;

		CalculateDeltaTimeAnimation();
	}

	void CreateAnimation () {
		Keyframe tempKeyFrame = new Keyframe(0, 0);
		string path;

		for(int jointIndex = 0; jointIndex < skeleton.Length; jointIndex++) {
			if(skeleton[jointIndex] == null) {
				continue;
			}
			xPositions = new Keyframe[newAnimation.Count];
			yPositions = new Keyframe[newAnimation.Count];
			zPositions = new Keyframe[newAnimation.Count];

			xRotations = new Keyframe[newAnimation.Count];
			yRotations = new Keyframe[newAnimation.Count];
			zRotations = new Keyframe[newAnimation.Count];
			wRotations = new Keyframe[newAnimation.Count];

			for(int frameIndex = 0; frameIndex < newAnimation.Count; frameIndex++) {
				tempKeyFrame.time = frameIndex * deltaTimeAnimation;
				tempKeyFrame.value = newAnimation[frameIndex].SkeletonPositions[jointIndex].x;
				xPositions[frameIndex] = tempKeyFrame;
				
				tempKeyFrame.value = newAnimation[frameIndex].SkeletonPositions[jointIndex].y;
				yPositions[frameIndex] = tempKeyFrame;
				
				tempKeyFrame.value = newAnimation[frameIndex].SkeletonPositions[jointIndex].z;
				zPositions[frameIndex] = tempKeyFrame;

			}

			path = "Animation_Robot/Robot" + GetJointPath(jointIndex);

			clip.SetCurve(path,typeof(Transform), "localPosition.x", new AnimationCurve(xPositions));
			clip.SetCurve(path,typeof(Transform), "localPosition.y", new AnimationCurve(yPositions));
			clip.SetCurve(path,typeof(Transform), "localPosition.z", new AnimationCurve(zPositions));

			for(int frameIndex = 0; frameIndex < newAnimation.Count; frameIndex++) {
				Quaternion tempQuaternion = newAnimation[frameIndex].SkeletonRotations[jointIndex];
				tempKeyFrame.time = frameIndex * deltaTimeAnimation;
				tempKeyFrame.value = tempQuaternion.x;
				xRotations[frameIndex] = tempKeyFrame;
				
				tempKeyFrame.value = tempQuaternion.y;
				yRotations[frameIndex] = tempKeyFrame;
				
				tempKeyFrame.value = tempQuaternion.z;
				zRotations[frameIndex] = tempKeyFrame;

				tempKeyFrame.value = tempQuaternion.w;
				wRotations[frameIndex] = tempKeyFrame;

			}
			
			clip.SetCurve(path,typeof(Transform), "localRotation.x", new AnimationCurve(xRotations));
			clip.SetCurve(path,typeof(Transform), "localRotation.y", new AnimationCurve(yRotations));
			clip.SetCurve(path,typeof(Transform), "localRotation.z", new AnimationCurve(zRotations));
			clip.SetCurve(path,typeof(Transform), "localRotation.w", new AnimationCurve(wRotations));
		}

		GetComponent<Animation>().AddClip(clip, clip.name);
	}

	private string GetJointPath(int index) {
		GameObject tempGO = skeleton[index];
		string path = "/" + tempGO.name;
		while(tempGO.transform.parent.name != "Robot") {
			tempGO = tempGO.transform.parent.gameObject;
			path = "/" + tempGO.name + path;
		}
		return path;
	}

	void CalculateDeltaTimeAnimation() {
		deltaTimeAnimation = (1f) / (clip.frameRate);
	}

	public bool SaveAnimation(string animationName) {
		if(newAnimation.Count > 0) {
			try{
				AssetDatabase.CreateAsset(clip, "Assets/Resources/Share/Animations/" + animationName + ".anim");
				AssetDatabase.SaveAssets();
			} catch(Exception ex) {
				Debug.Log(ex.Message);
				return false;
			}
			
			return true;
		}
		
		return false;
	}

	public void SetAnimation(List<SkeletonFrameEureka> list) {
		newAnimation = list;
		CreateAnimation();
	}

	public void SetAnimation(List<SkeletonFrameEureka> list, GameObject[] skeleton) {
		newAnimation = list;
		this.skeleton = skeleton;
		CreateAnimation();
	}

	public void PlayAnimation(string animationName) {
		currentAnimation = animationName;
		GetComponent<Animation>().Play(currentAnimation);
		AddStopEvent();
	}

	public void StopAnimation() {
		GetComponent<Animation>().Stop(currentAnimation);
	}

	public void PauseAnimation() {
		GetComponent<Animation>().enabled = false;
	}

	public void ContinueAnimation() {
		GetComponent<Animation>().enabled = true;
	}

	private void AddStopEvent() {
		AnimationClip tempClip = GetComponent<Animation>().GetClip(currentAnimation);

		AnimationEvent animEvent = new AnimationEvent();
		animEvent.functionName = "StopAnimation";
		animEvent.time = tempClip.length;
		tempClip.AddEvent(animEvent);
	}
}
#endif

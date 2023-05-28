#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.KinectUtils.Animations;
using Sandbox.GameUtils;

public enum CreateAnimationState {
	None,
	Recording,
	Playing,
	Paused
}

public class AnimationInterfaceController : MonoBehaviourSingleton<AnimationInterfaceController> {

	private CreateAnimationState currentState = CreateAnimationState.None;

	private KinectModelControllerV2 recorderMovements;
	private AnimationCreation animationCreation;
	private AnimationWindow animationWindow;

	private bool animationSaved = false;
	private AnimationClip[] allAnimations;

	public Text readyText;
	public Text recordingWarningText;
	public Text animationSavedText;
	public InputField animationName;

	void Awake() {
		readyText.gameObject.SetActive(true);

		animationCreation = AnimationCreation.Instance;
		recorderMovements = KinectModelControllerV2.Instance;
		animationWindow = AnimationWindow.Instance;

		LoadAnimations();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.RightControl)) {
			if(currentState == CreateAnimationState.Recording)
				StopRecording();
			else if(currentState == CreateAnimationState.None)
				StartRecording();
		}
	}

	private void LoadAnimations() {
		allAnimations = Resources.LoadAll<AnimationClip>("Share/Animations");
		animationWindow.CreateToggleGroup();

		SetAllAnimations();
	}

	private void SetAllAnimations() {
		for(int i = 0; i < allAnimations.Length; i++) {
			animationCreation.GetComponent<Animation>().AddClip(allAnimations[i], allAnimations[i].name);
		}
	}

	public int GetAllAnimationsQnty() {
		return allAnimations.Length;
	}

	public string GetAnimationName(int index) {
		return allAnimations[index].name;
	}

	public void StartRecording() {
		if(currentState == CreateAnimationState.None) {
			if(!recorderMovements.StartRecording()) {
				Debug.LogWarning("Animaçoes nao salvas foram deletadas");
			}

			currentState = CreateAnimationState.Recording;
			recordingWarningText.text = "Recording";
			recordingWarningText.gameObject.SetActive(true);
		}
	}

	public void StopRecording() {
		if(currentState == CreateAnimationState.Recording) {
			currentState = CreateAnimationState.None;
			recorderMovements.StopRecording();
			recordingWarningText.gameObject.SetActive(false);
		}
	}

	public void SaveRecording() {
		if(currentState == CreateAnimationState.None) {
			recorderMovements.SaveRecording();
			animationSaved = animationCreation.SaveAnimation(animationName.text);
			SaveAnimationDialogResult();
		}
	}

	public void PlayAnimation() {
		//if(currentState == CreateAnimationState.None) {
			recordingWarningText.gameObject.SetActive(true);
			recordingWarningText.text = "Playing";
			currentState = CreateAnimationState.Playing;
			animationCreation.PlayAnimation(animationWindow.GetSelectedAnimationName());
		//}
	}

	public void PauseAnimation() {
		//if(currentState == CreateAnimationState.Playing) {
			recordingWarningText.gameObject.SetActive(true);
			recordingWarningText.text = "Paused";
			currentState = CreateAnimationState.Paused;
			animationCreation.PauseAnimation();
		//}
	}

	public void ContinueAnimation() {
		//if(currentState == CreateAnimationState.Paused) {
			recordingWarningText.gameObject.SetActive(true);
			recordingWarningText.text = "Playing";
			currentState = CreateAnimationState.Playing;
			animationCreation.ContinueAnimation();
		//}
	}
	
	public void StopAnimation() {
		//if(currentState == CreateAnimationState.Playing) {
			recordingWarningText.gameObject.SetActive(false);
			currentState = CreateAnimationState.None;
			animationCreation.StopAnimation();
		//}
	}

	private void SaveAnimationDialogResult() {
		if(animationSaved) {
			animationSavedText.text = "Recording Saved!";
			LoadAnimations();
		} else {
			animationSavedText.text = "Error!";
		}
	}
}
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Sandbox.GameUtils;
using UnityEngine.UI;

public class AnimationWindow : MonoBehaviourSingleton<AnimationWindow> {
	#if UNITY_EDITOR 

	private AnimationInterfaceController animationInterfaceController;
	private List<Toggle> toggles = new List<Toggle>();

	public GameObject animationTogglePrefab;
	public ToggleGroup animationToggleGroup;

	Toggle animationToggle;
	string animationName;

	void Awake() {
		animationInterfaceController = AnimationInterfaceController.Instance;
	}

	public void CreateToggleGroup() {
		DestroyAllToggles();
		for(int i = 0; i < animationInterfaceController.GetAllAnimationsQnty(); i++) {
			CreateToggle(i);
		}
	}

	private void DestroyAllToggles() {
		for(int i = 0; i < toggles.Count; i++) {
			animationToggleGroup.UnregisterToggle(toggles[i]);
			GameObject.DestroyImmediate(toggles[i].gameObject);
		}
		toggles = new List<Toggle>();
	}

	private void CreateToggle(int i) {
		animationName = animationInterfaceController.GetAnimationName(i);
		
		animationToggle = (Instantiate(animationTogglePrefab) as GameObject).GetComponent<Toggle>();
		animationToggle.group = animationToggleGroup;
		animationToggleGroup.RegisterToggle(animationToggle);
		animationToggle.transform.parent = animationToggleGroup.transform;
		animationToggle.GetComponentInChildren<Text>().text = animationName;

		toggles.Add(animationToggle);
	}

	public string GetSelectedAnimationName() {
		for(int i = 0; i < toggles.Count; i++) {
			if(toggles[i].isOn) {
				return toggles[i].GetComponentInChildren<Text>().text;
			}
		}

		return toggles[0].GetComponentInChildren<Text>().text;
	}

	#endif
}


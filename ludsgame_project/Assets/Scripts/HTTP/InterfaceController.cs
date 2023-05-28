using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterfaceController : MonoBehaviour {

	private EventSystem currentSystem;
	private bool shiftDown = false;

	void Awake () {
		currentSystem = EventSystem.current;
	}

	void Update () {
		if (!shiftDown && Input.GetKeyDown(KeyCode.Tab)) {
			Selectable next = currentSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			
			if (next!= null) {
				
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null) {
					inputfield.OnPointerClick(new PointerEventData(currentSystem)); //if it's an input field, also set the text caret
				}
				
				currentSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(currentSystem));
			}
			//else Debug.Log("next nagivation element not found");
		}

		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			shiftDown = true;
		}

		if(Input.GetKeyUp(KeyCode.LeftShift)) {
			shiftDown = false;
		}

		if (shiftDown && Input.GetKeyDown(KeyCode.Tab)) {
			Selectable previous = currentSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
			
			if (previous!= null) {
				
				InputField inputfield = previous.GetComponent<InputField>();
				if (inputfield != null) {
					inputfield.OnPointerClick(new PointerEventData(currentSystem)); //if it's an input field, also set the text caret
				}
				
				currentSystem.SetSelectedGameObject(previous.gameObject, new BaseEventData(currentSystem));
			}
			//else Debug.Log("next nagivation element not found");
		}

		if(Input.GetKeyDown(KeyCode.Return)) {
			InputField inputField = currentSystem.currentSelectedGameObject.GetComponent<InputField>();

			if(inputField != null) {
				Selectable next = currentSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

				if(next != null) {
					Button button = next.GetComponent<Button>();
					if(button != null) {
						button.OnPointerClick(new PointerEventData(currentSystem));
					}
				}
			}
		}
	}
}

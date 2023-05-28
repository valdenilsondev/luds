using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace Sandbox.UI {
	public class ScrollManager : MonoBehaviour, IScrollHandler {
		
		public Scrollbar scroll;
		
		public void OnScroll(PointerEventData eventData) {
			scroll.value += eventData.scrollDelta.y;
		}
	}
}

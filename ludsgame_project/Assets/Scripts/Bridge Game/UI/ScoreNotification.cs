using UnityEngine;
using System.Collections;

namespace BridgeGame.UI {
	public class ScoreNotification : MonoBehaviour {
		public static ScoreNotification instance;
		public GameObject[] bonusGui;

		private string enterAnim = "Bonus_In";
		private string exitAnim = "Bonus_Out";
		
		public void ScreenBonusAnimation(int bonus){
			switch (bonus) {
			case 1:
				if(bonusGui[0].GetComponent<RectTransform>().anchoredPosition.y >= 50)
					bonusGui[0].GetComponent<Animation>().Play(enterAnim);

				if(bonusGui[1].GetComponent<RectTransform>().anchoredPosition.y != 50)
					bonusGui[1].GetComponent<Animation>().Play(exitAnim);

				if(bonusGui[2].GetComponent<RectTransform>().anchoredPosition.y != 50)
					bonusGui[2].GetComponent<Animation>().Play(exitAnim);
				break;
			case 2:
				bonusGui[1].GetComponent<Animation>().Play(enterAnim);
				bonusGui[0].GetComponent<Animation>().Play(exitAnim);
				break;
			case 3:
				bonusGui[2].GetComponent<Animation>().Play(enterAnim);
				bonusGui[1].GetComponent<Animation>().Play(exitAnim);
				break;
			default:
				print("nao chamou a animacao de bonus");
				break;
			}
		}
		
		void Awake (){
			instance = this;

			print (this.gameObject.name);
		}
		
	}
}

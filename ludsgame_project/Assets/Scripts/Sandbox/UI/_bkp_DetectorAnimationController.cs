using UnityEngine;
using System.Collections;

using Sandbox.GameUtils;
using Sandbox.Level;

namespace Sandbox.UI {
	public class _bkp_DetectorAnimationController : MonoBehaviour {
		
		private Transform hand = null;
		private Animator handAnimator = null;
		private RaycastHit hit;
		
		public Sprite detector;
		
		public static _bkp_DetectorAnimationController instance;
		
		void Awake(){
			instance = this;
		}
		
		void Start() {
			//handAnimator = this.hand.GetComponent<Animator>();
		}
		
		void Update() {
			KeyDown();
			
			if(hand != null && Physics.Raycast(hand.position, Vector3.forward, out hit)){
				int index = LevelManager.instance.GetPieceIndex(hit.transform);
				
				if(index >= 0){
					if(MatrixManager.instance.GetSceneObjectByIndex(index) != SceneObjects.None){
						if(!handAnimator.enabled)
							handAnimator.enabled = true;
					}else{
						if(handAnimator.enabled){
							handAnimator.enabled = false;
							SetDetectorSprite();
						}
					}
				}
			}
		}
		
		void KeyDown() {
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				handAnimator.enabled = true;
			}
			
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				handAnimator.enabled = false;
			}
		}
		
		public void SetDetectorSprite(){
			hand.GetComponent<SpriteRenderer>().sprite = detector;
		}
		
		public void SetHandDetector(Transform hand){
			
			this.hand = hand;
			handAnimator = this.hand.GetComponent<Animator>();
		}
		
	}
}
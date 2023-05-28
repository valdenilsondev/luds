using UnityEngine;
using System.Collections;

using Sandbox.GameUtils;
using Sandbox.Level;

namespace Sandbox.UI {
	public class DetectorAnimationController : MonoBehaviour {
		
		private Transform handTracked = null;
		private Animator handAnimator = null;
		private SpriteRenderer handSprite = null;
		
		private RaycastHit hit;
		
		public Sprite detector;
		public Sprite hand;
		
		public static DetectorAnimationController instance;
		
		private void Awake(){
			instance = this;
		}
		
		private void Update() {
			if(!PauseManager.isPaused){
				CheckItem();
			}
		}
		
		private void CheckItem() {
			if(handTracked != null && Physics.Raycast(handTracked.position, Vector3.forward, out hit)){
				int index = LevelManager.instance.GetPieceIndex(hit.transform);
				
				if(index >= 0){
					if(MatrixManager.instance.GetSceneObjectByIndex(index) != SceneObjects.None){
						PlayAnimation();
					}else{
						StopAnimation();
					}
				}
			}
		}
		
		private void PlayAnimation() {
			if(!handAnimator.enabled)
				handAnimator.enabled = true;
		}
		
		private void StopAnimation() {
			if(handAnimator != null && handAnimator.enabled){
				handAnimator.enabled = false;
				SetDetectorSprite();
			}
		}
		
		public void SetHandSprite() {
			if(handSprite != null){
				StopAnimation();
				handSprite.sprite = hand;
			}
		}
		
		public void SetDetectorSprite(){
			StopAnimation();
			handSprite.sprite = detector;
			//print("set detector sprite");
		}
		
		public void SetHandDetector(Transform hand){
			this.handTracked = hand;
			handAnimator = this.handTracked.GetComponent<Animator>();
			handSprite = this.handTracked.GetComponent<SpriteRenderer>();
		}
		
		public bool isHandWithDetector(){
			if(handSprite.sprite == detector){
				return true;
			}
			return false;
		}
		
	}
}
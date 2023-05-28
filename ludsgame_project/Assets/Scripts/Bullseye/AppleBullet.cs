using UnityEngine;
using System.Collections;
using Bullseye;
using Share.EventsSystem;
using Share.Managers;
using Assets.Scripts.Share.Enums;

public class AppleBullet : MonoBehaviour {
	
	public GameObject puffParticle;
	public Animation anim;
	
	float elapsedTime;
	public static AppleBullet instace;
	void Awake(){
		instace = this;
		anim = this.gameObject.GetComponent<Animation> ();
	}
	
	void Update(){
		/*	if(!Player.instance.handFull)
			elapsedTime += Time.deltaTime;

		if (elapsedTime > 2) {
			ApplesPool.Destroy(this.gameObject);
			Player.instance.PrepareApple ();
		}
		*/
		
	}
	
	public void PlayerAnimationBullet(float finalPosX){
		if (finalPosX > 1.5f) {
			anim.Play("ThrowRight");
		}
		if (finalPosX > -1.5f && finalPosX < 1.5f) {
			anim.Play("ThrowCenter");
		}
		if (finalPosX < -1.5f) {
			anim.Play("ThrowLeft");
		}
	}
	
	void OnCollisionEnter(Collision col) {
		if(col.transform.CompareTag("Floor")) 
		{
			ApplesPool.Destroy(this.gameObject);
			PlayerThrow.instance.PrepareApple ();
		}
		ApplesPool.Destroy(this.gameObject);
		ThrowManager.Instance.TargetHit(col.gameObject);
		//current target eh o target que esta levantado
		if(col.transform.parent.name == "Target Right")
		{
			ThrowSoundManager.Instance.PlayCrashPlaqueSfx();
			if(ThrowManager.Instance.currentTarget == Target.Right){
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Hits,1,0);
				ThrowManager.Instance.userFeedback.text = "ACERTOU!";
				Invoke ("HideFeedback", 1);
				PlayerThrow.instance.PrepareApple ();
			}else{
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Miss,1,0);
				ThrowManager.Instance.userFeedback.text = "ERROU!";
				Invoke ("HideFeedback", 1);
			}
		}
		else if(col.transform.parent.name == "Target Center")
		{
			ThrowSoundManager.Instance.PlayCrashPlaqueSfx();
			if(ThrowManager.Instance.currentTarget == Target.Center){
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Hits,1,0);
				ThrowManager.Instance.userFeedback.text = "ACERTOU!";
				Invoke ("HideFeedback", 1);
				PlayerThrow.instance.PrepareApple ();
			}else{
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Miss,1,0);
				ThrowManager.Instance.userFeedback.text = "ERROU!";
				Invoke ("HideFeedback", 1);
			}
		}
		else if(col.transform.parent.name == "Target Left")
		{
			ThrowSoundManager.Instance.PlayCrashPlaqueSfx();
			if(ThrowManager.Instance.currentTarget == Target.Left){
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Hits,1,0);
				ThrowManager.Instance.userFeedback.text = "ACERTOU!";
				Invoke ("HideFeedback", 1);
				PlayerThrow.instance.PrepareApple ();
			}else{
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Miss,1,0);
				ThrowManager.Instance.userFeedback.text = "ERROU!";
				Invoke ("HideFeedback", 1);
			}
		}
		ThrowManager.Instance.totalapple = ThrowManager.Instance.miss + ThrowManager.Instance.GetHits();
	}
	
	private void HideFeedback()
	{
		ThrowManager.Instance.userFeedback.text = string.Empty;
	}
	
}
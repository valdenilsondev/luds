using UnityEngine;
using System.Collections;
using Share.Managers;
using BridgeGame.Player;
using Bullseye;
using Assets.Scripts.Share.Enums;
using Runner.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using Share.Controllers;
using Goalkeeper.Managers;

public class CallStars : MonoBehaviour {
	private List<ScoreItem> scoreItemList;

	// Use this for initialization
	void Start () {
		scoreItemList = GameManagerShare.instance.GetScoreItems();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CallStarsToBlink(){

		if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Pig) {
			GameManagerShare.instance.BlinkStar (3);
		/*	if (ScoreManager_GK.instance.playerWon) {
				if (ScoreManager_GK.instance.Goals == 0) {
					GameManagerShare.instance.BlinkStar (3);
				}
				if (ScoreManager_GK.instance.Goals == 1) {
					GameManagerShare.instance.BlinkStar (2);
				}
				if (ScoreManager_GK.instance.Goals == 2) {
					GameManagerShare.instance.BlinkStar (1);
				}
			}
			if (ScoreManager_GK.instance.playerLost) {
				GameManagerShare.instance.BlinkStar (0);
			}*/
		}
		if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Goal_Keeper) {
			if (GoalkeeperManager.Instance().VerifyPlayerWin()) {
				var goals = scoreItemList.Where(x => x.type == ScoreItemsType.Goal).FirstOrDefault(); 
				if (goals.GetValue() == 0) {
					GameManagerShare.instance.BlinkStar (3);
				}
				if (goals.GetValue() == 1) {
					GameManagerShare.instance.BlinkStar (2);
				}
				if (goals.GetValue() == 2) {
					GameManagerShare.instance.BlinkStar (1);
				}
			}
			if (GoalkeeperManager.Instance().VerifyPlayerLose()) {
				GameManagerShare.instance.BlinkStar (0);
			}
		}

		if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Bridge) {
			if(LifesManager.instance.GetCurrentNumberOfHearts() == 3){
				GameManagerShare.instance.BlinkStar (3);
			}
			if(LifesManager.instance.GetCurrentNumberOfHearts() == 2){
				GameManagerShare.instance.BlinkStar (2);
			}
			if(LifesManager.instance.GetCurrentNumberOfHearts() == 1){
				GameManagerShare.instance.BlinkStar (1);
			}
			if(LifesManager.instance.GetCurrentNumberOfHearts() == 0){
				GameManagerShare.instance.BlinkStar (0);
			}

		}

		if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Sup) {

			if(SupManager.instance.TotalTimeScore >= 10 && SupManager.instance.TotalAppleScore >= 30){
				print ("blink 3");
				GameManagerShare.instance.BlinkStar (3);
			}
			else if(SupManager.instance.TotalAppleScore >= 30){
				print ("blink 2");
				GameManagerShare.instance.BlinkStar (2);
			}
			else if(SupManager.instance.TotalAppleScore >= 10 && SupManager.instance.TotalAppleScore < 30){
				print ("blink 1");
				GameManagerShare.instance.BlinkStar (1);
			}
			else if(SupManager.instance.TotalAppleScore == 0)

			{
				print ("blink 0");
				GameManagerShare.instance.BlinkStar (1);
			}
						
		}
		if(GameManagerShare.instance.game == Assets.Scripts.Share.Game.Throw)
		{
			//var apple = scoreItemList.Where(x => x.type == ScoreItemsType.Apple).FirstOrDefault();
			if(ThrowManager.Instance.GetHits() >= 15){
				GameManagerShare.instance.BlinkStar (3);
			}
			else if(ThrowManager.Instance.GetHits() >= 9 && ThrowManager.Instance.GetHits() < 15){
				GameManagerShare.instance.BlinkStar (2);
			}
			else if(ThrowManager.Instance.GetHits() >= 3 && ThrowManager.Instance.GetHits() < 8 ){
				GameManagerShare.instance.BlinkStar (1);
			}
			else if(ThrowManager.Instance.GetHits() == 0)
			{
				GameManagerShare.instance.BlinkStar(0);
			}
		}

		if (GameManagerShare.instance.game == Assets.Scripts.Share.Game.Fishing) {
			if( Assets.Scripts.Share.Managers.ScoreManager.instance.GetScore() > 300){
				GameManagerShare.instance.BlinkStar(3);
			}else{
				GameManagerShare.instance.BlinkStar(2);
			}
		}

	}

}

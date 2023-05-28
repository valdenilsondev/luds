using UnityEngine;
using System.Collections;

using Sandbox.GameUtils;
using Sandbox.Level;
using Sandbox.UI;
using Share.Database;

namespace Sandbox.PlayerControl {
	public class ScoreManager  {
		private static int playerGold;
		private static int playerRelics;
		
		public static void AddScore (SceneObjects colectedObj){
			switch (colectedObj){
			case SceneObjects.None:
				TimeBar.instance.DecreaseTime(5);
				break;
			case SceneObjects.Coin:
				playerGold += 10;
				TimeBar.instance.DecreaseTime(1);
				break;
			case SceneObjects.Relic:
				playerGold += 25;
				TimeBar.instance.DecreaseTime(1);
				break;
			case SceneObjects.Treasure:
				playerRelics++;
				TimeBar.instance.PauseTimeBar(true);
				break;
			case SceneObjects.Battery:
				TimeBar.instance.IncreaseTime(20);
				break;
			case SceneObjects.Enemy:
				TimeBar.instance.DecreaseTime(10);
				break;
			default:
				playerGold = playerGold;
				break;
			}
		}
		
		public static void ResetScore(){
//			float deltaTime = LevelManager.instance.timeManager.StopTimer();
			//MySQL.instance.SaveGameRecords(playerGold, playerRelics, (int)deltaTime, System.DateTime.Now, GameManager.instance.GetPathToSS());
			playerGold = 0;
			playerRelics = 0;
		}
		
		public static int GetGold(){
			return playerGold;
		}
		
		
		public static int GetRelics(){
			return playerRelics;
		}
		
		
		
	}
}
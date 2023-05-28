using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Level {
	public class Levels {
		public int nbBattery;
		public int nbTreasure;
		public int nbCoin;
		public int nbRelic;
		public int nbEnemy;
		
		public Levels(int nbBattery, int nbTreasure, int nbCoin, int nbRelic, int nbEnemy){
			this.nbBattery = nbBattery;
			this.nbTreasure = nbTreasure;
			this.nbCoin = nbCoin;
			this.nbRelic = nbRelic;
			this.nbEnemy = nbEnemy;
		}
	}
	
	public class ItensManager  {
		public static List<Levels> levels = new List<Levels> (){
			//		   baterry		treasure	coin		relic		enemy
			new Levels(0, 			1, 			0, 			0,			0),
			new Levels(0, 			1, 			1, 			0,			0),
			new Levels(0, 			1, 			2, 			0,			0),
			new Levels(1, 			1, 			2, 			0,			1),
			new Levels(2, 			1, 			2, 			1,			1),
			new Levels(2, 			1, 			2, 			1,			1),
			new Levels(2, 			1, 			3, 			1,			1),
			new Levels(2, 			1, 			3, 			1,			1)
			
		};
		
		public static int GetQuantity (SceneObjects sceneObj, int level){
			//Debug.Log("level: "+level);
			if (level < 0 || level > levels.Count - 1) {
				return 0;		
			}else{
				if(sceneObj.ToString() == SceneObjects.Battery.ToString()){
					return levels[level].nbBattery;
				}
				if(sceneObj.ToString() == SceneObjects.Coin.ToString()){
					return levels[level].nbCoin;
				}
				if(sceneObj.ToString() == SceneObjects.Relic.ToString()){
					return levels[level].nbRelic;
				}
				if(sceneObj.ToString() == SceneObjects.Treasure.ToString()){
					return levels[level].nbTreasure;
				}
				if(sceneObj.ToString() == SceneObjects.Enemy.ToString()){
					return levels[level].nbEnemy;
				}else{
					return 0;
				}
			}
		}
		
	}
}
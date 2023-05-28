using UnityEngine;
using System.Collections;

namespace Sandbox.PlayerControl {
	public class PlayerGameRecord  {
		int idGameRecord;
		int idGame;
		int numCoins;
		int numTreasure;
		int numDifficult;
		int timeBar;
		int treasurePos;
		int minPlayed;
		string datePlayed;
		string screenshotPath;
		
		//campo de projeçao 
		string treasurePos_tp_treasurePos;
		
		public PlayerGameRecord(int idGameRecord, int idGame, int numCoins, int numTreasure, int numDifficult, int timeBar, int treasurePos, int minPlayed, string datePlayed, string screenshotPath){
			this.idGameRecord = idGameRecord;	
			this.idGame = idGame;
			this.numCoins = numCoins;
			this.numTreasure = numTreasure;
			this.numDifficult = numDifficult;
			this.timeBar = timeBar;
			this.treasurePos = treasurePos;
			this.minPlayed = minPlayed;
			this.datePlayed = datePlayed;
			this.screenshotPath = screenshotPath;
		}
		
		
		public int GetIdGameRecord(){
			return idGameRecord;
		}
		
		public int GetIdGame(){
			return idGame;
		}
		
		public int GetCoins(){
			return numCoins;
		}
		
		public int GetNumTreasure(){
			return numTreasure;
		}
		
		public int GetDifficultGamePreferences(){
			return numDifficult;
		}
		
		public int GetTimeBarGamePreferences(){
			return timeBar;
		}
		
		public int GetTreasurePosGamePreferences(){
			return treasurePos;
		}
		
		public int GetMinPlayed(){
			return minPlayed;
		}
		
		public string GetDatePlayed(){
			return datePlayed;
		}
		
		public string GetScreenshotPath(){
			return screenshotPath;
		}
		
		public void SetTreasurePosTpTreasurePosition(string treasurePos){
			treasurePos_tp_treasurePos = treasurePos;
		}
		
		public string GetTreasurePosTpTreasurePosition(){
			return treasurePos_tp_treasurePos;
		}
		
	}
}
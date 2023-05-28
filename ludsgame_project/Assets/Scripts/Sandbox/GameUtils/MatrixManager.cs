using UnityEngine;
using System.Collections;

using Sandbox.Level;
using Sandbox.PlayerControl;

public enum SceneObjects{
	None,
	Treasure,
	Battery,
	Coin,
	Enemy,
	Relic
}

namespace Sandbox.GameUtils {
	public class MatrixManager : MonoBehaviour {
		
		private int xSize = 5, ySize = 4;
		private SceneObjects[,] matrix;
		public static MatrixManager instance;
		public int treasurePosition; // 0 - direita
		//1 - esquerda
		//2 - random
		void Awake() {
			instance = this;
		}
		
		void PrintValueInSceneObjects(){
			if((CheckClosestItem.instance.closest / ySize) == 0){
				//nesse caso o x == 0
				print (GetSceneObject(0, CheckClosestItem.instance.closest));
			}else{
				print (GetSceneObject(CheckClosestItem.instance.closest / ySize, CheckClosestItem.instance.closest % ySize));
			}
		}
		
		public bool TestIfItsTreasure(){
			if((CheckClosestItem.instance.closest / ySize) == 0){
				//nesse caso o x == 0
				if(GetSceneObject(0, CheckClosestItem.instance.closest) == SceneObjects.Treasure){
					return true;
				}
			}else{
				if(GetSceneObject(CheckClosestItem.instance.closest / ySize, CheckClosestItem.instance.closest % ySize) == SceneObjects.Treasure){
					return true;
				}
			}
			return false;
		}
		
		public int ReturnValueInEnum(){
			if((CheckClosestItem.instance.closest / ySize) == 0){
				//nesse caso o x == 0
				return (int)GetSceneObject(0, CheckClosestItem.instance.closest);
			}else{
				return (int)GetSceneObject(CheckClosestItem.instance.closest / ySize, CheckClosestItem.instance.closest % ySize);
			}
		}
		
		public SceneObjects ReturnNameInEnum(){
			if((CheckClosestItem.instance.closest / ySize) == 0){
				//nesse caso o x == 0
				return GetSceneObject(0, CheckClosestItem.instance.closest);
			}else{
				return GetSceneObject(CheckClosestItem.instance.closest / ySize, CheckClosestItem.instance.closest % ySize);
			}
		}
		
		public void CallTreasurePosition(){
			// 0 - direita
			//1 - esquerda
			//2 - random
			
			if (treasurePosition == 1) {
				RightTreasurePosition();
			}
			if (treasurePosition == 0) {
				LeftTreasurePosition();
			}
			if (treasurePosition == 2) {
				RandomTreasurePosition();
			}
		}
		
		public void SetTreasurePosition(int position){
			treasurePosition = position;
		}
		
		public int ReturnTreasurePosition(){
			return treasurePosition;
		}
		
		void ResetMatrix() {
			
			//		Debug.Log("x: "+xSize+" y: "+ySize);
			matrix = new SceneObjects[xSize, ySize];
		}
		
		public void InitMatrix() {
			xSize = LevelCreator.instance.GetLevelXSize();
			ySize = LevelCreator.instance.GetLevelYSize();
			
			ResetMatrix();
			
			SceneObjects so = SceneObjects.None;
			for(int i = 1; i < System.Enum.GetValues( typeof(SceneObjects) ).Length; i++){
				so = (SceneObjects) i;
				if(so == SceneObjects.Treasure){
					CallTreasurePosition();
				} else {
					RandomPosition(so, ItensManager.GetQuantity(so, LevelManager.instance.GetCurrentLevel()));
				}
			}
			
			//PrintMatrix();
		}
		string s = "";
		void PrintMatrix2(){
			for(int y = ySize - 1; y >= 0; y--){
				for(int x = 0; x < xSize; x++){
					s += matrix[x, y].ToString() + " ";
				}
				Debug.Log(s);
				s = "";
			}
		}
		
		void PrintMatrix(){
			for(int x = 0; x < xSize; x++){
				for(int y = 0; y < ySize; y++){
					s += matrix[x, y].ToString() + " ";
				}
				Debug.Log(s);
				s = "";
			}
		}
		
		
		void RandomPosition(SceneObjects so, int count) {
			int x, y;
			
			for(int i = 0; i < count; i++) {
				RandomVector2(out x, out y);
				matrix[x, y] = so;
			}
		}
		
		float rightPercent;
		
		void RandomTreasurePosition() {
			int x = 0, y = 0;
			/*if(random <= rightPercent/100) {
			x = Random.Range(2, xSize);
		} else {
			x = Random.Range(0, 3);
		}*/
			x = Random.Range(0, xSize);
			y = Random.Range(0, ySize);
			
			matrix[x, y] = SceneObjects.Treasure;
		}
		
		
		void RightTreasurePosition() {
			int x = 0, y = 0;	
			x = Random.Range(xSize/2, xSize);
			y = Random.Range(0, ySize);
			
			matrix[x, y] = SceneObjects.Treasure;
		}
		
		
		void LeftTreasurePosition() {
			int x = 0, y = 0;
			x = Random.Range(0, xSize/2);
			y = Random.Range(0, ySize);
			
			matrix[x, y] = SceneObjects.Treasure;
		}
		
		void RandomVector2(out int x, out int y) {
			int counterInfiniteLoop = 0;
			do{
				counterInfiniteLoop++;
				if(counterInfiniteLoop > 100){
					x = 0;
					y = 0;
					
					Debug.LogError("Loop Infinito");
					return;
				}
				
				x = Random.Range(0, xSize);
				y = Random.Range(0, ySize);
			}while(matrix[x, y] != SceneObjects.None);
		}
		
		public SceneObjects GetSceneObject(int x, int y) {
			if(matrix == null){
				return SceneObjects.None;
			}
			if((x >= 0 && x < xSize) && (y >= 0 && y < ySize))
				//Debug.Log(x + " " + y);
				return matrix[x, y];
			
			return SceneObjects.None;
		}
		
		public SceneObjects GetSceneObjectByIndex(int index){
			return GetSceneObject(index/ySize, index%ySize);
		}
	}
}
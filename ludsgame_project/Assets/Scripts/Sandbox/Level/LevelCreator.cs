using UnityEngine;
using System.Collections;

namespace Sandbox.Level {
	public class LevelCreator : MonoBehaviour {
		
		//classe que ira instanciar os prefabs dos levels
		
		private GameObject x;
		
		private GameObject x54;
		private GameObject x43;
		private GameObject x33;
		
		private GameObject x12BG;
		private GameObject x22BG;
		private GameObject x33BG;
		private GameObject x43BG;
		private GameObject x44BG;
		private GameObject x54BG;
		
		private int xSize = 1;
		private int ySize = 2;
		
		public static LevelCreator instance;
		
		private int lvl;
		private float xPos;
		
		// Use this for initialization
		void Start () {
			instance = this;
			x12BG = GameObject.Find("1x2SemPause").gameObject;
			x22BG = GameObject.Find("2x2SemPause").gameObject;
			x33BG = GameObject.Find("3x3SemPause").gameObject;
			x43BG = GameObject.Find("4x3SemPause").gameObject;
			x44BG = GameObject.Find("4x4SemPause").gameObject;
			x54BG = GameObject.Find("5x4SemPause").gameObject;
			DeactivateBGs();
		}
		
		public int GetLevelXSize(){
			return xSize;
		}
		
		public int GetLevelYSize(){
			return ySize;
		}
		
		public GameObject ReturnLevelX(){
			return x;
		}
		
		public void DestroyThis(){
			GameObject.Destroy(x);
		}
		
		public void DeactivateBGs(){
			x12BG.SetActive(false);
			x22BG.SetActive(false);
			x33BG.SetActive(false);
			x43BG.SetActive(false);
			x44BG.SetActive(false);
			x54BG.SetActive(false);
		}
		
		private GameObject GetXResource(int lvl){
			if(lvl == 0){
				//x = x1;
				x = (GameObject) Instantiate(Resources.Load("Sandbox/1x2"));
				x12BG.SetActive(true);
				xPos = 0;
				xSize = 2;
				ySize = 1;
			}else
			if(lvl == 1){
				//x = x22;
				x = (GameObject) Instantiate(Resources.Load("Sandbox/2x2"));
				x22BG.SetActive(true);
				xPos = 0;
				xSize = 2;
				ySize = 2;
			}else
			if(lvl == 2){
				//x = x33;
				x = (GameObject) Instantiate(Resources.Load("Sandbox/3x3"));
				x33BG.SetActive(true);
				xPos = 0;
				xSize = 3;
				ySize = 2;
			}else
			if(lvl == 3){
				//x = x43;
				x = (GameObject) Instantiate(Resources.Load("Sandbox/4x3"));
				x43BG.SetActive(true);
				xPos = -0.5f;
				xSize = 4;
				ySize = 2;
			}else
			if(lvl == 4){
				//x = x44;
				x = (GameObject) Instantiate(Resources.Load("Sandbox/4x4"));
				x44BG.SetActive(true);
				xPos = 0;
				xSize = 5;
				ySize = 3;
			}else
			if(lvl >= 5){
				//x = x54;
				x = (GameObject) Instantiate(Resources.Load("Sandbox/5x4"));
				x54BG.SetActive(true);
				xPos = -0.66f;
				xSize = 5;
				ySize = 3;
			}
			
			return x;
		}
		
		public void CreateNextLevel(){	
			LevelManager.instance.NextLevel();
			lvl = LevelManager.instance.GetCurrentLevel();
			
			DeactivateBGs();
			
			GetXResource(lvl);
			
			x.transform.parent = GameObject.Find("Levels").transform;
			iTween.MoveTo (x, iTween.Hash ("x", xPos, "time", 0.3f));//, "easeType", iTween.EaseType.spring, "isLocal", true));
		}
		
		public void StartGame(){
			lvl = LevelManager.instance.GetCurrentLevel();
			
			DeactivateBGs();
			
			GetXResource(lvl);
			
			x.transform.parent = GameObject.Find("Levels").transform;
			iTween.MoveTo (x, iTween.Hash ("x", xPos, "time", 0.3f));
			LevelManager.instance.isGameStarted = true;
			MenuManager.isUserPlaying = true;
		}
		
		public void GameOver(){
			DeactivateBGs();
			DestroyThis();
			LevelManager.instance.ResetLevel();	
			
			MenuManager.isUserPlaying = false;
			LevelManager.instance.isGameStarted = false;
		}
	}
}
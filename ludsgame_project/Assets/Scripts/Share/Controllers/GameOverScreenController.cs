using Assets.Scripts.Share.Enums;
using Assets.Scripts.Share.Managers;
using Share.Controllers;
using Share.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Goalkeeper.Managers;

namespace Assets.Scripts.Share.Controllers
{
    public class GameOverScreenController : MonoBehaviour
    {
        public static GameOverScreenController instance;
        //public Menu menu;
        public GameObject score;
        public GameObject items;
		public GameObject gameOver_menuScreen;
		private Sprite board_img;
		private Image score_img;

		public bool isGameOverMenu;

		private bool isGameOverScreenOn;


        void Awake()
        {
            instance = this;
			gameOver_menuScreen.gameObject.SetActive(false);
			score_img = score.transform.GetChild(0).GetComponent<Image>();
        }
		void Start()
		{
			isGameOverScreenOn = false;
			isGameOverMenu = false;
			SwitchCupImage();
			score_img.sprite = board_img;
		}
        #region [Enable / Disable]
		/*void Update()
		{
			print ("isGameOverMenu: " + isGameOverMenu);
		}*/

        public void Enable()
        {
            BorgManager.instance.borgItemSelected = false;
           // CanvasController.instance.Disable();
            //GUIController.instance.Disable();
			if(PlayerPrefsManager.GetGameOverFirstTime() == 1){
				//Show();
			//	ScoreResult();//?
				GameOver_Presentation.instance.ShowPresentation();
				PlayerPrefsManager.SetGameOverFirstTime(1);
			}else{
            	Show();
            	ScoreResult();//?
			}
        }
         
        public void Disable()
        {
            Hide();
        }

        #endregion
		//tela de gameover2 apos pop-up
		public void ShowMenuGameOver()
		{
			isGameOverScreenOn = true;
			this.transform.GetComponent<Animator> ().SetTrigger ("gameOverIn");
			score.gameObject.SetActive(false);
			gameOver_menuScreen.gameObject.SetActive(true);
		}

        public void Show()
        {
			//print ("ShotGameOver");
			this.transform.GetComponent<Animator> ().SetTrigger ("gameOverIn");
			isGameOverScreenOn = true;
			Invoke("MenuGameOver", 10);
        }

		private void MenuGameOver()
		{
			isGameOverMenu = true;
			score.gameObject.SetActive(false);
			gameOver_menuScreen.GetComponent<Transform>().localScale = new Vector3(1,1,1);
			gameOver_menuScreen.gameObject.SetActive(true);
		}

        private void Hide()
        {
			this.transform.GetComponent<Animator>().SetTrigger("gameOverOut");
			isGameOverScreenOn = false;
        }

		public bool IsGameOverScreenOn(){
			return isGameOverScreenOn;
		}

        public void ScoreResult()
        {      
			//organiza os icones da tela conforme o game
			var scoreItemsList = GameManagerShare.instance.GetScoreItems();
			int item_qt = scoreItemsList.Count;
			print("score result");
			//var itemsList = items.transform.GetComponentsInChildren<ScoreItems>();
			var prefab = (ScoreItem) Resources.Load("Share/ScoreItem", typeof(ScoreItem));
			var spriteItems = Resources.LoadAll<Sprite>("Share/ScoreItems");
			var count = 0;
			var offset = 30;
			var topValue = 0;
			
			//foreach (var itemsScore in scoreItemsList)
			for(int  i = 0; i < item_qt; i++){	
				float scaleFactor = 0.9f;
				if(scoreItemsList.Count >= 3){
					scaleFactor = 0.7f;
					offset = 15;
					topValue = 25;
				}
				
				var scoreItemPrefab = (ScoreItem)Instantiate(prefab);
				scoreItemPrefab.transform.SetParent(items.transform);
				scoreItemPrefab.transform.name = scoreItemsList[i].type.ToString();
				//escolhe a imagem do board conforme o game e seta posiçao da imagem e score
				switch(GameManagerShare.instance.game)
				{
				case Game.Pig:
					//pig nao tem board de game over
					scoreItemPrefab.transform.localPosition = new Vector3(0, -1*(110 * count + count*offset)+topValue, 0);
					break;
				case Game.Bridge://1 score item
					if(i == 0){
						scoreItemPrefab.transform.localPosition = new Vector3(-35, -1*(110 * count + count*offset)+topValue, 0);
					}else{
						scoreItemPrefab.transform.localPosition = new Vector3(940, 0, 0);
						scoreItemPrefab.transform.GetChild (0).gameObject.transform.localScale = new Vector3(0,0,0) ;
					}
					break;
				case Game.Goal_Keeper://2 score item
					offset = 850;
					scoreItemPrefab.transform.localPosition = new Vector3(100+ count*offset, 0, 0);//-1*(110 * count + count*offset)+topValue
					scoreItemPrefab.transform.GetChild(0).localPosition = new Vector3(-1*( -20* count + 153), -100, 0);//-1*( -20* count + 153)
					break;
				case Game.Sup://2 score item
					offset = 850;
					scoreItemPrefab.transform.localPosition = new Vector3(100+ count*offset, 0, 0);//-1*(110 * count + count*offset)+topValue
					scoreItemPrefab.transform.GetChild(0).localPosition = new Vector3(-1*( -20* count + 153), -100, 0);//-1*( -20* count + 153)
					break;
				case Game.Fishing://bait, fish1, fish2,fish3
					if(i == 0)//bait
					{
						offset = 850;
						scoreItemPrefab.transform.localPosition = new Vector3(85+ count*offset, 0, 0);
						scoreItemPrefab.transform.GetChild(0).localPosition = new Vector3(-1*( -20* count + 120), -153, 0);
					}else{//fishes
						offset = 150;
						scoreItemPrefab.transform.localPosition = new Vector3((250 + count*offset), -133, 0);
						scoreItemPrefab.transform.GetChild(0).localPosition = new Vector3(-1*( -20* count + 153), -153, 0);
					}
					break;
				case Game.Throw:// 2 score item
					offset = 850;
					scoreItemPrefab.transform.localPosition = new Vector3(100+ count*offset, 0, 0);//-1*(110 * count + count*offset)+topValue
					scoreItemPrefab.transform.GetChild(0).localPosition = new Vector3(-1*( -20* count + 153), -100, 0);//-1*( -20* count + 153)
					break;
				}
				//coloca as imagens e seta valores do score
				scoreItemPrefab.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
				
				var scoreItem = scoreItemsList.Where(x => x.type == scoreItemsList[i].type).FirstOrDefault();
				var scoreImage = spriteItems.Where(x=>x.name == scoreItemsList[i].type.ToString()).FirstOrDefault();
				if(GameManagerShare.instance.game == Game.Fishing && i == 0)
					scoreItemPrefab.SetValueText(BucketBaitsControl.instance.GetNumberOfBaits());
				else if((GameManagerShare.instance.game != Game.Fishing) || (GameManagerShare.instance.game == Game.Fishing && i != 0))
					scoreItemPrefab.SetValueText(scoreItem.GetValue());

				scoreItemPrefab.SetImage(scoreImage);
				scoreItemPrefab.transform.GetChild(1).GetComponent<Image>().SetNativeSize();
				count++;
			}

			if(GameManagerShare.instance.game != Game.Fishing){
				var value = ScoreManager.instance.GetScore().ToString();		
				score.transform.GetChild(6).GetComponent<Text>().text = value;
			}else{
				score.transform.GetChild(6).GetComponent<Text>().text = " ";
			}
        }

		private void SwitchCupImage()
		{
			switch(GameManagerShare.instance.game)
			{
			case Game.Bridge:
				board_img = BridgeManager.instance.GetGameOverBoard();

				break;
			case Game.Goal_Keeper:
				board_img = GoalkeeperManager.instance.GetGameOverBoard();
				break;
			case Game.Sup:
				board_img = SupManager.instance.GetGameOverBoard();
				break;
			case Game.Fishing:
				board_img = FishingManager.instance.GetGameOverBoard();
				break;
			case Game.Throw:
				board_img = ThrowManager.Instance.GetGameOverBoard();
				break;
			}
		}

	}
}

	
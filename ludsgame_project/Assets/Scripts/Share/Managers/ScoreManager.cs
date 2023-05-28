using Assets.Scripts.Share.Enums;
using Runner.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Share.Managers;
using Share.Controllers;
using Goalkeeper.Managers;
using BridgeGame.GameUtilities;

namespace Assets.Scripts.Share.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        public List<ScoreItem> scoreItemList;

        private float value = 0;
        private int score = 0;
        private float apple_score;
        private int varDec;
        private ScoreItem apple;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            scoreItemList = GameManagerShare.instance.GetScoreItems();
        }

        void Update()
        {
            if (GameManagerShare.instance.resetTime)
                DecrementValue(apple);
        }

        public void Initialize()
        {
            value = 0;
        }

		public void IncreaseScore(ScoreItemsType scoreItem, float amount, float amount2)
        {
            switch (scoreItem)
            {
                case ScoreItemsType.Apple:
                    var apple = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					if(GameManagerShare.instance.game == Game.Pig && PowerUpManager.Instance.currentPowerUp == PowerUps.DoublePoints )
					{
						apple.SetValue(apple.GetValue() + (amount * 2));
					}
					if(GameManagerShare.instance.game == Game.Throw)
					{
						ThrowManager.Instance.apple_txt.text = (apple.GetValue() + amount).ToString();//refazer
						//ThrowManager.Instance.miss =  apple.GetValue() - ThrowManager.Instance.hits;
					}
					apple.SetValue(apple.GetValue() + amount);
					break;
                case ScoreItemsType.Distance:
                    var distance = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
                    distance.SetValue(amount);
                    break;
                case ScoreItemsType.Goal:
                    var goal = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
                    goal.SetValue(goal.GetValue() + 1);
                    break;
                case ScoreItemsType.Defense:
                    var defense = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
                    defense.SetValue(defense.GetValue() + 1);
                    break;
				case ScoreItemsType.Bridge_Distance:
					var distance_bridge = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					distance_bridge.SetValue(amount);					
					break;
				case ScoreItemsType.TimerSup:
				//segundos = amount % 60 --- minutos = (amount/60);

					var timer = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();					
					var segundos = amount2;
					timer.SetValue((int)segundos);
					if(GameManagerShare.instance.game == Game.Sup ){
						if(segundos >= 10){
							SupManager.instance.clock_txt.text = (amount + ":" + (int)(amount2));
						}
						else{
							SupManager.instance.clock_txt.text = (amount + ":" + "0" + (int)(amount2));
						}
					}
					break;

				case ScoreItemsType.Hits:
					/*var hits_counter = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					ThrowManager.Instance.hits_txt.text = (hits_counter.GetValue() + amount).ToString();
					hits_counter.SetValue(hits_counter.GetValue() + amount);*/
					var hits = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					
					//ThrowManager.Instance.hits = (int)(hits_counter.GetValue());
					hits.SetValue(hits.GetValue() + amount);
					ThrowManager.Instance.SetHits((int)(hits.GetValue()));
					ThrowManager.Instance.hits_txt.text = (hits.GetValue()).ToString();
					
				break;

				case ScoreItemsType.Miss:
					var miss = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					miss.SetValue(miss.GetValue() + amount);
					ThrowManager.Instance.miss = (int)(miss.GetValue());
					ThrowManager.Instance.miss_txt.text = (miss.GetValue()).ToString();
					break;

				case ScoreItemsType.Fish1:
					var fish1 = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
			//		print ("fish1: " + fish1.GetValue());
					fish1.SetValue(FishingManager.instance.qt_fishYellow + amount);
					FishingManager.instance.fishYellow.text = (1).ToString();
					FishingManager.instance.qt_fishYellow = fish1.GetValue();
					break;

				case ScoreItemsType.Fish2:
					var fish2 = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
				    fish2.SetValue(fish2.GetValue()+ amount);
					FishingManager.instance.fishBaiacu.text = (fish2.GetValue()).ToString();
					FishingManager.instance.qt_fishBaiacu = fish2.GetValue();
					break;

				case ScoreItemsType.Fish3:
					var fish3 = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					fish3.SetValue(fish3.GetValue()+ amount);
					FishingManager.instance.fishBlue.text = (fish3.GetValue()).ToString();
					FishingManager.instance.qt_fishBlue = fish3.GetValue();
					break;
				case ScoreItemsType.Bait:
					var bait = scoreItemList.Where(x => x.type == scoreItem).FirstOrDefault();
					int n = 0;
					n  = BucketBaitsControl.instance.GetNumberOfBaits();
				print(n);
					bait.SetValue(n);
					break;
				default:
                	break;
            }

            //item.text = value.ToString();
        }

        public void Disable()
        {
            this.gameObject.SetActive(false);
        }

		//resetar durante o jogo
        public void Reset()
        {
            var game = GameManagerShare.instance.game;

            switch (game)
            {
                case Game.Pig:
                    apple = PigRunnerManager.instance.scoreItems.Where(x => x.type == ScoreItemsType.Apple).FirstOrDefault();
                    GameManagerShare.instance.resetTime = true;

                    apple_score = apple.GetValue();
                    break;
                case Game.Goal_Keeper:
                    var items = GoalkeeperManager.instance.scoreItems;
                    foreach (var item in items)
                    {
                        item.SetValue(0);
                    }

                    break;
                case Game.Bridge:
                    break;
                case Game.Throw:
                    break;            
                default:
                    break;
            }
        }

        private void DecrementValue(ScoreItem item)
        {
            if (!GameManagerShare.IsPaused())
            {
                if (apple_score > 10)
                {
                    apple_score = Mathf.Lerp(apple_score, 0, 2 * Time.deltaTime);
                    System.Math.Round(apple_score, 0);
                    varDec = Convert.ToInt32(apple_score);
                    item.SetValue(varDec);//apple_score);
                }
                else
                {
                    item.SetValue(0);
                    GameManagerShare.instance.resetTime = false;
                }
            }
        }

        public int GetScore()
        {
            //calcula score de acordo com o game
            float amount = 0;

            if (GameManagerShare.instance.game == Game.Pig)
            {
                foreach (var item in scoreItemList)
                {
                    amount += item.GetValue() * item.multiplyingFactor;
				
                }
            }
            if (GameManagerShare.instance.game == Game.Goal_Keeper)
            {
                foreach (var item in scoreItemList)
                {
                    amount += item.GetValue() * item.multiplyingFactor;
                }
                amount = amount * 100;
                if (amount < 0)
                {
                    amount = 50;
                }
            }
			//revisar
			if (GameManagerShare.instance.game == Game.Bridge)
			{
				foreach (var item in scoreItemList)
				{
					amount += item.GetValue() * item.multiplyingFactor;
				}
			}

			if(GameManagerShare.instance.game == Game.Throw)
			{
				foreach (var item in scoreItemList)
				{
					amount = item.GetValue() * 100;
				}
				/*if(ThrowManager.Instance.hits == (int)(apple.GetValue()/2))
				{
					amount += (amount/2);
				}
				else if(ThrowManager.Instance.hits > (int)(apple.GetValue()/2))
				{
					amount += 3*(amount/2);
				}*/
			}

			if(GameManagerShare.instance.game == Game.Sup)
			{			
				foreach (var item in scoreItemList)
				{		
					amount += item.GetValue() * item.multiplyingFactor;					
				}
			}

			if(GameManagerShare.instance.game == Game.Fishing)
			{			
				foreach (var item in scoreItemList)
				{		
					amount += item.GetValue() * item.multiplyingFactor * 100;					
				}
			}

            return int.Parse(amount.ToString());
        }


		public int GetScoreOfType(ScoreItemsType itemType)
		{
			//qual a diferença de apple no pigrunner e no sup?
			if(itemType == ScoreItemsType.Apple){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Apple){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}
			if(itemType == ScoreItemsType.Distance){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Distance){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.Defense){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Defense){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}
			if(itemType == ScoreItemsType.Goal){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Goal){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.Fish1){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Fish1){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.Fish2){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Fish2){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.Fish3){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Fish3){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.Hits){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Hits){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.Miss){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.Miss){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}

			if(itemType == ScoreItemsType.TimerSup){
				foreach (var item in scoreItemList)
				{
					if(item.type == ScoreItemsType.TimerSup){
						return int.Parse(item.GetValue().ToString()); 
					}
				}
			}
		
			return 0;
		}


        public void SetScore(int _score)
        {
            score = _score;
        }
    }
}

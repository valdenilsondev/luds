using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BridgeGame.UI;
using BridgeGame.Player;
using BridgeGame.KinectControl;
using UnityEngine.UI;
using Assets.Scripts.Share.Managers;
using Assets.Scripts.Share.Enums;
using Share.Managers;

namespace BridgeGame.GameUtilities {
	public class ScoreManagerBridge : MonoBehaviour {
		
		private GameObject esfera;
		//private GameObject bonusParent;

		private GameObject scoreText;
		private GameObject playerReference;
		
		//public GUIText bonusText;
		private float playerScore;
		private float elapsedTime;
		
		private int bonus = 1;
		private float distPercorrida;
		private bool isFirewallActivated = true;
		private float initialPlayerPos;
		
		private static int aux;
		public static ScoreManagerBridge instance;
		
		void Awake(){				
			instance = this;
			esfera = GameObject.Find("BalanceBar").transform.Find("player_bar").gameObject;
		
		}

		void Start(){
			initialPlayerPos = PlayerControl.instance.GetPlayer().transform.position.z;
		}

		public GameObject GetScoreText(){
			return scoreText;
		}

		void Update(){
			if(GameManagerShare.IsStarted() && !GameManagerShare.IsGameOver() && !BridgeManager.instance.IsPlayerOnTheWater())
			CalculateScore ();			
		}

		public void CalculateScore(){
			//
			GameObject go = PlayerControl.instance.GetPlayer();
			distPercorrida = initialPlayerPos - go.transform.position.z;
			initialPlayerPos = go.transform.position.z;
			
			playerScore += distPercorrida * PlayerControl.instance.playerSpeed;	
			aux = (int)playerScore;

			GameManagerShare.instance.IncreaseScore(ScoreItemsType.Bridge_Distance, aux, 0);
		}
		
		public static int GetScore(){
			return aux;
		}
	}
} 
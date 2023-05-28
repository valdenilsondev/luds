using UnityEngine;
using System.Collections;
using BridgeGame.Player;

namespace BridgeGame.Bridge {
	public class BridgeControl : MonoBehaviour {
		
		#region variables
		private Transform bridgePiecesParent;
		private bool sidePieceMovement = true;
		
		private float finalXPos;
		private float finalYPos;
		private float finalXRotation;
		
		//valores de incremento para os lados
		private float powerXCoord;
		public float minPowerXCoord;
		public float maxPowerXCoord;
		
		//valores de incremento para cima e para baixo
		private float powerYDirection;
		public float minPowerYDirection;
		public float maxPowerYDirection;
		
		//valores de incremento para as rotacoes
		private float powerXRotation;
		public float minPowerXRotation;
		public float maxPowerXRotation;
		
		private int rndXSide;
		private int rndYDirection;
		//private int rndXRotation;
		
		//informa para qual lado a ponte esta entortando left or right
		private string Xside;
		
		//var usada para guardar a posicao inicial das pieces
		private float initialXPosOfPieces;
		
		private string Ydirection;
		
		//informa para que lado sera a rotacao
		private string XRotation;
		
		private float closestPieceInd;
		public float rangeEffectValue;
		public GameObject[] bridgePieces;
		
		//private GameObject playerCamera;
		
		public static BridgeControl instance;
		#endregion
		
		void Awake(){
			instance = this;
		}
		
		void Start () {
			bridgePiecesParent = GameObject.Find("BridgeManager").transform;
			//playerCamera = GameObject.Find("Main Camera").gameObject;
			bridgePieces = new GameObject[bridgePiecesParent.childCount];
			
			for(int i = 0; i < bridgePiecesParent.childCount; i++){
				bridgePieces[i] = bridgePiecesParent.GetChild(i).gameObject;
			}
			
			//se o range value for 0
			if(rangeEffectValue == 0){
				rangeEffectValue = 15;
			}
			
			int pos = bridgePieces.Length/2;
			initialXPosOfPieces = bridgePieces[pos].transform.position.x;
			
			//chamada para rotacionar a ponte z 
			//MoveBridge();
			//chamada para mover a ponte x e y
			//MoveBridge2();
		}
		
		public GameObject[] GetBridgePieces(){
			return bridgePieces;
		}
		
		//retorna pra que lado a ponte esta sendo entortada
		public string GetBridgeXSide(){
			return Xside;
		}
		
		public float GetBridgePowerOnXCoord(){
			int pos = bridgePieces.Length/2;
			float result = bridgePieces[pos].transform.position.x - initialXPosOfPieces;
			return Mathf.Abs(result);
		}
		
		/*#region move
		//metodo 1 - ponte move-se de acordo com a movimentacao do player
		private void MoveBridge(){
			
			float value = 0;
			float frontEffect = 0;
			float backEffect = 0;
			
			//peca mais proxima do player
			closestPieceInd = PlayerControl.instance.GetClosestPieceInd();
			
			for(int i = 0; i < bridgePiecesParent.childCount; i++){
				
				if(i >= closestPieceInd - rangeEffectValue && i <= closestPieceInd + rangeEffectValue){
					if(i > closestPieceInd){
						value = ( (closestPieceInd + rangeEffectValue) - i ) / (rangeEffectValue);
						backEffect = PlayerControl.instance.transform.rotation.eulerAngles.z * (value);
						if(PlayerControl.instance.transform.rotation.eulerAngles.z > 90){
							backEffect = -(360-PlayerControl.instance.transform.rotation.eulerAngles.z) * (value);
						}
						iTween.RotateTo(bridgePieces[i], iTween.Hash ("z", backEffect, "time", 2f, "easytype", iTween.EaseType.linear) );
					}else if(i <= closestPieceInd){
						value = ( (closestPieceInd - rangeEffectValue) - i ) / (rangeEffectValue);
						frontEffect = -PlayerControl.instance.transform.rotation.eulerAngles.z * (value);
						if(PlayerControl.instance.transform.rotation.eulerAngles.z > 90){
							frontEffect = (360-PlayerControl.instance.transform.rotation.eulerAngles.z) * (value);
						}
						//Debug.Log("player ang :"+PlayerControl.instance.transform.rotation.eulerAngles.z+" front effect: "+frontEffect);
						iTween.RotateTo(bridgePieces[i], iTween.Hash ("z", frontEffect, "time", 2f, "easytype", iTween.EaseType.linear) );
					}
				}
				
				if(value <= 0){
					value = 0;
				}
			}
			Invoke("MoveBridge", 0.05f);
		}
		#endregion*/
		
		/*#region move2
		//metodo 2 - ponte move-se sozinha
		private void MoveBridge2(){
			
			rndXSide = Random.Range(0,2);
			rndYDirection = Random.Range(1,2); // so esta sendo usado o down
			//rndXRotation = Random.Range(0,2);
			
			powerXCoord = Random.Range(minPowerXCoord,maxPowerXCoord);
			powerYDirection = Random.Range(minPowerYDirection,maxPowerYDirection);
			//powerXRotation = Random.Range(minPowerXRotation,maxPowerXRotation);
			
			if(rndXSide == 0){Xside = "right";}	else{Xside = "left";}
			if(rndYDirection == 0){Ydirection = "top";}	else{Ydirection = "down";}
			//if(rndXSide == 1){XRotation = "positive";}	else{XRotation = "negative";}
			
			for(int i = 0; i < bridgePiecesParent.childCount; i++){
				
				finalXPos = ReturnWindPowerToBridgePosition(97f, i, powerXCoord, Xside);
				finalYPos = ReturnWindPowerToBridgePosition(50, i, powerYDirection, Ydirection);
				//finalXRotation = ReturnWindPowerToBridgePosition(0, i, powerXRotation, XRotation);
				
				iTween.MoveTo (bridgePieces[i], iTween.Hash ("x", finalXPos, "y", finalYPos, "time", 30, "easytype", iTween.EaseType.linear) );
				//iTween.RotateTo(bridgePieces[i], iTween.Hash ("z", finalXRotation, "time", 30, "easytype", iTween.EaseType.linear) );
			}
			Invoke("MoveBridge2", 15);
		}
		#endregion*/
		
		#region PowerToBridge
		private float ReturnWindPowerToBridgePosition(float posGiven, int position, float power, string way){
			float newPos = posGiven;
			float percentage;
			
			if(position != 0){
				percentage = ( ( ((float) positionBasedOnHalfOfArraySize(position)) / ( (float) ((bridgePieces.Length) / 2f) ) ) );
				
				if(percentage > 0.9f){
					percentage = 0.9f;
				}
				
				if(way == "right" || way == "positive"){
					newPos = newPos +(power * percentage);
				}else if(way == "left" || way == "negative"){
					newPos = newPos -(power * percentage);
				}
				
				if(way == "top"){
					newPos = newPos +(power * percentage);
				}else if(way == "down"){
					newPos = newPos -(power * percentage);
				}
			}
			
			return newPos;
		}
		
		private int positionBasedOnHalfOfArraySize(int i){
			if(i > bridgePieces.Length/2){
				return bridgePieces.Length - i;
			}else{
				return i;
			}
		}
		#endregion
	}
}

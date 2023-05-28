using UnityEngine;
using System.Collections;

namespace BridgeGame.Bridge {
	public class BridgeGenerator : MonoBehaviour {
		
		//numero total
		public int NumberOfPiecesInBridge;
		
		//posicao da primeira piece
		public Vector3 startPosition;
		
		//posicao do anterior
		private Vector3 previousPosition;

		//obj anterior
		private GameObject previousGO;
		
		//espaco entre cada uma
		public float spaceBetweenPieces;
		
		//escala de x y e z de cada
		public float piecesSizeX;
		public float piecesSizeY;
		public float piecesSizeZ;

		//gatilho para travar no segundo tronco
		private bool secondArayPiece = false;
		
		//array de pieces
		private GameObject[] bridgeArrayPieces;
		
		// Use this for initialization
		void Start () {
			//array de game objects das pieces
			bridgeArrayPieces = new GameObject[NumberOfPiecesInBridge];
			
			//var de referencia go
			GameObject go;
			
			//clonando gameoject
			//GameObject clone = GameObject.Find("piece");
			
			//primeiro previous
			previousPosition = new Vector3(-1,-1,-1);
			
			//escala padrao 1
			if(piecesSizeX == 0){
				piecesSizeX = 1;
			}else if(piecesSizeY == 0){
				piecesSizeY = 1;
			}else if(piecesSizeZ == 0){
				piecesSizeZ = 1;
			}
			
			//distancia padrao entre as pieces 2
			if(spaceBetweenPieces == 0){
				spaceBetweenPieces = 2;	 
			}
			
			for(int i = 0; i < bridgeArrayPieces.Length; i++){
				//instanciando piece na var go
				go = (GameObject) GameObject.Instantiate(Resources.Load("BridgePiece/tronco_bridge"));
				
				//carregando escala
				go.transform.localScale = new Vector3(piecesSizeX,piecesSizeY,piecesSizeZ);

				if(previousPosition == new Vector3(-1,-1,-1)){
					//primeira piece
					go.transform.position = startPosition; 
					//salvando referencia da startpos na previouspos
					previousPosition = go.transform.position;
					//
					go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				}else if(i != bridgeArrayPieces.Length-1){
					//pos da piece incrementada com a previous
					go.transform.position = new Vector3 (previousPosition.x, previousPosition.y, previousPosition.z - spaceBetweenPieces);
					//salvando referencia da nova previous
					previousPosition = go.transform.position;
					//
				}else{
					//pos da piece incrementada com a previous
					go.transform.position = new Vector3 (previousPosition.x, previousPosition.y, previousPosition.z - spaceBetweenPieces);
					//salvando referencia da nova previous
					previousPosition = go.transform.position;
					//
					//go.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
					go.GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezeAll;
				}
				//definindo o pai do go
				go.transform.parent = GameObject.Find("BridgeManager").transform;
				//salvando referencia no array de pieces
				bridgeArrayPieces[i] = go;			

				//go.AddComponent<HingeJoint>();

				if(previousGO != null){
					//qualquer outro menos o primeiro
					go.GetComponent<HingeJoint>().connectedBody = previousGO.GetComponent<Rigidbody>();
					if(!secondArayPiece){
						previousGO.GetComponent<HingeJoint>().connectedBody = go.GetComponent<Rigidbody>();
						secondArayPiece = true;
					}
				}
				//salvando obj previous
				previousGO = go;
			}
		}
	}
}

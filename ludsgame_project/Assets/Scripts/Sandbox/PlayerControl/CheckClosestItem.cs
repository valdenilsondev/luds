using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Sandbox.UI;

namespace Sandbox.PlayerControl {
		public class CheckClosestItem : MonoBehaviour {

			//classe usada para verificar o que o push acertou in game

			private GameObject go;
			public GameObject[] arrayPieces;
			private GameObject closestPiece;
			private Transform rhand;
			private bool isOnGame = false;
			private List<GameObject> goColliders;
			public int closest = 0;
			public static CheckClosestItem instance;

			void Awake ()
			{
					instance = this;
			}	        

			// Use this for initialization
			void Start ()
			{
					rhand = GameObject.Find ("rightHand").transform;
					closestPiece = GameObject.Find ("ClosestPiece");
					go = GameObject.Find ("Levels");
			}
	
			void Update ()
			{		
					GetLevelsGO ();		
					GameSelector ();
			}

			public void SetHand (Transform hand)
			{
					rhand = hand;
					DetectorAnimationController.instance.SetHandDetector (rhand);
			}

			private void GameSelector ()
			{
					if (go.transform.childCount != 0) {
							if (closest >= arrayPieces.Length) {
									closest = 0;
							}
			
							for (int i = 0; i < arrayPieces.Length; i++) {
									if (Vector3.Distance (arrayPieces [i].transform.position, rhand.transform.position) <
											Vector3.Distance (arrayPieces [closest].transform.position, rhand.transform.position)) {
											if (arrayPieces [i].name != "PauseCollider")
													closest = i;
									}
							}
							closestPiece.transform.position = new Vector3 (arrayPieces [closest].transform.position.x, arrayPieces [closest].transform.position.y, 0.5f);
					}
			}
	
			private void GetLevelsGO () {
				if (go == null) {
					go = GameObject.Find ("Levels");
					isOnGame = false;
				}
				if (go.transform.childCount != 0 && !isOnGame) {
					go = go.transform.GetChild (0).gameObject;		
	
					arrayPieces = new GameObject[go.transform.childCount];
	
					for (int i = 0; i < go.transform.childCount; i++) {
						arrayPieces [i] = go.transform.GetChild (i).gameObject;
					}
					isOnGame = true;
				}
		}

		}
}
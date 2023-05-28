using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.UI {
	public class SelectorControl : MonoBehaviour {
		
		private GameObject go;
		public GameObject[] arrayPieces;
		public GameObject handSelector;
		
		private bool isOnGame = false;
		private List<GameObject> goColliders;
		
		public int smaller = 0;
		private bool isPaused = false;
		
		public static SelectorControl instance;
		
		void Awake() {
			instance = this;
		}
		
		void Start () {
			go = GameObject.Find("levels");
		}
		
		void Update () {
			
			GetLevelsGO();
			
			MoveSelector();
		}
		
		private void MoveSelector(){
			if(go.transform.childCount != 0){
				if(smaller >= arrayPieces.Length){
					smaller = 0;
				}
				
				for(int i = 0; i < arrayPieces.Length; i++){
					if(Vector3.Distance(arrayPieces[i].transform.position, handSelector.transform.GetChild(0).transform.position) <
					   Vector3.Distance(arrayPieces[smaller].transform.position, handSelector.transform.GetChild(0).transform.position)){
						smaller = i;
					}
				}
				this.transform.position = new Vector3(arrayPieces[smaller].transform.position.x, arrayPieces[smaller].transform.position.y, 0.5f);
			}
		}
		
		private void GetLevelsGO(){
			if(go == null){
				go = GameObject.Find("levels");
				isOnGame = false;
			}
			if(go.transform.childCount != 0 && !isOnGame){
				go = go.transform.GetChild(0).gameObject;		
				
				arrayPieces = new GameObject[go.transform.childCount];
				
				for(int i = 0; i < go.transform.childCount; i++){
					arrayPieces[i] = go.transform.GetChild(i).gameObject;
				}
				isOnGame = true;
			}
			
			if(GameObject.Find("levels").transform.childCount != 0){
				Vector3 vec3 = GameObject.Find("levels").transform.GetChild(0).transform.GetChild(0).transform.localScale;
				this.transform.localScale = new Vector3(vec3.x*1.2f, vec3.y*1.2f, vec3.z);
			}
		}
		
		/// <summary>
		/// Checks the color.
		/// </summary>
		/// <returns><c>true</c>, if color was checked, <c>false</c> otherwise.</returns>
		/// <param name="color">Color.</param>
		public bool CheckColor(Color color) {
			if(color == arrayPieces[smaller].GetComponent<SpriteRenderer>().color) {
				return true;
			}
			
			smaller = 0;
			
			return false;
		}
		
		public Color GetColorOverSelector(){
			return arrayPieces[smaller].GetComponent<SpriteRenderer>().color;
		}
	}
}
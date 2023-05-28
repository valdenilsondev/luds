using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreateMiniGamesIcons : MonoBehaviour {

	private FillGameSelection fillGame;
	public GameObject miniGameIconPrefab;
	public List<GameObject> miniGameIconsList;
	public Sprite[] spritesIcon;

	public static CreateMiniGamesIcons instance;
	// Use this for initialization
	void Awake () {
		instance = this;
		fillGame = new FillGameSelection();
		miniGameIconsList = new List<GameObject> ();
		Create ();
	}


	void Create(){
		float initalX = 150;

		for (int i = 0; i<fillGame.miniGamesList.Count; i++) {
			GameObject miniGameIcon = Instantiate (miniGameIconPrefab);				
			miniGameIcon.gameObject.transform.SetParent (this.gameObject.transform);
			miniGameIcon.GetComponent<Image>().sprite = spritesIcon[i];
			miniGameIcon.GetComponent<Image>().SetNativeSize();
			miniGameIcon.GetComponentInChildren<Text>().text = fillGame.miniGamesList[i].GetMiniGameName();
			miniGameIcon.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			miniGameIcon.GetComponent<MiniGameIconBtn>().gameIndex = i;
			
			//		miniGameIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(initalX,0);
			miniGameIconsList.Add(miniGameIcon);	
			
		}
		
		miniGameIconsList[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(150,0);
		miniGameIconsList[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(699,0);
		miniGameIconsList[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(1319,0);
		miniGameIconsList[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(2000,0);
		miniGameIconsList[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(2548,0);
		miniGameIconsList[5].GetComponent<RectTransform>().anchoredPosition = new Vector2(3155,0);

	}
	// Update is called once per frame
	void Update () {
	
	}
}

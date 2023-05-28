using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniGamesController : MonoBehaviour {

	public int gamesArraySize;

	public Sprite[] gamesPictures;
	public string[] gamesNames;

	private GameObject gameBox;
	private GameObject[] gameBoxes;
    private GameObject[] categoriasBoxes;

    public GameObject miniGamesParent;
    private Transform categoriasParent;

	private int currentScaled = 0;
	public bool iTweenRunning = false;

    private static Animator mini_game_box_anim;

	public static MiniGamesController instance;

    private GameObject GB;

    void Awake()
    {
        //GB = GameObject.Find("game_boxes").GetComponent<MiniGamesController>().gameObject;
    }

	void Start () {
		instance = this;
        mini_game_box_anim = this.gameObject.GetComponent<Animator>();

		//iniciando o array de gameboxes pelo valor setado no inspector
		gameBoxes = new GameObject[gamesArraySize];

		//recebendo referencias
		//gameBox = this.transform.FindChild("game_box").gameObject;

		gameBox = null;// (GameObject) Resources.LoadAssetAtPath("Assets/Prefabs/Screens/game_box.prefab", typeof(GameObject));
        miniGamesParent = this.gameObject;
		//miniGamesParent = GameObject.FindObjectOfType<MiniGamesController>().gameObject.transform;

		//setando primeira posicao do array
		//gameBoxes[0] = gameBox;

		//populando o array de game boxes com clones do game box principal
		for(int i = 0; i < gameBoxes.Length; i++){
			GameObject go = (GameObject)Instantiate(gameBox);
			gameBoxes[i] = go;
            go.transform.SetParent(miniGamesParent.transform);
            go.transform.localPosition = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
			go.name = "game_box"+(i);
		}

		//atribuindo nomes dos jogos
		for(int i = 0; i < gameBoxes.Length; i++){
			gameBoxes[i].GetComponentInChildren<Text>().text = gamesNames[i];
		}

		//atribuindo imagens dos jogos
		for(int i = 0; i < gameBoxes.Length; i++){
			gameBoxes[i].transform.Find("game_image").GetComponent<Image>().sprite = gamesPictures[i];
        }

        MoveGameBoxes();
    }

	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			MoveGameBoxesToLeft();
		}else if(Input.GetKeyDown(KeyCode.RightArrow)){
			MoveGameBoxesToRight();
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			LoadGameBasedOnSelected();
		}
	}

    public void MoveGameBoxKinect(string side)
    {
        if(side == "right")
        {
            MoveGameBoxesToRight();
        }
        else if(side == "left")
        {
            MoveGameBoxesToLeft();
        }
    }

	private void MoveGameBoxesToLeft(){
        //float miniGamesParentX = GB.transform.localPosition.x;
        //float miniGamesParentY = GB.transform.localPosition.y;
        //if(currentScaled < gamesArraySize-1 && iTweenRunning == false){
        //    //
        //    DecreaseGameBoxScale();
        //    currentScaled++;
        //    IncreaseGameBoxScale();
        //    miniGamesParentX = miniGamesParentX - 410;
        //    //
        //    iTweenRunning = true;

        //    //mini_game_boxes.transform.position = new Vector3(miniGamesParentX, miniGamesParentY, 0);
        //    iTween.MoveTo(GB, iTween.Hash("position", new Vector3(miniGamesParentX, miniGamesParentY, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear));
        //    //impede que sejam chamados itweens ao mesmo tempo
        //    Invoke("ResetItweenRunning", 0.4f);
        //}
	}

	private void MoveGameBoxesToRight(){
        float miniGamesParentX = miniGamesParent.transform.position.x;
        float miniGamesParentY = miniGamesParent.transform.position.y;
		if(currentScaled > 0  && iTweenRunning == false){
			//
			DecreaseGameBoxScale();
			currentScaled--;
			IncreaseGameBoxScale();
			miniGamesParentX = miniGamesParentX + 410;
			//
			iTweenRunning = true;
            iTween.MoveTo(miniGamesParent, iTween.Hash("position", new Vector3(miniGamesParentX, miniGamesParentY, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear));
			//impede que sejam chamados itweens ao mesmo tempo
            Invoke("ResetItweenRunning", 0.4f);
		}
	}

	public void LoadGameBasedOnSelected(){
		if(GameBoxName() == "Pig Runner"){
			//carregar jogo runner
			SceneManager.LoadScene("PigRunner");
		}else if(GameBoxName() == "Goalkepper"){
			SceneManager.LoadScene("GoalKeeper");
		}else if(GameBoxName() == "Bridge"){
			SceneManager.LoadScene("Bridge");
		}else if(GameBoxName() == "Bulls Eye"){
			//carregar jogo
		}else if(GameBoxName() == "Sandbox Treasure"){
			SceneManager.LoadScene("Sandbox");
			//carregar jogo
		}
	}

	private string GameBoxName(){
		return gameBoxes[currentScaled].transform.Find("game_name").GetComponent<Text>().text;
	}

	private void ResetItweenRunning(){
		iTweenRunning = false;
	}

	private void DecreaseGameBoxScale(){
		iTween.ScaleTo(gameBoxes[currentScaled], new Vector3(0.8f, 0.8f, 0.8f), 0.5f);
	}

	private void IncreaseGameBoxScale(){
		iTween.ScaleTo(gameBoxes[currentScaled], new Vector3(1, 1, 1), 0.5f);
	}

	public void MoveGameBoxes(){
		Vector3 gameBoxPos = gameBox.transform.position;
		float incPosX = 0;

        gameBoxes[0].transform.localScale = new Vector3(1f, 1f, 1f);
        gameBoxes[0].transform.localPosition = new Vector3(0, 0, 0);
        for (int i = 0; i < 200; i++)
        {
            iTween.MoveTo(gameBoxes[0], iTween.Hash("position", new Vector3(0, -200 + i, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear));    
        }
        

        for(int i = 1; i < gameBoxes.Length; i++){
			gameBoxes[i].transform.localScale = new Vector3(0.8f,0.8f,0.8f);
			gameBoxes[i].transform.localPosition = new Vector3(0, 0, 0);
			incPosX = incPosX + 551;
			iTween.MoveTo(gameBoxes[i], iTween.Hash("position", new Vector3(incPosX, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear));
		}
	}

	public void HideGameBoxes(){
		for(int i = 0; i < gameBoxes.Length; i++){
			iTween.MoveTo(gameBoxes[i], iTween.Hash("position", new Vector3(550*i/*gameBoxes[i].transform.position.x*/, -800, 0), "islocal", true, "time", 0.5f));
		}
	}

    public void ShowMiniGameBoxes()
    {
        Debug.Log("ShowMiniGameBoxes");
        mini_game_box_anim.SetTrigger("ShowGameBox");
    }
    public void HideMiniGameBoxes()
    {
        mini_game_box_anim.SetTrigger("HideGameBox");
    }

}

using UnityEngine;
using System.Collections;
using Assets.Scripts.Share;
using UnityEngine.UI;
using Share.Managers;

public class ComandLoader : MonoBehaviour {

	private Game game;

	public Text c1_txt, c2_txt, c3_txt;

	public static string path;
	private Comand game_comand;
	/*
	private static ComandLoader instance;
	public static ComandLoader Instance
	{ 
		get {
			if(instance == null) {
				instance = FindObjectOfType<ComandLoader>();
				if(instance == null) {
					Debug.Log ("Objeto do tipo ComandLoader nao foi encontrado");
				}
			}
			return instance;
		}
	}*/

	// Use this for initialization
	void Awake()
	{
	//	instance = this;

	}

	void Start () {
		game = GameManagerShare.instance.game;

	/*	c1_txt =  comand1.gameObject.GetComponent<Text>();
		c2_txt = comand2.gameObject.GetComponent<Text>();
		c3_txt = comand3.gameObject.GetComponent<Text>();*/
		path = "Comands";
	
		SaveData.Load(path);
		
		CheckComandsByGame();
		//Debug.Log( SaveData.cmdContainer.comands[0].cmd3);
	}

	private void CheckComandsByGame(){
		switch (game)
		{
		case(Game.Pig):

			if(PlayerPrefsManager.GetIdPerfilPigrunner() == 0){//GetPigRunnerSetupMovement() == 0){
				c1_txt.text = SaveData.cmdContainer.comands[0].cmd1;
				c2_txt.text = SaveData.cmdContainer.comands[0].cmd2;
				c3_txt.text = SaveData.cmdContainer.comands[0].cmd3;
			}
			else{
				c1_txt.text = SaveData.cmdContainer.comands[1].cmd1;
				c2_txt.text = SaveData.cmdContainer.comands[1].cmd2;
				c3_txt.text = SaveData.cmdContainer.comands[1].cmd3;
			}
			break;
		case(Game.Goal_Keeper):
			if(PlayerPrefsManager.GetIdPerfilGK() == 0){
				c1_txt.text = SaveData.cmdContainer.comands[2].cmd1;
				//c2_txt.text = SaveData.cmdContainer.comands[2].cmd2;
				//c3_txt.text = SaveData.cmdContainer.comands[2].cmd3;
			}
			else{
				c1_txt.text = SaveData.cmdContainer.comands[3].cmd1;
				//c2_txt.text = SaveData.cmdContainer.comands[3].cmd2;
				//c3_txt.text = SaveData.cmdContainer.comands[3].cmd3;
			}
			break;
		case(Game.Bridge):
				c1_txt.text = SaveData.cmdContainer.comands[4].cmd1;
				c2_txt.text = SaveData.cmdContainer.comands[4].cmd2;
				//c3_txt.text = SaveData.cmdContainer.comands[4].cmd3;

			break;
		case(Game.Throw):
				c1_txt.text = SaveData.cmdContainer.comands[5].cmd1;
				c2_txt.text = SaveData.cmdContainer.comands[5].cmd2;
				//c3_txt.text = SaveData.cmdContainer.comands[5].cmd3;
			break;
		case(Game.Sup):
			if(PlayerPrefsManager.GetIdPerfilSup() == 0){
				c1_txt.text = SaveData.cmdContainer.comands[6].cmd1;
				c2_txt.text = SaveData.cmdContainer.comands[6].cmd2;
				c3_txt.text = SaveData.cmdContainer.comands[6].cmd3;
			}
			else{
				c1_txt.text = SaveData.cmdContainer.comands[7].cmd1;
				c2_txt.text = SaveData.cmdContainer.comands[7].cmd2;
				c3_txt.text = SaveData.cmdContainer.comands[7].cmd3;
			}
			break;
		case(Game.Fishing):
			c1_txt.text = SaveData.cmdContainer.comands[8].cmd1;
			c2_txt.text = SaveData.cmdContainer.comands[8].cmd2;
			//c3_txt.text = SaveData.cmdContainer.comands[8].cmd3;
			break;
		
		default:
			break;
			
		}

	}
	

	/*private void CheckComandsByGame()
	{
		//pega nome do jogo no xml Comands
		switch (game)
		{
		case(Game.Pig):
			if(game_comand.name.text == "PigDefault")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
				c3_txt.text = game_comand.comand3.text;
			}
			else if(game_comand.name.text == "PigVariation")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
				c3_txt.text = game_comand.comand3.text;
			}
			break;
		case(Game.Goal_Keeper):
			if(game_comand.name.text == "GkDefault")
			{
				c1_txt.text = game_comand.comand1.text;
			}
			break;
		case(Game.Bridge):
			if(game_comand.name.text == "BridgeDefault")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
			}
			break;
		case(Game.Sup):
			if(game_comand.name.text == "SupDefault")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
				c3_txt.text = game_comand.comand3.text;
			}
			else if(game_comand.name.text == "SupVariation")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
			}
			break;
		case(Game.Fishing):
			if(game_comand.name.text == "FishingDefault")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
				c3_txt.text = game_comand.comand3.text;
			}
			break;
		case(Game.Throw):
			if(game_comand.name.text == "ThrowDefault")
			{
				c1_txt.text = game_comand.comand1.text;
				c2_txt.text = game_comand.comand2.text;
			}
			break;
		default:
			break;
		}
	}
*/
}
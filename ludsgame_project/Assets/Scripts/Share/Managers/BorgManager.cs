using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System;
using Share.Managers;
using Assets.Scripts.Share.Managers;
using Assets.Scripts.Share;
using Assets.Scripts.Share.Controllers;
using Ludsgame;
using UnityStandardAssets.ImageEffects;

public class BorgManager : MonoBehaviour {
	//public GameObject continueScreen;
	public static BorgManager instance;
	public List<string> borgIds;
	private float timeBorg;
	public bool countingBorgTime;
    public bool borgItemSelected =false;
	private bool borgAdded, hide_borg = true;
    private bool borgVisible;
	private GameObject tela_apresentacao_do_Borg;
	
	public GameObject borg_bg;
	public GameObject BorgList;
	private bool borgSelected;

	void Awake(){
		instance = this;
		borgIds = new List<string>();
		borgSelected = false;
	}

	void Start(){
		tela_apresentacao_do_Borg = GameObject.Find("Tela_apresentacao_do_Borg");	
	}

	// Use this for initialization
	void Update () {
		if (countingBorgTime) {
			timeBorg += Time.deltaTime;
		}


		if(Input.GetKeyDown(KeyCode.H)){
			ShowBorgScreen();
		}

		if (HandCollider2D.handOnButtonTag == "BorgItem" && borgItemSelected == false)
        {
			OnBorgSelected(HandCollider2D.handOnButtonName);//3x
			HandCollider2D.handOnButtonTag = string.Empty;
			//BorgManager.instance.AddBorgToList(HandCollider2D.instance.GetObject().name);            
			//PauseManagerShare.instance.PauseOn();
		}
	}

    public void BorgItemClick(string id)
    {
        OnBorgSelected(id);
    }

	public void ShowBorgScreen(){
		//hide_borg = false;
		this.transform.GetComponent<Animator> ().SetTrigger ("ShowBorg");
		Camera.main.GetComponent<BlurOptimized>().enabled = true;
		countingBorgTime = true;
		int borgFirstTime = PlayerPrefsManager.GetBorgFirstTime();
		borg_bg = GameObject.Find("borg_bg");
		BorgList = GameObject.Find("BorgList");
		if(borgFirstTime == 1){	
			borg_bg.SetActive(false);		
			BorgList.SetActive(false);
			Borg_Presentation.instance.ShowPresentation();
			PlayerPrefsManager.SetBorgFirstTime(0);
		}else{		
			borg_bg.SetActive(true);
			BorgList.SetActive(true);
			try{
				Destroy(Borg_Presentation.instance.gameObject);
			}
			catch(Exception e){
				print(e);
			}
		}

	}

	public void HideBorgScreen(){
			this.transform.GetComponent<Animator> ().SetTrigger ("HideBorg");
			/*if (GameManagerShare.instance.game == Game.Pig || (GameManagerShare.instance.game == Game.Sup && SupManager.instance.startclock )) {
				ShowContinueScreen ();
			}*/
	}

	public void ShowContinueScreen(){
		//continueScreen.transform.GetComponent<Animator> ().SetTrigger ("PauseDown");
	}

	public void HideContinueScreen(){
		//continueScreen.transform.GetComponent<Animator> ().SetTrigger ("PauseUp");
	}
	//adiciona o borg selecionado a uma lista junto com informacoes do round
	public void AddBorgToList(string id){
		string id_player = PlayerPrefsManager.GetPlayerID().ToString();
		string id_game = ((int)GameManagerShare.instance.GetCurrentGame()).ToString(); //PlayerPrefsManager.GetCurrentGameID().ToString();
		string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();
		string borg_id = id;
		string tempo_espera_borg = timeBorg.ToString(FormatConfig.Nfi);
		//TODO: tempo de jogo em q o borg foi escolhido
		string tempo_exato_noRound_borg = Time.timeSinceLevelLoad.ToString(FormatConfig.Nfi);

		string score = ScoreManager.instance.GetScore().ToString();
		//criado por  valdenilson
		string date = System.DateTime.Today.Year + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Day;


		string sb = "@" + id_player + "@" + id_game + "@" + current_match + "@" + borg_id + "@" + tempo_espera_borg + "@" + tempo_exato_noRound_borg + "@" + score;

		

		


		borgIds.Add(sb);



		countingBorgTime = false;
		timeBorg = 0;
	}

	//envia a lista de borgs para o servidor
	public void SendBorg(){
		string folder = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + @"\insertedBorg\";

		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\insertedBorg\"+
			System.DateTime.Today.Day+"-"+System.DateTime.Today.Month+"-"+System.DateTime.Today.Hour+
				"-"+ System.DateTime.Today.Minute+".txt";
		//junta todos os itens da lista numa grande string
		StringBuilder sb = new StringBuilder(); 
		foreach (string st in borgIds) {
			sb.AppendLine(st);			
		}		

		//se o diretoria nao exitir sera criado
		if (!Directory.Exists (folder)) {
			Directory.CreateDirectory (folder);
		}
		
		File.WriteAllText(path, sb.ToString());

		new HttpController().InserBorg (path);

		//limpa a lista em memoria apos o envio
		borgIds.Clear ();
	}

	public void SetBorgAdded(bool value){
		borgAdded = value;
	}

	public bool IsBorgAdded(){
		return borgAdded;
	}

    public void ShowMenu()
    {
		
    }

    public void OnBorgSelected(string id)
	{
        borgItemSelected = true;
        HideBorgScreen();//?
        BorgManager.instance.AddBorgToList(id);
    }

 
   
}

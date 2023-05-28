using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using Ludsgame;
using UnityEngine.UI;

public class InsertMotionTest : MonoBehaviour {

	public static List<string> motionInserts;
	
	void Start(){
		motionInserts = new List<string> ();
		print ("Aperte I para adicionar itens a lista  \n Aperte S para enviar");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			Debug.Log("inseriu");
			AddMotionToList();
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			Debug.Log("Mandou!");
			SendMotionToDatabase();
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			SendRoundToDatabase();
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			AddBorgToList();
		}

		
		if (Input.GetKeyDown (KeyCode.J)) {
			SendBorg();
		}

	}
	private static void AddMotionToList( ){
		string id_player = PlayerPrefsManager.GetPlayerID().ToString();
		string id_game = PlayerPrefsManager.GetCurrentGameID().ToString();
        string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();
		string id_physio = PlayerPrefsManager.GetPhysioID().ToString();
		string id_clinic = PlayerPrefsManager.GetClinicID().ToString();
		string desloc = "0.5";
		string gestureTime = "0.85";
		string sb = "@0.123@0.321@24@0.99@130@2@19@31@0"; // "@" + desloc + "@" + "2" + "@" + gestureTime + "@" + id_player + "@" + id_game + "@" + current_match + "@" + id_physio + "@" + id_clinic;
		print(sb);
		motionInserts.Add(sb);
	}

	public void SendMotionToDatabase(){
		string folder = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + @"\insertedMotion\";
		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedMotion\" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Now.Hour +
			"-" + System.DateTime.Now.Minute + ".txt";
		
		//varre a lista de gestos e gera uma string para ser inserida
		StringBuilder sb = new StringBuilder();

		foreach (string st in motionInserts) {
			sb.AppendLine (st);
			
		}			
		//se o diretoria nao exitir sera criado
		if (!Directory.Exists(folder))
		{
			Directory.CreateDirectory(folder);
		}
		
		//salva no arquivo texto os dados
		System.IO.File.WriteAllText (path, sb.ToString ());
		
		//tenta inserir e retorna o resultado
		HttpController http =  new HttpController();
		http.InsertMotion(path);
		//limpa a lista de gestos
		motionInserts.Clear ();

	}

	public Text debug;

	private void SendRoundToDatabase(){
		string folder = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments) + @"\insertedRound\";
		
		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\insertedRound\" + System.DateTime.Today.Day + "-" + System.DateTime.Today.Month + "-" + System.DateTime.Today.Hour +
			"-" + System.DateTime.Today.Minute + ".txt";
		
		//==== DADOS COMUNS ENTRE OS ROUNDS ==========================
		string roundInfo;
		string id_player = PlayerPrefsManager.GetPlayerID().ToString();
		int id_game = 3;
		string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();	
		string gameTime = ((int)(Time.timeSinceLevelLoad)).ToString(FormatConfig.Nfi);
		string score = "12355";
		//=============================================================
		
		//========================DADOS EXCLUSIVOS DE JOGOS ==========================================
		string applesColetadas = "0";
		string qtdQuedas = "0";
		string tempoInclinado = "0";
		string colisoesPiranha = "0";
		string qtdGoals = "0";
		string qtdDefenses = "0";
		string distanceSup = "0";
		string arremessos = "0";
		string acertos_alvo = "0";
		string erros = "0";
		string qtdCardume = "5";
		string pescados = "3";
		
	
		string data_secao = System.DateTime.Today.Month + "-" + System.DateTime.Today.Day + "-" +System.DateTime.Today.Year;
		string tempo_inc_esquerda = "0"; 
		string tempo_inc_direita = "0"; 
		string trofeus = "0"; 
		string vitorias = "0"; 
		string derrotas = "0";
		string vidas_perdidas = "0";

		//===============================================================================================
		
		// concatenacao de todos os dados exclusivos
		string allExclusiveData = applesColetadas + "@" + qtdQuedas + 
			"@" + colisoesPiranha + "@" + qtdGoals + "@" + qtdDefenses + "@" + distanceSup +
				"@" + arremessos + "@" + acertos_alvo + "@" + erros + "@" + qtdCardume + "@" + pescados + "@" + data_secao + "@" + tempo_inc_esquerda + "@" + tempo_inc_direita +
				"@" + trofeus + "@" + vitorias + "@" + derrotas + "@" + vidas_perdidas;
		
		roundInfo = "@" + current_match + "@" + id_game + "@" + id_player + "@" + score + "@" + tempoInclinado + "@" + gameTime + "@" + allExclusiveData;
		print(roundInfo);
		//debug.text = "round info: " + roundInfo;
		//se o diretoria nao exitir sera criado
		if (!Directory.Exists (folder)) {
			Directory.CreateDirectory (folder);
		}
		
		File.WriteAllText(path, roundInfo);
		
		//bool inserted = new HttpController().InsertRoundTime (roundInfo);
		new HttpController().InsertRoundTime (path);
		//se foi inserido apaga o arquivo
		/*if(inserted){
				System.IO.File.Delete(path);
			}*/
	}

	public List<string> borgIds;
	public void AddBorgToList(){
		string id_player = PlayerPrefsManager.GetPlayerID().ToString();
		string id_game = "2"; //PlayerPrefsManager.GetCurrentGameID().ToString();
		string current_match = PlayerPrefsManager.GetCurrentMatch().ToString();
		string borg_id = "5";
		string tempo_espera_borg = "232";
		//TODO: tempo de jogo em q o borg foi escolhido
		string tempo_exato_noRound_borg = Time.timeSinceLevelLoad.ToString(FormatConfig.Nfi);
		
		string score = "5466";
		
		//string sb = "@" + current_match  + "@" + id_player  + "@" + id_game  + "@" + tempo_exato_noRound_borg  + "@" + borg_id + "@" + score;
		string sb = "@" + id_player + "@" + id_game + "@" + current_match + "@" + borg_id + "@" + tempo_espera_borg + "@" + tempo_exato_noRound_borg + "@" + score;
		
		borgIds.Add(sb);
	}



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
		print(sb.ToString());
		new HttpController().InserBorg (path);
		
		//limpa a lista em memoria apos o envio
		borgIds.Clear ();
	}


}

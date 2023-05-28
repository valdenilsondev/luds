using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;
using System.IO;
using System;
using UnityEngine.SceneManagement;

enum UserType
{
	Physiotherapist = 1,
	Clinic = 2,
	Patient = 3
}

#region Classes
public class Plano
{
	public Plano(){
		this.maquinas_simultaneas = 0;
	}
	public int id_plan { get; set; }
	public string nome_plan { get; set; }
	public int maquinas_simultaneas { get; set; }
	public Decimal valor_aquisicao { get; set; }
	//public int qtd_mensalidades_prepagas { get; set; }
	public int qtd_jogadores { get; set; }
	public int qtd_jogadores_vinculados { get; set; }
	public int qtd_pessoas { get; set; }
	public int qtd_pessoas_vinculadas { get; set; }
	public bool plano_mensal { get; set; }
	public bool desativado { get; set; }
	public string id_asp_net_user { get; set; }
	public Decimal valor_mensal { get; set; }
	public DateTime data_ativacao { get; set; }
	public DateTime data_vencimento { get; set; }
	
	// utilizado somente para comunicação com o frontend
	public bool vencido { get; set; }
	public bool not_found { get; set; }
	public string user_message { get; set; }
}

public class Clinic {
	public Clinic(string token) {
		this.IdClinic = 0;
		this.IdAspNetUser = string.Empty;
		this.NameClinic = string.Empty;
		this.DescClinic = string.Empty;
		this.Token = token;
		this.TokenType = string.Empty;
	}
	
	public Clinic(int idClinic, string idAspNetUser, string nameClinic, string descClinic, string token, string tokenType, int idLicence) {
		this.IdClinic = idClinic;
		this.IdAspNetUser = idAspNetUser;
		this.NameClinic = nameClinic;
		this.DescClinic = descClinic;
		this.Token = token;
		this.TokenType = tokenType;
		this.IdLicence = idLicence;
	}
	
	public int IdClinic { get; set; }
	public string NameClinic { get; set; }
	public string DescClinic { get; set; }
	public string IdAspNetUser { get; set; }
	public int IdLicence { get; set; }
	
	public string Token { get; set; }
	public string TokenType { get; set; }
}

public class Physiotherapist {
	public Physiotherapist(string token) {
		this.IdPhysiotherapist = 0;
		this.NamePhysiotherapist = string.Empty;
		this.DescPhysiotherapist = string.Empty;
		this.Token = token;
		this.TokenType = string.Empty;
	}
	
	public Physiotherapist(int idPhysiotherapist, string idAspNetUser, string namePhysiotherapist, string descPhysiotherapist, string token, string tokenType) {
		this.IdPhysiotherapist = idPhysiotherapist;
		this.IdAspNetUser = idAspNetUser;
		this.NamePhysiotherapist = namePhysiotherapist;
		this.DescPhysiotherapist = descPhysiotherapist;
		this.Token = token;
		this.TokenType = tokenType;
	}
	
	public int IdPhysiotherapist { get; set; }
	public int IdTitularPatient { get; set; }
	public string NamePhysiotherapist { get; set; }
	public string DescPhysiotherapist { get; set; }
	public string IdAspNetUser { get; set; }
	public string crefito     { get; set; }
	public int tipo  { get; set; }
	public int ativo   { get; set; }
	public bool liberal   { get; set; }

	public string Token { get; set; }
	public string TokenType { get; set; }
	
	
}

public class Patient {
	public Patient() {
		this.IdPatient = 0;
		this.NamePatient = string.Empty;
		//	this.DescPatient = string.Empty;
		this.IdClinic = 0;
		this.IdPhysiotherapist = 0;
	}
	
	public Patient(int idPatient, string namePatient, int idClinic, int idPhysiotherapist, string sexo, string data_avaliacao_inicial,
	               string profissao, string endereco, string observacoes, string telefone, string aspNetUser
	               /* float lateralAmountAnalog, float lateralAmountDigital, float handSensibility, float movingSideAmount, float jumpAmountNeeded, float rollAmountNeeded,
	               float initialMapSpeed, float mapSpeedLimit, int incrementSpeedInterval, int sideToMove, int playerDeficiency, string miniGames*/) {
		this.IdPatient = idPatient;
		this.NamePatient = namePatient;
		//this.DescPatient = descPatient;
		this.IdClinic = idClinic;
		this.IdPhysiotherapist = idPhysiotherapist;
		
		this.sexo = sexo;
		this.data_avaliacao_inicial = data_avaliacao_inicial;
		this.profissao = profissao;
		this.endereco = endereco;
		this.observacoes = observacoes;
		this.telefone = telefone;
		this.aspNetUser = aspNetUser;
	}
	
	public int IdPatient { get; set; }
	public string NamePatient { get; set; }
	//public string DescPatient { get; set; }
	public int IdClinic { get; set; }
	public int IdPhysiotherapist { get; set; }
	public string sexo  { get; set; }
	public string data_avaliacao_inicial { get; set; }
	public string profissao { get; set; }
	public string endereco { get; set; }
	public string observacoes { get; set; }
	public string telefone { get; set; }
	public string aspNetUser { get; set; }
	public string Token { get; set; }

	
	public float LateralAmountAnalog { get; set; }
	public float LateralAmountDigital { get; set; }
	public float HandSensibility { get; set; }
	public float MovingSideAmount { get; set; }
	public float JumpAmountNeeded { get; set; }
	public float RollAmountNeeded { get; set; }
	public float InitialMapSpeed { get; set; }
	public float MapSpeedLimit { get; set; }
	public int IncrementSpeedInterval { get; set; }
	public int SideToMove { get; set; }
	public int PlayerDeficiency { get; set; }
	public string MiniGames { get; set; }
	public int Current_pig_level_group { get; set; }
	public int tutorial_pig_runner { get; set; }
	public int current_pig_group_type {get; set;}
	//add variavel nova
}

public class Computer {
	//3E9E396325B0323DB8D6D6EA76313BE0
	public Computer() {
		this.IdComputer = 0;
		this.DescComputer = string.Empty;
		this.NameComputer = string.Empty;
	}
	
	public Computer(int idComputer, string descComputer, string nameComputer, string macComputer, string idApsNet) {
		this.IdComputer = idComputer;
		this.DescComputer = descComputer;
		this.NameComputer = nameComputer;
		this.MacComputer = macComputer;
		this.IdAspNet = idApsNet;
	}
	
	public int IdComputer { get; set; }
	public string DescComputer { get; set; }
	public string NameComputer { get; set; }
	public string MacComputer { get; set; }
	public string IdAspNet { get; set; }
	
}
#endregion


public class HttpController : MonoBehaviour {
	
	public InputField usernameClinic, passwordClinic;
	public InputField descriptionPc;
	public InputField usernamePhysio, passwordPhysio;
	public InputField username, password;
	
	public GameObject clinicScreen, computerScreen, physioScreen, errorScreen, updateScreen, mustUpdateScreen, computerListScreen, computerAddFromClinicScreen, computerAddScreen, computerRemovedScreen, computerNotRemovedScreen, physioListScreen;
	
	private string clinicToken, clinicTokenType;
	private Clinic clinic;
	private Physiotherapist physiotherapist;
	private Computer computer;
	private Patient patient;
	private Plano plano;
	private string errorMsg = "$msg:error$";
	private string authorizationMsg = "Authorization has been denied for this request.";
	public bool isAnonymous = false;
	
	private string token;
	private string tokenType;
	
	public static string url = "https://ludsbackend.azurewebsites.net";
	//private static string url = "http://localhost:5003";
	public static string urlWeb = "http://web.ludsgames.com.br";
	
	public GameObject pc_prefab, physio_prefab;
	private Vector3 last_pc_pos;
	private static int pcToDeleteIndex;
	private static string physioToSelectUser;
	private List<GameObject> pcList, physioList;
	private string appVersion = "1.5.0";
	private string linkToDownload;
	public GameObject load_icon;
	private bool showGUI;

	void Awake()
	{
		clinic = new Clinic(PlayerPrefsManager.GetClinicToken());
		physiotherapist = new Physiotherapist(PlayerPrefsManager.GetPhysiotherapistToken());
		patient = new Patient();
		computer = new Computer();
		plano = new Plano();
		
		if (SceneManager.GetActiveScene().name == "Http")
		{
			InitScreens();
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.F7)){
			showGUI = !showGUI;
		}
	/*	if (Input.GetKeyDown (KeyCode.F10)) {
			//limpa a pilha do log
			myLogStack.Clear ();
			myLog = "";
		}*/
	}

	private float largura = 600;
	private float altura = 600;

	private float vScrollbarValue;
	public Vector2 scrollPosition = Vector2.zero;
	private string logText = "hue";

	public float vSbarValue;

	string myLog=" ";

	void OnGUI(){
		if(showGUI){
			//pigrunner
			GUI.Box(new Rect(10,10,200,300), "PigRunner");
			GUI.Label(new Rect(10,25,200,90), "InitialMapSpeed: " + PlayerPrefsManager.GetInitialMapSpeed().ToString());
			GUI.Label(new Rect(10,40,200,90), "PigRunnerGroupType: " + PlayerPrefsManager.GetPigRunnerGroupType().ToString());
			GUI.Label(new Rect(10,55,200,90), "PigRunnerLevelGroup: " + PlayerPrefsManager.GetPigRunnerLevelGroup().ToString());
			GUI.Label(new Rect(10,70,200,90), "BorgTime: " + PlayerPrefsManager.GetBorgTime().ToString());
			GUI.Label(new Rect(10,85,200,90), "Jump1: " + PlayerPrefsManager.GetJump1().ToString());
			GUI.Label(new Rect(10,100,200,90), "Jump2:" + PlayerPrefsManager.GetJump2().ToString());
			GUI.Label(new Rect(10,115,200,90), "MoveLeft: " + PlayerPrefsManager.GetMoveLeft().ToString());
			GUI.Label(new Rect(10,130,200,90), "MoveRight: " + PlayerPrefsManager.GetMoveRight().ToString());
			GUI.Label(new Rect(10,145,200,90), "Squat: " + PlayerPrefsManager.GetSquat().ToString());
			GUI.Label(new Rect(10,160,200,90), "IdPerfilPigrunner: " + PlayerPrefsManager.GetIdPerfilPigrunner().ToString());

			//goalkeeper
			GUI.Box(new Rect(220,10,200,300), "Goalkeeper");
			GUI.Label(new Rect(220,25,200,90), "BarSpeed: " + PlayerPrefsManager.GetBarSpeed().ToString());
			GUI.Label(new Rect(220,40,200,90), "TimeToShoot " + PlayerPrefsManager.GetTimeToShoot().ToString());
			GUI.Label(new Rect(220,55,200,90), "GoalsLimit: " + PlayerPrefsManager.GetGoalsLimit().ToString());
			GUI.Label(new Rect(220,70,200,90), "DefenseLimit: " + PlayerPrefsManager.GetDefenseLimit().ToString());
			GUI.Label(new Rect(220,85,200,90), "IdPerfilGK: " + PlayerPrefsManager.GetIdPerfilGK().ToString());

			//SUP
			GUI.Box(new Rect(430,10,200,300), "SUP");
			GUI.Label(new Rect(430,25,200,90), "RemaingTime: " + PlayerPrefsManager.GetRemaingTime().ToString());
			GUI.Label(new Rect(430,40,200,90), "RemarLeft " + PlayerPrefsManager.GetRemarLeft().ToString());
			GUI.Label(new Rect(430,55,200,90), "RemarRight: " + PlayerPrefsManager.GetRemarRight().ToString());
			GUI.Label(new Rect(430,70,200,90), "IdPerfilSup: " + PlayerPrefsManager.GetIdPerfilSup().ToString());
		
			//Throw
			GUI.Box(new Rect(640,10,200,300), "Throw");
			GUI.Label(new Rect(640,25,200,90), "PlaqueTime: " + PlayerPrefsManager.GetPlaqueTime().ToString());
			GUI.Label(new Rect(640,40,200,90), "RemaingtimeThrow " + PlayerPrefsManager.GetRemaininigtimethrow().ToString());
			GUI.Label(new Rect(640,55,200,90), "IdPerfilThrow: " + PlayerPrefsManager.GetIdPerfilThrow().ToString());
		
			//Fishing
			GUI.Box(new Rect(850,10,200,300), "Fishing");
			GUI.Label(new Rect(850,25,200,90), "ThrowRight: " + PlayerPrefsManager.GetThrowRight().ToString());
			GUI.Label(new Rect(850,40,200,90), "FishingDifficult " + PlayerPrefsManager.GetFishingDifficult().ToString());
			GUI.Label(new Rect(850,55,200,90), "IdPerfilFishing " + PlayerPrefsManager.GetIdPerfilFishing().ToString());
			GUI.Label(new Rect(850,70,200,90), "NumberOfBaits " + PlayerPrefsManager.GetNumberofBaits().ToString());
			GUI.Label(new Rect(850,85,200,90), "FishingQuantity: " + PlayerPrefsManager.GetFishingQuantity().ToString());
			
			//Bridge
			GUI.Box(new Rect(1060,10,200,300), "Bridge");
			GUI.Label(new Rect(1060,25,200,90), "SetLateralSensibilit: " + PlayerPrefsManager.GetLateralSensibilit().ToString());
			GUI.Label(new Rect(1060,40,200,90), "SetBridgeBoost " + PlayerPrefsManager.GetBridgeBoost().ToString());
			GUI.Label(new Rect(1060,55,200,90), "SetIdPerfilBridge " + PlayerPrefsManager.GetIdPerfilBridge().ToString());


			GUI.Box(new Rect(10,320,220,300), "Outras preferencias");
			GUI.Label(new Rect(10,335,220,90), "IsUsingKinect: " + PlayerPrefsManager.GetUsingKinect().ToString());
			GUI.Label(new Rect(10,350,220,90), "UserType: " + ut.ToString());
			GUI.Label(new Rect(10,365,220,90), "Circle Enable (0-false  1-true): " + PlayerPrefsManager.GetIsCircleOn());


		/*	vScrollbarValue = GUI.VerticalScrollbar(new Rect(400, 300, 100, 30), vScrollbarValue, 1.0f, 0.0f, 100.0f);
			scrollPosition = GUI.BeginScrollView (new Rect (400, 350, largura , 300), scrollPosition, new Rect (0, 0, 190, 400));
			logText = GUI.TextArea (new Rect (0, 0, largura , altura), myLog);
			GUI.EndScrollView ();*/
		}	
	}
		
	/*Stack myLogStack = new Stack();

	void OnEnable () {
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable () {
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type){
		myLog = logString;
		string newString = "\n [" + type + "] : " + myLog;
		if(!myLogStack.Contains(newString))
			myLogStack.Push(newString);
		if (type == LogType.Exception)
		{
			newString = "\n" + stackTrace;
			if(!myLogStack.Contains(newString))
				myLogStack.Push(newString);
		}
		myLog = string.Empty;
		foreach(string mylog in myLogStack){
			myLog += mylog;
		}
	}*/

	
	private void InitScreens() {
		/*if(!clinic.Token.Equals("0") && !isAnonymous) {
			LoginClinic();
		} else {
			clinicScreen.SetActive(true);
		}*/
		//clinicScreen.SetActive(true);
		
		LoginAuto();
		//CheckVersion();
	}
	
	#region Clinic Methods
	public void SignUpClinic() {
		
	}

	private void CheckVersion(string token){
		Hashtable data = new Hashtable ();
		data.Add("name",appVersion);

		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/gameversion/get", data);
		someRequest.AddHeader("Authorization", "Bearer " + token);
		print(token);
		someRequest.Send( ( request ) => {
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing))
			{
				if (thing["new_version"].b)
				{
					if (thing["must_update"].b)
					{
						print("must_update");
						ShowMustUpdateScreen(thing["user_message"].str, thing["link"].str);
					}
					else
					{
						if (updateScreen)
						{
							ShowUpdateScreen(thing["user_message"].str, thing["link"].str);
							print("showUpdateScreen");
						}
						else
						{
							GetPlanoByIdUser(token);
						}
					}
				}
				else
				{
					GetPlanoByIdUser(token);
				}
			}
			else
			{
				ShowErrorScreen(thing["user_message"].str);
			}
		});

	}

	public void Login() { 
		WWWForm form = new WWWForm();
		form.AddField( "userName", username.text );
		form.AddField( "password", password.text );
		form.AddField( "grant_type", "password" );
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/token", form );
		load_icon.SetActive(true);
		someRequest.Send( ( request ) => {
			if(request.response !=null){
				JSONObject thing = new JSONObject( request.response.Text );
				load_icon.SetActive(false);
				if(thing.HasField("access_token")) {
					//GetPlanoByIdUser(thing["access_token"].str);
					token = thing["access_token"].str;
					PlayerPrefsManager.SetMainToken(token);
					CheckVersion(thing["access_token"].str);
				} else {
					ShowErrorScreen(thing["error_description"].str);
				}
			}else{
				load_icon.SetActive(false);
				ShowErrorScreen("Erro de conexão");
			}
		});
	}

	public void LoginAuto() { 
		WWWForm form = new WWWForm();
		form.AddField( "userName", PlayerPrefsManager.GetUser());
		form.AddField( "password", PlayerPrefsManager.GetPassword());
		form.AddField( "grant_type", "password" );
		load_icon.SetActive(true);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			if(request.response !=null){
				JSONObject thing = new JSONObject( request.response.Text );
				load_icon.SetActive(false);
				if(thing.HasField("access_token")) {
					token = thing["access_token"].str;
					PlayerPrefsManager.SetMainToken(token);
					CheckVersion(thing["access_token"].str);

				} else {
					clinicScreen.SetActive (true);
				}
			}else{
				clinicScreen.SetActive (true);
				load_icon.SetActive(false);
				ShowErrorScreen("Erro de conexão");
			}

		});
	}



	public void LoginPhysioClinic() { 
		WWWForm form = new WWWForm();
		form.AddField( "userName", usernamePhysio.text );
		form.AddField( "password", passwordPhysio.text );
		form.AddField( "grant_type", "password" );
		load_icon.SetActive(true);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );

			if(thing.HasField("access_token")) {
				physiotherapist.Token = thing["access_token"].str;
				//TODO: Getphysioterapeuta().. dentro dele tem o getpatienet()
				GetPhysiotherapistClinic();
			} else {
				load_icon.SetActive(false);
				ShowErrorScreen(thing["error_description"].str);
			}
		});
	}

	public void GetPlanoByIdUserFromBtn(){
		GetPlanoByIdUser(token);
	}

	public void GetUserType(string token){	
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/TipoUsuario/ObterTipoUsuario");
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.AddHeader("Content-Length", "0");

		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
		
			Debug.Log (request.response.Text);
			
			if(CheckRequest(thing)) {
				ut = (UserType)(thing["tipoUsuario"].n);
				switch(ut){
				case UserType.Clinic:				
					clinic.Token = token;
					PlayerPrefsManager.SetClinicToken(token);
					SaveLogin();
					ComputerRegistration(clinic.Token);
					break;
				case UserType.Patient:
					patient.Token = token;
					ComputerRegistration(patient.Token);			
					break;
				case UserType.Physiotherapist:
					PlayerPrefsManager.SetPhysiotherapistToken(token);
					physiotherapist.Token = token;
					GetPhysiotherapist();
					break;
				}
			}else{
				ShowErrorScreen(request.response.Text);
			}
		});
	}

	private void SaveLogin(){
		if(username.text != ""){
			PlayerPrefsManager.SetUser(username.text);
			PlayerPrefsManager.SetPassword(password.text);
		}
	}

	public void ClearLogin(){
		PlayerPrefsManager.ClearLogin();
	}

	public void CallListAllPhysioFromBtn(){
		ListAllPhysioById(clinic.Token);
	}

	private void ListAllPhysioById(string token){
		physioListScreen.SetActive(true);
		physioList = new List<GameObject>();
		HTTP.Request someRequest = new HTTP.Request("post", $"{url}/api/Clinic/GetFisioterapeutas");
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.AddHeader("Content-Length", "0");
		someRequest.Send( ( request ) => {
			JSONObject thing = new JSONObject( request.response.Text);
			int size = thing.Count;
			for (int i = 0; i<size; i++){
				GameObject physio = Instantiate(physio_prefab); 
				physio.transform.SetParent(GameObject.Find("physio_itens_container").transform);
				physio.transform.localScale = new Vector3(1,1,1);
				physioList.Add(physio);
				//preencher os valores dos campos
				physio.transform.GetChild(0).GetComponent<Text>().text = thing.list[i].GetField("namePhysiotherapist").str;
				physio.transform.GetChild(1).GetComponent<Text>().text = thing.list[i].GetField("login").str;
				physio.transform.GetChild(2).GetComponent<Text>().text = thing.list[i].GetField("crefito").str;
				
				physio.GetComponent<PhysioOnList>().thisPhysioUser  = thing.list[i].GetField("login").str;						
			}
		});
	}

	public void SelectPhysio(){
		ClearPhysioList();
		usernamePhysio.text = physioToSelectUser;
		usernamePhysio.enabled = false;
	}

	public void ClearPhysioList(){
		foreach( GameObject p in physioList){
			Destroy(p.gameObject);
		}
	}

	public void GetPlanoByIdUser(string token){	
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/Plano/ObterByIdUser");
		UserType ut;
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.AddHeader("Content-Length", "0");
		
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			Debug.Log (request.response.Text);
			
			if(CheckRequest(thing)) {
				if(thing["vencido"].b || thing["not_found"].b){
					ShowErrorScreen(thing["user_message"].str); 
				}else{
					plano.maquinas_simultaneas = (int)(thing["maquinas_simultaneas"].n);			
					GetUserType(token);
				}
			}else{
				ShowErrorScreen("ERRO");
			}
		});		
	}

	public void LoginClinic() {
		WWWForm form = new WWWForm();
		form.AddField( "userName", usernameClinic.text );
		form.AddField( "password", passwordClinic.text );
		form.AddField( "grant_type", "password" );
		
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/token", form );
		//	HTTP.Request someRequest = new HTTP.Request( "post", "http://192.168.0.46:5003/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
					Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );//descomentei valdenilson
			
			if (thing.HasField("access_token")) {
				clinic.Token = thing["access_token"].str;
				clinic.TokenType = thing["token_type"].str;
				
				GetClinic();
				/*	string machineName = System.Environment.MachineName;
				string desc ="";
				string mac = GetNetworkInterfaces();
				string idApsNet;
				RegisterComputer(machineName + desc + mac + idApsNet);*/
			} else {
				errorScreen.SetActive(true);
				Debug.Log("Sem objeto: " + request.response.Text );
			}
			
		});
	}
	private void RegisterMotion (string insert) {
		
		
		
	}
	
	private void GetClinic() {
		HTTP.Request someRequest = new HTTP.Request( "get", $"{url}/api/clinic/get" );
		//HTTP.Request someRequest = new HTTP.Request( "get", "http://192.168.0.46:5003/api/clinic/get" );
		someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log ("Int: " + (int)thing["id_Clinic"].n);
			//Debug.Log ("String: " + thing["id_Clinic"].str);
			Debug.Log (request.response.Text);
			if(CheckRequest(thing)) {
				clinic.IdClinic = (int)thing["id_Clinic"].n;
				clinic.NameClinic = thing["clinicName"].str;
				clinic.DescClinic = thing["desc_Clinic"].str;
				clinic.IdAspNetUser = thing["id_Asp_Net_User"].str;
				clinic.IdLicence = (int)thing["id_Licence"].n;
				PlayerPrefsManager.SetClinicToken(clinic.Token);
				
				ComputerRegistration(clinic.Token);
				
			} else {
				ShowErrorScreen("ERRO AO OBTER CLINICA");
			}
		});
	}
	#endregion
	
	#region Computer Methods
	public void InsertMotion(string motionData) {
		//bool inserted = false;
		Hashtable data = new Hashtable ();
		string fileContent = File.ReadAllText(motionData);
		//string motionData= "testando!";
		data.Add("MotionUnity",fileContent);
		
		HTTP.Request someRequest = new HTTP.Request ("post", $"{url}/api/motion/register", data);
		string token = PlayerPrefsManager.GetMainToken();
		someRequest.AddHeader("Authorization", "Bearer " + token);
		
		someRequest.Send( ( request ) => {
			try{
				JSONObject thing = new JSONObject( request.response.Text );
				Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );//descomentwei valdenilson
				if(CheckRequest(thing)) {								
					//se foi inserido apaga o arquivo				
					//System.IO.File.Delete(motionData);
					Debug.Log ("Insert Works!");
				} else {
					//inserted = false;
					Debug.Log ("Insert DOESNT WORK!");					
				}
			}			
			catch(Exception e){
			//inserted = false;
								print(e.Message);			
			}		
		});
		
	}
	public static void InsertLog(string motionData) {
		Hashtable data = new Hashtable ();
		//string motionData= "testando!";
		data.Add("MotionUnity",motionData);
		
		HTTP.Request someRequest = new HTTP.Request ("post", $"{url}/api/log/register", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + PlayerPrefsManager.GetMainToken());
		someRequest.Send( ( request ) => {
		});
	}
	
	public void InsertRoundTime(string timeData) {
		Hashtable data = new Hashtable ();
		string fileContent = File.ReadAllText(timeData);
		data.Add("roundunity",fileContent);
		HTTP.Request someRequest = new HTTP.Request ("post", $"{url}/api/round/registertime", data);
		//HTTP.Request someRequest = new HTTP.Request ("post", "http://localhost:5003/api/round/insert", data);
		someRequest.AddHeader("Authorization", "Bearer " + PlayerPrefsManager.GetMainToken());
		someRequest.Send( ( request ) => {
			try{
				JSONObject thing = new JSONObject( request.response.Text );
				Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );//descomentei valdenilson
				if(CheckRequest(thing)) {								
					//se foi inserido apaga o arquivo				
					//System.IO.File.Delete(timeData);
					Debug.Log ("Insert Round Works!");
				} else {
					Debug.Log ("Insert Round DOESNT WORK!");					
				}
			}
			
			catch(Exception e){		
					print(e.Message);				
			}	
		});
	}
	
	public void InserBorg(string borgData) {
		Hashtable data = new Hashtable ();
		


		string fileContent = File.ReadAllText(borgData);
		data.Add("roundunity", fileContent);

		HTTP.Request someRequest = new HTTP.Request ("post", $"{url}/api/round/registerborg", data);

		//HTTP.Request someRequest = new HTTP.Request("post", "http://localhost:5432/api/round/registerborg", data);

	someRequest.AddHeader("Authorization", "Bearer " + PlayerPrefsManager.GetMainToken());

		someRequest.Send((request) => {
			try {
				JSONObject thing = new JSONObject(request.response.Text);

				Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n);//descomentado valdenilson
				if (CheckRequest(thing)) {
					//se foi inserido apaga o arquivo				
					//System.IO.File.Delete(borgData);
					//	Debug.Log ("Insert Borg Works!");
				}
				else {
					Debug.Log("Insert Borg DOESNT WORK!");
				}
			}

			catch (Exception e) {
				print(e.Message);
			}
		});
	}
	
	public void DisablePigTutorial() {
		Hashtable data = new Hashtable ();
		PlayerPrefsManager.SetPigTutorial(0);
		data.Add("IdPatient", PlayerPrefsManager.GetPlayerID());
		
		HTTP.Request someRequest = new HTTP.Request ("post", $"{url}/api/patient/DisablePigTutorial", data);
		someRequest.AddHeader("Authorization", "Bearer " + PlayerPrefsManager.GetMainToken());
		someRequest.Send( ( request ) => {
			try{
				JSONObject thing = new JSONObject( request.response.Text );
				//Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
				if(CheckRequest(thing)) {								
					//se foi inserido apaga o arquivo				
					//					System.IO.File.Delete(borgData);
					Debug.Log ("Disable tutorial Works!");
				} else {
					Debug.Log ("Disable tutorial DOESNT WORK!");					
				}
			}
			
			catch(Exception e){		
				print(e.Message);				
			}	
		});
	}
	public void ComputerRegistration(string token) {
		Hashtable data = new Hashtable ();

		data.Add ("MacComputer", GetNetworkInterfaces());
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/computer/get", data );
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if(CheckRequest(thing)) {
				CheckComputers(token);
			} else {
				ShowErrorScreen("ERRO AO RETORNAR COMPUTADOR");
			}
			
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	private UserType ut;


	public void InsertComputer() {
		//computerScreen.SetActive(true);
		Hashtable data = new Hashtable ();	
		data.Add ("NameComputer", System.Environment.MachineName);
		data.Add ("DescComputer", descriptionPc.text);
		data.Add ("MacComputer", GetNetworkInterfaces());
		string IdAspNetUser = "0";
		string token = "0";
		switch(ut){
		case UserType.Clinic:
			IdAspNetUser = clinic.IdAspNetUser;
			token = clinic.Token;
			break;
		case UserType.Patient:
			IdAspNetUser = patient.aspNetUser;
			token = patient.Token;
			break;
		case UserType.Physiotherapist:
			token = physiotherapist.Token;
			IdAspNetUser = physiotherapist.IdAspNetUser;
			break;
		}

		data.Add ("IdAspNet", IdAspNetUser);
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/computer/insert", data );
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
			if(CheckRequest(thing)) {
				//	physioScreen.SetActive(true);
				ShowComputerAddedScreen();

				computerScreen.SetActive(false);
				Debug.Log ("Insert Works!");
			} else {
				ShowErrorScreen(request.response.Text);
			}
			
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

	private void ShowComputerAddedScreen(){
		if(ut == UserType.Clinic){
			computerAddFromClinicScreen.SetActive(true);
		}else{
			computerAddScreen.SetActive(true);
		}
	}
	
	public void CheckComputers(string token){
		pcList = new List<GameObject>();
		int qtdComputers = 0;
		int computersLimit = plano.maquinas_simultaneas;
			
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/Computer/GetAllById");		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.AddHeader("Content-Length", "0");

		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			Debug.Log (request.response.Text);
			
			if(CheckRequest(thing)) {
				int size = thing.Count;
				qtdComputers = size;
				bool computerFounded = true;
			/*	for( int i = 0; i< size; i++){
					if(thing.list[i].GetField("macComputer").str.Contains(GetNetworkInterfaces())){
						computerFounded = true;
					}
				}*/
				
				if(qtdComputers == computersLimit && !computerFounded){
					
					computerListScreen.SetActive(true);
					
					for (int i = 0; i<size; i++){
						GameObject pc = Instantiate(pc_prefab); 
						pc.transform.SetParent(GameObject.Find("pc_itens_container").transform);
						pc.transform.localScale = new Vector3(1,1,1);
						pcList.Add(pc);
						//preencher os valores dos campos
						pc.transform.GetChild(0).GetComponent<Text>().text = thing.list[i].GetField("nameComputer").str;
						pc.transform.GetChild(1).GetComponent<Text>().text = thing.list[i].GetField("descComputer").str;
						pc.transform.GetChild(2).GetComponent<Text>().text = thing.list[i].GetField("macComputer").str;
						
						pc.GetComponent<ComputerOnList>().thisPcIndex  = (int) thing.list[i].GetField("idComputer").n;						
					}
				} 
				 else{
					if(qtdComputers < computersLimit && !computerFounded){
						computerScreen.SetActive(true);
					}else{	
						if(ut == UserType.Clinic){
							ListAllPhysioById(token);
						}else{
							GetPatient();
						}
					}
				}
			}else{
				ShowErrorScreen("ERRO AO OBTER LISTA DE COMPUTADORES");
			}
		});
	}

	private string sceneToCall = "Calibration";

	public void TryToLoadCalibrationScreen(){
		print("physiotherapist.titular_patient " + physiotherapist.IdTitularPatient);
		print("patient.IdPatientt " + patient.IdPatient);
		switch(ut){
		case UserType.Clinic:
			if(physiotherapist.IdTitularPatient == 0 || physiotherapist.IdTitularPatient == null){
				ShowErrorScreen("PACIENTE NAO ENCONTRADO");
			}else{
				SceneManager.LoadScene(sceneToCall);
			}
			break;
		case UserType.Patient:
			if(patient.IdPatient == 0 || patient.IdPatient == null){
				ShowErrorScreen("PACIENTE NAO ENCONTRADO");
			}else{
				SceneManager.LoadScene(sceneToCall);
			}
			break;
		case UserType.Physiotherapist:
			if(physiotherapist.IdTitularPatient == 0 || physiotherapist.IdTitularPatient == null){
				ShowErrorScreen("PACIENTE NAO ENCONTRADO");
			}else{
				SceneManager.LoadScene(sceneToCall);
			}
			break;
		}
	}

	public void RemoveComputerFromList(){
		Hashtable data = new Hashtable ();	
		data.Add ("IdComputer", pcToDeleteIndex);
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/Computer/Remove",data );
		string token = "0";
		switch(ut){
		case UserType.Clinic:
			token = clinic.Token;
			break;
		case UserType.Patient:
			token = patient.Token;
			break;
		case UserType.Physiotherapist:
			token = physiotherapist.Token;
			break;
		}
		someRequest.AddHeader("Authorization", "Bearer " + token);

		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			Debug.Log (request.response.Text);
			
			if(CheckRequest(thing)) {
				computerListScreen.SetActive(false);
				computerRemovedScreen.SetActive(true);
				foreach( GameObject p in pcList){
					Destroy(p.gameObject);
				}
				
			}else{
				computerListScreen.SetActive(false);
				computerNotRemovedScreen.SetActive(true);
			}
		});
	}
	
	public static void SelectComputerFromList(int index){
		pcToDeleteIndex = index;
	}

	public static void SelectPhysioFromList(string user){
		physioToSelectUser = user;
		print("fisio " + physioToSelectUser);
	}

	public void CloseApplication(){
		Application.Quit();
	}
	
	#endregion
	
	#region Physiotherapist
	public void SignUpPhysiotherapist() {
		
	}
	
/*	public void LoginPhysiotherapist() {
		WWWForm form = new WWWForm();
		form.AddField( "userName", usernamePhysio.text );
		form.AddField( "password", passwordPhysio.text );
		form.AddField( "grant_type", "password" );
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );
			
			if(thing.HasField("access_token")) {
				physiotherapist.Token = thing["access_token"].str;
				physiotherapist.TokenType = thing["token_type"].str;
				PlayerPrefsManager.SetPhysiotherapistToken(physiotherapist.Token);
				GetPhysiotherapist();
			} else {
				errorScreen.SetActive(true);
			}
			
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}*/
	
	public void GetPhysiotherapist(){
		SendLogs();
		HTTP.Request someRequest = new HTTP.Request( "get", $"{url}/api/physiotherapist/get" );
		
		someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				physiotherapist.IdPhysiotherapist = (int)thing["idPhysiotherapist"].n;
				physiotherapist.NamePhysiotherapist = thing["namePhysiotherapist"].str;
				physiotherapist.DescPhysiotherapist = thing["descPhysiotherapist"].str;
				physiotherapist.IdAspNetUser = thing["idAspNetUser"].str;
				physiotherapist.IdTitularPatient = (int)thing["idNextPatient"].n;
				PlayerPrefsManager.SetPlayerID(physiotherapist.IdTitularPatient);
				physiotherapist.crefito = thing["crefito"].str;
				physiotherapist.tipo = (int)thing["tipo"].n;
				physiotherapist.ativo = (int)thing["ativo"].n;
				physiotherapist.liberal = thing["liberal"].b;

				if(physiotherapist.liberal){
					if(physiotherapist.IdTitularPatient == 0 || physiotherapist.IdTitularPatient == null){
						ShowErrorScreen(thing["user_message"].str);
					}else{
						ComputerRegistration(physiotherapist.Token);
					}
				}else{
					ShowErrorScreen(thing["user_message"].str);
				}
			
			} else {
				ShowErrorScreen("ERRO AO OBTER FISIOTERAPEUTA");
			}
			
			Debug.Log("GetPhysiotherapist: " + request.response.Text );
		});
	}

	public void GetPhysiotherapistClinic(){
		SendLogs();
		HTTP.Request someRequest = new HTTP.Request( "get", $"{url}/api/physiotherapist/get" );
		print(physiotherapist.Token);
		someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				physiotherapist.IdPhysiotherapist = (int)thing["idPhysiotherapist"].n;
				physiotherapist.NamePhysiotherapist = thing["namePhysiotherapist"].str;
				physiotherapist.DescPhysiotherapist = thing["descPhysiotherapist"].str;
				physiotherapist.IdAspNetUser = thing["idAspNetUser"].str;
				physiotherapist.IdTitularPatient = (int)thing["idNextPatient"].n;
				PlayerPrefsManager.SetPlayerID(physiotherapist.IdTitularPatient);
				physiotherapist.crefito = thing["crefito"].str;
				physiotherapist.tipo = (int)thing["tipo"].n;
				physiotherapist.ativo = (int)thing["ativo"].n;
				physiotherapist.liberal = thing["liberal"].b;
				
				if(!physiotherapist.liberal){
					GetPatient();
				}else{
					ShowErrorScreen(thing["user_message"].str);
				}
				
			} else {
				ShowErrorScreen("ERRO AO OBTER FISIOTERAPEUTA");
			}
			
			Debug.Log("GetPhysiotherapist: " + request.response.Text );
		});
	}
	#endregion
	
	#region Patient Methods
	
	public void GetPatient() {
		Hashtable data = new Hashtable ();
		string id;
		HTTP.Request someRequest;
		load_icon.SetActive(true);
		switch(ut){		
		case UserType.Patient:
			someRequest = new HTTP.Request( "post", $"{url}/api/Patient/ObterByIdUser");	
			someRequest.AddHeader("Authorization", "Bearer " + patient.Token);
			someRequest.AddHeader("Content-Length", "0");
			break;
		case UserType.Physiotherapist:
			id = physiotherapist.IdTitularPatient.ToString();
			data.Add ("IdPatient", id);
			someRequest = new HTTP.Request( "post", $"{url}/api/patient/get", data);	
			someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
			break;
		case UserType.Clinic:
			id = physiotherapist.IdTitularPatient.ToString();
			data.Add ("IdPatient", id);
			someRequest = new HTTP.Request( "post", $"{url}/api/patient/get", data);	
			someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
			break;
		default:
			someRequest = new HTTP.Request( "post", $"{url}/api/Patient/ObterByIdUser");	
			someRequest.AddHeader("Authorization", "Bearer " + patient.Token);
			someRequest.AddHeader("Content-Length", "0");
			break;
		}
				
			someRequest.Send( ( request ) => {				
				JSONObject thing = new JSONObject( request.response.Text );
				
				if (CheckRequest(thing)) {
					patient.IdPatient = (int)thing["idPatient"].n;
					patient.NamePatient = thing["nome"].str;
					patient.IdClinic = (int)thing["id_clinic"].n;
					patient.IdPhysiotherapist = (int)thing["id_physio"].n;
					patient.sexo  = thing["sexo"].str;
					patient.data_avaliacao_inicial = thing["data_avaliacao_inicial"].str;
					patient.endereco = thing["endereco"].str;
					patient.observacoes = thing["observacoes"].str;
					patient.telefone = thing["telefone"].str;
					patient.aspNetUser = thing["id_asp_net_user"].str;
				if(!thing["jogaremcasa"].b && ut == UserType.Patient){
					ShowErrorScreen(thing["user_message"].str);
				}else{
					LoadPatient();

				}
					
				} 
				
				Debug.Log("Sem objeto: " + request.response.Text );
			});

	}
	
	
	public void GetPerfilByIdPatientPigRunner() {
		Hashtable data = new Hashtable ();
		string id = patient.IdPatient.ToString();
		print("idpatiente " + patient.IdPatient);
		PlayerPrefsManager.SetPlayerID(patient.IdPatient);
		data.Add ("IdPatient", id);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/PerfilPigRunner/GetPerfilByIdPatient", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
			//	Debug.Log ("thing");
				float velporco = (float)thing["velocidadePorco"].n;
				PlayerPrefsManager.SetInitialMapSpeed(velporco);
				PlayerPrefsManager.SetPigRunnerGroupType((int)thing["ladoPreferencial"].n);

				PlayerPrefsManager.SetPigRunnerLevelGroup((int)thing["qtdObstaculos"].n);

				PlayerPrefsManager.SetBorgTime((int)thing["tempoDeJogo"].n);
				PlayerPrefsManager.SetJump1((int)thing["idMovimentoPuloDir"].n);
				PlayerPrefsManager.SetJump2((int)thing["idMovimentoPuloEsq"].n);
				PlayerPrefsManager.SetMoveLeft((int)thing["idMovimentoEsquerda"].n);
				PlayerPrefsManager.SetMoveRight((int)thing["idMovimentoDireita"].n);
				PlayerPrefsManager.SetSquat((int)thing["idMovimentoAgachar"].n);
				PlayerPrefsManager.SetIdPerfilPigrunner((int)thing["idPerfilPigRunner"].n);
				//PlayerPrefsManager.SetAllowSquat((string)thing["habilitarAgachar"].b);
				//	PlayerPrefsManager.SetAllowJump((string)thing["habilitarPulo"].b);
				//PlayerPrefsManager.SetQtdObstacles((int)thing["qtdObstaculos"].n);
				GetPerfilByIdPatientGoalkeeper();
				
			} else {
				Debug.LogWarning ("Patient Failed");
				GetPerfilByIdPatientGoalkeeper();
			}
			
		//	Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	public void GetPerfilByIdPatientBridge() {
		Hashtable data = new Hashtable ();
		string id = patient.IdPatient.ToString();
		print(id);
		data.Add ("IdPatient", id);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/PerfilBridge/GetPerfilByIdPatient", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				//Debug.Log ("thing");
			//	print("Count bridg thing "  + thing.Count);
				//print("get filed "  + thing.GetField("sensibilidadeIncLateral").n);
				
				PlayerPrefsManager.SetLateralSensibilit((float)thing["sensibilidadeIncLateral"].n);
				PlayerPrefsManager.SetBridgeBoost((int)thing["velocidadeIncLateral"].n);
				PlayerPrefsManager.SetIdPerfilBridge((int)thing["idPerfil"].n);
				//PlayerPrefsManager.SetRecoverSensibility((int)thing["margemdeRecuperacao"].n);


				
				DisablePigTutorial();
				SendLogs();
				//if(wantToLoad)
				TryToLoadCalibrationScreen();
				//
				
			} else {
				Debug.LogWarning ("Patient Failed");			
				
				DisablePigTutorial();
				SendLogs();

				TryToLoadCalibrationScreen();
			}
			
		//	Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	
	public void GetPerfilByIdPatientGoalkeeper() {
		Hashtable data = new Hashtable ();
		string id = patient.IdPatient.ToString();
		print(id);
		data.Add ("IdPatient", id);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/PerfilGoalkeeper/GetPerfilByIdPatient", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
			//	Debug.Log ("thing");

				//ta faltando id do movimento?
				PlayerPrefsManager.SetBarSpeed((int)thing["velocidadeBarra"].n);
				PlayerPrefsManager.SetTimeToShoot((int)thing["tempoBarra"].n);
				PlayerPrefsManager.SetGoalsLimit((int)thing["qtdGols"].n);
				PlayerPrefsManager.SetDefenseLimit((int)thing["qtdDefesas"].n);
				PlayerPrefsManager.SetIdPerfilGK((int)thing["idPerfilGK"].n);
				//PlayerPrefsManager.SetRecoverSensibility((int)thing["margem_de_recuperacao"].n);
				GetPerfilByIdPatientSUP();
				
			} else {
				Debug.LogWarning ("Patient Failed");
				GetPerfilByIdPatientSUP();
			}
			
		//	Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	public void GetPerfilByIdPatientSUP() {
		Hashtable data = new Hashtable ();
		string id = patient.IdPatient.ToString();
		print(id);
		data.Add ("IdPatient", id);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/PerfilSUP/GetPerfilByIdPatient", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
		//		Debug.Log ("thing");
				
				PlayerPrefsManager.SetRemaingTime((float)thing["tempo"].n);
				PlayerPrefsManager.SetRemarLeft((int)thing["idMovRemadaEsquerda"].n);
				PlayerPrefsManager.SetRemarRight((int)thing["idMovRemadaDireita"].n);
				PlayerPrefsManager.SetIdPerfilSup((int)thing["idPerfil"].n);
				
				//PlayerPrefsManager.SetRecoverSensibility((int)thing["margem_de_recuperacao"].n);
				GetPerfilByIdPatientThrow();
				
			} else {
				Debug.LogWarning ("Patient Failed");
				GetPerfilByIdPatientThrow();
			}
			
			//Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	public void GetPerfilByIdPatientThrow() {
		Hashtable data = new Hashtable ();
		string id = patient.IdPatient.ToString();
		print(id);
		data.Add ("IdPatient", id);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/PerfilThrow/GetPerfilByIdPatient", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				Debug.Log ("thing");			
				PlayerPrefsManager.SetPlaqueTime((float)thing["velocidade_placa"].n);
				PlayerPrefsManager.SetRemaingtimeThrow((int)thing["tempo_jogo"].n);

				PlayerPrefsManager.SetIdPerfilThrow((int)thing["id_perfil"].n);
				
				//PlayerPrefsManager.SetRecoverSensibility((int)thing["margem_de_recuperacao"].n);
				GetPerfilByIdPatientFishing();
				
			} else {
				Debug.LogWarning ("Patient Failed");
				GetPerfilByIdPatientFishing();
			}
			
		//	Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	
	public void GetPerfilByIdPatientFishing() {
		Hashtable data = new Hashtable ();
		string id = patient.IdPatient.ToString();
		print(id);
		data.Add ("IdPatient", id);
		HTTP.Request someRequest = new HTTP.Request( "post", $"{url}/api/PerfilFishing/GetPerfilByIdPatient", data);
		
		someRequest.AddHeader("Authorization", "Bearer " + token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				Debug.Log ("thing");
				
				//PlayerPrefsManager.SetFishMovementSetup(Int32.Parse(thing["nomePerfil"].str));
				PlayerPrefsManager.SetThrowRight((int)thing["idMovimentoArremesso"].n);
				PlayerPrefsManager.SetFishingDifficult((int)thing["sensibilidadePuxao"].n);
				PlayerPrefsManager.SetIdPerfilFishing((int)thing["idPerfil"].n);

				PlayerPrefsManager.SetNumberOfBaits((int)thing["qtdIscas"].n);
				PlayerPrefsManager.SetFishingQuantity((int)thing["qtdPeixes"].n);
				
				//PlayerPrefsManager.SetRecoverSensibility((int)thing["margem_de_recuperacao"].n);
				GetPerfilByIdPatientBridge();
				
			} else {
				Debug.LogWarning ("Patient Failed");
				GetPerfilByIdPatientBridge();
			}
			
			//Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	public void SendLogs () {
		Debug.Log ("entrou no SendLogs");
		DirectoryInfo diretorio = new DirectoryInfo(@"C:\LudsGame\Log\");
		string enviado = @"C:\LudsGame\Log\enviado";
		if (!System.IO.Directory.Exists(enviado))
		{
			System.IO.Directory.CreateDirectory(enviado);
		}
		
		FileInfo[] Arquivos = diretorio.GetFiles("*.*");
		foreach (FileInfo fileinfo in Arquivos)
		{
			
			System.IO.StreamReader sr = new 
				System.IO.StreamReader(@"C:\LudsGame\Log\"+fileinfo.Name);
			string log = sr.ReadToEnd();
			InsertLog(log);
			sr.Close();
			if (!System.IO.File.Exists(@"C:\LudsGame\Log\enviado\"+ fileinfo.Name)) {
				InsertLog(log);
				System.IO.File.Copy(@"C:\LudsGame\Log\"+fileinfo.Name, @"C:\LudsGame\Log\enviado\"+ fileinfo.Name);
			}
			System.IO.File.Delete(@"C:\LudsGame\Log\"+fileinfo.Name);
			//Debug.Log("conteudo = "+ log);
		}
		
		
	}
	
	public void LoadPatient() {
		// Set PlayerPrefs do paciente
		GetPerfilByIdPatientPigRunner();
		/*
		GetPerfilByIdPatientGoalkeeper();
		GetPerfilByIdPatientSUP();
		GetPerfilByIdPatientThrow();
		GetPerfilByIdPatientFishing();
		GetPerfilByIdPatientBridge();*/
		
		 
		
	}
	
	public bool wantToLoad = false;
	
	#endregion
	
	#region Licence Methods
	private void CheckLicence() {		
		HTTP.Request someRequest = new HTTP.Request( "get", $"{url}/api/clinic/check" );
		
		someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if(CheckRequest(thing)) {
				if(thing["message"].str.Equals("RaiseLicence")) {
					Debug.Log ("Raise Licence");
				}
			}
			
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	#endregion
	
	#region Hash Methods
	private byte[] GetHash(string inputString) {
		HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
		return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
	}
	
	private string GetHashString(string inputString) {
		StringBuilder sb = new StringBuilder();
		foreach (byte b in GetHash(inputString))
			sb.Append(b.ToString("X2"));
		
		return sb.ToString();
	}
	#endregion
	
	private bool CheckRequest(JSONObject json) {	
		if(json.HasField("message")) {
			if(json["message"].str.Equals(errorMsg)) {
				Debug.Log("Error");
				return false;
			}
			if(json["message"].str.Equals(authorizationMsg)) {
				Debug.Log(authorizationMsg);
				return false;
			}
		}
		
		return true;
	}
	
	private string GetNetworkInterfaces() {
		IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
		NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
		
		PhysicalAddress address = nics[0].GetPhysicalAddress();
		byte[] bytes = address.GetAddressBytes();
		string mac = null;
		for (int i = 0; i < bytes.Length; i++) {
			mac = string.Concat(mac +(string.Format("{0}", bytes[i].ToString("X2"))));
			if (i != bytes.Length - 1) {
				mac = string.Concat(mac + "-");
			}
		}
		
		return mac;
	}

	private void ShowErrorScreen(string msg){
		errorScreen.SetActive(true);
		errorScreen.GetComponentInChildren<Text>().text = msg;
	}

	private void ShowMustUpdateScreen(string msg, string link){
		mustUpdateScreen.SetActive(true);
		mustUpdateScreen.GetComponentInChildren<Text>().text = msg;
		linkToDownload = link;
	}

	private void ShowUpdateScreen(string msg, string link){
		updateScreen.SetActive(true);
		updateScreen.GetComponentInChildren<Text>().text = msg;
		linkToDownload = link;
	}

	public void DownloadGame(){
		Application.OpenURL(linkToDownload);
	}

	private void HideErrorScreen(){		
		errorScreen.SetActive(false);
	}

	private void HideMustUpdateScreen(){		
		mustUpdateScreen.SetActive(false);
	}

	private void HideUpdateScreen(){		
		updateScreen.SetActive(false);
	}



}
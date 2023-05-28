using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;


public class Tutorial : MonoBehaviour {

	public InputField usernameClinic, passwordClinic;
	public InputField descriptionPc;
	public InputField usernamePhysio, passwordPhysio;

	public GameObject clinicScreen, computerScreen, physioScreen, errorScreen;

	private string clinicToken, clinicTokenType;
	private Clinic clinic;
	private Physiotherapist physiotherapist;
	private Computer computer;
	private Patient patient;
	private string errorMsg = "$msg:error$";
	public bool isAnonymous = false;

	private string token;
	private string tokenType;

	void Awake() {
		clinic = new Clinic(PlayerPrefsManager.GetClinicToken());
		physiotherapist = new Physiotherapist(PlayerPrefsManager.GetPhysiotherapistToken());
		patient = new Patient();
		computer = new Computer();

		InitScreens();
	}

	private void InitScreens() {
		if(!clinic.Token.Equals("0") && !isAnonymous) {
			LoginClinic();
		} else {
			clinicScreen.SetActive(true);
		}
	}

	#region Clinic Methods
	public void SignUpClinic() {
		signUp(usernameClinic.text, passwordClinic.text);
	}
	
	public void LoginClinic() {
		WWWForm form = new WWWForm();
		form.AddField( "userName", usernameClinic.text );
		form.AddField( "password", passwordClinic.text );
		form.AddField( "grant_type", "password" );
		
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );

			if (thing.HasField("access_token")) {
				clinic.Token = thing["access_token"].str;
				clinic.TokenType = thing["token_type"].str;
				GetClinic();
			} else {
				errorScreen.SetActive(true);
			}

			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

	private void GetClinic() {
		HTTP.Request someRequest = new HTTP.Request( "get", $"{HttpController.url}/api/clinic/get" );
		someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log ("Int: " + (int)thing["id_Clinic"].n);
			//Debug.Log ("String: " + thing["id_Clinic"].str);
			if(CheckRequest(thing)) {
				clinic.IdClinic = (int)thing["id_Clinic"].n;
				clinic.NameClinic = thing["clinicName"].str;
				clinic.DescClinic = thing["desc_Clinic"].str;
				clinic.IdAspNetUser = thing["id_Asp_Net_User"].str;

				PlayerPrefsManager.SetClinicToken(clinic.Token);

				ComputerRegistration();
			} else {
				errorScreen.SetActive(true);
			}
		});
	}
	#endregion

	#region Computer Methods
	public void ComputerRegistration() {
		Hashtable data = new Hashtable ();
//		Debug.Log ("Mac: " + GetHashString(GetNetworkInterfaces() + "#" + clinic.IdClinic.ToString()));
//		Debug.Log ("Hash: " + GetNetworkInterfaces());
//		Debug.Log ("IdClinic: " + clinic.IdClinic.ToString());
		data.Add ("NameComputer", GetHashString(GetNetworkInterfaces() + "#" + clinic.IdClinic.ToString()));

		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/computer/get", data );
		someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );

			if(CheckRequest(thing)) {
				physioScreen.SetActive(true);
			} else {
				InsertComputer();
			}

			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

	public void InsertComputer() {
		computerScreen.SetActive(true);
		Hashtable data = new Hashtable ();
		data.Add ("descComputer", descriptionPc.text);
		data.Add ("nameComputer", GetHashString(GetNetworkInterfaces() + "#" + clinic.IdClinic.ToString()));
		data.Add ("idClinic", clinic.IdClinic);

		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/computer/insert", data );
		someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
			if(CheckRequest(thing)) {
				physioScreen.SetActive(true);
				Debug.Log ("Insert Works!");
			} else {
				errorScreen.SetActive(true);
			}

			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	#endregion

	#region Physiotherapist
	public void SignUpPhysiotherapist() {
		signUp(usernamePhysio.text, passwordPhysio.text);
	}

	public void LoginPhysiotherapist() {
		WWWForm form = new WWWForm();
		form.AddField( "userName", usernamePhysio.text );
		form.AddField( "password", passwordPhysio.text );
		form.AddField( "grant_type", "password" );

		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/token", form );
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
	}

	public void GetPhysiotherapist(){
		HTTP.Request someRequest = new HTTP.Request( "get", $"{HttpController.url}/api/physiotherapist/get" );
		someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				physiotherapist.IdPhysiotherapist = (int)thing["idPhysiotherapist"].n;
				physiotherapist.NamePhysiotherapist = thing["namePhysiotherapist"].str;
				physiotherapist.DescPhysiotherapist = thing["descPhysiotherapist"].str;
				physiotherapist.IdAspNetUser = thing["idAspNetUser"].str;

				GetPatient();
			} else {
				errorScreen.SetActive(true);
			}
			
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	#endregion

	#region Patient Methods

	public void GetPatient() {
		Hashtable data = new Hashtable ();
		data.Add ("namePatient", "Paulo");
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/patient/get", data);
		someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			
			if (CheckRequest(thing)) {
				patient.IdPatient = (int)thing["idPatient"].n;
				patient.NamePatient = thing["namePatient"].str;
				//patient.DescPatient = thing["descPatient"].str;
				patient.IdClinic = (int)thing["idClinic"].n;
				patient.IdPhysiotherapist = (int)thing["idPhysiotherapist"].n;

				patient.LateralAmountAnalog = thing["lateralAmountAnalog"].n;
				patient.LateralAmountDigital = thing["lateralAmountDigital"].n;
				patient.HandSensibility = thing["handSensibility"].n;
				patient.MovingSideAmount = thing["movingSideAmount"].n;
				patient.JumpAmountNeeded = thing["jumpAmountNeeded"].n;
				patient.RollAmountNeeded = thing["rollAmountNeeded"].n;
				patient.InitialMapSpeed = thing["initialMapSpeed"].n;
				patient.MapSpeedLimit = thing["mapSpeedLimit"].n;
				patient.IncrementSpeedInterval = (int)thing["incrementSpeedInterval"].n;
				patient.SideToMove = (int)thing["sideToMove"].n;
				patient.PlayerDeficiency = (int)thing["playerDeficiency"].n;
				patient.MiniGames = thing["miniGames"].str;

				LoadPatient();
			} else {
				Debug.LogWarning ("Patient Failed");
			}
			
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

	public void LoadPatient() {
		// Set PlayerPrefs do paciente

		PlayerPrefsManager.SetLateralAmountAnalog(patient.LateralAmountAnalog);
		PlayerPrefsManager.SetLateralAmountDigital(patient.LateralAmountDigital);
		PlayerPrefsManager.SetMoveHandSensibility(patient.HandSensibility);
		PlayerPrefsManager.SetMovingSideAmount(patient.MovingSideAmount);
		PlayerPrefsManager.SetJumpAmountNeeded(patient.JumpAmountNeeded);
		PlayerPrefsManager.SetRollAmountNeeded(patient.RollAmountNeeded);
		PlayerPrefsManager.SetInitialMapSpeed(patient.InitialMapSpeed);
		PlayerPrefsManager.SetMapSpeedLimit(patient.MapSpeedLimit);
		PlayerPrefsManager.SetIncrementSpeedInterval(patient.IncrementSpeedInterval);
		PlayerPrefsManager.SetPlayerDeficiency(patient.PlayerDeficiency);
		PlayerPrefsManager.SetMiniGames(patient.MiniGames);

		//SceneManager.LoadScene("Menu");
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

	#region Test
	public void Conectar() {
		HTTP.Request someRequest = new HTTP.Request( "get", $"{HttpController.url}/api/modelo/listar" );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			thing.GetField("id_modelo");
 			Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
			Debug.Log("Sem objeto: " + request.response.Text );
		});

		Debug.Log ("Cliquei");
	}
	
	public void Insert() {
		Hashtable data = new Hashtable ();
		data.Add ("id_ghost", "1");
		data.Add ("id_googleplus", "13213213");
		data.Add ("max_score", "7500");
		data.Add ("nm_ghost", "Everton");
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/ghost/register", data );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
			Debug.Log ("Insert Works!");
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	public void InsertSecurity() {
		Hashtable data = new Hashtable ();
		data.Add ("id_ghost", "1");
		data.Add ("id_googleplus", "13213213");
		data.Add ("max_score", "7500");
		data.Add ("nm_ghost", "Everton");
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/ghost/register", data );
		someRequest.AddHeader("Authorization","Bearer fVSxKCpeAX0q2a2uOlGesVncgp0-FYAaH98CG5RHiskangSEqxwHYE8migO7FjSF-9NFUh8RMuUSFJxuTxkDx8M3qTiIZwumle0U3iiT0WGTCmfO954SHByo5okFEsiGew5Ixq8Zjhi5nB7qKNxHRNRLITRec_CMYScPTV8EYcYwwMdByURmBtpsOX-l3nlbUTdCyVhdLUAXzJ--3CG5ofNe9dsg0Wp_UPz9ObG-xmYa136811yXKu2uE-nownmkYlImw_elU446lHwJZs0-Y2ioGEmu97Znm2s6w4ctzWfkW5hhAoPSke_FcMGdlgxnsrgTfoW9rJde8uvzZc-oYA");
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
			Debug.Log ("Insert Works!");
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	public void signUp(string username, string password) {
		Hashtable data = new Hashtable ();
		data.Add ("userName", username);
		data.Add ("password", password);
		data.Add ("confirmPassword", password);

		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/account/register", data );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
			Debug.Log ("Signed UP!!");
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}
	
	public void login() {
		Hashtable data = new Hashtable ();
		data.Add ("userName", "eurekamob");
		data.Add ("password", "123456789");

		WWWForm form = new WWWForm();
		form.AddField( "userName", userName.text );
		form.AddField( "password", password.text );
		form.AddField( "grant_type", "password" );

		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );
			this.token = thing["access_token"].str;
			this.tokenType = thing["token_type"].str;
			Debug.Log ("Logged in!!");
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

	public void getClinic(string token) {
		Hashtable data = new Hashtable ();
		data.Add ("id_clinic", "1");
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/clinic/listar", data );
		someRequest.AddHeader("Authorization","Bearer "+token);
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );
			//Debug.Log("JSONObject desc_clinic: " + thing["desc_Clinic"].str);
			Debug.Log ("Clinica autenticada!!");
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

	
	public InputField userName;
	public InputField password;
	public void LoginClinica() {
//		WWWForm form = new WWWForm();
//		form.AddField( "id_clinic", userName.text );
//		//form.AddField( "password", password.text );
//		//form.AddField( "grant_type", "password" );
//		
//		Hashtable data = new Hashtable ();
//		data.Add ("id_clinic", "1");
//		
//		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/clinic/listar", data );
//		someRequest.Send( ( request ) => {
//			// parse some JSON, for example:
//			JSONObject thing = new JSONObject( request.response.Text );
//			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );
//			//Debug.Log("JSONObject desc_clinic: " + thing["desc_Clinic"].str);
//			Debug.Log ("Logged in!!");
//			Debug.Log("Sem objeto: " + request.response.Text );
//		});

		WWWForm form = new WWWForm();
		form.AddField( "userName", userName.text );
		form.AddField( "password", password.text );
		form.AddField( "grant_type", "password" );
		
		
		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/token", form );
		someRequest.Send( ( request ) => {
			// parse some JSON, for example:
			JSONObject thing = new JSONObject( request.response.Text );
			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );
			this.token = thing["access_token"].str;
			this.tokenType = thing["token_type"].str;

			getClinic(this.token);


			Debug.Log ("Logged in!!");
			Debug.Log("Sem objeto: " + request.response.Text );
		});
	}

//
//	public InputField userName;
//	public InputField password;
//	public void LoginClinic() {
//		WWWForm form = new WWWForm();
//		form.AddField( "id_clinic", userName.text );
//		//form.AddField( "password", password.text );
//		//form.AddField( "grant_type", "password" );
//
//		Hashtable data = new Hashtable ();
//		data.Add ("id_clinic", "1");
//		
//		HTTP.Request someRequest = new HTTP.Request( "post", $"{HttpController.url}/api/clinic/listar", data );
//		someRequest.Send( ( request ) => {
//			// parse some JSON, for example:
//			JSONObject thing = new JSONObject( request.response.Text );
//			//Debug.Log("JSONObject access_token: " + thing.list[0]["access_token"].str );
//			//Debug.Log("JSONObject desc_clinic: " + thing["desc_Clinic"].str);
//			Debug.Log ("Logged in!!");
//			Debug.Log("Sem objeto: " + request.response.Text );
//		});
//	}
	#endregion
}

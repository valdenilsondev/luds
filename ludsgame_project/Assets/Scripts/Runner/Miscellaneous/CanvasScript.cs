
using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {

    private Clinic clinic;
    private Physiotherapist physiotherapist;
    private Patient patient;

    public GameObject clinicScreen, computerScreen, physioScreen, errorScreen;

    private string userName = "Everton";
    private string password = "123456";

    private string usernamePhysio = "Herica";
    private string passwordPhysio = "123456";

    private string errorMsg = "$msg:error$";

    public InputField descriptionPc;


	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this);
        //clinic = new Clinic(PlayerPrefsManager.GetClinicToken());
      //  physiotherapist = new Physiotherapist(PlayerPrefsManager.GetPhysiotherapistToken());

        //LoginClinic();
    //    LoginPhysiotherapist();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoginClinic()
    {
        var form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("password", password);
        form.AddField("grant_type", "password");

		//http://localhost:5003
        var someRequest = new HTTP.Request("post", $"{HttpController.url}/token", form);
		//var someRequest = new HTTP.Request("post", "http://localhost:5003/token", form);
        someRequest.Send((request) =>
        {
            var thing = new JSONObject(request.response.Text);

            if (thing.HasField("access_token"))
            {
                clinic.Token = thing["access_token"].str;
                clinic.TokenType = thing["token_type"].str;
                GetClinic();
            }
            else
            {
                Debug.Log("Sem objeto: " + request.response.Text);
            }

        });
    }

    public void LoginPhysiotherapist()
    {
        var form = new WWWForm();
        form.AddField("userName", userName);
        form.AddField("password", password);
        form.AddField("grant_type", "password");


        var someRequest = new HTTP.Request("post", $"{HttpController.url}/token", form);
		//var someRequest = new HTTP.Request("post", "http://localhost:5003/token", form);
        someRequest.Send((request) =>
        {
            var thing = new JSONObject(request.response.Text);

            if (thing.HasField("access_token"))
            {
                physiotherapist.Token = thing["access_token"].str;
                physiotherapist.TokenType = thing["token_type"].str;
                PlayerPrefsManager.SetPhysiotherapistToken(physiotherapist.Token);
                GetPhysiotherapist();
            }
            else
            {
                Debug.Log("Error: ");
            }

            Debug.Log("Sem objeto: " + request.response.Text);
        });
    }

    public void GetPhysiotherapist()
    {
        SendLogs();
		//http://localhost:5003
        HTTP.Request someRequest = new HTTP.Request("get", $"{HttpController.url}/api/physiotherapist/get");
		//HTTP.Request someRequest = new HTTP.Request("get", "http://localhost:5003/api/physiotherapist/get");
        someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
        someRequest.Send((request) =>
        {
            var thing = new JSONObject(request.response.Text);

            if (CheckRequest(thing))
            {
                physiotherapist.IdPhysiotherapist = (int)thing["idPhysiotherapist"].n;
                physiotherapist.NamePhysiotherapist = thing["namePhysiotherapist"].str;
                physiotherapist.DescPhysiotherapist = thing["descPhysiotherapist"].str;
                physiotherapist.IdAspNetUser = thing["idAspNetUser"].str;

                GetPatient();
            }
            else
            {
                Debug.Log("Error: ");
            }

            Debug.Log("Sem objeto: " + request.response.Text);
        });
    }

    public void GetPatient()
    {
        var data = new Hashtable();
        data.Add("namePatient", "Paulo");
	//http://localhost:5003
        var someRequest = new HTTP.Request("post", $"{HttpController.url}/api/patient/get", data);
		//var someRequest = new HTTP.Request("post", "http://localhost:5003/api/patient/get", data);
        someRequest.AddHeader("Authorization", "Bearer " + physiotherapist.Token);
        someRequest.Send((request) =>
        {
            var thing = new JSONObject(request.response.Text);

            if (CheckRequest(thing))
            {
                patient.IdPatient = (int)thing["idPatient"].n;
                patient.NamePatient = thing["namePatient"].str;
             //   patient.DescPatient = thing["descPatient"].str;
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
            }
            else
            {
                Debug.LogWarning("Patient Failed");
            }

            Debug.Log("Sem objeto: " + request.response.Text);
        });
    }

    public void LoadPatient()
    {
        // Set PlayerPrefs do paciente

        //adicionado por paulo
     //   PlayerPrefsManager.SetPlayerID(patient.IdPatient);

        PlayerPrefsManager.SetPhysioID(patient.IdPhysiotherapist);
        PlayerPrefsManager.SetClinicID(patient.IdClinic);

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

        SendLogs();

        SceneManager.LoadScene("Calibration");
    }

    public void SendLogs()
    {
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
                System.IO.StreamReader(@"C:\LudsGame\Log\" + fileinfo.Name);
            string log = sr.ReadToEnd();
            InsertLog(log);
            sr.Close();
            if (!System.IO.File.Exists(@"C:\LudsGame\Log\enviado\" + fileinfo.Name))
            {
                InsertLog(log);
                System.IO.File.Copy(@"C:\LudsGame\Log\" + fileinfo.Name, @"C:\LudsGame\Log\enviado\" + fileinfo.Name);
            }
            System.IO.File.Delete(@"C:\LudsGame\Log\" + fileinfo.Name);
           // Debug.Log("conteudo = " + log);
        }
    }

    public static void InsertLog(string motionData)
    {
        Hashtable data = new Hashtable();
        //string motionData= "testando!";
        data.Add("MotionUnity", motionData);
		//http://localhost:5003
        HTTP.Request someRequest = new HTTP.Request("post", $"{HttpController.url}/api/log/register", data);
		//HTTP.Request someRequest = new HTTP.Request("post", "http://localhost:5003/api/log/register", data);
        someRequest.AddHeader("Authorization", "Bearer " + PlayerPrefsManager.GetPhysiotherapistToken());
        someRequest.Send((request) =>
        {
        });
    }

    private void GetClinic()
    {
		//http://localhost:5003
        var someRequest = new HTTP.Request("get", $"{HttpController.url}/api/clinic/get");
		//var someRequest = new HTTP.Request("get", "http://localhost:5003/api/clinic/get");
        someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
        someRequest.Send((request) =>
        {
            // parse some JSON, for example:
            JSONObject thing = new JSONObject(request.response.Text);
            //Debug.Log ("Int: " + (int)thing["id_Clinic"].n);
            //Debug.Log ("String: " + thing["id_Clinic"].str);
            Debug.Log(request.response.Text);
            if (CheckRequest(thing))
            {
                clinic.IdClinic = (int)thing["id_Clinic"].n;
                clinic.NameClinic = thing["clinicName"].str;
                clinic.DescClinic = thing["desc_Clinic"].str;
                clinic.IdAspNetUser = thing["id_Asp_Net_User"].str;
                clinic.IdLicence = (int)thing["id_Licence"].n;

                PlayerPrefsManager.SetClinicToken(clinic.Token);

                ComputerRegistration();
            }
            else
            {
                errorScreen.SetActive(true);
            }
        });
    }

    private bool CheckRequest(JSONObject json)
    {
        if (json.HasField("message"))
        {
            if (json["message"].str.Equals(errorMsg))
            {
                Debug.Log("Error");
                return false;
            }
        }

        return true;
    }

    public void ComputerRegistration()
    {
        Hashtable data = new Hashtable();
        data.Add("NameComputer", GetHashString(GetNetworkInterfaces() + "#" + clinic.IdClinic.ToString()));
		//http://localhost:5003
        HTTP.Request someRequest = new HTTP.Request("post", $"{HttpController.url}/api/computer/get", data);
		//HTTP.Request someRequest = new HTTP.Request("post", "http://localhost:5003/api/computer/get", data);
        someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
        someRequest.Send((request) =>
        {
            // parse some JSON, for example:
            var thing = new JSONObject(request.response.Text);

            if (CheckRequest(thing))
            {
                //physioScreen.SetActive(true);
            }
            else
            {
                InsertComputer();
            }

            Debug.Log("Sem objeto: " + request.response.Text);
        });
    }

    #region Hash Methods
    private byte[] GetHash(string inputString)
    {
        var algorithm = MD5.Create();  //or use SHA1.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    private string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
    #endregion

    public void InsertComputer()
    {
        //computerScreen.SetActive(true);
        Hashtable data = new Hashtable();
        data.Add("descComputer", descriptionPc.text);
        //data.Add("nameComputer", GetHashString(GetNetworkInterfaces() + "#" + clinic.IdClinic.ToString()));
        data.Add("idClinic", clinic.IdClinic);
		//http://localhost:5003
        HTTP.Request someRequest = new HTTP.Request("post", $"{HttpController.url}/api/computer/insert", data);
		//HTTP.Request someRequest = new HTTP.Request("post", "http://localhost:5003/api/computer/insert", data);
        someRequest.AddHeader("Authorization", "Bearer " + clinic.Token);
        someRequest.Send((request) =>
        {
            // parse some JSON, for example:
            JSONObject thing = new JSONObject(request.response.Text);
            //Debug.Log("JSONObject id_modelo: " + thing.list[0]["id_modelo"].n );
            if (CheckRequest(thing))
            {
                //physioScreen.SetActive(true);
                Debug.Log("Insert Works!");
            }
            else
            {
               // errorScreen.SetActive(true);
            }

            Debug.Log("Sem objeto: " + request.response.Text);
        });
    }

    private string GetNetworkInterfaces()
    {
        var computerProperties = IPGlobalProperties.GetIPGlobalProperties();
        var nics = NetworkInterface.GetAllNetworkInterfaces();
        var address = nics[0].GetPhysicalAddress();
        var bytes = address.GetAddressBytes();
        string mac = null;
        for (int i = 0; i < bytes.Length; i++)
        {
            mac = string.Concat(mac + (string.Format("{0}", bytes[i].ToString("X2"))));
            if (i != bytes.Length - 1)
            {
                mac = string.Concat(mac + "-");
            }
        }

        return mac;
    }
    
}

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class HandleUncaughtException : MonoBehaviour {

	public string lastOutput = "lastoutput";
	public string output = "";
	public string stack = "";
	public LogType typeError = LogType.Log;
	private StringBuilder sbError = new StringBuilder();
	private StringBuilder sbLog = new StringBuilder();
	public List<string> outputsList;

	void OnEnable() {
	//	outputsList = new List<string> ();
		//Debug.Log ("Enable HandleUncaughtException: ");
		//Application.RegisterLogCallbackThreaded(HandleLog);
		Application.RegisterLogCallback(HandleLog);
	}
	void OnDisable() {
		//Debug.Log ("Disable HandleUncaughtException: ");
		//Application.RegisterLogCallbackThreaded(null);
		Application.RegisterLogCallbackThreaded(null);
		//Debug.Log("logString: " + output + " - stackTrace: " + stack + " type: " + typeError.ToString());
		SaveLogError (sbError.ToString ());
		SaveLog (sbLog.ToString ());
	}
	void HandleLog(string logString, string stackTrace, LogType type) {
		output = logString;
		stack = stackTrace;
		typeError = type;
		if (type != LogType.Log) {
			BuildErrorString (output);
		}
		else{
			BuildLogString (output);
		}
		//Debug.log() nao funciona aqui
	}

	void BuildErrorString(string output){
		if (!outputsList.Contains(output)) {
			outputsList.Add(output);
			sbError.AppendLine ("logString: " + output + " - stackTrace: " + stack + " type: " + typeError.ToString());
		}
	}

	void BuildLogString(string output){
		if (!outputsList.Contains(output)) {
			outputsList.Add(output);
			sbLog.AppendLine ("logString: " + output + " - stackTrace: " + stack + " type: " + typeError.ToString());
		}
	}


	void SaveLogError(string error){
		//string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\LogErrorLudsGame\";
		string folder = @"C:\LudsGame\Log\";
		string path = folder + "error " + System.DateTime.Now.Day+
			"-"+System.DateTime.Now.Month+
				"-"+System.DateTime.Now.Year+
				"-"+System.DateTime.Now.Hour+
				"-"+System.DateTime.Now.Minute+
				"-"+System.DateTime.Now.Second+".txt";
		
		bool exists = System.IO.Directory.Exists(folder);
		if(!exists)
			System.IO.Directory.CreateDirectory(folder);

		System.IO.File.WriteAllText(path, error);
	}

	void SaveLog(string log){
		//string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\LogErrorLudsGame\";
		string folder = @"C:\LudsGame\Log\";
		string path = folder + "log " + System.DateTime.Now.Day+
			"-"+System.DateTime.Now.Month+
				"-"+System.DateTime.Now.Year+
				"-"+System.DateTime.Now.Hour+
				"-"+System.DateTime.Now.Minute+
				"-"+System.DateTime.Now.Second+".txt";
		
		bool exists = System.IO.Directory.Exists(folder);
		if(!exists)
			System.IO.Directory.CreateDirectory(folder);
		
		System.IO.File.WriteAllText(path, log);
	}

}

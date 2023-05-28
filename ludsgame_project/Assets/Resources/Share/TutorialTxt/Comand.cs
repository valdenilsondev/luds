using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class Comand : MonoBehaviour {
	
	public ComandData data = new ComandData();
	
	public string name = "game";
	public string comand1 = "comand1";
	public string comand2 = "comand2";
	public string comand3 = "comand1";



	public void LoadData()
	{
		name = data.name;
		comand1 = data.cmd1;
		comand2 = data.cmd2;
		comand3 = data.cmd3;
	}
	
	void OnEnable()
	{
		SaveData.OnLoaded += delegate { LoadData(); };
		
	}
	
	void OnDisable()
	{
		SaveData.OnLoaded -= delegate { LoadData(); };
		
	}
	
}

public class ComandData
{
	[XmlAttribute("Name")]
	public string name;
	
	[XmlElement("Comand1")]
	public string cmd1;
	
	[XmlElement("Comand2")]
	public string cmd2;
	
	[XmlElement("Comand3")]
	public string cmd3;

}

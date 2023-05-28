using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("ComandsCollection")]
public class ComandsContainer {

	[XmlArray("Comands")] 
	[XmlArrayItem("Comand")]
	public List<ComandData> comands = new List<ComandData>();	
}

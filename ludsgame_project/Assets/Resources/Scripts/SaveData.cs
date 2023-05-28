using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class SaveData
{

	public static ComandsContainer cmdContainer = new ComandsContainer();

    public delegate void SerializeAction();
    public static event SerializeAction OnLoaded;
    public static event SerializeAction OnBeforeSave;

    public static void Load(string path)
    {
		cmdContainer = LoadActors(path);

		foreach (ComandData data in cmdContainer.comands)
        {
         /*  GameController.CreateActor(data, GameController.playerPath,
                new Vector3(data.posX, data.posY, data.posZ), Quaternion.identity);*/
		//	Debug.Log(data.name);
		//	Debug.Log(data.cmd1);

        }

     //   OnLoaded();
    }

  
	private static ComandsContainer LoadActors(string path)
    {
		XmlSerializer serializer = new XmlSerializer(typeof(ComandsContainer));

		var textAsset = (TextAsset) Resources.Load(path);
		var reader = new StringReader(textAsset.text);
        
		ComandsContainer cmds = serializer.Deserialize(reader) as ComandsContainer;

		return cmds;
    }

  /*  private static void SaveActors(string path, ActorContainer actors)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ActorContainer));

        FileStream stream = new FileStream(path, FileMode.Truncate);

        serializer.Serialize(stream, actors);

        stream.Close();
    }*/

}

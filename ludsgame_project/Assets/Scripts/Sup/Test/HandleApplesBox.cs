using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleApplesBox : MonoBehaviour {
	public int appleBox_respawn_time;
	
	private List<GameObject> applesBox_array;
	private List<GameObject> deactivated_applesBox_array;
	private int applesBox_array_size;
	
	public static HandleApples instance;
	
	/*void Awake()
	{
		instance = this;
	}*/
	
	void Start () 
	{
		applesBox_array_size = this.transform.childCount;
		applesBox_array = new List<GameObject>(applesBox_array_size);
		deactivated_applesBox_array = new List<GameObject>(applesBox_array_size);
		
		for(int i = 0; i < applesBox_array_size; i++)
		{
			applesBox_array.Add(this.transform.GetChild(i).gameObject);
		}
		
		if(appleBox_respawn_time == 0)
			appleBox_respawn_time = 30;
		
		//reativar macas
		ReactivateApplesBox();
	}
	
	public void CollectAppleBox(GameObject go)
	{
		for(int i = 0; i < applesBox_array_size; i++)
		{
			if(applesBox_array[i].name == go.name)
			{
				applesBox_array.RemoveAt(i);
				deactivated_applesBox_array.Add(go);
				go.SetActive(false);
				applesBox_array_size--;
				return;
			}
		}
	}
	
	private void ReactivateApplesBox()
	{
		if(deactivated_applesBox_array.Count > 0)
		{
			deactivated_applesBox_array[0].SetActive(true);
			applesBox_array.Add(deactivated_applesBox_array[0].gameObject);
			deactivated_applesBox_array.RemoveAt(0);
			applesBox_array_size++;
		}
		Invoke("ReactivateApples", appleBox_respawn_time);
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}

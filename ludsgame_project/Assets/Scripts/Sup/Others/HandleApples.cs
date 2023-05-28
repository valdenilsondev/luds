using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandleApples : MonoBehaviour {

	public int apple_respawn_time;

	private List<GameObject> apples_array;
	private List<GameObject> deactivated_apples_array;
	private int apples_array_size;
	public GameObject applesContainer;
	public static HandleApples instance;

	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
		List<Transform> childList = new List<Transform>();

		for (int i = 0; i < applesContainer.transform.childCount; i++) {
			childList.Add(applesContainer.transform.GetChild(i));
		}
		foreach(Transform t in childList){
			t.SetParent(this.transform);
		}

		//Destroy (applesContainer.gameObject);
		apples_array_size = this.transform.childCount;
		apples_array = new List<GameObject>(apples_array_size);
		deactivated_apples_array = new List<GameObject>(apples_array_size);

		for(int i = 0; i < apples_array_size; i++)
		{
			apples_array.Add(this.transform.GetChild(i).gameObject);
		}

		if(apple_respawn_time == 0)
			apple_respawn_time = 30;

		//reativar macas
		ReactivateApples();


	}
	
	public void CollectApple(GameObject go)
	{
		for(int i = 0; i < apples_array_size; i++)
		{
			if(apples_array[i].name == go.name)
			{
				apples_array.RemoveAt(i);
				deactivated_apples_array.Add(go);
				go.SetActive(false);
				apples_array_size--;
				return;
			}
		}
	}

	private void ReactivateApples()
	{
		if(deactivated_apples_array.Count > 0)
		{
			deactivated_apples_array[0].SetActive(true);
			apples_array.Add(deactivated_apples_array[0].gameObject);
			deactivated_apples_array.RemoveAt(0);
			apples_array_size++;
		}
		Invoke("ReactivateApples", apple_respawn_time);
	}
























}

using UnityEngine;
using System.Collections;
using Share.Managers;

public class Starscreen_Characters : MonoBehaviour {
	public GameObject batata, grandpa, azeitona;
	float elapsedTime1,elapsedTime2,elapsedTime3;
	float batataRdTime, grandpaRdTime, azeitonaRdTime;

	// Use this for initialization
	void Start () {
		batataRdTime = Random.Range(10,25);
		grandpaRdTime = Random.Range(10,25);
		azeitonaRdTime = Random.Range(11,26);
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime1 += Time.deltaTime;
		elapsedTime2 += Time.deltaTime;
		elapsedTime3 += Time.deltaTime;

		if(elapsedTime1 >= batataRdTime){
			batata.GetComponent<Animator>().SetTrigger("iddle_v2");
			batataRdTime = Random.Range(16,30);
			elapsedTime1 = 0;
		}
		
		if(elapsedTime2 >= grandpaRdTime){
			grandpa.GetComponent<Animator>().SetTrigger("iddle_v2"); 
			grandpaRdTime = Random.Range(15,21);
			elapsedTime2 = 0;
		}
		
		if(elapsedTime3 >= azeitonaRdTime){
			azeitona.GetComponent<Animator>().SetTrigger("iddle_v2");
			azeitonaRdTime = Random.Range(10,16);
			elapsedTime3 = 0;
		}

	}

	/*
	void OnDisable()
	{
		SoundManager.Instance.backGroundMusic[5].Stop();
		print ("tirar som");
	}*/

}

using UnityEngine;
using System.Collections;

public class PlayIddle : MonoBehaviour {

	private int rnd;

	void Start () {
		this.GetComponent<Animator>().SetTrigger("iddle");
		SortAnim();
	}

	private void SortAnim(){
		rnd = Random.Range(0,2);
		if(rnd == 0){
			this.GetComponent<Animator>().SetTrigger("iddle");
		}else if(rnd == 1){
			this.GetComponent<Animator>().SetTrigger("death");
		}
		Invoke("SortAnim", 10);
	}
}

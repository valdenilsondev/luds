using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BucketBaitsControl : MonoBehaviour {

	//quantidade de iscas
	private int numberOfBaits;

	public static BucketBaitsControl instance;

	void Awake(){
		instance = this;
	}

	void Start () {
		//iniciar numero de iscas por spot
		numberOfBaits = PlayerPrefsManager.GetNumberofBaits();

		//UIHandler.instance.SetBaitsBucketQuantity(6);
		//salvando numero de iscas
		//numberOfBaits = UIHandler.instance.GetBaitsBucketQuantity();
	
	}


	public int GetNumberOfBaits(){
		return numberOfBaits;
	}

	public bool UseBait(){
		if(numberOfBaits > 0){
			numberOfBaits--;
			UIHandler.instance.SetBaitsBucketQuantity(numberOfBaits);
			return true;
		}else{
			return false;
		}

	}
}

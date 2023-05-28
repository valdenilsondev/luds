using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fish : MonoBehaviour {

	public enum FishType{
		Yellow = 1,
		Brown = 2 ,
		Blue = 3	
	}

	private FishType ft;
	
	public FishType GetFishType(){
		return ft;
	}

	public Fish(FishType fts){
		this.ft = fts;
	}
}

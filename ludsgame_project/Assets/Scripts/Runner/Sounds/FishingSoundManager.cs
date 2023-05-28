using UnityEngine;
using System.Collections;
using Share.Managers;

public class FishingSoundManager : MonoBehaviour {
	
	private static FishingSoundManager instance;
	public static FishingSoundManager Instance
	{ 
		
		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<FishingSoundManager>();
				//print("instance " + instance.name);
				
				if(instance == null) {
					//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}		
			return instance;
		}
	}

	public GameObject fishing_obj;
	private int count_sfx;
	public AudioSource[] fishing_sfx;
	private AudioSource watter_environment, fishing_environment, throwing, pulling, catch_warning, you_got, you_lost, boat_moving, boat_iddle;
	
	// Use this for initialization
	void Awake()
	{
		if(FishingSoundManager.instance != null){
			fishing_obj = GameObject.Find("Fishing_sfx").gameObject;
			//efeitos do pig runner
			count_sfx = fishing_obj.transform.childCount;
			for(int i = 0; i < count_sfx; i++)
			{
				fishing_sfx[i] = fishing_obj.transform.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}
	
	public void PlayWatter_Environment()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[0].Play();
		}
	}
	public void StopWatter_Environment()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[0].Stop();
		}
	}
	public void PlayFishing_Environment()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[1].Play();
		}
	}
	public void StopFishing_Environment()
	{
		if(SoundManager.isSoundFxOn)
		{
			fishing_sfx[1].Stop();
		}
	}

	public void PlayThrowing()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[2].PlayDelayed(0.7f);
		}
	}

	public void PlayPulling()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[3].Play();
		}
	}
	public void PlayCatchWarnning()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[4].Play();
		}
	}
	public void PlayYouGot()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[5].Play();
		}
	}
	public void PlayYouLost()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[6].Play();
		}
	}
	public void PlayBoatMoving()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[7].Play();
		}
	}
	public void StopBoatMoving(){
		if(SoundManager.isSoundFxOn){
			fishing_sfx[7].Stop();
		}
	}
	public void PlayBoatIddle()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[8].Play();
		}
	}
	public void StopBoatIddle()
	{
		if(SoundManager.isSoundFxOn){
			fishing_sfx[8].Stop();
		}
	}
	public void PlayDelaied(){
		//boat idle and environment
		if(SoundManager.isSoundFxOn){
			fishing_sfx[8].PlayDelayed(3.2f);
			fishing_sfx[0].PlayDelayed(3.2f);

		}
	}
}



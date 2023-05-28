using UnityEngine;
using System.Collections;
using Share.Managers;

public class SupSoundManager : MonoBehaviour {
	
	private static SupSoundManager instance;
	public static SupSoundManager Instance
	{ 
		
		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<SupSoundManager>();
				//print("instance " + instance.name);
				
				if(instance == null) {
					//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}		
			return instance;
		}
	}
	
	public GameObject sup_obj;
	private int count_sfx;
	public AudioSource[] sup_sfx;
	//ordem sup_environment, remada, watter_environment;
	
	// Use this for initialization
	void Awake()
	{
		if(SupSoundManager.instance != null){
			sup_obj = GameObject.Find("Fishing_sfx").gameObject;
			//efeitos do pig runner
			count_sfx = sup_obj.transform.childCount;
			for(int i = 0; i < count_sfx; i++)
			{
				sup_sfx[i] = sup_obj.transform.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}
	
	public void PlaySupWatter_Environment()
	{
		if(SoundManager.isSoundFxOn){
			sup_sfx[2].Play();
		}
	}
	public void StopSupWatter_Environment()
	{
		if(SoundManager.isSoundFxOn){
			sup_sfx[2].Stop();
		}
	}
	public void PlaySup_Environment()
	{
		if(SoundManager.isSoundFxOn){
			sup_sfx[0].Play();
		}
	}
	public void StopSup_Environment()
	{
		if(SoundManager.isSoundFxOn){
			sup_sfx[0].Stop();
		}
	}
	//remada do sup
	public void PlayRow()
	{
		if(SoundManager.isSoundFxOn){
			sup_sfx[1].Play();
		}
	}

	/*public void PlayDelaied(){
		if(SoundManager.isSoundFxOn){
			sup_sfx[8].PlayDelayed(3.2f);
			sup_sfx[0].PlayDelayed(3.2f);
			
		}
	}*/
}


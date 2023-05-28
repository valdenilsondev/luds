using UnityEngine;
using System.Collections;
using Share.Managers;

public class GoalKeeperSoundManager : MonoBehaviour {

	private static GoalKeeperSoundManager instance;
	public static GoalKeeperSoundManager Instance
	{
		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<GoalKeeperSoundManager>();
				//print("instance " + instance.name);
				
				if(instance == null) {
					//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}
			return instance;
		}
	}
	
	private GameObject gk_sfx;
	private int count_sfx;
	public AudioSource[] goalkeeper_sfx;
	private AudioSource feet_kick, enviroment, run, wall_kick;
	
	// Use this for initialization
	void Awake()
	{
		if(GoalKeeperSoundManager.instance != null){
			gk_sfx = GameObject.Find("GoalKeeper_SFX").gameObject;
			//efeitos do pig runner
			count_sfx = gk_sfx.transform.childCount;
			for(int i = 0; i < count_sfx; i++)
			{
				goalkeeper_sfx[i] = gk_sfx.transform.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}

	public void PlayFeetKick()
	{
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[0].Play();
		}
	}
	public void PlayEnviroment(){
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[1].Play();
		}
	}
	public void StopEnviroment(){
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[1].Stop();
		}
	}
	public void PlayRun()
	{
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[2].Play();
		}
	}
	public void PlayWallKick()
	{
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[3].Play();
		}
	}
	public void PlayClaps()
	{
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[4].Play();
		}
	}
	public void PlayOhSound()
	{
		if(SoundManager.isSoundFxOn)
		{
			goalkeeper_sfx[5].Play();
		}
	}
}

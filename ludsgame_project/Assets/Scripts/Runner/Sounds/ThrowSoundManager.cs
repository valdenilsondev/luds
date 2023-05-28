using UnityEngine;
using System.Collections;
using Share.Managers;

public class ThrowSoundManager : MonoBehaviour {

	private static ThrowSoundManager instance;
	public static ThrowSoundManager Instance
	{
		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<ThrowSoundManager>();
				//print("instance " + instance.name);
				
				if(instance == null) {
					//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}
			return instance;
		}
	}
	
	private GameObject thr_sfx;
	private int count_sfx;
	public AudioSource[] throw_sfx;
	private AudioSource crash_box_n_tree, crash_plaque, run, slide, change_lane;
	
	// Use this for initialization
	void Awake()
	{
		if(ThrowSoundManager.instance != null){
			thr_sfx = GameObject.Find("Throw_SFX").gameObject;
			//efeitos do pig runner
			count_sfx = thr_sfx.transform.childCount;
			for(int i = 0; i < count_sfx; i++)
			{
				throw_sfx[i] = thr_sfx.transform.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}

	public void PlayThrowSfx()
	{
		if(SoundManager.isSoundFxOn)
		{
			throw_sfx[0].Play();
		}
	}
	public void PlayRisePlaqueSfx()
	{
		if(SoundManager.isSoundFxOn)
		{
			throw_sfx[1].Play();
		}
	}
	public void PlayCrashPlaqueSfx()
	{
		if(SoundManager.isSoundFxOn)
		{
			throw_sfx[2].Play();
		}
	}
	public void PlayEnviromentSfx()
	{
		if(SoundManager.isSoundFxOn)
		{
			throw_sfx[3].Play();
		}
	}
	public void StopEnviromentSfx()
	{
		if(SoundManager.isSoundFxOn)
		{
			throw_sfx[3].Stop();
		}
	}

}

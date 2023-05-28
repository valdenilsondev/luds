using UnityEngine;
using System.Collections;
using Share.Managers;
using BridgeGame.Player;
using BridgeGame.KinectControl;

public class BridgeSoundManager : MonoBehaviour {

	public GameObject steps_sounds;
	private static BridgeSoundManager instance;
	public static BridgeSoundManager Instance
	{ 
		
		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<BridgeSoundManager>();
				//print("instance " + instance.name);
				
				if(instance == null) {
					//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}		
			return instance;
		}
	}
	
	private GameObject bridge_obj;
	private int count_sfx;
	public AudioSource[] bridge_sfx;
	private AudioSource steps, piranha_bite, piranha_splash, bridge_noise, river_runing, pig_fall_splash;
	
	// Use this for initialization
	void Awake()
	{
		if(BridgeSoundManager.instance != null){
			bridge_obj = GameObject.Find("Bridge_sfx").gameObject;
			//efeitos do pig runner
			count_sfx = bridge_obj.transform.childCount;
			for(int i = 0; i < count_sfx; i++)
			{
				bridge_sfx[i] = bridge_obj.transform.GetChild(i).GetComponent<AudioSource>();
			}
			//passos desativados
			//steps.gameObject.SetActive(false);
		}
	}

	public void PlaySteps()
	{
		if(SoundManager.isSoundFxOn){
			//bridge_sfx[0].Play();
			bridge_sfx[0].pitch = 2f;
		}
	}
	public void StopSteps()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[0].Stop();
		}
	}
	public void PlaySpeedSteps()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[0].pitch = 2.2f;
		}
	}

	public void PlayPiranha_Splash()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[1].Play();
		}
	}
	public void PlayPiranha_Bite()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[2].Play();
		}
	}
	public void PlayBridgeNoise()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[3].Play();
		}
	}
	public void StopBridgeNoise()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[3].Stop();
		}
	}
	public void PlayRiverRunning()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[4].Play();
		}
	}
	public void StopRiverRunning()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[4].Stop();
		}
	}
	public void PlayPigFallSplash()
	{
		if(SoundManager.isSoundFxOn){
			bridge_sfx[5].PlayDelayed(1.5f);
		}
	}
}

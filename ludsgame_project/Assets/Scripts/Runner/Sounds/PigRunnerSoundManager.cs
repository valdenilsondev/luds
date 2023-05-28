using UnityEngine;
using System.Collections;
using Share.Managers;

public class PigRunnerSoundManager : MonoBehaviour {

	private static PigRunnerSoundManager instance;
	public static PigRunnerSoundManager Instance
	{ 

		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<PigRunnerSoundManager>();
				//print("instance " + instance.name);
				
				if(instance == null) {
					//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}

			return instance;
		}
	}

	private GameObject pr_sfx;
	private int countPR_sfx, currentBite = 7;
	public AudioSource[] pig_runner_sfx;// selectbite;
	//SFX ordem: crash_box_n_tree, crash_plaque, run, slide, change_lane, jumnp, powerUp, bites, bites1, bites2, bites3, bites4;

	// Use this for initialization
	void Awake()
	{
		if(PigRunnerSoundManager.instance != null){
			pr_sfx = GameObject.Find("PigRunner_SFX").gameObject;
			//efeitos do pig runner
			countPR_sfx = pr_sfx.transform.childCount;
			for(int i = 0; i < countPR_sfx; i++)
			{
				pig_runner_sfx[i] = pr_sfx.transform.GetChild(i).GetComponent<AudioSource>();
			}
		}
	}
	

	//SFX DO PIG RUNNER
	public void PlayCrashBoxNtree()
	{
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[0].Play();
		}
	}
	public void PlayCrashPlaque()
	{
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[1].Play();
		}
	}
	public void PlayRunSound()
	{
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[2].Play();
		}
	}
	public void StopRunSound()
	{
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[2].Stop();
		}
	}
	public void PlaySlide()
	{
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[3].Play();
		}
		print ("teste som slide");
	}
	public void PlayChangeLane()
	{
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[4].Play();
		}
	}
	public void PlayJump() 
	{
		if(SoundManager.isSoundFxOn == true)
			pig_runner_sfx[5].Play();
	}
	public void PlayPowerUps() {
		if(SoundManager.isSoundFxOn == true){
			pig_runner_sfx[6].Play();
		}
	}
	public void Bite() 
	{
		if(SoundManager.isSoundFxOn == true){
			//print ("current bite: " + currentBite);
			pig_runner_sfx[currentBite].Play ();
			currentBite++;
			if(currentBite == 11){ 
				currentBite = 7;
			}
		}
	}

}

using UnityEngine;
using System.Collections;
using Assets.Scripts.Share;
using UnityEngine.UI;

namespace Share.Managers{
public class SoundManager : MonoBehaviour {

	//public AudioClip jumpSFX;
	public AudioClip rollSFX;
	//public AudioClip runSFX;
	//public AudioClip lateralHitSFX;
	//public AudioClip gameOverHitSFX;
	//public AudioClip gameTransitionSFX;
	//public AudioSource powerUpSFX;

	//controla alfa dos botoes de configuraçao de som
	//public Image musicbtn_img, sfxbtn_img, music_icon, sfx_icon;
	public Sprite musicon, musicoff, sfxon, sfxoff;
	public Image music_img, sfx_img;
	public GameObject musicbtn_obj, sfxbtn_obj;
	private Game game;

	public AudioSource[] backGroundMusic;
	//musicas temas na ordem pig_BGmusic, bridge_BGmusic, goalKeeper_BGmusic, sup_BGmusic, trhow_BGmusic;
	public AudioSource[] selectionSounds;
	//sfx na ordem bull, gallop, confirmMenu, changeSelection, minigame_transition, nature ambient, machine;
	private int countSfx, countBg;
	//associar componentes de som na ordem
	private GameObject soundFX, bg_music, sfx;
	public static bool isSoundFxOn = true, isMusicOn = true;
	private static SoundManager instance;

	public static SoundManager Instance 
	{
		get 
		{
			if(instance == null) {
				instance = FindObjectOfType<SoundManager>();
				//print("instance " + instance.name);

				if(instance == null) {
				//	Debug.Log ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}
			return instance;
		}
	}

	void Awake() {
		instance = this;
		if(SoundManager.instance != null){
				//pai
			soundFX = GameObject.Find("Sounds").gameObject;
				//filhos
			bg_music = GameObject.Find ("BG_Music").gameObject;
			sfx = GameObject.Find("SFX").gameObject;
			/*bridge_sfx = GameObject.Find("Bridge_SFX").gameObject;
			fishing_sfx = GameObject.Find("Fishing_SFX").gameObject;
			sup_sfx = GameObject.Find("Sup_SFX").gameObject;*/

		}
		//audioSource = this.GetComponent<AudioSource>();
		//sons de background
		countBg = bg_music.transform.childCount;
		backGroundMusic = new AudioSource[countBg];
		for(int i = 0; i < countBg; i++)
		{
			backGroundMusic[i] = bg_music.transform.GetChild(i).GetComponent<AudioSource>();
		}

		//efeitos sonoros
		countSfx = sfx.transform.childCount;
		selectionSounds = new AudioSource[countSfx];
		for(int i = 0; i < countSfx; i++)
		{
			selectionSounds[i] = sfx.transform.GetChild(i).GetComponent<AudioSource>();
		}
		//efeitos do pig runner
		/*countPR_sfx = pr_sfx.transform.childCount;
		for(int i = 0; i < countPR_sfx; i++)
		{
			pig_runner_sfx[i] = pr_sfx.transform.GetChild(i).GetComponent<AudioSource>();
		}*/

		DontDestroyOnLoad(soundFX);
	}

	/*public void ChangeSelection()
	{	
		//switch15 sound
		if(isSoundFxOn)
			selectionSounds[7].Play();
	}*/

	public void ConfirmMenu()
	{
		if(isSoundFxOn)
			selectionSounds[3].Play();
	}

	public void PlayGameTransition() 
	{
		if(isSoundFxOn){
			selectionSounds[4].Play();
		}
	}

	private void PlayNatureAmbient()
	{
		if(isSoundFxOn)
			selectionSounds[5].Play();
	}
		//usado na startscreen e sup
	private void StopNatureAmbient()
	{
		if(isSoundFxOn)
			selectionSounds[5].Stop();
	}
	private void PlayMachine()
	{
		if(isSoundFxOn){
			selectionSounds[6].Play();
		}
	}
	private void StopMachine()
	{
		if(isSoundFxOn){
			selectionSounds[6].Stop();
		}
	}
	public void StopStartscreenSound()
	{
		StopMachine();
		StopNatureAmbient();
		if(isMusicOn)
			backGroundMusic[5].Stop();
	}
	
		//respeitar o indice das musicas para nao desvincular o audioClip no objeto da cena
		public void PlayBGmusic()
		{
			game = GameManagerShare.instance.game;
				//if(isMusicOn == false){
				//	isMusicOn = true;
				switch(game){			
					case Game.Pig:
						backGroundMusic[0].Play();
						break;
					case Game.Bridge:
						backGroundMusic[1].Play();
						break;
					case Game.Goal_Keeper:
						backGroundMusic[2].Play();
						break;
					case Game.Sup:
						backGroundMusic[3].Play();
						//PlayNatureAmbient();
						break;
					case Game.Throw:
						backGroundMusic[4].Play();
						break;
					case Game.startScreenNew:
						backGroundMusic[5].Play ();
						break;
					case Game.Fishing:
						backGroundMusic[6].Play();
						break;
					default:
						print ("No game found for play BG music.");
						break;
					}
				//}
		}
		public void PlayBGDelayed(float time)
		{
			//if(isMusicOn == true){
				switch(game)
				{
					case Game.Pig:
						print("stop pig music");
						backGroundMusic[0].PlayDelayed(time);
						break;
					case Game.Bridge:
						backGroundMusic[1].PlayDelayed(time);
						break;
					case Game.Goal_Keeper:
						backGroundMusic[2].PlayDelayed(time);
						break;
					case Game.Sup:
						backGroundMusic[3].PlayDelayed(time);
						break;
					case Game.Throw:
						backGroundMusic[4].PlayDelayed(time);
						break;
					case Game.startScreenNew:
						backGroundMusic[5].PlayDelayed(time);
						break;
					case Game.Fishing:
						backGroundMusic[6].PlayDelayed(time);
						break;
					default:
						print ("No game found for stop BG music.");
						break;
				}
			//}
		}

		public void StopBGmusic()
		{
			print("stop bg music");
			//if(isMusicOn == true){
			//	isMusicOn = false;
				switch(game)
				{
					case Game.Pig:
						backGroundMusic[0].Stop();
						break;
					case Game.Bridge:
						backGroundMusic[1].Stop();
						break;
					case Game.Goal_Keeper:
						backGroundMusic[2].Stop();
						break;
					case Game.Sup:
						backGroundMusic[3].Stop();
						break;
					case Game.Throw:
						backGroundMusic[4].Stop();
						break;
					case Game.startScreenNew:
						backGroundMusic[5].Stop();
						break;
					case Game.Fishing:
						backGroundMusic[6].Stop();
						break;
					default:
						print ("No game found for stop BG music.");
						break;
				}
			//}
		}

		public void PlaySfx()
		{
			print("stop sfx");
			if(isSoundFxOn == false){
				isSoundFxOn = true;
				switch(game)
				{
				case Game.Pig:
					PigRunnerSoundManager.Instance.PlayRunSound();
					break;
				case Game.Bridge:
					BridgeSoundManager.Instance.PlaySteps();
					BridgeSoundManager.Instance.PlayBridgeNoise();
					BridgeSoundManager.Instance.PlayRiverRunning();
					break;
				case Game.Goal_Keeper:
					GoalKeeperSoundManager.Instance.PlayEnviroment();
					break;
				case Game.Sup:
					SupSoundManager.Instance.PlaySupWatter_Environment();
					break;
				case Game.Throw:
					ThrowSoundManager.Instance.PlayEnviromentSfx();
					break;
				case Game.startScreenNew:
					//PlayNatureAmbient();
					PlayMachine();
					break;
				case Game.Fishing:
					FishingSoundManager.Instance.PlayWatter_Environment();
					FishingSoundManager.Instance.PlayBoatMoving();
					FishingSoundManager.Instance.PlayBoatIddle();
					FishingSoundManager.Instance.PlayFishing_Environment();
					break;
				default:
					print ("No game found for stop BG music.");
					break;
				}
			}
		}

		public void StopSfx()
		{
			print("stop sfx");
			//if(isSoundFxOn == true){
				switch(game)
				{
				case Game.Pig:
					PigRunnerSoundManager.Instance.StopRunSound();
					PigRunnerSoundManager.Instance.Bite();
					break;
				case Game.Bridge:
					BridgeSoundManager.Instance.StopSteps();
					BridgeSoundManager.Instance.StopBridgeNoise();
					BridgeSoundManager.Instance.StopRiverRunning();
					break;
				case Game.Goal_Keeper:
					GoalKeeperSoundManager.Instance.StopEnviroment();
					break;
				case Game.Sup:
					SupSoundManager.Instance.StopSupWatter_Environment();
					break;
				case Game.Throw:
					ThrowSoundManager.Instance.StopEnviromentSfx();
					break;
				case Game.startScreenNew:
					StopNatureAmbient();
					StopMachine();
					break;
				case Game.Fishing:
					FishingSoundManager.Instance.StopWatter_Environment();
					FishingSoundManager.Instance.StopBoatMoving();
					FishingSoundManager.Instance.StopBoatIddle();
					FishingSoundManager.Instance.StopFishing_Environment();
					break;
				default:
					print ("No game found for stop BG music.");
					break;
				}
			//}
		}
		//trocar o alpha do btn nao eh mais usado
		private void SetTransparency(float transparency, Image btn) { //transparency is a value in the [0,1] range

			Color color = btn.color;
			color.a = transparency;
			btn.color = color;
		}

		public void SwitchMusicSituation(){
			if(isMusicOn == true){
				StopBGmusic();
				//SetTransparency(0.65f, musicbtn_img);
				//SetTransparency(0.65f, music_icon);
				music_img.sprite = musicoff;
				musicbtn_obj.transform.GetChild(0).gameObject.SetActive(false);//desativa imagem do icone pq tem 2 imagens, 1 img de btn e 1 de icone
				isMusicOn = false;
			}
			else{
				PlayBGmusic();
				music_img.sprite = musicon;
				musicbtn_obj.transform.GetChild(0).gameObject.SetActive(true);
				//SetTransparency(1f, musicbtn_img);
				//SetTransparency(1f, music_icon);
				isMusicOn = true;
			}
			print("isMusicOn " + isMusicOn);
		}

		public void SwitchSoundFXSituation(){
			if(isSoundFxOn == true){
				StopSfx();
				sfx_img.sprite = sfxoff;
				sfxbtn_obj.transform.GetChild(0).gameObject.SetActive(false);
				//SetTransparency(0.65f, sfxbtn_img);
				//SetTransparency(0.65f, sfx_icon);
				isSoundFxOn = false;
			}
			else{
				PlaySfx();
				sfx_img.sprite = sfxon;
				sfxbtn_obj.transform.GetChild(0).gameObject.SetActive(true);
				//SetTransparency(1f, sfxbtn_img);
				//SetTransparency(1f, sfx_icon);
				isSoundFxOn = true;
			}
			print("isSoundFxOn " + isSoundFxOn);
		}

	}
}
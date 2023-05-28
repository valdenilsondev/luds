using UnityEngine;
using System.Collections;

public enum Sounds{
	BubblePop,
	EnemyLaugh,
	Digging
}

namespace Sandbox.GameUtils {
	public class SoundManager_Sandbox : MonoBehaviour {
		
		private static SoundManager_Sandbox instance;
		
		public AudioSource bubblePop;
		public AudioSource enemyLaugh;
		public AudioSource digging;
		
		void Awake () {
			instance = this;
		}

		//nao utilizado
		public void Play(Sounds sound){
			switch(sound){
			case Sounds.BubblePop:
				bubblePop.Play();
				break;
			case Sounds.EnemyLaugh:
				enemyLaugh.Play();
				break;
			case Sounds.Digging:
				digging.Play();
				break;
			}
		}
		
		public static SoundManager_Sandbox Instance(){
			if(!instance){
				instance = FindObjectOfType<SoundManager_Sandbox>();
				if(!instance){
					Debug.LogError("Nenhum Game Object do tipo SoundManager_Sandbox foi encontrado");
				}
			}
			
			return instance;
		}
	}
}
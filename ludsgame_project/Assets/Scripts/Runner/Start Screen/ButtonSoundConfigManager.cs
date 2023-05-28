using UnityEngine;
using System.Collections;

public class ButtonSoundConfigManager : MonoBehaviour {
	public static bool music_apertado = false;
	public static bool sfx_apertado = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		var obj = GameObject.Find(HandCollider2D.current_handOnButtonName).gameObject;
		obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		//resetar escalas
		for(int i = 0; i < StartScreenManager.instance.GetConfigSelectionBoxes().Length; i++){
			if(StartScreenManager.instance.GetConfigSelectionBoxes()[i].name != HandCollider2D.current_handOnButtonName){
				StartScreenManager.instance.GetConfigSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
			}
		}
		if(LoadingSelection.instance.GetIsTimerComplete() == true && (music_apertado == false && sfx_apertado == false))
		{
			if(col.CompareTag("config_music_box"))
			{
				//music_apertado = false;
				OptionScreen.instance.TurnMusic();
			}
			else if (col.CompareTag("config_sound_box"))
			{
				//sfx_apertado = false;
				OptionScreen.instance.TurnOnSoundFX();
			}
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.CompareTag("config_music_box"))
		{
			music_apertado = true;
		}
		else if (col.CompareTag("config_sound_box"))
		{
			sfx_apertado = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(LoadingSelection.instance.GetIsTimerComplete() == false)
		{
			if(col.CompareTag("config_music_box"))
			{
				music_apertado = false;
			}
			else if (col.CompareTag("config_sound_box"))
			{
				sfx_apertado = false;
			}
		}
	}

}

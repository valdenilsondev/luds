using UnityEngine;
using System.Collections;
using Share.Managers;
using BridgeGame.Player;
using Assets.Scripts.GameOverScreen;

public class HandCollider2D : MonoBehaviour
{
	private Vector3 playerhand;
    public static HandCollider2D instance;
    public static string handOnButtonTag;
	public static string handOnButtonName;
	public static string current_handOnButtonTag;
	public static string current_handOnButtonName;
    private string btnActual;

	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManagerShare.instance.IsUsingKinect() && PlayerHandController.instance != null)
        {
			playerhand = PlayerHandController.instance.GetPlayerCursor();
		}else{
			playerhand = Input.mousePosition;
		}
	}
	//button_restart //button_minigames //button_menu //button_continue //button_stop
	public string GetButtonTag(){
		var hit = Physics2D.Raycast(playerhand, -Vector2.up, 5f);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(playerhand, -Vector2.up, color);


        var hit2 = Physics2D.Raycast(playerhand, -Vector2.right, 5f);
        Color color2 = hit2 ? Color.yellow : Color.blue;
        Debug.DrawRay(playerhand, -Vector2.up, color2);

        if (hit.collider != null)
        {
            return hit.collider.tag;
        }
		return "nothing";
	}

	public GameObject GetObject(){
		var hit = Physics2D.Raycast(playerhand, -Vector2.up, 5f);
		Color color = hit ? Color.green : Color.red;
		Debug.DrawRay(playerhand, -Vector2.up, color);
		
		
		var hit2 = Physics2D.Raycast(playerhand, -Vector2.right, 5f);
		Color color2 = hit2 ? Color.yellow : Color.blue;
		Debug.DrawRay(playerhand, -Vector2.up, color2);
		
		if (hit.collider != null && hit.collider.name != "PlayerHand")
		{
			return hit.collider.gameObject;
		}
		return null;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
		print("ontriggerenter2d handcollider2d");
        handOnButtonTag = string.Empty;

        if (LoadingSelection.instance.IsLoading())
			LoadingSelection.instance.StopLoadingSelection();

        LoadingSelection.instance.StartLoadingSelection();
        var btn = other.GetComponent<Button>();
        if(btn != null)
            btn.Active();

		current_handOnButtonTag = other.tag;
		current_handOnButtonName = other.name;

        btnActual = other.tag;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (LoadingSelection.instance.GetIsTimerComplete() &&  string.IsNullOrEmpty(handOnButtonTag))
        {
			print ("jogo pausado? "+ GameManagerShare.IsPaused());
            switch (collider.tag)
            {
				case "button_continue":
					handOnButtonTag = collider.tag;
					break;
				case "button_stop":
					handOnButtonTag = collider.tag;
					break;
                case "button_menu":
                    handOnButtonTag = collider.tag;
                    break;
                case "button_restart":
                    handOnButtonTag = collider.tag;
                    break;
                case "button_minigames":
                    handOnButtonTag = collider.tag;
                    break;
				case "BorgItem":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;
				/*case "BorgItem1":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;*/
			
				case "game_selection_box":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;
				/*case "config_selection_box":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;*/
				case "config_music_box":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;
				case "config_sound_box":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;
				case "startscreen_back_btn":
					handOnButtonTag = collider.tag;
					handOnButtonName = collider.name;
					break;
				case "button_decline":
					handOnButtonTag = collider.tag;
					break;
				case "button_accept":
					handOnButtonTag = collider.tag;
					break;
				case "skip_button":
					handOnButtonTag = collider.tag;
					break;
                default:
                    break;
            }

         //   LoadingSelection.instance.ApplySelection();
        }
    }

	//colisor de botoes
    void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag != "LoadingSelection") {
			if (btnActual == other.tag) {
				handOnButtonTag = "nothing";
				LoadingSelection.instance.StopLoadingSelection ();
			}

			current_handOnButtonTag = "nothing";
			current_handOnButtonName = "nothing";

			var btn = other.GetComponent<Button> ();
			if (btn != null)
				btn.Normal ();
		} else {
			Debug.Log ("exit LoadingSelection");
		}
    }
}


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MenuButton : MonoBehaviour {

    public ButtonType type;

    public bool isActive;
    
	private static string BTNACTIVEPATH = "Share/GAMES/BUTTON-ACTIVE_314x106_Games";//"Assets/Textures/PigRunner/BUTTONS/GAMES/BUTTON-ACTIVE_314x106_Games.png";
	private static string BTNDESACTIVEPATH = "Share/GAMES/BUTTON_314x106_Games";//"Assets/Textures/PigRunner/BUTTONS/GAMES/BUTTON_314x106_Games.png";

    public void Active()
    {
        var activeBtn = (Sprite)Resources.Load(BTNACTIVEPATH, typeof(Sprite));
        var image = new List<Image>(this.gameObject.GetComponentsInChildren<Image>()).Where(x => x.name == "image").FirstOrDefault();
        image.sprite = activeBtn;

        isActive = true;
    }

    public void Inactive()
    {
        var activeBtn = (Sprite)Resources.Load(BTNDESACTIVEPATH, typeof(Sprite));
        var image = new List<Image>(this.gameObject.GetComponentsInChildren<Image>()).Where(x => x.name == "image").FirstOrDefault();
        image.sprite = activeBtn;

        isActive = false;
    }
}

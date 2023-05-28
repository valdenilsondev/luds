using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public List<MenuButton> buttons;

    private static Animator menu;
    private MenuButton btnDefault;


    void Awake()
    {
        instance = this;
        menu = this.gameObject.GetComponent<Animator>();
        btnDefault = (MenuButton)buttons.Where(x => x.type == ButtonType.MiniGames).First();

    }

    public void Initialize(MenuButton btn)
    {
        Show();
        OnClickButton(btnDefault);
    }

    public void OnClickButton(MenuButton btn)
    {
        var listDisableButtons = buttons.Where(x => x.name != btn.name && x.type != ButtonType.Exit).ToList();

        if (!btn.isActive)
        {
            switch (btn.type)
            {
                case ButtonType.Exit:
                    //CarouselBoxController.instance.Hide();
                    //Hide();
                    MiniGameScreenController.instance.Hide();
                    break;
                case ButtonType.MiniGames:
                    btn.Active();
                    InactiveButtons(listDisableButtons);
                    CarouselBoxController.instance.Initialize(BoxType.MiniGame);
                    break;
                case ButtonType.Categorias:
                    btn.Active();
                    InactiveButtons(listDisableButtons);
                    CarouselBoxController.instance.Initialize(BoxType.Categoria);
                    break;
                case ButtonType.Recomendados:
                    btn.Active();
                    InactiveButtons(listDisableButtons);
                    break;
            }
        }
    }

    private void InactiveButtons(List<MenuButton> buttons)
    {
        foreach (var btn in buttons)
        {
            btn.Inactive();
        }
    }

    private void Show()
    {
        menu.SetBool("ShowMenu", true);
        //Initialize();
    }

    public void Hide()
    {
        menu.SetBool("ShowMenu", false);
        //MiniGameScreenController.instance.Hide();

        var listDisableButtons = buttons.Where(x => x.type != ButtonType.Exit).ToList();

        InactiveButtons(listDisableButtons);
        //btnDefault.Active();



        //CarouselBoxController.instance.Hide();
        //iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(0, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear));
    }

    public MenuButton GetCurrentMenuActive()
    {
        return buttons.Where(x => x.isActive == true).FirstOrDefault();
    }
}

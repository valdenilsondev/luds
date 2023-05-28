using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Share.Managers;
using Runner.Managers;

public class MiniGameScreenController : MonoBehaviour
{

    public MenuController menu;
    public CarouselBoxController carouselBox;
    public static MiniGameScreenController instance;

    public List<MiniGame> miniGames;
    public List<Category> categories;

    void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        iTween.tweens.Clear();
        MenuController.instance.Initialize(null);
        //menu.Show();
        //carouselBox.ClearCarouselBox();
    }

    public void MoveGameBoxKinect(Direction direction, float speed)
    {
        switch (direction)
        {
            case Direction.Left:
                carouselBox.MoveCarouselBoxTo(Direction.Left, speed);
                break;
            case Direction.Right:
                carouselBox.MoveCarouselBoxTo(Direction.Right, speed);
                break;
            default:
                break;
        }
    }


	void OnDisable(){
		//LoadingSelection.instance.StopLoadingSelection();
	}

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            carouselBox.MoveCarouselBoxTo(Direction.Left, 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            carouselBox.MoveCarouselBoxTo(Direction.Right, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!carouselBox.iTweenRunning)
                LoadBoxSelected();
        }
    }

    public void LoadBoxSelected()
    {
        var box = carouselBox.GetBoxSelected();

        switch (box.type)
        {
            case BoxType.MiniGame:
                var miniGame = box.name.ToUpper().Trim().Replace(" ","");
				//musica do barn
				SoundManager.Instance.backGroundMusic[5].Stop();
				//SoundManager.Instance.StopBGmusic();
                LoadMiniGame(miniGame);
                break;
            case BoxType.Categoria:
                SelectMiniGamesByCategory(box);
                break;
            default:
                break;
        }
	}

    private void LoadMiniGame(string miniGame)
    {
        switch (miniGame)
        {
            case "PIGRUNNER":
				if(PlayerPrefsManager.GetPigTutorial() == 0){
					LoadingScreen.instance.LoadScene("PigRunner");
				}else{
					LoadingScreen.instance.LoadScene("Tutorial");
				}
                break;
            case "GOALKEEPER":
				LoadingScreen.instance.LoadScene("Goalkeeper");
                break;
            case "BRIDGE":
				LoadingScreen.instance.LoadScene("Bridge");
                break;
            case "THROW":
				LoadingScreen.instance.LoadScene("Throw");
                break;
            case "SANDBOXTREASURE":
				LoadingScreen.instance.LoadScene("Sandbox");
                break;
			case "SUP":
				LoadingScreen.instance.LoadScene("Sup");
				break;
			case "FISHING":
				LoadingScreen.instance.LoadScene("Fishing");
				break;
            default:
                break;
        }
    }
    
    private void SelectMiniGamesByCategory(Box category)
    {
        //var button = MenuController.instance.buttons.Where(x => x.type == ButtonType.Categorias).FirstOrDefault();
        carouselBox.SelectMiniGamesByCategory(category);
    }

    public void Show()
    {

    }

    public void Hide()
    {
        MenuController.instance.Hide();
        CarouselBoxController.instance.Hide();
        MouseOnClickWall.instance.click_mini_game_back = true;
        MouseOnClickWall.instance.EnableGameCube();
        StartScreenKinectController.instance.isMiniGameActived = false;
    }
}

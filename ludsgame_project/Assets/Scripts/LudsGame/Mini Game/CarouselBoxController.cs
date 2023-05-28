using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;


public class CarouselBoxController : MonoBehaviour
{
    private List<Box> boxList;
    private List<Box> miniGamesBoxesList;
    private List<Box> categoriesBoxesList;
    private List<Box> suggestedBoxesList;

    private Box boxPrefab;
    public static CarouselBoxController instance;

    public string[] miniGamesNamesList;
    public List<Sprite> miniGamesPictures;

    public string[] categoriesNamesList;
    public List<Sprite> categoriesPictures;

    public string[] suggestedNamesList;

    private int currentBox = 1;

    public bool iTweenRunning = false;

    private float offset = 20f;

    void Awake()
    {
        instance = this;
        boxList = new List<Box>();
        miniGamesBoxesList = new List<Box>();
        categoriesBoxesList = new List<Box>();
        suggestedBoxesList = new List<Box>();
		boxPrefab = (Box)Resources.Load("Share/box", typeof (Box));
    }

    void Update()
    {
        //if(!iTweenHideRunning)
        //    Show();
    }

    public void Initialize(BoxType boxType)
    {

        //iTween.Stop();
        //Hide();
        ClearCarouselBox();
        FillCarouselBox(boxType);
        Show();
    }

    public void MoveCarouselBoxTo(Direction direction, float speed)
    {
        switch (direction)
        {
            case Direction.Left:
                if (currentBox != 1 && iTweenRunning == false)
                {
                    currentBox--;
                    MoveBoxes(direction, speed);
                }
                else return;
                break;
            case Direction.Right:
                if (currentBox != boxList.Count && iTweenRunning == false)
                {
                    currentBox++;
                    MoveBoxes(direction, speed);
                }
                else return;
                break;
            default:
                return;
        }        
    }

    
    private void MoveBoxes(Direction direction, float speed)
    {
        foreach (var box in boxList)
        {
            var delta = box.GetComponent<RectTransform>().rect.width;
            var dist = box.GetComponent<RectTransform>().localPosition.x + (delta + offset) * (int)direction;

            box.DecreaseBox();
            iTweenRunning = true;
            iTween.MoveTo(box.gameObject, iTween.Hash("position", new Vector3(dist, 0, 0), "islocal", true, "time", speed, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted", "oncomplete", "iTweenCompleted", "oncompletetarget", this.gameObject));
        }

        GetBoxSelected().IncreaseBox();
    }

    public void MoveBoxesOnComplete()
    {
        iTweenRunning = false;
        Debug.Log("MoveBoxesOnComplete");
    }

    public void FillCarouselBox(BoxType boxType)
    {
        switch (boxType)
        {
            case BoxType.MiniGame:
                //CreateBoxes(miniGamesNamesList, miniGamesPictures, boxType);
                var miniGames = MiniGameScreenController.instance.miniGames;
                CreateBoxes(miniGames, boxType);
                break;
            case BoxType.Categoria:
                //var categories = new List<string>(Enum.GetNames(typeof(BoxType))).ToList();
                var categories = MiniGameScreenController.instance.categories;
                CreateBoxes(categories, boxType);
                break;
        }
    }

    public Box GetBoxSelected()
    {
        return boxList[currentBox - 1];
    }

    private void CreateBoxes(string[] boxes, List<Sprite> boxPictures, BoxType type)
    {
        int count = 0;
        currentBox = 1;

        foreach (var boxName in boxes)
        {
            float moveX = 0;

            var box = Instantiate(boxPrefab) as Box;

            switch (type)
            {
                case BoxType.MiniGame:                    
                    //box.gameObject.AddComponent<MiniGameBox>();
                    break;
                case BoxType.Categoria:
                    //box.gameObject.AddComponent<Category>();
                    break;
                case BoxType.Recomendado:
                    break;
                default:
                    break;
            }

            var widht = box.GetComponent<RectTransform>().rect.width;
            
            if(count >= 2)
                moveX = (widht + offset) * (count - 1);

            count++;

            box.transform.SetParent(this.gameObject.transform);
            box.name = type.ToString() + "_" + boxName;
            box.type = type;

            if (count == 1)
            {
                box.transform.localScale = Vector3.one;
                box.transform.localPosition = new Vector3(0, -500, 0);
            }
            else
            {
                box.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                box.transform.localPosition = new Vector3(moveX, 0, 0);
            }

            box.GetComponentInChildren<Text>().text = boxName;
            box.transform.Find("game_image").GetComponent<Image>().sprite = boxPictures[count - 1];
            boxList.Add(box);
            if (type == BoxType.MiniGame && (miniGamesBoxesList.Count <= miniGamesNamesList.Length))
                miniGamesBoxesList.Add(box);
            else if (type == BoxType.Categoria && (categoriesBoxesList.Count <= categoriesNamesList.Length))
                categoriesBoxesList.Add(box);
            else if (type == BoxType.Recomendado && (suggestedBoxesList.Count <= suggestedNamesList.Length))
                suggestedBoxesList.Add(box);
        }
    }

    private void CreateBoxes<T>(IEnumerable<T> list, BoxType type)
    {
        int count = 0;
        currentBox = 1;
        //IEnumerable<MiniGame> minigames = null;
        //IEnumerable<Category> categories = null;

        //switch (type)
        //{
        //    case BoxType.MiniGame:
        //        minigames = (List<MiniGame>) list;
        //        break;
        //    case BoxType.Categoria:
        //        categories = (List<Category>) list;
        //        break;
        //    case BoxType.Recomendado:
        //        break;
        //    default:
        //        break;
        //}

        foreach (var item in list)
        {
            if((item as MiniGame).active)
            {
                float moveX = 0;

                var box = Instantiate(boxPrefab) as Box;

                box.type = type;

                if (type == BoxType.MiniGame)
                {
                    box.name = (item as MiniGame).name;
                    box.picture = (item as MiniGame).picture;
                }
                else if (type == BoxType.Categoria)
                {
                    box.name = (item as Category).name;
                    box.picture = (item as Category).picture;
                    box.category = (item as Category).category;
                }

                box.gameObject.name = type.ToString() + "_" + box.name;
                box.transform.SetParent(this.gameObject.transform);

                var widht = box.GetComponent<RectTransform>().rect.width;

                if (count >= 2)
                    moveX = (widht + offset) * (count - 1);

                count++;

                if (count == 1)
                {
                    box.transform.localScale = Vector3.one;
                    box.transform.localPosition = new Vector3(0, -500, 0);
                }
                else
                {
                    box.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    box.transform.localPosition = new Vector3(moveX, 0, 0);
                }

                box.GetComponentInChildren<Text>().text = box.name;
                box.transform.Find("game_image").GetComponent<Image>().sprite = box.picture;
                boxList.Add(box);
                if (type == BoxType.MiniGame && (miniGamesBoxesList.Count <= miniGamesNamesList.Length))
                    miniGamesBoxesList.Add(box);
                else if (type == BoxType.Categoria && (categoriesBoxesList.Count <= categoriesNamesList.Length))
                    categoriesBoxesList.Add(box);
                else if (type == BoxType.Recomendado && (suggestedBoxesList.Count <= suggestedNamesList.Length))
                    suggestedBoxesList.Add(box);
            }
            
        }
    }

    public void ClearCarouselBox()
    {
        foreach (var box in boxList)
        {
            Destroy(box.gameObject);
        }

        miniGamesBoxesList.Clear();
        categoriesBoxesList.Clear();
        suggestedBoxesList.Clear();
        boxList.Clear();
    }

    public void Show()
    {
        var firstBox = new Box();
        var othersBoxes = new List<Box>();

        var menu = MenuController.instance.GetCurrentMenuActive();
        
        if(menu.type == ButtonType.MiniGames)
        {
            firstBox = miniGamesBoxesList.First();
            othersBoxes = miniGamesBoxesList.Where(x => x != firstBox).ToList();
        }
        else if(menu.type == ButtonType.Categorias)
        {
            firstBox = categoriesBoxesList.First();
            othersBoxes = categoriesBoxesList.Where(x => x != firstBox).ToList();
        }
        else if(menu.type == ButtonType.Recomendados)
        {
            firstBox = suggestedBoxesList.First();
            othersBoxes = suggestedBoxesList.Where(x => x != firstBox).ToList();
        }

        //var firstBox = boxList.First();

        //FIXME - Ao sair da tela de gameover, abrindo a opção de minigame os boxes do caroussel estão ficando sobrepostos
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(0, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted", "oncomplete", "iTweenCompleted"));
        iTween.MoveTo(firstBox.gameObject, iTween.Hash("position", new Vector3(0, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted", "oncomplete", "iTweenCompleted"));

        foreach (var box in othersBoxes)
        {
            var delta = box.GetComponent<RectTransform>().rect.width;
            var dist = box.GetComponent<RectTransform>().localPosition.x + (delta + offset) * (int)Direction.Left;


            iTween.MoveTo(box.gameObject, iTween.Hash("position", new Vector3(dist, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted", "oncomplete", "iTweenCompleted"));    
        }
    }

    public void Hide()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(0, -800, 0), "islocal", true, "time", 1f, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted", "oncomplete", "iTweenCompleted", "oncompletetarget", this.gameObject));
        ClearCarouselBox();
    }

    //private void HideRunning()
    //{
    //    //iTweenHideRunning = true;
    //    Debug.Log("HideRunning");
    //}

    //private void HideOnComplete ()
    //{
    //    //iTweenHideRunning = false;
    //    ClearCarouselBox();
    //    Debug.Log("HideOnComplete");
    //}

    private void iTweenStarted()
    {
        iTweenRunning = true;
    }

    private void iTweenCompleted()
    {
        iTweenRunning = false;
    }

    public void SelectMiniGamesByCategory(Box box)
    {
        var miniGames = MiniGameScreenController.instance.miniGames.Where(x => x.categoryList.Any(c => c == box.category)).ToList();
        
        ClearCarouselBox();
        CreateBoxes(miniGames, BoxType.MiniGame);
        ShowMiniGamesByCategory();
    }

    public void ShowMiniGamesByCategory()
    {
        var firstBox = miniGamesBoxesList.First();
        var othersBoxes = miniGamesBoxesList.Where(x => x != firstBox).ToList();

        iTween.MoveTo(firstBox.gameObject, iTween.Hash("position", new Vector3(0, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted"));

        foreach (var box in othersBoxes)
        {
            var delta = box.GetComponent<RectTransform>().rect.width;
            var dist = box.GetComponent<RectTransform>().localPosition.x + (delta + offset) * (int)Direction.Left;


            iTween.MoveTo(box.gameObject, iTween.Hash("position", new Vector3(dist, 0, 0), "islocal", true, "time", 0.5f, "easytype", iTween.EaseType.linear, "onstart", "iTweenStarted"));
        }
    }
}

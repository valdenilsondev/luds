using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Share.Managers;
using Assets.Scripts.Share;

public class StartScreenKinectController : MonoBehaviour
{

    private float currentHandY;
    private float currentHandX;
	private float currentHandCursorX;
	private float currentHandCursorY;
    private float hipHeight;
    private float centerChest;
    private float headY;

    private bool userInFrontOfKinect = false;
    private bool triggerLoading = false;
    private KinectManager kinect;

    private Color white;
	private Color colorOnSelection;
	
	public GameObject creditsCube;
    public GameObject gameCube;
    public GameObject creditsBackBtn;
	public GameObject miniGamesBackBtn;

    public bool miniGameStateStartScreen;
    public bool creditsStateStartScreen;
    public bool creditsBackBtnStateStartScreen;
	public bool miniGameSelectionAreaStateStartScreen;
	public bool miniGamesBackBtnStateStartScreen;
	public bool miniGamesRightSideStateStartScreen;
	public bool miniGamesLeftSideStateStartScreen;

    public bool isMiniGameActived = false;
    public bool isCreditsActived = false;

	private GameObject GodRayCredits;
	private GameObject GodRayMiniGames;

    public Text debug;

    public static StartScreenKinectController instance;

    void Awake()
    {
        instance = this;
        kinect = KinectManager.Instance;
    }

    void Start()
    {
        white = new Color(1, 1, 1, 1);
        colorOnSelection = new Color(1, 1, 0, 1);

		GodRayCredits = GameObject.Find("GodRayLightCredits");
		GodRayMiniGames = GameObject.Find("GodRayLightMiniGames");

		DeactivateGodRayLights();
    }

    // Update is called once per frame
    void Update()
    {
        if (kinect == null) return;

        userInFrontOfKinect = kinect.IsUserDetected();

        hipHeight = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).y;
        centerChest = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter).x;
        
        currentHandY = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y;

        currentHandX = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x;

        headY = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y;

		if(GameManagerShare.instance.game == Game.startScreenNew){
	        if (currentHandY > hipHeight && userInFrontOfKinect)
	        {
	            ManageStates();
				ControlMiniGamesScreen();

	            debug.text = "iTween(s): " + iTween.tweens.Count.ToString();
	        }
	        else if (currentHandY < hipHeight || !userInFrontOfKinect)
	        {
				SetAllStatesToFalse();            
	            ResetCubesColors();

	            LoadingSelection.instance.StopLoadingSelection();
	        }
		}
    }

	public void ControlMiniGamesScreen(){
		//debug.GetComponent<Text>().text = "X: "+PlayerHandController.instance.GetPlayerCursor().x.ToString()+" Y "+PlayerHandController.instance.GetPlayerCursor().y.ToString();
		currentHandCursorX = PlayerHandController.instance.GetPlayerCursor().x;
		currentHandCursorY = PlayerHandController.instance.GetPlayerCursor().y;

		if(isMiniGameActived && iTween.tweens.Count == 0){
			if(currentHandCursorY > Screen.height / 3 && currentHandCursorY < Screen.height / 1.6f){
				//aumentando a zona de alcance do cursor com o objeto mais proximo
				SelectionController.instance.SetMinimalDistancetoObject(250);
				//
				if (currentHandCursorX > Screen.width * 0.68f  &&  currentHandCursorX < Screen.width * 0.8f)
	            {//direita 1
					miniGamesLeftSideStateStartScreen = true;
					miniGamesRightSideStateStartScreen = false;
	                MiniGameScreenController.instance.MoveGameBoxKinect(Direction.Right, 1.2f);
				}
				else if (currentHandCursorX > Screen.width * 0.8f  &&  currentHandCursorX < Screen.width)
				{//direita 2
					miniGamesLeftSideStateStartScreen = true;
					miniGamesRightSideStateStartScreen = false;
					MiniGameScreenController.instance.MoveGameBoxKinect(Direction.Right, 0.5f);
				}
				else if (currentHandCursorX > Screen.width * 0.21f  &&  currentHandCursorX < Screen.width * 0.31f)
				{//esquerda 1
					miniGamesLeftSideStateStartScreen = false;
					miniGamesRightSideStateStartScreen = true;
					MiniGameScreenController.instance.MoveGameBoxKinect(Direction.Left, 1.2f);
				}
				else if (currentHandCursorX > 0  &&  currentHandCursorX < Screen.width * 0.21f)
				{//esquerda 2
					miniGamesLeftSideStateStartScreen = true;
					miniGamesRightSideStateStartScreen = false;
					MiniGameScreenController.instance.MoveGameBoxKinect(Direction.Left, 0.5f);
				}
				else
				{
					miniGamesLeftSideStateStartScreen = false;
					miniGamesRightSideStateStartScreen = false;
				}

				if(currentHandCursorX > Screen.width * 0.35f && currentHandCursorX < Screen.width * 0.65f && miniGameSelectionAreaStateStartScreen == false)
				{//centro
					miniGamesLeftSideStateStartScreen = false;
					miniGamesRightSideStateStartScreen = false;

					LoadingSelection.instance.StopLoadingSelection();
					LoadingSelection.instance.StartLoadingSelection();
					SelectionController.instance.ForceMiniGameSelectionArea();

				}else if(currentHandCursorX < Screen.width * 0.35f || currentHandCursorX > Screen.width * 0.65f)
				{
					SelectionController.instance.ResetMiniGameSelectionArea();
					miniGameSelectionAreaStateStartScreen = false;
				}

			}else{
				//resetando valor padrao do alcance do cursor com o objeto mais proximo
				SelectionController.instance.ResetMinimalDistanceToObejct();
				//teste
				SelectionController.instance.ResetMiniGameSelectionArea();
				miniGameSelectionAreaStateStartScreen = false;
			}
		}
	}

	private void SetAllStatesToFalse()
	{
		miniGameStateStartScreen = false;
		creditsStateStartScreen = false;
		creditsBackBtnStateStartScreen = false;
		miniGamesBackBtnStateStartScreen = false;
		miniGamesRightSideStateStartScreen = false;
		miniGamesLeftSideStateStartScreen = false;
		miniGameSelectionAreaStateStartScreen = false;
		SelectionController.instance.ResetMiniGameSelectionArea();
	}
	
	public void StartScreenOnCreditsEnter()
    {
        miniGameStateStartScreen = false;
        creditsStateStartScreen = false;
		ResetCubesColors();
		LoadingSelection.instance.StopLoadingSelection();
    }

    public void StartScreenOnCreditsExit()
    {
        creditsBackBtn.GetComponent<Image>().color = Color.white;
        miniGameStateStartScreen = false;
        creditsStateStartScreen = false;
        ResetCubesColors();
		LoadingSelection.instance.StopLoadingSelection();
    }

	public void StartScreenOnMiniGamesEnter()
	{
		miniGameStateStartScreen = false;
		creditsStateStartScreen = false;
		ResetCubesColors();
		LoadingSelection.instance.StopLoadingSelection();
	}

	public void StartScreenOnMiniGamesExit()
	{
		miniGamesBackBtn.GetComponent<Image>().color = Color.white;
		miniGameStateStartScreen = false;
		creditsStateStartScreen = false;
		ResetCubesColors();
		LoadingSelection.instance.StopLoadingSelection();
	}

	private void ActivateGodRayLightCredits(){
		GodRayCredits.SetActive(true);
	}

	private void ActivateGodRayLightMiniGames(){
		GodRayMiniGames.SetActive(true);
	}

	private void DeactivateGodRayLights(){
		GodRayCredits.SetActive(false);
		GodRayMiniGames.SetActive(false);
	}

    private void ResetCubesColors(){
        gameCube.GetComponent<Renderer>().material.color = white;
        creditsCube.GetComponent<Renderer>().material.color = white;
		creditsBackBtn.GetComponent<Image>().color = Color.white;
		miniGamesBackBtn.GetComponent<Image>().color = Color.white;
		DeactivateGodRayLights();
	}

    private void ManageStates()
    {
        //TODO - Verificar calculo de distancia dos boxes
        var status = SelectionController.instance.ReturnCloserObjectToHandInStartScreen();

        switch (status)
        {
            //TODO - Utilizar metodos para reinicializar valor default para variáveis de controle
            case SelectionController.InterfaceStartScreenEnum.StartScreenCredits:
                if (creditsStateStartScreen == false && !isCreditsActived && !isMiniGameActived)
                {
                    creditsStateStartScreen = true;
                    miniGameStateStartScreen = false;
                    gameCube.GetComponent<Renderer>().material.color = white;
					ActivateGodRayLightCredits();
                    creditsCube.GetComponent<Renderer>().material.color = colorOnSelection;
					LoadingSelection.instance.StopLoadingSelection();
                    LoadingSelection.instance.StartLoadingSelection();
                    BarnBalloonsShowHide.instance.ShowCreditsBallon();
                }
                break;
            case SelectionController.InterfaceStartScreenEnum.StartScreenMiniGame:
			if (miniGameStateStartScreen == false && !isCreditsActived && !isMiniGameActived)
                {
                    creditsStateStartScreen = false;
                    miniGameStateStartScreen = true;
                    creditsCube.GetComponent<Renderer>().material.color = white;
					ActivateGodRayLightMiniGames();
                    gameCube.GetComponent<Renderer>().material.color = colorOnSelection;
					LoadingSelection.instance.StopLoadingSelection();
                    LoadingSelection.instance.StartLoadingSelection();
					BarnBalloonsShowHide.instance.ShowGamesBallon();
                }
                break;
            case SelectionController.InterfaceStartScreenEnum.StartScreenCreditsBackBtn:
			if (creditsBackBtnStateStartScreen == false && isCreditsActived && !isMiniGameActived)
                {
                    creditsBackBtnStateStartScreen = true;
                    creditsStateStartScreen = false;
                    miniGameStateStartScreen = false;
                    creditsBackBtn.GetComponent<Image>().color = colorOnSelection;
					LoadingSelection.instance.StopLoadingSelection();
                    LoadingSelection.instance.StartLoadingSelection();
                }
                break;
			case SelectionController.InterfaceStartScreenEnum.StartScreenMiniGamesBackBtn:
				if(miniGamesBackBtnStateStartScreen == false && isCreditsActived == false && isMiniGameActived == true)
				{
					miniGamesBackBtnStateStartScreen = true;
					creditsStateStartScreen = false;
					miniGameStateStartScreen = false;
					miniGamesBackBtn.GetComponent<Image>().color = colorOnSelection;
					LoadingSelection.instance.StopLoadingSelection();
					LoadingSelection.instance.StartLoadingSelection();
				}
				break;		
			case SelectionController.InterfaceStartScreenEnum.StartScreenMiniGameSelection:
				if(miniGameSelectionAreaStateStartScreen == false){
					miniGamesBackBtnStateStartScreen = false;
					miniGameSelectionAreaStateStartScreen = true;

                    var btn = new MenuButton { type = ButtonType.MiniGames };
                    MenuController.instance.Initialize(btn);
				}
				break;
            case SelectionController.InterfaceStartScreenEnum.StartScreenCategoriesSelection:
                if (miniGameSelectionAreaStateStartScreen == false)
                {
                    miniGamesBackBtnStateStartScreen = false;
                    miniGameSelectionAreaStateStartScreen = true;

                    var btn = new MenuButton { type = ButtonType.Categorias };
                    MenuController.instance.Initialize(btn);
                }
                break;
            default:
                if (isCreditsActived == false && isMiniGameActived == false)
                {
                    creditsStateStartScreen = false;
                    miniGameStateStartScreen = false;
                    ResetCubesColors();
                }
                else
                {
					ResetCubesColors();
                }

				SetAllStatesToFalse();
				//SoundManager.Instance.ChangeSelection();
                LoadingSelection.instance.StopLoadingSelection();
				BarnBalloonsShowHide.instance.NewState();
                break;
        }
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Runner.Managers;
using Share.Managers;
using UnityEngine.SceneManagement;

public class PlayerHandController : MonoBehaviour
{

    private KinectManager kinect;

    private float playerHandPosX;
    private float playerHandPosY;

    public float minimumValueX;
    public float maximumValueX;
    private float minimumValueY;
    private float maximumValueY;

    private bool minTriggerValueX;
    private bool minTriggerValueY;

    private bool isRightHand = true;
    private bool isLefttHand = false;

    public float HandSmoothFactor;
    public Image PlayerCursor;
    public Sprite cursorRightTexture;
    public Sprite cursorLeftTexture;

    private uint playerID;
    public static PlayerHandController instance;

    private int screenWidth = 1920;
    private int screenHeight = 1080;

    private float minLimitPlayerCursorX = 0;
    private float minLimitPlayerCursorY = 0;
    private float maxLimitPlayerCursorX;
    private float maxLimitPlayerCursorY;

    public enum UsedHand
    {
        Right,
        Left
    }

    private static UsedHand myHand;

    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    void Start()
    {
        kinect = KinectManager.Instance;

        PlayerCursor = this.GetComponent<Image>();
        PlayerCursor.transform.SetAsFirstSibling();
		if(kinect != null)
        	playerID = kinect.GetPlayer1ID();
        maxLimitPlayerCursorX = screenWidth - PlayerCursor.rectTransform.rect.width;
        maxLimitPlayerCursorY = screenHeight - PlayerCursor.rectTransform.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
		if (kinect != null) {
			if (!kinect.IsUserDetected ())
				playerID = 0;
			else
				playerID = kinect.GetPlayer1ID ();
		}
        var levelName = SceneManager.GetActiveScene().name;

        switch (levelName)
        {
            case "Calibration":
                if (isCursorActive())
                    HideCursor();
                break;
			case "startScreenNew":
                if (!isCursorActive())
                    ShowCursor();

                if (CheckIsHeadNull())
                {
                    if (isRightHand)
                    {
                        playerHandPosY = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y;
                        playerHandPosX = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x;

                        HandlePlayerCursorBasedOnKinect(playerHandPosX, playerHandPosY);
                    }
                    else if (isLefttHand)
                    {
                        playerHandPosY = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft).y;
                        playerHandPosX = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft).x;

                        HandlePlayerCursorBasedOnKinect(playerHandPosX, playerHandPosY);
                    }
                }
                break;
			/*case "Fishing":
				if(KinectGestures.isInPoseStop && GameManagerShare.IsStarted() &&
				   !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() && !CountDownManager.instance.IsCounting())
				{
					ShowCursor();
					Debug.Log ("Showcursor do pause");
				//para mao direita
					if (isRightHand && CheckIsHeadNull())
					{
						playerHandPosY = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y;
						playerHandPosX = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x;
						HandlePlayerCursorBasedOnKinect(playerHandPosX, playerHandPosY);
					}
				}
				if(!KinectGestures.isInPoseStop){
					HideCursor();
				}
				break;*/
            default:
                //se sair da tela HideCursos() else
							
				if (GameManagerShare.IsStarted() && (GameManagerShare.IsPaused() || GameManagerShare.IsGameOver())
			    	|| (!GameManagerShare.IsStarted() && TutorialScreen.instance.isTutorialOn) )
	            {
					if(TutorialScreen.instance.isTutorialOn)
					{
						ShowCursor();
					}
	                    if (DistanceCalibrator.instance.CheckOutOfScreen() == false)
	                    {
	                        if (isCursorActive() == false)
	                            ShowCursor();
	                        if (isRightHand && CheckIsHeadNull())
	                        {
	                            playerHandPosY = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y;
	                            playerHandPosX = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x;
	                            HandlePlayerCursorBasedOnKinect(playerHandPosX, playerHandPosY);
	                        }
						}
						else
	                    {
	                        HideCursor();
	                       // LoadingSelection.instance.StopLoadingSelection();
	                    }
					}

					else
	                {
	                    HideCursor();
	                    //LoadingSelection.instance.StopLoadingSelection();
	                }
				
				break;
        }
    }

    private bool CheckIsHeadNull()
    {
        return playerID > 0 && kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y > 0;
    }

    private void CheckIfPlayerChanged()
    {
        //em teste
        if (!kinect.IsUserDetected())
        {
            maximumValueY = 0;
            minimumValueY = 0;
        }
    }

    public void HideCursor()
    {
        this.GetComponentInChildren<Image>().enabled = false;
    }

    public void ShowCursor()
    {
        this.GetComponentInChildren<Image>().enabled = true;
    }

    public bool isCursorActive()
    {
        return this.GetComponentInChildren<Image>().IsActive();
    }

    private void HandlePlayerCursorBasedOnKinect(float posX, float posY)
    {
        //x -> 0 a 1920 y -> 0 a 1080
        //assumindo que a posicao da mao do player em x vai de 0 a 0.4, se o valor de x capturado pelo kinect for multiplicado pela divisao 1920/0.4 = 4800
        //obteremos o valor necessario para manipulacao do cursor na coordenada x, o mesmo eh feito com a coordenada y, foi verificado uma altura minima de
        // 1.5 e maxima de 1.8, uma diferenca de 0 a 0.3, ou seja 1080/0.3 = 3600

        float calcX = 0; float calcY = 0;
        var teste = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y;
        if (minimumValueY <= 0 && maximumValueY <= 0 && teste > 0)
        {
            maximumValueY = kinect.GetJointPosition(playerID, (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y - 0.1f;
            minimumValueY = maximumValueY - 0.3f;
        }

        if (minimumValueX != 0)
        {
            posX = posX - minimumValueX;
            if (minTriggerValueX == false)
            {
                maximumValueX = maximumValueX - minimumValueX;
                minTriggerValueX = true;
            }
        }

        if (minimumValueY != 0)
        {
            posY = posY - minimumValueY;
            if (minTriggerValueY == false)
            {
                maximumValueY = maximumValueY - minimumValueY;
                minTriggerValueY = true;
            }
        }

        calcX = 1920 / maximumValueX;
        calcY = 1080 / maximumValueY;

        var distX = posX * calcX;
        var distY = posY * calcY;

        if (distX < minLimitPlayerCursorX)
            distX = minLimitPlayerCursorX;
        if (distX > maxLimitPlayerCursorX)
            distX = maxLimitPlayerCursorX;

        if (distY < minLimitPlayerCursorY)
            distY = minLimitPlayerCursorY;
        if (distY > maxLimitPlayerCursorY)
            distY = maxLimitPlayerCursorY;

        PlayerCursor.rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(PlayerCursor.rectTransform.anchoredPosition.x, distX, Time.deltaTime * HandSmoothFactor),
                                                                   Mathf.Lerp(PlayerCursor.rectTransform.anchoredPosition.y, distY, Time.deltaTime * HandSmoothFactor));
    }

    public void HandlePlayerCursorWithMouse()
    {
        PlayerCursor.rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public Vector3 GetPlayerCursor()
    {
        return PlayerCursor.transform.position;
    }

    public void SetHand(UsedHand hand)
    {
        myHand = hand;

        if (myHand == UsedHand.Left)
        {
            isRightHand = false;
            isLefttHand = true;
            PlayerCursor.GetComponent<Image>().sprite = cursorLeftTexture;
            //definir textura
            //PlayerCursor.gameObject.texture... = texture...
            //

        }
        else if (myHand == UsedHand.Right)
        {
            isRightHand = true;
            isLefttHand = false;
            PlayerCursor.GetComponent<Image>().sprite = cursorRightTexture;
            //definir textura
            //PlayerCursor.gameObject.texture... = texture...
            //
        }
    }

    public static UsedHand GetHand()
    {
        return myHand;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    LoadingSelection.instance.StartLoadingSelection();
    //}

    //void OnTriggerStay2D(Collider2D collider)
    //{
    //    if(LoadingSelection.instance.IsTimerComplete())
    //    {
    //        switch (collider.name)
    //        {
    //            case "Menu_Button":
    //                GameOverSelection.instance.goToMenuState = true;
    //                break;
    //            case "Restart_Button":
    //                GameOverSelection.instance.restartGameState = true;
    //                break;
    //            case "MiniGame_Button":
    //                GameOverSelection.instance.miniGameState = true;
    //                break;
    //            default:
    //                break;
    //        }

    //        LoadingSelection.instance.ApplySelection();
    //    }
    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    LoadingSelection.instance.StopLoadingSelection();
    //}
}

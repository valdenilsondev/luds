using UnityEngine;
using System.Collections;
using Share.EventSystem;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class MouseOnClickWall : MonoBehaviour
{

    //background
    Vector3 background_cam_position = new Vector3(0, 1, 2.65f);
    //Vector3 background_cam_rotate = new Vector3 (6,9,0);
    bool click_blackground = false;

    //config e minigame
    private Vector3 config_cam_position;
    private Vector3 mini_game_cam_position;

    bool click_config = false;
    bool click_mini_game = false;

    public float time;

    Vector3 initial_position = new Vector3(0, -2f, -20f);
    Vector3 atual_position = new Vector3(0, 1.5f, -5.3f);
    Vector3 minigames_box_position = new Vector3(4, -4.4f, -11);
    Vector3 credits_box_position = new Vector3(-3.1f, -4.4f, -1.4f);

    //variavel de controle quando o player clicar no botao de voltar
    private bool click_config_back;
    public bool click_mini_game_back;

    // Use this for initialization
    private static Animation play_screen_anim;
    private static Animation config_screen_anim;
    private static Animator mini_game_screen_anim;

    //private static Collider background_cube;
    private static Collider config_cube;
    public static Collider mini_game_cube;

    public static Animation credits_screen;
    bool click_credits;
    bool credits_flag = true;
    public static MouseOnClickWall instance;

    //referencia para a camera da barn
    private GameObject cameraGO;

    //variavel de controle da animacao da camera
    private bool cameraMovingToCredits = false;
    private bool cameraMovingToMiniGame = false;
    private bool cameraMovingToInitialPosition = false;
    private bool cameraBackFromCredits = false;
    private bool cameraBackFromMiniGames = false;

    //forcar ir para minigames
	private bool check = false;
	public static bool goToMiniGames = false;

    void Awake()
    {
        instance = this;

        //recebendo referencias das animacoes
        /*credits_screen = GameObject.Find("credits_screen").GetComponent<Animation>();
        play_screen_anim = GameObject.Find("PlayScreen").GetComponent<Animation>();
        config_screen_anim = GameObject.Find("CreditsScreen").GetComponent<Animation>();
        mini_game_screen_anim = GameObject.Find("MiniGamesScreen").GetComponent<Animator>();

        //recebendo referencia para os colisores das caixas
        //background_cube = GameObject.Find ("game_cube").GetComponent<Collider> ();
        config_cube = GameObject.Find("box_credits").GetComponent<Collider>();
        mini_game_cube = GameObject.Find("box_minigames").GetComponent<Collider>();

        //recebendo posicoes das caixas
        config_cam_position = config_cube.transform.position;
        mini_game_cam_position = mini_game_cube.transform.position;

        //referencia para a camera principal
        cameraGO = GameObject.Find("CameraGO").gameObject;*/
    }

    void Start()
    {
        if (time == 0)
        {
            time = 30;
        }
        cameraGO.gameObject.GetComponent<BlurOptimized>().enabled = false;
    }

    //TODO - Retirar o iTween, substituir por Animation
    // Update is called once per frame
    void Update()
    {
		if(!check && goToMiniGames){
			ClickMiniGame();
			check = true;
		}

        if (click_credits)
        {
            print("pegou o botao!");
            credits_screen.Play("ShowPlayScreen");
            credits_flag = !credits_flag;
            click_credits = false;
        }

        /*	if (click_blackground) {
             background_cube.enabled = false;
                MoveTo (background_cam_position);
                if (Vector3.Distance (Camera.main.transform.position, background_cam_position) == 0) {
                    click_blackground = false;
                    play_screen_anim.Play ("ShowPlayScreen");				
                }
            }*/

        if (click_config)
        {
            CallCreditsScreen();
			cameraGO.transform.GetComponent<Animator>().SetTrigger("credits");
           // iTween.MoveTo(cameraGO, iTween.Hash("path", iTweenPath.GetPath("creditsIn"), "easetype", iTween.EaseType.linear, "time", 1));
        }

        if (click_mini_game)
        {
            CallMiniGameScreen();
			cameraGO.transform.GetComponent<Animator>().SetTrigger("miniGames");
            //iTween.MoveTo(cameraGO, iTween.Hash("path", iTweenPath.GetPath("miniGamesIn"), "easetype", iTween.EaseType.linear, "time", 1));
        }

        if (cameraMovingToCredits)
        {
            //iTween.LookUpdate(cameraGO, config_cube.transform.position, 0.5f);//500*Time.deltaTime*/);
            iTween.LookUpdate(cameraGO, iTween.Hash("looktarget", config_cube.transform.position, "easytype", iTween.EaseType.linear, "time", 4));

            if (Vector3.Distance(Camera.main.transform.position, config_cam_position) <= 2)
            {
                config_screen_anim.Play("ShowPlayScreen");
                cameraMovingToCredits = false;
                cameraGO.transform.position = credits_box_position;
                cameraGO.gameObject.GetComponent<BlurOptimized>().enabled = true;
                iTween.Stop();
            }
        }

        if (cameraMovingToMiniGame)
        {
            iTween.LookUpdate(cameraGO, iTween.Hash("looktarget", mini_game_cube.transform.position, "easytype", iTween.EaseType.linear, "time", 4));

            if (Vector3.Distance(Camera.main.transform.position, mini_game_cam_position) <= 2f)
            {
                //mini_game_screen_anim.SetTrigger("showMiniGamesScreen");

                

                cameraMovingToMiniGame = false;
                cameraGO.transform.position = minigames_box_position;
                cameraGO.gameObject.GetComponent<BlurOptimized>().enabled = true;
                iTween.RotateTo(cameraGO, iTween.Hash("rotation", new Vector3(0, 47, 0), "time", 1f, "oncompletetarget", this.gameObject ,"oncomplete", "oncompletecameraGO"));
                MiniGameScreenController.instance.Initialize();
            }
        }

        if (cameraMovingToInitialPosition)
        {
            cameraGO.gameObject.GetComponent<BlurOptimized>().enabled = false;
            if (cameraBackFromCredits)
            {
                iTween.LookUpdate(cameraGO, config_cube.transform.position, 0.02f);//500*Time.deltaTime*/);
            }

            if (cameraBackFromMiniGames)
            {
                iTween.LookUpdate(cameraGO, mini_game_cube.transform.position, 0.02f/*500*Time.deltaTime*/);
            }

            if (Vector3.Distance(cameraGO.transform.position, initial_position) <= 0.5f)
            {
                cameraBackFromCredits = false;
                cameraBackFromMiniGames = false;

                if (cameraMovingToInitialPosition == true)
                {
                    cameraMovingToInitialPosition = false;
                    iTween.RotateTo(cameraGO, new Vector3(10, 0, 0), 1f);
                    cameraGO.transform.position = initial_position;
                }
            }
        }

        if (click_config_back)
        {
            //iTween.MoveTo(cameraGO, iTween.Hash("path", iTweenPath.GetPath("backFromCredits"), "easetype", iTween.EaseType.linear, "time", 1));
			cameraGO.transform.GetComponent<Animator>().SetTrigger("returnCredits");
            cameraMovingToInitialPosition = true;
            click_config_back = false;
            cameraBackFromCredits = true;
        }

        if (click_mini_game_back)
        {
			cameraGO.transform.GetComponent<Animator>().SetTrigger("returnMiniGames");
            //iTween.MoveTo(cameraGO, iTween.Hash("path", iTweenPath.GetPath("backFromMiniGames"), "easetype", iTween.EaseType.linear, "time", 1));
            cameraMovingToInitialPosition = true;
            click_mini_game_back = false;
            cameraBackFromMiniGames = true;
        }
    }

    public void oncompletecameraGO()
    {
        //MenuController.instance.Initialize();
        //MiniGamesController.instance.MoveGameBoxes();
    }

    public void CallCreditsScreen()
    {
        StartScreenKinectController.instance.isCreditsActived = true;
        config_cube.enabled = false;
        click_config = false;
        cameraMovingToCredits = true;
    }

    public void CallMiniGameScreen()
    {
        StartScreenKinectController.instance.isMiniGameActived = true;
        mini_game_cube.enabled = false;
        click_mini_game = false;
        cameraMovingToMiniGame = true;
    }

    void OnMouseDown()
    {
        if (this.gameObject.tag == "game_cube")
        {
            //	background_cube.enabled = false;
            click_blackground = true;
        }
        if (this.gameObject.tag == "mini_game_cube")
        {
            ClickMiniGameDelay();
        }
        if (this.gameObject.tag == "config_cube")
        {
            ClickCreditsDelay();
        }

    }
    void MoveTo(Vector3 final_position)
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, final_position, time * Time.deltaTime);
        Camera.main.transform.LookAt(this.transform);
        if ((Vector3.Distance(Camera.main.transform.position, final_position) < 0.03))
        {
            Camera.main.transform.position = final_position;
        }
    }

    /*public void BackGroundBackButton () {
        play_screen_anim.Play("HidePlayScreen");
//		background_cube.enabled = true;	
        click_back = true;			
    }*/

    public void ConfigBackButton()
    {
        config_screen_anim.Play("HidePlayScreen");
        config_cube.enabled = true;
        click_config_back = true;
        StartScreenKinectController.instance.isCreditsActived = false;
    }

    public void MiniGameBackButton()
    {
        mini_game_screen_anim.Play("HideMiniGamesScreen");
        //MiniGamesController.instance.HideGameBoxes();
		//MiniGameScreenController.instance.Hide();
        EnableGameCube();
        //click_mini_game_back = true;
        //StartScreenKinectController.instance.isMiniGameActived = false;
    }

    public void EnableGameCube()
    {
        mini_game_cube.enabled = true;
    }

    public void CreditsButton()
    {
        click_credits = true;
        print("credits button!");
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("PigRunner");
    }

    public void ShowPlayScreen(GameObject Screen)
    {
        string tag = Screen.transform.tag;
        Animation screen = GameObject.Find(tag).GetComponent<Animation>();
        screen.Play("ShowPlayScreen");

    }

    public void HidePlayScreen(GameObject Screen)
    {
        string tag = Screen.transform.tag;
        Animation screen = GameObject.Find(tag).GetComponent<Animation>();
        screen.Play("HidePlayScreen");

    }

    public void PlayMiniButton()
    {
        SceneManager.LoadScene("MiniGame");
    }

    public void ClickCredits()
    {
        click_config = true;
    }

    private void ClickCreditsDelay()
    {
        iTween.LookTo(cameraGO, config_cube.transform.position, 0.2f);
        Invoke("ClickCredits", 0.05f);
    }

    public void ClickMiniGame()
    {
        click_mini_game = true;
    }

    private void ClickMiniGameDelay()
    {
        //iTween.LookTo(cameraGO, mini_game_cube.transform.position, 0.4f);
        Invoke("ClickMiniGame", 0.05f);
    }



}

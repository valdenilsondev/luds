using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.Managers;
using Assets.Scripts.Share;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class StartScreenManager : MonoBehaviour {

	//booleans
	private bool on_main_screen = true;
	private bool on_games;
	private bool on_configuration;
	private bool presentationON;
	private bool config_sound = false;

	//ray cast
	private Ray ray;
	private Ray2D ray2d;
	private RaycastHit hit;
	private RaycastHit2D hit2d;

	//strings
	private string message;
	
	//floats
	private float elapsedTime;
	private float elapsedWriteTime = 0;
	private float letterPause = 0.05f;

	//objects
	private GameObject presentationScreen;
	private GameObject selectGameScreen;
	private GameObject ambient_3D;
	private GameObject games_back_btn;
	private GameObject config_back_btn;
	public GameObject[] game_selection_boxes;
	public GameObject[] config_selection_boxes;
	public GameObject texto_explicativo_go;
	public GameObject banner_LevelToLoad;


	//images
	public Sprite gk_story_banner, pig_story_banner, bridge_story_banner, sup_story_banner, throw_story_banner, fishing_story_banner;
	public Sprite gk_tutorial_banner, pig_tutorial_banner, bridge_tutorial_banner, sup_tutorial_banner, throw_tutorial_banner, fishing_tutorial_banner;

	//instance
	public static StartScreenManager instance;

	void Awake(){
		instance = this;
	}

	void Start () {
	//	presentationScreen =  GameObject.Find("Tela_apresentacao_do_Jogo").gameObject;
		selectGameScreen  =  GameObject.Find("Tela_Selecao_de_Jogos").gameObject;
		ambient_3D  =  GameObject.Find("Environment").gameObject;
		games_back_btn = GameObject.Find ("GamesBackButton").gameObject;
		config_back_btn = GameObject.Find ("ConfigBackButton").gameObject;
		banner_LevelToLoad = GameObject.Find("Banner_LevelToLoad").gameObject;
	//	HidePresentation();
		HideGameSelection();
		//seta como a primeira vez que executa o jogo
		//PlayerPrefsManager.SetPresentationFirstTime(1);

		//se for a primeira vez que executa o jogo mostra a apresentacao
	/*	if(PlayerPrefsManager.IsPresentationFirstTime() == 1){
			ShowPresentation();
			presentationON = true;
			PlayerPrefsManager.SetPresentationFirstTime(0);
		}*/
	//	PlayerPrefs.DeleteAll();
	//	PlayerPrefs.Save();
	}

	#region update

	void Update () {
	/*	if(Input.GetKeyDown(KeyCode.C)){
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}*/
		elapsedTime += Time.deltaTime;
		elapsedWriteTime += Time.deltaTime;
	/*	if(elapsedTime>60 && presentationON){
			HidePresentation();
			presentationON = false;
		}*/

		//condicao, tela principal ativa
		if(on_main_screen){
			if(GameManagerShare.instance.IsUsingKinect()){
				StartScreenKinectManager.instance.OnHandOverStartScreenObjects();
			}else{
				OnMouseOverStartScreenObjects();
			}
		}
		//condicao, tela selecao de jogos ativa
		else if(on_games){
			if(GameManagerShare.instance.IsUsingKinect()){
				StartScreenKinectManager.instance.OnHandOverStartScreenBoxes();
				//StartScreenKinectManager.instance.OnHandOverSelectSoundConfig();
			}else{
				OnMouseOverGameSelectionBoxes();	
			}

		}
		//condicao, tela de configuracoes ativa
		else if(on_configuration){
			if(GameManagerShare.instance.IsUsingKinect()){

					StartScreenKinectManager.instance.OnHandOverStartScreenBoxes();		

			}else{
				OnMouseOverGameSelectionBoxes();	
			}
		}
	}

	#endregion

	#region private methods

	private void OnMouseOverStartScreenObjects(){
		//not using kinect
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	
		if(Physics.Raycast(ray, out hit))	{
			if(hit.collider.tag == "games_cube"){
				if(Input.GetMouseButtonDown(0)){
					OnGamesSelectionEnter();
				}				
			}

			if(hit.collider.tag == "config_cube"){
				if(Input.GetMouseButtonDown (0)){
					MoveInOptionScreen();
				}
			}	
		}
	}


	void OnDisable()
	{
		SoundManager.Instance.StopStartscreenSound();
		//print ("tirar som");
	}


	private void OnMouseOverGameSelectionBoxes(){
		//not using kinect
		hit2d = Physics2D.Raycast(new Vector2(Input.mousePosition.x, Input.mousePosition.y), Vector2.zero, 100);

		if(Physics2D.Raycast(new Vector2(Input.mousePosition.x, Input.mousePosition.y), Vector2.zero, 100)){
			for(int i = 0; i < game_selection_boxes.Length; i++){
				if(hit2d.collider.name == "GameBoxImage"+(i+1) ){
					game_selection_boxes[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
				}else{
					game_selection_boxes[i].transform.localScale = new Vector3(1, 1, 1);
				}
			}
		}else{
			for(int i = 0; i < game_selection_boxes.Length; i++){
				game_selection_boxes[i].transform.localScale = new Vector3(1, 1, 1);
			}
		}
	}

	private void HidePresentation(){
		Camera.main.GetComponent<BlurOptimized>().enabled = false;
		presentationScreen.SetActive(false);
	}
	
	private void ShowPresentation(){
		presentationScreen.transform.localScale = new Vector3(1,1,1);
		Camera.main.GetComponent<BlurOptimized>().enabled = true;
		presentationScreen.SetActive(true);
		/*message = GameObject.Find("texto_explicativo").GetComponent<Text>().text;
		texto_explicativo_go = GameObject.Find("texto_explicativo");
		texto_explicativo_go.GetComponent<Text>().text = "";*/
		StartCoroutine(ClosePresentation ());
	}

	private void MoveInGameSelection(){
		//iTween.MoveTo(selectGameScreen, iTween.Hash("x", 0, "y", 0, "islocal", true, "time", 1, "oncompletetarget", this.gameObject, "oncomplete", "OnMoveInGameSelectionComplete"));
		//iTween.RotateTo(selectGameScreen, Vector3.zero, 1);
		iTween.ScaleTo(selectGameScreen, iTween.Hash("scale", new Vector3(1,1,1), "islocal", true, "time", 1, "oncompletetarget", this.gameObject, "oncomplete", "OnMoveInGameSelectionComplete"));
	}

	private void MoveOutGameSelection(){
		//iTween.MoveTo(selectGameScreen, iTween.Hash("x", -460, "y", 115, "islocal", true, "time", 1));
		//iTween.RotateTo(selectGameScreen, new Vector3(0,0,12), 1);
		iTween.ScaleTo(selectGameScreen, iTween.Hash("scale", Vector3.zero, "islocal", true, "time", 1));

	}

	private void MoveInOptionScreen(){
		OptionScreen.instance.ShowScreen();
	}

	private void MoveOutOptionScreen(){
		OptionScreen.instance.HideScreen();
	}

	private void OnMoveInGameSelectionComplete(){
		ambient_3D.SetActive(false);
	}

	private void HideGameSelection(){
		selectGameScreen.SetActive(false);	
	}
	
	private void ShowGameSelection(){
		selectGameScreen.SetActive(true);	
	}

	IEnumerator ClosePresentation(){
		yield return new WaitForSeconds (5);
		HidePresentation();
	}

	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) {
			//GetComponent<GUIText>().text += letter;	
			texto_explicativo_go.GetComponent<Text>().text += letter;			
			if(texto_explicativo_go.GetComponent<Text>().text == message){
				yield return new WaitForSeconds (5);
				HidePresentation();
			}
			/*if (sound)
				audio.PlayOneShot (sound);*/
			yield return 0;
			yield return new WaitForSeconds (letterPause);
		}      
	}

	#endregion

	#region public methods

	public GameObject[] GetGameSelectionBoxes(){
		return game_selection_boxes;
	}

	public GameObject[] GetConfigSelectionBoxes(){
		return config_selection_boxes;
	}

	public void OnBackButtonPress(){
		if(on_games){
			MoveOutGameSelection();
		}else if(on_configuration){
			MoveOutOptionScreen();
		}
		on_main_screen = true;
		on_configuration = false;
		on_games = false;
		ambient_3D.SetActive(true);
	}

	public void OnGamesSelectionEnter(){
		if(selectGameScreen.activeSelf == false){
			ShowGameSelection();
		}
		MoveInGameSelection();
		on_games = true;
		on_main_screen = false;
		//ambient_3D.SetActive(false);
	}

	public void OnConfigurationEnter(){
		OptionScreen.instance.ShowScreen();
		on_configuration = true;
		on_main_screen = false;
		ambient_3D.SetActive(false);
	}

	public void OnMouseOverBackButton(GameObject go){
		go.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
	}

	public void OnMouseExitBackButton(GameObject go){
		go.transform.localScale = new Vector3(1, 1, 1);
	}

	public bool IsOnGames(){
		return on_games;
	}

	public bool IsOnMainScreen(){
		return on_main_screen;
	}

	public bool IsOnConfigurations(){
		return on_configuration;
	}

	public GameObject GetGamesBackBtn(){
		return games_back_btn;
	}

	public GameObject GetConfigBackBtn(){
		return config_back_btn;
	}

	//on mouse click
	public void OnGameEnter(GameObject go){
		var game_name = go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text;
		banner_LevelToLoad.GetComponent<Image>().sprite = gk_tutorial_banner;
		banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
		if(game_name == "A FAZENDA"){
			SceneManager.LoadScene("GoalKeeper");
		}else if(game_name == "O CIRCO"){
			SceneManager.LoadScene("Throw");
		}else if(game_name == "A ESTRADA"){	
			//SceneManager.LoadScene("PigRunner");
			SceneManager.LoadScene("Corridadavacina");
		}
		else if(game_name == "A PONTE"){
			SceneManager.LoadScene("Bridge");
		}else if(game_name == "O VALE"){
			SceneManager.LoadScene("Sup");
		}else if(game_name == "O LAGO"){
			SceneManager.LoadScene("Fishing");
		}
	/*	if(game_name == "A FAZENDA"){
			if(GameManagerShare.instance.IsThisGameFirstTime(Game.Goal_Keeper)){
				banner_LevelToLoad.GetComponent<Image>().sprite = gk_story_banner;
			}else{
				banner_LevelToLoad.GetComponent<Image>().sprite = gk_tutorial_banner;
			}
			banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
			SceneManager.LoadScene("GoalKeeper");
		}else if(game_name == "O CIRCO"){
			if(GameManagerShare.instance.IsThisGameFirstTime(Game.Throw)){
				banner_LevelToLoad.GetComponent<Image>().sprite = throw_story_banner;
			}else{
				banner_LevelToLoad.GetComponent<Image>().sprite = throw_tutorial_banner;
			}		
			banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
			SceneManager.LoadScene("Throw");
		}else if(game_name == "A ESTRADA"){
			if(GameManagerShare.instance.IsThisGameFirstTime(Game.Pig)){
				banner_LevelToLoad.GetComponent<Image>().sprite = pig_story_banner;
			}else{
				banner_LevelToLoad.GetComponent<Image>().sprite = pig_tutorial_banner;
			}
			banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
			SceneManager.LoadScene("PigRunner");
		}else if(game_name == "A PONTE"){
			if(GameManagerShare.instance.IsThisGameFirstTime(Game.Bridge)){
				banner_LevelToLoad.GetComponent<Image>().sprite = bridge_story_banner;
			}else{
				banner_LevelToLoad.GetComponent<Image>().sprite = bridge_tutorial_banner;
			}
			banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
			SceneManager.LoadScene("Bridge");
		}else if(game_name == "O VALE"){
			if(GameManagerShare.instance.IsThisGameFirstTime(Game.Sup)){
				banner_LevelToLoad.GetComponent<Image>().sprite = sup_story_banner;
			}else{
				banner_LevelToLoad.GetComponent<Image>().sprite = sup_tutorial_banner;
			}
			banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
			SceneManager.LoadScene("Sup");
		}else if(game_name == "O LAGO"){
			if(GameManagerShare.instance.IsThisGameFirstTime(Game.Fishing)){
				banner_LevelToLoad.GetComponent<Image>().sprite = fishing_story_banner;
			}else{
				banner_LevelToLoad.GetComponent<Image>().sprite = fishing_tutorial_banner;
			}
			banner_LevelToLoad.GetComponent<Image>().SetNativeSize();
			SceneManager.LoadScene("Fishing");
		}*/
	}

	#endregion

}

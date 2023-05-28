using UnityEngine;
using System.Collections;
using Share.Managers;

public class StartScreenKinectManager : MonoBehaviour {

	//floats
	private float currentHandY;
	private float currentHandX;
	private float currentHandCursorX;
	private float currentHandCursorY;
	private float hipHeight;
	private float centerChest;
	private float headY;

	//ray cast
	private Ray ray;
	private Ray2D ray2d;
	private RaycastHit hit;
	private RaycastHit2D hit2d;

	//bools
	private bool onlyOnce = false;

	//instances
	public static StartScreenKinectManager instance;

	private KinectManager kinect;
			
	void Awake(){
		instance = this;
	}

	void Update () {
		if (kinect == null) return;
		
		hipHeight = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter).y;
		centerChest = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter).x;		
		currentHandY = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).y;		
		currentHandX = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight).x;		
		headY = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Head).y;	
	}

	#region public methods

	public void OnHandOverStartScreenObjects(){
		var hand_pos = PlayerHandController.instance.GetPlayerCursor();
		ray = Camera.main.ScreenPointToRay(hand_pos);
		
		if(Physics.Raycast(ray, out hit)){
			print ("hit: "+hit.collider.tag);
			if(hit.collider.tag == "games_cube"){
				if(!onlyOnce){
					onlyOnce = true;
					LoadingSelection.instance.StartLoadingSelection();
				}
				if(LoadingSelection.instance.GetIsTimerComplete()){
					StartScreenManager.instance.OnGamesSelectionEnter();
				}
			}
			else if(hit.collider.tag == "config_cube"){
				if(!onlyOnce){
					onlyOnce = true;
					LoadingSelection.instance.StartLoadingSelection();
				}
				if(LoadingSelection.instance.GetIsTimerComplete()){
					StartScreenManager.instance.OnConfigurationEnter();
				}
			}
			else if(hit.collider.tag != "config_cube" && hit.collider.tag != "games_cube"){
				print ("StartScreenKinectManager if(hit.collider.tag != \"config_cube\" && hit.collider.tag != \"games_cube\")");
				LoadingSelection.instance.StopLoadingSelection();
				onlyOnce = false;
			}
		}else{
			print ("StartScreenKinectManager  else");
			LoadingSelection.instance.StopLoadingSelection();
			onlyOnce = false;
		}
	}

	public void OnHandOverStartScreenBoxes(){
		if(HandCollider2D.current_handOnButtonTag == "game_selection_box"){
			var obj = GameObject.Find(HandCollider2D.current_handOnButtonName).gameObject;
			obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			//resetar escalas
			for(int i = 0; i < StartScreenManager.instance.GetGameSelectionBoxes().Length; i++){
				if(StartScreenManager.instance.GetGameSelectionBoxes()[i].name != HandCollider2D.current_handOnButtonName){
					StartScreenManager.instance.GetGameSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
				}
			}
			//selecionar jogo
			if(LoadingSelection.instance.GetIsTimerComplete()){
				StartScreenManager.instance.OnGameEnter(obj);
			}
		}

		else if(HandCollider2D.current_handOnButtonTag == "startscreen_back_btn"){
			print("startscreen_back_btn");
			var obj = GameObject.Find(HandCollider2D.current_handOnButtonName).gameObject;
			StartScreenManager.instance.OnMouseOverBackButton(obj);
			//selecionar btn
			if(LoadingSelection.instance.GetIsTimerComplete()){
				StartScreenManager.instance.OnBackButtonPress();
			}
		}

		else if(HandCollider2D.current_handOnButtonTag == "config_music_box" || (HandCollider2D.current_handOnButtonTag == "config_sound_box"))
		{
			print("config_music_box ou config_sound_box");
			var obj = GameObject.Find(HandCollider2D.current_handOnButtonName).gameObject;
			obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			//resetar escalas
			for(int i = 0; i < StartScreenManager.instance.GetConfigSelectionBoxes().Length; i++){
				if(StartScreenManager.instance.GetConfigSelectionBoxes()[i].name != HandCollider2D.current_handOnButtonName){
					StartScreenManager.instance.GetConfigSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
				}//trocar som
			}
			//selecionar opcao
			if(LoadingSelection.instance.GetIsTimerComplete() == true)
			{
				print ("selecionado: " + LoadingSelection.instance.GetIsTimerComplete());
				if(HandCollider2D.current_handOnButtonTag == "config_music_box" )
				{
					OptionScreen.instance.TurnMusic();
					print ("seleçao de musica");
					LoadingSelection.instance.StopLoadingSelection();
					//ButtonSoundConfigManager.music_apertado = true;
					
				}else if(HandCollider2D.current_handOnButtonTag == "config_sound_box")
				{
					OptionScreen.instance.TurnOnSoundFX();
					print ("seleçao de efeitos");
					LoadingSelection.instance.StopLoadingSelection();
					//ButtonSoundConfigManager.sfx_apertado = true;
				}
				print ("selecionado: " + LoadingSelection.instance.GetIsTimerComplete());
				
			}
		}
		else{
			print ("a mao nao esta em nenhum dos 4 tipos objeto " + HandCollider2D.current_handOnButtonTag);

			LoadingSelection.instance.StopLoadingSelection();
			//reset game boxes scale
			for(int i = 0; i < StartScreenManager.instance.GetGameSelectionBoxes().Length; i++){
				StartScreenManager.instance.GetGameSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
			}
			//reset config boxes scale
			for(int i = 0; i < StartScreenManager.instance.GetConfigSelectionBoxes().Length; i++){
				StartScreenManager.instance.GetConfigSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
			}
			//reset back btn scale
			StartScreenManager.instance.OnMouseExitBackButton(StartScreenManager.instance.GetGamesBackBtn());
			StartScreenManager.instance.OnMouseExitBackButton(StartScreenManager.instance.GetConfigBackBtn());
		}
	}

/*	public void OnHandOverSelectSoundConfig()
	{
		if(HandCollider2D.current_handOnButtonTag == "config_music_box" || (HandCollider2D.current_handOnButtonTag == "config_sound_box"))
		{
			var obj = GameObject.Find(HandCollider2D.current_handOnButtonName).gameObject;
			obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			//resetar escalas
			for(int i = 0; i < StartScreenManager.instance.GetConfigSelectionBoxes().Length; i++){
				if(StartScreenManager.instance.GetConfigSelectionBoxes()[i].name != HandCollider2D.current_handOnButtonName){
					StartScreenManager.instance.GetConfigSelectionBoxes()[i].transform.localScale = new Vector3(1, 1, 1);
				}//trocar som
			}
			//selecionar opcao
			/*if(LoadingSelection.instance.GetIsTimerComplete() == true)
			{
				print ("selecionado: " + LoadingSelection.instance.GetIsTimerComplete());
				if(HandCollider2D.current_handOnButtonTag == "config_music_box" )
				{
					OptionScreen.instance.TurnMusic();
					print ("seleçao de musica");
					//ButtonSoundConfigManager.music_apertado = true;
					
				}else if(HandCollider2D.current_handOnButtonTag == "config_sound_box")
				{
					OptionScreen.instance.TurnOnSoundFX();
					print ("seleçao de efeitos");
					//ButtonSoundConfigManager.sfx_apertado = true;
				}
				print ("selecionado: " + LoadingSelection.instance.GetIsTimerComplete());
				
			}
		}
	}*/
	
	#endregion

}

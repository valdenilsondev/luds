using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Share.Managers;

public class PullBaitControl : MonoBehaviour {

	//vars que podem ser alteradas pelo usuario
	public float easyPullSpeed;
	public float mediumPullSpeed;
	public float hardPullSpeed;

	[SerializeField]
	private float pullingRodStrength;
	[SerializeField]
	private float bigRodPullStrength;

	public float minTimetToGetFish;
	public float maxTimetToGetFish;
	private float timeToGetFish;

	//contador de peixes por dificuldade
	private int fish_by_spot;

	//limites da barra
	private float bottomLimit;
	private float topLimit;
	private float callBigPull_botLimit;
	private float callBigPull_topLimit;
	[SerializeField]
	private Sprite red_on, green_on, red_off, green_off;
	public GameObject light_green, light_red;

	//var ira receber uma das dificuldades easy, medium ou hard
	private float difficultyMultiplier;

	//posicao do peixe na barra
	private float fishBarPos;

	//index do peixe sendo pescado
	private int currentFish = 0;

	//gatilhos
	private bool fishBaited = false;
	public bool pullingRodHope = false;
	public bool bigRodHopePull = false;

	//timer usado na hora que se joga a isca
	private bool baitThrown;
	private float baitThrownTimer;

	//referencias para a barra de intesidade e o peixe 
	private GameObject baitMarkerBG;
	private GameObject baitMarkerCenter;

	//tempo limite para pegar o peixe
	private float elapsedFishingTime;
	public float fishingTimeLimit = 25;

	public static PullBaitControl instance;

	void Awake(){
		instance = this;
		light_green.gameObject.GetComponent<Image>().sprite = green_off;
		light_red.gameObject.GetComponent<Image>().sprite = red_off;
		pullingRodStrength = PlayerPrefsManager.GetPullingStrengh();
		bigRodPullStrength = PlayerPrefsManager.GetBigPullingStrengh();

	}

	void Start(){
		//se os valores nao forem definidos no inspector, um padrao sera dado
		if(pullingRodStrength == 0){pullingRodStrength = 10;}
		if(bigRodPullStrength == 0){bigRodPullStrength = 15;}
		//cehca gesto da configuraçao

		//valores limite da barra do marcador
		bottomLimit = -((UIHandler.instance.barHeight/2) - UIHandler.instance.barHeight * 0.1f);
		topLimit = (UIHandler.instance.barHeight/2) - UIHandler.instance.barHeight * 0.1f;
		callBigPull_botLimit = -(UIHandler.instance.barHeight/3.8f);
		callBigPull_topLimit = (UIHandler.instance.barHeight/3.8f);
		elapsedFishingTime = 0;
	}

	void Update () {
		//inputs de teste
		if(Input.GetKeyDown(KeyCode.E) && !GameManagerShare.IsPaused() && !CountDownManager.instance.IsCounting() && !GameManagerShare.IsGameOver()){
			print ("key code e");
			ThrowBait();
			GameManagerShare.instance.IncreaseScore(Assets.Scripts.Share.Enums.ScoreItemsType.Bait, 1,0);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow) && fishBaited && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver()){
			print ("key code down arrow");
			pullingRodHope = true;
			BatataFishAnimatorController.instance.PlayBatataRoll();

		}
		if(Input.GetKeyDown(KeyCode.UpArrow)  && fishBaited && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver() &&
		   (baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y < callBigPull_botLimit || baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y > callBigPull_topLimit))
		{
			print ("key code up arrow");
			bigRodHopePull = true;
			var rnd = Random.Range(0,2);
			var str = "";
			if(rnd == 1){
				str = "left";
			}else{
				str = "right";
			}
			BatataFishAnimatorController.instance.PlayBatataPull();
		}
		//jogar isca
		if(baitThrown == true)
		{
			//um valor de tempo aleatorio entre o min e o max possivel sera setado
			if(timeToGetFish == 0){
				timeToGetFish = Random.Range(minTimetToGetFish, maxTimetToGetFish);
			}
			//contando tempo que a isca foi jogada
			baitThrownTimer = baitThrownTimer + Time.deltaTime;
			//verificando se o tempo excedeu
			if(baitThrownTimer >= timeToGetFish){
				//assim que pegar o peixe
				string dif;
				if(PlayerPrefsManager.GetFishingDifficult() == 0){
					dif = "easy";
				}
				else if(PlayerPrefsManager.GetFishingDifficult() == 1){
					dif = "medium";
				}
				else if(PlayerPrefsManager.GetFishingDifficult() == 2){
					dif = "hard";
				}
				else{
					dif = "easy";
				}
				FishBaited(dif);
				baitThrownTimer = 0;
				baitThrown = false;
			}
		}

		//se fisgado iniciara o pull
		if(fishBaited && !GameManagerShare.IsPaused() && !GameManagerShare.IsGameOver())
		{

			BeginFishPullingBait();
			//movimento da manivela para puxar a corda
			if(pullingRodHope){
				PullingRodHope();
				//teste 
			}
			//movimento de puxar a vara bruscamente
			else if(bigRodHopePull){
				if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y < callBigPull_botLimit  
				   || baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y > callBigPull_topLimit){
						BigRodHopePull();
				}
			}
			elapsedFishingTime += Time.deltaTime;
		}
	}

	//metodos publicos
	public void Pull(){
		if(fishBaited){
			pullingRodHope = true;
		}
		BatataFishAnimatorController.instance.PlayBatataRoll();
	}

	public void BigPullRight(){//PullRight
		if(fishBaited)
			bigRodHopePull = true;
		BatataFishAnimatorController.instance.PlayBatataPull();
	}

	public void BigPullLeft(){
		if(fishBaited)
			bigRodHopePull = true;
		BatataFishAnimatorController.instance.PlayBatataPull();
	}

	public void ThrowBait(){
		if(fishBaited == false & RopeManager.instance.throwing_rope == false){
			if(BucketBaitsControl.instance.UseBait()){
				baitThrown = true;
				BatataFishAnimatorController.instance.PlayBatataThrow();
				//
				RopeManager.instance.AnimateRopeOnThrow();
				FishingSoundManager.Instance.PlayThrowing();
			}
		}
	}

	public bool IsBaitThrow(){
		return baitThrown;
	}

	public float GetFishBarPos(){
		return fishBarPos + ( UIHandler.instance.barHeight/2 );
	}

	public bool IsFishBaited(){
		return fishBaited;
	}
	//fim metodos publicos


	//metodos privados
	private void PullingRodHope(){
		//metodo ira puxar a corda de forma constante assim que chamado
		pullingRodHope = false;
		FishingSoundManager.Instance.PlayPulling();
		//baitMarkerCenter.GetComponent<RectTransform>().localScale = new Vector3(1,-1,1); ou aqui ou no marker
		baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition = new Vector2 (baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.x,
		                                                                               baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y - pullingRodStrength);
	}
	
	private void BigRodHopePull(){
		//metodo ira puxar a corda de maneira bruta e repentina
		bigRodHopePull = false;
		FishingSoundManager.Instance.PlayPulling();
		//baitMarkerCenter.GetComponent<RectTransform>().localScale = new Vector3(1,-1,1); ou esse ou no marker
		baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition = new Vector2(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.x,
		                                                                              baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y - bigRodPullStrength);
	}
	
	private void FishBaited(string difficulty){
		//verificando se os marcadores estao ativos
		if(UIHandler.instance.GetBaitMarkerBG() == null){
			return;
		}else{
			baitMarkerBG = UIHandler.instance.GetBaitMarkerBG();
			baitMarkerCenter = UIHandler.instance.GetBaiMarkerCenter();
		}

		//definindo dificuldade
		if(difficulty == "easy")	{	difficultyMultiplier = easyPullSpeed;	print ("dificuldade: " + difficulty);}
		if(difficulty == "medium")	{	difficultyMultiplier = mediumPullSpeed;	print ("dificuldade: " + difficulty);}
		if(difficulty == "hard")	{	difficultyMultiplier = hardPullSpeed;	print ("dificuldade: " + difficulty);}

		//aumentando scale do center marker para monstrar que a isca foi fisgada
		string tempDifficulty = difficulty;
		StartCoroutine(WaitToPullBait());

		//iniciar a mover o fish
		FishMovementControl.instance.CreateFishAndHandleMov();
	}

	IEnumerator WaitToPullBait() { 		
		iTween.ScaleTo(baitMarkerCenter, new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
		yield return new WaitForSeconds (0.3f);
		iTween.ScaleTo(baitMarkerCenter, new Vector3(1, 1, 1), 0.2f);
		fishBaited = true;
		FishingSoundManager.Instance.PlayCatchWarnning();
	}

	IEnumerator FishCaught()
	{
		//fishBaited true esta coletando 8 peixes, 
		fishBaited = false;
		iTween.ScaleTo(baitMarkerCenter, new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
		yield return new WaitForSeconds (0.3f);
		iTween.ScaleTo(baitMarkerCenter, new Vector3(1, 1, 1), 0.2f);
		SendMarkerBackToCenter();	
		int fishType = (int)FishingManager.instance.GetCurrenFish().GetFishType();
		FishingManager.instance.DecreaseFishOnCountList();
		FishingManager.instance.IncreaseScoreFishType(fishType);
		RopeManager.instance.RopeToRestingPoint();
		elapsedFishingTime = 0;
		light_green.gameObject.GetComponent<Image>().sprite = green_off;
	}

	IEnumerator FishEscaped(){
		fishBaited = false;
		iTween.ScaleTo(baitMarkerCenter, new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
		yield return new WaitForSeconds (0.3f);
		iTween.ScaleTo(baitMarkerCenter, new Vector3(1, 1, 1), 0.2f);
		GotFishScreenControl.instance.ShowFishScapeScreen();
		FishingSoundManager.Instance.PlayDelaied();
		if(SoundManager.Instance != null)
		{
			SoundManager.Instance.PlayBGDelayed(3.2f);
		}
		SendMarkerBackToCenter();
		RopeManager.instance.RopeToRestingPoint();
		elapsedFishingTime = 0;
		light_red.gameObject.GetComponent<Image>().sprite = red_off;
	}

	private void SendMarkerBackToCenter(){
		baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition = new Vector2(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.x, 0);
		ResetMarkerBarsFillAmount();
		//mudar tipo dinamicamente 1, 2, 3
	}

	private void ResetMarkerBarsFillAmount(){
		//UIHandler.instance.GetRedBar().GetComponent<Image>().fillAmount = 0.5f;
		UIHandler.instance.GetGreenBar().GetComponent<Image>().fillAmount = 0.5f;
		//UIHandler.instance.GetYellowBar().GetComponent<Image>().fillAmount = 0.5f;
	}

	private void ManageMarkerBar(){
		UIHandler.instance.GetGreenBar().GetComponent<Image>().fillAmount = 1 * (GetFishBarPos() / UIHandler.instance.barHeight);

		/*if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y < callBigPull_botLimit  
		   || baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y > callBigPull_topLimit)
		{
			GotFishScreenControl.instance.ShowTimeToPull();
		}*/
		  if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y > (UIHandler.instance.barHeight/2) * 0.05f){
			//ativa a linha da barra
			light_red.gameObject.GetComponent<Image>().sprite = red_on;
			baitMarkerCenter.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
			UIHandler.instance.ActivateBaitMarkerGreenBar();
			//desativa as outras barras
			light_green.gameObject.GetComponent<Image>().sprite = green_off;
			//UIHandler.instance.DeactivateBaitMarkerGreenBar();
			//UIHandler.instance.DeactivateBaitMarkerYellowBar();
		}
		/*else if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y < (UIHandler.instance.barHeight/2) * 0.05f
		         && baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y > -( (UIHandler.instance.barHeight/2) * 0.05f)){
			//ativa a barra amarela
			//UIHandler.instance.ActivateBaitMarkerYellowBar();
			//UIHandler.instance.GetGreenBar().GetComponent<Image>().fillAmount = 1 * (GetFishBarPos() / UIHandler.instance.barHeight);
			//desativa as outras barras
			//UIHandler.instance.DeactivateBaitMarkerRedBar();
			//UIHandler.instance.DeactivateBaitMarkerGreenBar();
		}*/
		else if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y < -( (UIHandler.instance.barHeight/2) * 0.05f)){
			//ativa a barra verde
			light_green.gameObject.GetComponent<Image>().sprite = green_on;
			baitMarkerCenter.GetComponent<RectTransform>().localScale = new Vector3(1,-1,1);
			//UIHandler.instance.ActivateBaitMarkerGreenBar();
			//desativa as outras barras
			light_red.gameObject.GetComponent<Image>().sprite = red_off;
			//UIHandler.instance.DeactivateBaitMarkerRedBar();
			//UIHandler.instance.DeactivateBaitMarkerYellowBar();
		}
	}


	private void BeginFishPullingBait(){
		//metodo que aplica forca constante do peixe no marcador de intensidade
		if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y > topLimit || elapsedFishingTime >= fishingTimeLimit){
			light_red.gameObject.GetComponent<Image>().sprite = red_on;
			StartCoroutine(FishEscaped());
		}else if(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y < bottomLimit){
			//criar metodo fish caught, usado este pois o comportamento do metodo vai ser igual
			light_green.gameObject.GetComponent<Image>().sprite = green_on;
			StartCoroutine(FishCaught());
		}
		//move o peixe da barra
		baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition,
                                                                                  new Vector2(baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.x,
		            																		  baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y + 6), difficultyMultiplier);
		//o que chama a anmiaçao de força do peixe

		//info de onde esta o peixe na barra
		fishBarPos = baitMarkerCenter.GetComponent<RectTransform>().anchoredPosition.y;
		//gerencia a cor para o tamanho das barras
		ManageMarkerBar();
	}
}

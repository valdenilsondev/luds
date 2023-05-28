using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Share.EventsSystem;
using VacuumShaders.CurvedWorld;
using Runner.Pool;
using Runner.Managers;
using UnityEngine.UI;
using Assets.Scripts.Share.Managers;
using Assets.Scripts.Share.Enums;
using System.Linq;

public class FloorMovementControl : MonoBehaviour
{
    public GameObject floor1;
    public GameObject floor2;
    public GameObject floor3;

    public float floor1PosZ;
    public float floor2PosZ;
    public float floor3PosZ;
    public float distanceToReset;

    private float mapSpeed;
    public float initialMapSpeed;
    private float mapSpeedLimit;
    public float incrementSpeedValue;
    private float incrementSpeedInterval;
	private float tempTimer;

    private int chosenSide = -1;

	public bool isEndless = false;
    public int numOfMapsTillTransitionMap;
    private bool permissionToMove;
    private bool gameOver = false;
    public bool timeBasedGame = false;
    public int gameTimeLimit;

    public float floorScale;
    private Transform MapsPoolParent;
    private Transform MapsPoolTransitionParent;
    private Transform MapsManagerParent;
    private int floorTunnelSorted;

    //private float timer;
    private int floorCounter;
    private static float lastXBend;

	//variaveis do curve floor
	private float elapsedCurveTime;
	private bool curveUp = true;
	private int randomYCurveFloor;

    public static FloorMovementControl instance;

    private PlatformSO[] platforms;
    private ItemsPool itemsPool;

    private GameObject cam;

	public bool startGame = true;

	private bool canItMove;
    public bool slowMotion = false;

    void Awake()
    {
        InitPlatformsList();
        itemsPool = ItemsPool.Instance;
        instance = this;
    }

    void Start()
    {
        cam = GameObject.Find("CameraRunner");

        //iniciando varaveis de controle        
		InitializeMapMovementVariables ();

        //verificando transition number
        if (numOfMapsTillTransitionMap == 0)
        {
            numOfMapsTillTransitionMap = 1;
        }

        //verificando se game time limit > 0
        if (timeBasedGame)        {
            if (gameTimeLimit < 1)
            {
                gameTimeLimit = 1;
            }
        }

        //iniciando variavel da velocidade do mapa
        mapSpeed = initialMapSpeed;

        //buscando referencias para o pai do pool de mapas e o pai do gerenciador de mapas usados
        MapsPoolTransitionParent = GameObject.Find("MapsPoolTransition").transform;
        MapsManagerParent = GameObject.Find("MapsManager").transform;

        floor1.transform.parent = GameObject.Find("MapsManager").transform;
        floor2.transform.parent = GameObject.Find("MapsManager").transform;
        floor3.transform.parent = GameObject.Find("MapsManager").transform;

        floor1.transform.GetChild(0).gameObject.SetActive(true);
        floor2.transform.GetChild(0).gameObject.SetActive(true);
        floor3.transform.GetChild(0).gameObject.SetActive(true);

        floor1.transform.position = new Vector3(floor1.transform.position.x, floor1.transform.position.y, floor1PosZ);
        floor2.transform.position = new Vector3(floor2.transform.position.x, floor2.transform.position.y, floor2PosZ);
        floor3.transform.position = new Vector3(floor3.transform.position.x, floor3.transform.position.y, floor3PosZ);

        InstantiateMap(floor1.transform.GetChild(0).gameObject, platforms[0]);
        InstantiateMap(floor2.transform.GetChild(0).gameObject, platforms[10]);
        InstantiateMap(floor3.transform.GetChild(0).gameObject, platforms[11]);

        permissionToMove = true;
    }

	private void InitializeMapMovementVariables(){
	/*	int pigLevel = PigRunnerManager.instance.GetCurrentGroupType();
		switch (pigLevel) {
		case 1:
			initialMapSpeed = 7;
			mapSpeedLimit = 25;
			incrementSpeedInterval = 6;
			incrementSpeedValue = 0.2f;
			break;

		case 2:
			initialMapSpeed = 12;
			mapSpeedLimit = 50;
			incrementSpeedInterval = 7;
			incrementSpeedValue = 0.5f;
			break;

		default:
			initialMapSpeed = 7;
			mapSpeedLimit = 25;
			incrementSpeedInterval = 6;
			incrementSpeedValue = 0.2f;
			break;
		}*/
		float value = PlayerPrefsManager.GetInitialMapSpeed ();

		if(value == 0)
		{
			value = 6;
		}else if(value==1)
		{
			value=8;
		}else if(value==2)
		{
			value=10;
		}

		initialMapSpeed = value;
		mapSpeedLimit = PlayerPrefsManager.GetMapSpeedLimit ();
		incrementSpeedInterval = PlayerPrefsManager.GetIncrementSpeedInterval();
		incrementSpeedValue = PlayerPrefsManager.GetIncrementSpeedValue();
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.R))
        {
            //Events.RaiseEvent<StopRecordingEvent>();
            //SceneManager.LoadScene("Replay");
			print(GetMapSpeed());
        }

        if (GetPermisionToMove())
        {
			if (canItMove)
            {
				//PigRunnerSoundManager.Instance.PlayRunSound();
                MoveFloor(floor1);
                MoveFloor(floor2);
                MoveFloor(floor3);
                CheckResetFloorPos();

				TimeManager.instance.StartTimer();

				//usado para aumentar dificuldade
				tempTimer = tempTimer + Time.deltaTime;

                if (tempTimer > incrementSpeedInterval)
                {
					tempTimer = 0;

                    if (PowerUpManager.Instance.currentPowerUp != PowerUps.Boost)
                        IncreaseMapSpeed();
                }

                CurveFloor();
			}else{
				//PigRunnerSoundManager.Instance.StopRunSound();
			}

        }
        else if (TimeManager.instance.GetCurrentTime() > 0)
        {
			TimeManager.instance.StopTimer();
        }
    }    

    public IEnumerator Slow()
    {
        slowMotion = true;
        mapSpeed = mapSpeed * 0.001f;
		PigRunnerController.instance.SlowDownAnimsSpeed();
        yield return new WaitForSeconds(2.5f);
        slowMotion = true;
        for (float i = mapSpeed; i < initialMapSpeed; i+= Time.deltaTime * 2)
        {
            mapSpeed = i;
			if(PigRunnerController.instance.GetAnimator().speed < 1){
				PigRunnerController.instance.GetAnimator().speed = FloorMovementControl.instance.GetMapSpeed()*0.4f;
			}
            yield return null;
        }
		PigRunnerController.instance.ResetAnimsSpeed();
    }

	public void MoveFloor()
	{
		canItMove = true;
	}

    private void InitPlatformsList()
    {
        if (platforms != null) return;
        platforms = Resources.LoadAll<PlatformSO>("Scriptable Objects");        
    }

    public int GetItemsCount(ItemsType type)
    {
        if (platforms == null) InitPlatformsList();

        int firstMapCount = 0, secondMapCount = 0, thirdMapCount = 0;

        // Percorre por todas as plataformas
        for (int i = 0; i < platforms.Length; i++)
        {
            int platformCount = 0;
            //Conta a quantidade de itens do tipo passado como parametro
            for (int j = 0; j < platforms[i].GetCount(); j++)
            {
                if (platforms[i].GetItemType(j) == type)
                {
                    platformCount++;
                }
            }

            // Testa se a qntd esta entre as tres maiores
            if (platformCount >= firstMapCount)
            {
                firstMapCount = platformCount;
            }
            else if (platformCount >= secondMapCount)
            {
                secondMapCount = platformCount;
            }
            else if (platformCount >= thirdMapCount)
            {
                thirdMapCount = platformCount;
            }
        }

        return firstMapCount + secondMapCount + thirdMapCount;
    }
	int diference=1;
    void CurveFloor()
    {
        elapsedCurveTime += Time.deltaTime;
		float diference=1;
        if (elapsedCurveTime > 15 && IsFloorMoving())
        {
            curveUp = !curveUp;
            elapsedCurveTime = 0;
			//int randomAnt = randomYCurveFloor;
            randomYCurveFloor = Random.Range(-7, 7);
			//diference = Mathf.Abs(randomAnt - randomYCurveFloor);

			if(diference == 0){
				diference = 1;
			}
        }
		CurvedWorld_Controller.get._V_CW_Bend_Y = Mathf.Lerp(CurvedWorld_Controller.get._V_CW_Bend_Y, randomYCurveFloor, (0.01f*Mathf.Abs(randomYCurveFloor)) * Time.deltaTime);
    }

	public int GetRandomYCurveFloor(){
		return randomYCurveFloor;
	}

    public static void CurveWorldZ(float zAmount)
    {
		CurvedWorld_Controller.get._V_CW_Bend_Z = zAmount;
    }

    public static void CurveWorldX(float xAmount)
    {
		lastXBend = CurvedWorld_Controller.get._V_CW_Bend_X;
		CurvedWorld_Controller.get._V_CW_Bend_X = xAmount;
    }

    public static void CurveWorldXToLastBend()
    {
		CurvedWorld_Controller.get._V_CW_Bend_X = lastXBend;
    }

    public bool IsFloorMoving()
    {
        return permissionToMove;
    }

    public void Pause()
    {
        permissionToMove = false;
    }


    public void UnPauseGame()
    {
        permissionToMove = true;
    }

    public void SetPermisionToMove(bool permission)
    {
        permissionToMove = permission;
    }

    public bool GetPermisionToMove()
    {
        return permissionToMove;
    }

    private void OnUnPause()
    {
		//fix verificar se ainda eh necessario
        UnPauseGame();
    }

    private void OnPause()
    {
		//fix verificar se ainda eh necessario
        Pause();
    }

    private void OnGameOver()
    {
        Pause();
    }

    private void CheckResetFloorPos()
    {//verifica se algum mapa saiu da area limite
        if (floor1.transform.position.z < distanceToReset)
        {
            floor1.transform.position = new Vector3(0, 0, floor3.transform.position.z + floorScale);
            SortMap(floor1);
        }
        if (floor2.transform.position.z < distanceToReset)
        {
            SortMap(floor2);
            floor2.transform.position = new Vector3(0, 0, floor1.transform.position.z + floorScale);
        }
        if (floor3.transform.position.z < distanceToReset)
        {
            SortMap(floor3);
            floor3.transform.position = new Vector3(0, 0, floor2.transform.position.z + floorScale);
        }

    }

    private void MoveFloor(GameObject go)
    {
        go.transform.position = Vector3.MoveTowards(go.transform.position, new Vector3(go.transform.position.x,
                                                                                       go.transform.position.y,
                                                                                       go.transform.position.z - 2f), mapSpeed * Time.deltaTime);
    }

    private void SortTransitionMap(GameObject go)
    {
        //nesse momento go eh o pai de um mapa (floor1 -> Map1 por exemplo)
        go.transform.GetChild(0).gameObject.SetActive(false);
        go.transform.GetChild(0).transform.parent = MapsPoolParent;
        //Map1 foi desativado e devolvido para o pool

        //agora Floor1 recebe um mapa qualquer do pool de transitions
        MapsPoolTransitionParent.GetChild(Random.Range(0, MapsPoolTransitionParent.childCount - 1)).transform.parent = go.transform;
        go.transform.GetChild(0).gameObject.SetActive(true);
        go.transform.GetChild(0).transform.localPosition = new Vector3(0, -1, 0);
        //Floor1 foi ativado e sua posicao local setada
    }

    private void SortMap(GameObject go)
    {
        //contando quantos floors foram sorteados        
		if(isEndless == false){
			floorCounter++;
		}

        if (floorCounter > numOfMapsTillTransitionMap)
        {
            floorCounter = 0;
            SortTransitionMap(go);
        }
        else
        {
            SendMapBackToPool(go);

            //agora Floor1 recebe um mapa qualquer do pool
            ChooseMapFromPool(go.transform.GetChild(0).gameObject);
            go.transform.GetChild(0).gameObject.SetActive(true);
            go.transform.GetChild(0).transform.localPosition = new Vector3(0, -1, 0);
            //Floor1 foi ativado e sua posicao local setada
        }
    }

    private void SendMapBackToPool(GameObject go)
    {
        //nesse momento go eh o pai de um mapa (floor1 -> Map1 por exemplo)
        go.transform.GetChild(0).gameObject.SetActive(false);
        //Map1 foi desativado e devolvido para o pool
    }

    private string lastTag = "Floor3Ways";
    private void ChooseMapFromPool(GameObject go)
    {
        int platformIndex = 0;

		//retorna group type salvo no prefs
        var actualGroup = (GroupTypes)PigRunnerManager.instance.GetCurrentGroupType();
		//print((GroupTypes)PigRunnerManager.instance.GetCurrentGroupType());

		var tempMapList = new List<PlatformSO>();
		//busca mapas com obstaculos na esquerda, direita, meio ou qualquer lugar. Depende do group type
		if(PlayerPrefsManager.GetAnyGroupType() == 0){
			tempMapList = platforms.Where(x => x.groupTypes == actualGroup).ToList();
		}else{
			tempMapList = platforms.Where(x => (x.groupTypes == GroupTypes.Left || x.groupTypes == GroupTypes.Middle || x.groupTypes == GroupTypes.Right) ).ToList();
		}

		//retorna level salvo no prefs
		var actualLevel = (LevelGroups)PigRunnerManager.instance.GetCurrentLevelGroup();

		//dentre os mapas do group type, deve-se buscar o level de interesse
		var mapList = tempMapList.Where(x => x.levelGroups == actualLevel).ToList();

        do
        {
            platformIndex = Random.Range(0, mapList.Count - 1);

			if (mapList[platformIndex].platformTag != "Untagged")
            {
                floorTunnelSorted++;
            }
            else if (go.tag != "Floor3Ways")
            {
                floorTunnelSorted = 0;
            }

		} while (mapList[platformIndex].platformTag == "Untagged" && lastTag == "Untagged");
			InstantiateMap (go, mapList [platformIndex]);
        	lastTag = go.tag;
    	}

    private void DestroyItems(Transform go)
    {
        while (go.childCount > 0)
        {
            itemsPool.DestroyItem(go.GetChild(0).gameObject);
        }
    }

    private void InstantiateMap(GameObject go, PlatformSO plataform)
    {
        Transform item;
        Transform itemsContainer = go.transform.Find("ItemsContainer");

        DestroyItems(itemsContainer);

        for (int i = 0; i < plataform.GetCount(); i++)
        {
            item = itemsPool.InstantiateItem(plataform.GetItemType(i));
            item.SetParent(itemsContainer);
            item.localPosition = plataform.GetItemPos(i);
            item.localEulerAngles = plataform.GetItemRotation(i);
            item.gameObject.SetActive(true);
        }

        go.tag = plataform.platformTag;
//        go.GetComponent<PlataformInfo>().SetPlataformSide(plataform.platformInfo);
    }

    public float GetMapSpeed()
    {
        if (permissionToMove)
        {
            return mapSpeed;
        }
        return 0;
    }

    public void PowerBoost(float speed, float time)
    {
        mapSpeed = speed * mapSpeed;
    }

    public void PowerBoostDown()
    {
        mapSpeed = mapSpeed / 2;
    }

    public void IncreaseMapSpeed()
    {
        if(!slowMotion)
        {
            if (mapSpeed < mapSpeedLimit)
            {
                mapSpeed = mapSpeed + incrementSpeedValue;
				PigRunnerController.instance.IncreaseAnimsSpeed();
            }
        }        
    }

    public void DecreaseMapSpeed()
    {
        if (mapSpeed > initialMapSpeed)
        {
            mapSpeed = mapSpeed - incrementSpeedValue;
        }
    }

}



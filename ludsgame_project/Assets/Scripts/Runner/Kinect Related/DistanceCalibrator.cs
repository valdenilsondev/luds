using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.EventsSystem;
using Runner.Managers;
using Share.Managers;
using System;
using Share.KinectUtils;
using UnityEngine.SceneManagement;

public enum FeetBase
{
    Open,
    Close,
    LeftFootFront,
    RightFootFront
}

public enum Posture
{
    Aligned,
    Inclined,
    InclinedRightHand,
    InclinedLeftHand,
    InclinedBothHands
}

public class DistanceCalibrator : MonoBehaviour
{

    public static DistanceCalibrator instance;

    public float minDistance = 1.5f;
    public float warningDistance = 1f;

    private float minLateralDistance = -0.15f;
    private float maxLateralDistance = 0.15f;

    //public Sprite aguardando;
    //public Sprite naoDefinida;
    //public Sprite ideal;

    public Text distanceText;
    public Text distanceLateralText;
    public Text playerCalibratedText;
    public GameObject waveText;
    public Text baseDistText;
    public Text postureText;
    public Text gestureText;
    public Text angleText;
	public Text welcomeText;
	public Text kneeAngle;
    //private Image mySpriteRenderer;
    private bool isDistanceOk = false;
    private bool isLateralDistanceOk = false;
    public bool isGestureDone = false;
    private float distance;
    public KinectManager kinect;
    private Vector4 initialVector = new Vector4(-1, -1, -1, -1);
    private float playerXPos;
    private float baseLateralDistance;
    private float baseFrontalDistance;
    private float VecLateralDistance; //vec = kinect.GetUserPosition(playerId);

    private FeetBase fb;
    private Posture pp;

    public Animator calibTextAnim;
    private KinectWrapper.NuiSkeletonPositionIndex handUsed;

    float previousHeadPoint = 0;
    float previousDistance = 0;
    bool heightOk = false;
    bool distanceOk = false;
    private IEnumerator coroutine;
    private bool isCalibrated = false;
    private bool allPointsDetected = false;
    private double previousHeadPointPosition;
    private bool calibratedRunning = false;

    private bool levitate = false;

    private float time;
    private bool timeWait = true;

    void Start()
    {
        instance = this;
        //	if (SceneManager.GetActiveScene().name == "Calibration"){
        DontDestroyOnLoad(this);
        //	} 

        //		mySpriteRenderer = this.GetComponentInChildren <Image>();
        kinect = KinectManager.Instance;
        distanceText.enabled = false;
        distanceLateralText.enabled = false;

        waveText.SetActive(false);
		welcomeText.text = ("Bem-vindo " + PlayerPrefsManager.GetPlayerName());

    }

    void Update()
    {
        int shoulderRight = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderRight;
        int hipRight = (int)KinectWrapper.NuiSkeletonPositionIndex.HipRight;
        int kneeRight = (int)KinectWrapper.NuiSkeletonPositionIndex.KneeRight;
		int kneeLeft = (int)KinectWrapper.NuiSkeletonPositionIndex.KneeLeft;

		int rightHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipRight;
		int rightKneeIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.KneeRight;
		int rightAnkleIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.AnkleRight;

		int leftHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipLeft;
		int leftKneeIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.KneeLeft;
		int leftAnkleIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.AnkleLeft;


        var playerId = kinect.GetPlayer1ID();
        Vector4 distance = kinect.GetUserPosition(playerId);

        if (kinect.IsPlayerCalibrated(playerId))
        {
            waveText.SetActive(false);
            Vector4 vec;
            vec = kinect.GetUserPosition(playerId);
		/*	kneeAngle.text = "lef " +   (int)Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(leftHipIndex, leftKneeIndex, leftAnkleIndex)) +
				"right " + (int) Mathf.Abs((float)GenericKinectMethods.instance.GetBodySegmentAngleFrontal(rightHipIndex, rightKneeIndex, rightAnkleIndex));*/       
            if (vec != initialVector)
            {
                if (SceneManager.GetActiveScene().name == "Calibration")
                {
                    Vector3 target = new Vector3(distance.x * 20, distance.y, (distance.z * 3.5f) - 10);
                  
                    if (CheckHeadAndFootJoints())
                    {
                       

                        CheckBase();
                        CheckPosture();
                        CheckDistanceLateral(vec.x);

                        if (isLateralDistanceOk && !isCalibrated)
                        {
                            //coroutine = Calibrated();
                            StartCoroutine("Calibrated");
                        }
                    }
                    else
                    {
                      
                        isCalibrated = false;
                        distanceText.enabled = false;
                        distanceLateralText.enabled = false;
						
						welcomeText.gameObject.SetActive(false);

                        //                        Debug.Log("Posição boy: " + GameObject.Find("grp_all").transform.position);
                        //                      Debug.Log("objetivo: " + GameObject.Find("goal").transform.position);

                        if (calibratedRunning)
                        {
                            StopCoroutine("Calibrated");
                            calibratedRunning = false;
                        }
                    }
                }
				if (SceneManager.GetActiveScene().name != "startScreenNew" && SceneManager.GetActiveScene().name != "Calibration")
                {
                    VecLateralDistance = vec.x;
                    CheckOutOfScreen();
                }
            }
        }
        else
        {
            time += Time.deltaTime;

            if (time > 4 && timeWait)
            {
                timeWait = false;
                StartCoroutine(Blink(waveText));
            }

        }
    }

    IEnumerator Blink(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);

        time = 0;
        timeWait = true;
    }

    private bool CheckHeadAndFootJoints()
    {
        var playerId = kinect.GetPlayer1ID();
        var headJointId = (int)KinectWrapper.NuiSkeletonPositionIndex.Head;
        var leftFootJointId = (int)KinectWrapper.NuiSkeletonPositionIndex.FootLeft;
        var rightFootJointId = (int)KinectWrapper.NuiSkeletonPositionIndex.FootRight;

        var result = KinectManager.Instance.IsJointTracked(playerId, headJointId) && (KinectManager.Instance.IsJointTracked(playerId, leftFootJointId) || KinectManager.Instance.IsJointTracked(playerId, rightFootJointId));

        return result;
    }

    bool ispositionok;

    public void SetGestureDone()
    {
        if (SceneManager.GetActiveScene().name == "Calibration")
          //  boy_calib.GetComponent<Animator>().SetTrigger("madeWave");
        isGestureDone = true;
    }

    private void TestCalibrationAndGesture()
    {
        if (SceneManager.GetActiveScene().name == "Calibration" && isLateralDistanceOk && isDistanceOk && isGestureDone)
        {
            distanceLateralText.enabled = false;
            distanceText.enabled = false;
            //mySpriteRenderer.enabled = false;
            baseDistText.enabled = false;

            if (!ispositionok)
            {
                GameObject.Find("boy_clbr").GetComponent<Animator>().SetTrigger("isPositionOk");
                ispositionok = true;
				
				welcomeText.gameObject.SetActive(false);
            } 
            if (playerCalibratedText.gameObject.activeSelf == false)
            {
                playerCalibratedText.gameObject.SetActive(true);
            }
			
			welcomeText.gameObject.SetActive(false);
            CallCalibratedTextAnim();
        }
    }

    public KinectWrapper.NuiSkeletonPositionIndex GetHandUsed()
    {
        return handUsed;
    }

    public void CallCalibratedTextAnim()
    {
        calibTextAnim.SetTrigger("playerCalibrated");
        //playerCalibratedText.gameObject.SetActive(false);
    }

    public string gameToLoad;

    public void LoadGame()
    {    
		LoadingScreen.instance.LoadScene (gameToLoad);
        //SceneManager.LoadScene(gameToLoad);//game do runner   
		DesactiveAllUI();
    }

    public void LoadStartScreen()
    {
        //	SceneManager.LoadScene (gameToLoad);//game do runner
		LoadingScreen.instance.LoadScene ("startScreenNew");
        DesactiveAllUI();
    }

    private void DesactiveAllUI()
    {
        distanceText.gameObject.SetActive(false);
        distanceLateralText.gameObject.SetActive(false);
        playerCalibratedText.gameObject.SetActive(false);
        waveText.SetActive(false);
        baseDistText.gameObject.SetActive(false);
        postureText.gameObject.SetActive(false);
        gestureText.gameObject.SetActive(false);
    }

    private IEnumerator Calibrated()
    {

        calibratedRunning = true;
        yield return new WaitForSeconds(2.5f);
        calibratedRunning = false;
        if (SceneManager.GetActiveScene().name == "Calibration")
        {
            //TODO - Não está descendo na posição correta			
			welcomeText.gameObject.SetActive(false);
            
            CallCalibratedTextAnim();
            isCalibrated = true;
        }
    }

    private void CheckDistanceLateral(float lateralDistance)
    {
        //se estiver fora do limite central
        if (lateralDistance >= minLateralDistance && lateralDistance <= maxLateralDistance)
        {
            distanceLateralText.enabled = false;
            isLateralDistanceOk = true;
            //distanceLateralText.text = "Centro OK ";
            isLateralDistanceOk = true;
        }
        if (lateralDistance <= minLateralDistance)
        {
            distanceLateralText.enabled = true;
            distanceLateralText.text = "Para direita. ";
            isLateralDistanceOk = false;
        }
        if (lateralDistance >= maxLateralDistance)
        {
            distanceLateralText.enabled = true;
            distanceLateralText.text = "Para esquerda.";
            isLateralDistanceOk = false;
        }
    }


    /*private void CheckDistanceLateralGK(float lateralDistance) {
        //se estiver fora do limite central
        if (lateralDistance >= maxLateralDistance || lateralDistance <= minLateralDistance) {
            distanceLateralText.enabled = true;	
            //GameManagerGoalkeeper.PauseGame ();
            Share.EventsSystem.Events.RaiseEvent<PauseEvent>();	
        } else {
            Share.EventsSystem.Events.RaiseEvent<UnPauseEvent>();		
            //GameManagerGoalkeeper.UnpauseGame();			
        }
		
        if (GameManagerGoalkeeper.IsPaused ()) {	
            distanceLateralText.enabled = true;					
        } else {
            distanceLateralText.enabled = false;					
        }
    }*/


    public void CheckBase()
    {
        Vector3 footLeft = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.FootLeft);
        Vector3 footRight = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.FootRight);

        baseLateralDistance = (footRight.x - footLeft.x);
        baseFrontalDistance = (footRight.z - footLeft.z);
        //	baseDistText.text = "Base lateral " + baseLateralDistance.ToString() + "  " + IsLateralBaseOpen().ToString() +
        //	" \n Base Frontal " + baseFrontalDistance.ToString() + " " + IsFrontalBaseOpen().ToString() +
        //		"\n FB: " + fb.ToString();


        if (IsFrontalBaseOpen())
        {
            if (baseFrontalDistance > 0)
            {
                fb = FeetBase.LeftFootFront;
            }
            else
            {
                fb = FeetBase.RightFootFront;
            }
        }
        else
        {
            if (IsLateralBaseOpen())
            {
                fb = FeetBase.Open;
            }
            else
            {
                fb = FeetBase.Close;
            }
        }
        CheckPosture();

    }


    private float torsoInclination;

    private void CheckPosture()
    {
        //spinebase = 0 = hip center
        //spine = 1 = spineMid
        //spineShoulder = 20? = shouldercenter 2(neck?)
        Vector3 spineMid = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.Spine);
        Vector3 spineBase = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter);
        Vector3 shoulderCenter = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter);
        Vector3 handLeft = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft);
        Vector3 handRight = kinect.GetJointPosition(kinect.GetPlayer1ID(), (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight);
        float leftHandDistanceFromTorso = handLeft.z - shoulderCenter.z;
        float rightHandDistanceFromTorso = handRight.z - shoulderCenter.z;

        torsoInclination = spineBase.z - shoulderCenter.z;
        //	postureText.text = "handLeft " + leftHandDistanceFromTorso + " handRight " + rightHandDistanceFromTorso + " Posture " + pp.ToString();

        if (torsoInclination > 0.10)
        {
            if (leftHandDistanceFromTorso > 0.20 && rightHandDistanceFromTorso > 0.20)
            {
                pp = Posture.InclinedBothHands;
            }
            if (leftHandDistanceFromTorso > 0.20 && rightHandDistanceFromTorso < 0.20)
            {
                pp = Posture.InclinedLeftHand;
            }
            if (leftHandDistanceFromTorso < 0.20 && rightHandDistanceFromTorso > 0.20)
            {
                pp = Posture.InclinedRightHand;
            }

        }
        else
        {
            pp = Posture.Aligned;
        }
    }

    public float GetBaseLateralDistance()
    {
        return baseLateralDistance;
    }

    public bool IsLateralBaseOpen()
    {
        if (baseLateralDistance > 0.20)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetBaseFrontalDistance()
    {
        return baseFrontalDistance;
    }

    public bool IsFrontalBaseOpen()
    {
        if (baseFrontalDistance > 0.20f || baseFrontalDistance < -0.20f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckOutOfScreen()
    {
        float lateralDistance = VecLateralDistance;
        if (lateralDistance <= -0.8f || lateralDistance >= 0.8f)// && GameManagerShare.IsStarted())
        {
			print ("CheckOutOfScreen2");
            distanceLateralText.enabled = true;
            distanceLateralText.text = "Volte para o jogo";
            //verificaçao para nao chamar o evento todo frame
            return true;
            /*if (!GameManagerShare.IsGameOver() && !GameManagerShare.IsPaused())
            {
               PauseManagerShare.instance.PauseOutOfScreen();
                //GameManagerShare.instance.PauseGame();
            }*/            
        }
        else 
        {
            return false;
        }
    }

    public int GetPlayerPosx()
    {
        //centro
        if (playerXPos <= 0.18f && playerXPos >= -0.18f)
            return 2;
        //esquerda
        if (playerXPos < -0.18f && playerXPos >= -0.36f)
            return 1;
        //esquerda distante
        if (playerXPos < -0.36f)
            return 0;
        //direita
        if (playerXPos > 0.18f && playerXPos <= 0.36f)
            return 3;
        //mais a direita
        if (playerXPos > 0.36f)
            return 4;

        return 5;
    }

    public float GetRawPlayerPosx()
    {
        return playerXPos;
    }

    public bool IsDistanceOk()
    {
        return isDistanceOk;
    }

    public bool IsLateralDistanceOk()
    {
        return isLateralDistanceOk;
    }

    public float GetJointsAngle(int top, int middle, int bottom)
    {
        /*	Vector2 topPos = new Vector2(kinect.GetJointPosition(kinect.GetPlayer1ID(), top).y, kinect.GetJointPosition(kinect.GetPlayer1ID(), top).z);
            Vector2 middlePos = new Vector2 (kinect.GetJointPosition (kinect.GetPlayer1ID (), middle).y, kinect.GetJointPosition (kinect.GetPlayer1ID (), middle).z);
            Vector2 bottomPos = new Vector2(kinect.GetJointPosition(kinect.GetPlayer1ID(), bottom).y, kinect.GetJointPosition(kinect.GetPlayer1ID(), bottom).z);*/

        Vector3 topPos = kinect.GetJointPosition(kinect.GetPlayer1ID(), top);
        Vector3 middlePos = kinect.GetJointPosition(kinect.GetPlayer1ID(), middle);
        Vector3 bottomPos = kinect.GetJointPosition(kinect.GetPlayer1ID(), bottom);

        float angle = 0;
        // calculating the 3 distances

        /*	float ab = Vector2.Distance(topPos, middlePos);
			
            float bc = Vector2.Distance(middlePos, bottomPos);
			
            float ac = Vector2.Distance(topPos, bottomPos);
			
            float cosB = Mathf.Pow(ac, 2) - Mathf.Pow(ab, 2) - Mathf.Pow(bc, 2);
		
            cosB /= (2 * ab * bc);
			
            angle = (Mathf.Acos(cosB) * 180 / Mathf.PI);*/

        angle = Vector3.Angle(middlePos, bottomPos);

        return angle;
    }

    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

}

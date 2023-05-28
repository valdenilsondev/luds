using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.EventsSystem;
using Runner.Managers;
using Share.Managers;
using System;
using Share.KinectUtils;
using UnityEngine.SceneManagement;

public class DistanceCalibratorNew : MonoBehaviour {

	
	public static DistanceCalibratorNew instance;

	private float minLateralDistance = -0.15f;
	private float maxLateralDistance = 0.15f;
	private Vector4 initialVector = new Vector4(-1, -1, -1, -1);
	private GameObject calib_info;
	private bool isLateralDistanceOk;
	private bool isCalibrated = false;
	private bool calibratedRunning = false;


	public float minDistance = 1.5f;
	public float warningDistance = 1f;
	public KinectManager kinect;
	public Sprite greenAreaSprite, redAreaSprite, green_player_Sprite, red_player_Sprite;
	private float compensationY = 800;

	private GameObject calib_area;
	private GameObject player_img;
	private GameObject goback_arrow_img;


	// Use this for initialization
	void Start () {
		instance = this;
	
		kinect = KinectManager.Instance;

		calib_info = GameObject.Find("calib_info_bar");
		calib_area = GameObject.Find("calib_area");
		player_img = GameObject.Find("player_calib");
		goback_arrow_img = GameObject.Find("goback_arrow_img");

	}
	
	// Update is called once per frame
	void Update () {
		var playerId = kinect.GetPlayer1ID();
		Vector4 distance = kinect.GetUserPosition(playerId);
		
		if (kinect.IsPlayerCalibrated(playerId))
		{
			Vector4 vec;
			vec = kinect.GetUserPosition(playerId);

			if (vec != initialVector)
			{
				if (SceneManager.GetActiveScene().name == "Calibration")
				{
					//TODO: mover o personagem
					//Vector3 target = new Vector3(distance.x * 20, distance.y, (distance.z * 3.5f) - 10);
					float posX = vec.x*500;
					float posY = (-vec.z*500)+ compensationY;
					if(posY >0){
						posY = 0;
					}
					if(posY < -245){
						posY = -245;
					}

					player_img.gameObject.transform.localPosition = new Vector3(posX, posY, 1);// Vector3.MoveTowards(player_img.gameObject.transform.position, target, 5000);
					
					//se esta capturando da cabeca aos pes
					if (CheckHeadAndFootJoints())
					{
						goback_arrow_img.SetActive(false);
						CheckDistanceLateral(vec.x);
						
						if (isLateralDistanceOk && !isCalibrated)
						{
							StartCoroutine("Calibrated");
						}
					}
					else
					{
						isCalibrated = false;					
						calib_info.gameObject.SetActive(true);						
						calib_info.GetComponentInChildren<Text>().text = "SE AFASTE";
						calib_area.GetComponent<Image>().sprite = redAreaSprite;
						player_img.GetComponent<Image>().sprite = red_player_Sprite;
						goback_arrow_img.SetActive(true);

						if (calibratedRunning)
						{
							StopCoroutine("Calibrated");
							calibratedRunning = false;
						}
					}

				}
			}
		}
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


	
	private void CheckDistanceLateral(float lateralDistance)
	{
		//se estiver fora do limite central
		if (lateralDistance >= minLateralDistance && lateralDistance <= maxLateralDistance)
		{
			calib_info.gameObject.SetActive(false);
			isLateralDistanceOk = true;
			calib_area.GetComponent<Image>().sprite = greenAreaSprite;
			player_img.GetComponent<Image>().sprite = green_player_Sprite;
		}
		if (lateralDistance <= minLateralDistance)
		{
			calib_info.gameObject.SetActive(true);
			calib_info.GetComponentInChildren<Text>().text = "MOVA PARA DIREITA";
			isLateralDistanceOk = false;
			calib_area.GetComponent<Image>().sprite = redAreaSprite;
			player_img.GetComponent<Image>().sprite = red_player_Sprite;
		}
		if (lateralDistance >= maxLateralDistance)
		{
			calib_info.gameObject.SetActive(true);
			calib_info.GetComponentInChildren<Text>().text = "MOVA PARA ESQUERDA";
			isLateralDistanceOk = false;
			calib_area.GetComponent<Image>().sprite = redAreaSprite;
			player_img.GetComponent<Image>().sprite = red_player_Sprite;
		}
	}


	private IEnumerator Calibrated()
	{
		
		calibratedRunning = true;
		yield return new WaitForSeconds(2.5f);
		calibratedRunning = false;
		if (SceneManager.GetActiveScene().name == "Calibration")
		{
			isCalibrated = true;
			print("go to start screen");
			SceneManager.LoadScene("startScreenNew");
		}
	}
}

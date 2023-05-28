using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;
//using Share.Managers;

//controle de camera do sup
public class CameraZoomControl : MonoBehaviour {

	[SerializeField]
	private float zoomInCameraY; 
	[SerializeField]
	private float zoomInViewReferenceY;
	[SerializeField]
	private float zoomInSmoothDist;

	public float zoomOutCameraY;
	public float zoomOutViewReferenceY;
	public float zoomOutSmoothDist;
	public float zoomTime;
	public GameObject viewReference;

	private GameObject mainCamera;
	private bool zoomIn;
	private bool changingZoom;
	//referencia de camera
	private Transform cam_parent;
	private GameObject cameraSup;
	private SmoothFollow followcam;

	public static CameraZoomControl instance;

	void Awake()
	{
		//referencia para a camera principal do jogo de pesca
		cameraSup = GameObject.Find ("Main Camera").gameObject;
	}
	void Start () {
		instance = this;
		mainCamera = this.gameObject;
		//
		var cameraPos = mainCamera.transform.position;
		mainCamera.transform.position = new Vector3(cameraPos.x, zoomInCameraY, cameraPos.z);
		var viewReferencePos = viewReference.transform.position;
		viewReference.transform.position = new Vector3(viewReferencePos.x, zoomInViewReferenceY, viewReferencePos.z);		
		mainCamera.GetComponent<SmoothFollow>().distance = zoomInSmoothDist;

		//referencia do menino para animaçao inicial
		//cam_parent = SupManager.instance.GetSupBoy().transform;;
		//cameraSup.transform.SetParent(cam_parent);

		cameraSup.GetComponent<SmoothFollow>().enabled = false;


	}

	void Update()
	{
		if(changingZoom){
			if(!zoomIn)
			{
				ChangeValues(zoomOutCameraY,zoomOutViewReferenceY,zoomOutSmoothDist);
				if(mainCamera.transform.position.y >= zoomOutCameraY*0.98f){
					zoomIn = true;
					changingZoom = false;
				}
			}
			else
			{
				ChangeValues(zoomInCameraY,zoomInViewReferenceY,zoomInSmoothDist);
				if(mainCamera.transform.position.y <= zoomInCameraY*1.02f){
					zoomIn = false;
					changingZoom = false;
				}
			}
		}
	}

	public void HandleCameraZoom()
	{
		changingZoom = true;
	}
	public GameObject GetCamSup(){
		return cameraSup;
	}

	private void ChangeValues(float cameraY, float viewRefY, float smoothDist)
	{
		var cameraPos = mainCamera.transform.position;
		mainCamera.transform.position = Vector3.Lerp(cameraPos, new Vector3(cameraPos.x, cameraY, cameraPos.z), zoomTime*Time.deltaTime);

		var viewReferencePos = viewReference.transform.position;
		viewReference.transform.position = Vector3.Lerp(viewReferencePos, new Vector3(viewReferencePos.x, viewRefY, viewReferencePos.z), zoomTime*Time.deltaTime);

		mainCamera.GetComponent<SmoothFollow>().distance = Mathf.Lerp(mainCamera.GetComponent<SmoothFollow>().distance, smoothDist, zoomTime*Time.deltaTime);
	}

	public void PlaySupStartCam_1(){
		CamSup_spot();
	}
	private void CamSup_spot(){
		cameraSup.GetComponent<Animator>(). SetTrigger("StartGame1");	
	}
	private void StopCamSup(){
		//cameraSup.GetComponent<SmoothFollow>().enabled = true;
		cameraSup.GetComponent<Animator>().speed = 0;
	}
	
}

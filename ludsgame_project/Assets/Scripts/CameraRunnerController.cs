using UnityEngine;
using Runner.Managers;
using System.Collections;

public class CameraRunnerController : MonoBehaviour {

    private float duration = 1.0f;
    private float magnitude = 0.5f;

	public bool folowPlayer = false;

    public static CameraRunnerController instance;

    void Awake()
    {
        instance = this;
    }

	void Update(){
		if(folowPlayer){
			FolowPlayer();
		}
	}

	private void FolowPlayer(){
		Vector3 actual = this.transform.parent.transform.position;
		if( PlayerTrailMovement.GetCurrentPosition() == 0 ){
			this.transform.parent.transform.position = Vector3.Lerp(actual, new Vector3(-4, actual.y, actual.z), 6*Time.deltaTime);
		}else if( PlayerTrailMovement.GetCurrentPosition() == 1 ){
			this.transform.parent.transform.position = Vector3.Lerp(actual, new Vector3(0, actual.y, actual.z), 6*Time.deltaTime);
		}else if( PlayerTrailMovement.GetCurrentPosition() == 2 ){
			this.transform.parent.transform.position = Vector3.Lerp(actual, new Vector3(4, actual.y, actual.z), 6*Time.deltaTime);
		}
	}

    public void MovementInitialize()
    {
        PigRunnerManager.instance.cameraMovementAroundTheWorldCompleted = false;
    }

    public void MovementIsCompleted()
    {
		PigRunnerManager.instance.cameraMovementAroundTheWorldCompleted = true;
    }

    public void Shake()
    {
        this.GetComponent<Animator>().enabled = false;
        StartCoroutine(ShakeCamera());
    }

    IEnumerator EarthQuake()
    {
        CameraRunnerController.instance.GetComponent<CameraFilterPack_FX_EarthQuake>().gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        CameraRunnerController.instance.GetComponent<CameraFilterPack_FX_EarthQuake>().gameObject.SetActive(false);
    }

    IEnumerator ShakeCamera()
    {

        float elapsed = 0.0f;

        Vector3 originalCamPos = Camera.main.transform.position;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            Camera.main.transform.position = new Vector3(x, originalCamPos.y, originalCamPos.z);

            yield return null;
        }

        Camera.main.transform.position = originalCamPos;
        this.GetComponent<Animator>().enabled = true;
    }
}

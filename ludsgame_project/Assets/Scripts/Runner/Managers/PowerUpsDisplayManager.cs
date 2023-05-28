using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using Runner.PlayerControl;
using Assets.Scripts.Share.Managers;

public class PowerUpsDisplayManager : MonoBehaviour {

	private ScoreManager scoreManager;
	//private PlayerCollision playerCollision;
	private bool powerBoost;
	private GameObject powerBoostDisplay;
	private float boostTime = 8;
	private float boostDisplayTime = 3;
	private float auxtime;
	private bool commit;
	private Image backgrundImage;
	public Image boostCountImage;
	public Text boosttimeText;
	PowerUps powerUps;
	public Sprite boostImage, shieldImage, doubleImage, magneticImage;
	private FloorMovementControl floor;
	private bool isMoving;
    public static PowerUpsDisplayManager instance;


    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
		floor = GameObject.Find ("MapsManager").GetComponent<FloorMovementControl>();
		backgrundImage = GameObject.Find ("Image_powerup").GetComponent<Image>();
		scoreManager = GameObject.Find ("GameManager").GetComponent<ScoreManager> ();
		//playerCollision = GameObject.Find ("pig").GetComponent<PlayerCollision> ();
		//playerCollision = GameObject.Find ("Robot").GetComponent<PlayerCollision> ();
		powerBoostDisplay = GameObject.Find ("PowerUpsDisplay");

		auxtime = boostTime;
		boosttimeText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        var currentPowerUp = PowerUpManager.Instance.GetPowerUp();

        //powerUps = playerCollision.getPoweUp ();
        powerBoost = PowerUpManager.Instance.powerBoost;//playerCollision.isPowerBoost();
        isMoving = floor.IsFloorMoving ();

        if ((currentPowerUp == PowerUps.None))
        {
            Disable();
        }

        if (currentPowerUp != PowerUps.None)
        {
            PowerBoostOn();
        }
	}

	public void PowerBoostOn () {
        //if (!isMoving) {
        //    powerBoostDisplay.SetActive(false);
        //}

		if (isMoving) {
			auxtime = auxtime - Time.deltaTime;
		}

		boostCountImage.fillAmount = ((auxtime)/boostTime);

        //if ((auxtime<=boostDisplayTime) && (auxtime>0)) {

        //    if (isMoving) {
        //    powerBoostDisplay.SetActive(true);
        //    boosttimeText.text = ((int)auxtime + 1 ).ToString ();
        //    }
        //}

        if(auxtime<0) {
            PowerUpManager.Instance.DisablePowerUp();
        }
	}
	public void ChangeBackGround(PowerUps powerUps) {

		if (powerUps == PowerUps.Boost) {
			backgrundImage.sprite = boostImage; 
		}
		if (powerUps == PowerUps.DoublePoints) {
			backgrundImage.sprite = doubleImage; 
		}
		if (powerUps == PowerUps.Magnetic) {
			backgrundImage.sprite = magneticImage; 
		}
		if (powerUps == PowerUps.Shield) {
			backgrundImage.sprite = shieldImage;
		}
	}

    public void EnablePowerUpDisplay()
    {
        powerBoostDisplay.SetActive(true);
        ResetAuxTime();
    }

    public void Disable()
    {
        powerBoostDisplay.SetActive(false);
        ResetAuxTime();
    }

    public void ResetAuxTime()
    {
        auxtime = boostTime;
    }

}



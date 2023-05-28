using UnityEngine;
using System.Collections;
using Runner.Managers;
using Runner.Pool;
using Share.Managers;

public enum PowerUps {
	None=1,
	DoublePoints,
	Shield,
	Boost,
	Magnetic
}

public class PowerUpManager : MonoBehaviour {

	private static PowerUpManager instance;
	public PowerUps currentPowerUp;

    public float powerBoostSpeed = 2;
    public float powerBoostTime = 8;
	public bool powerBoost = false;
	public bool powerShield = false;
	private GameObject camera;
	public GameObject magneticCollider;

	public static PowerUpManager Instance {
		get {
			if(instance == null) {
				instance = FindObjectOfType<PowerUpManager>();

				if(instance == null) {
					Debug.LogWarning ("Objeto do tipo Sound Manager nao foi encontrado");
				}
			}

			return instance;
		}
	}
	
	void Awake () {
		instance = this;
        currentPowerUp = PowerUps.None;
		//myAudioSource = this.GetComponent<AudioSource>();
        magneticCollider = GameObject.Find("azeitona").transform.Find("Magnetic Collider").gameObject;//pig no lugar de azeitona
		camera = Camera.main.gameObject;
		camera.GetComponent<CameraFilterPack_Blur_Focus> ().enabled = false;

	}
	
	public void SetPowerUp(PowerUps powerUp) {
		currentPowerUp = powerUp;
        PowerUpsDisplayManager.instance.ChangeBackGround(powerUp);
		if(PigRunnerSoundManager.Instance != null)
			PigRunnerSoundManager.Instance.PlayPowerUps();

        switch (powerUp)
        {
            case PowerUps.None:
                break;
            case PowerUps.DoublePoints:
                break;
            case PowerUps.Shield:
                powerShield = true;
                break;
            case PowerUps.Boost:
				EnablePowerBoostState();
				camera.GetComponent<CameraFilterPack_Blur_Focus> ().enabled = true;
                FloorMovementControl.instance.PowerBoost(powerBoostSpeed, powerBoostTime);
                break;
            case PowerUps.Magnetic:
                EnableMagneticCollider();
                break;
        }
	}

	public PowerUps GetPowerUp() {
		return currentPowerUp;
	}

	/// <summary>
	/// Enables the power up. DESATIVADO!!!!
	/// </summary>
	/// <param name="powerUp">Power up.</param>
    public void EnablePowerUp(PowerUps powerUp){
		/*
        SoundManager.Instance.PlayPowerUps();
        
        //Verifica se já existe algum powerUp ativo para não existir acumulo de powerUps!
        if (currentPowerUp != PowerUps.None)
            DisablePowerUp();

        SetPowerUp(powerUp);
        ActiveAura(powerUp);
        PowerUpsDisplayManager.instance.EnablePowerUpDisplay();*/
    }

    public void DisablePowerUp()
    {
        switch (currentPowerUp)
        {
            case PowerUps.DoublePoints:
                break;
            case PowerUps.Shield:
                powerShield = false;
                break;
            case PowerUps.Boost:
				camera.GetComponent<CameraFilterPack_Blur_Focus> ().enabled = false;
                FloorMovementControl.instance.PowerBoostDown();
                DisablePowerBoostState();
                break;
            case PowerUps.Magnetic:
                MagneticController.instance.Disable();
                //magneticCollider.SetActive(false);
                break;
        }

        SetPowerUp(PowerUps.None);
        PowerUpsDisplayManager.instance.Disable();
    }

    private void ActiveAura(PowerUps powerUp)
    {
        switch (powerUp)
        {
            case PowerUps.DoublePoints:
                AuraController.instance.ActivateRedAura();
                break;
            case PowerUps.Shield:
                AuraController.instance.ActivateBlueAura();
                break;
            case PowerUps.Boost:
                AuraController.instance.ActivateGreenAura();
                break;
            case PowerUps.Magnetic:
                AuraController.instance.ActivateYellowAura();
                break;
        }        
    }
	
	public void PowerShield (Collider col)	{
		if (powerShield == true) {
			PowerUpManager.Instance.SetPowerUp (PowerUps.Shield);
			if (PowerUpManager.Instance.currentPowerUp == PowerUps.Shield) {
				if (col.gameObject.tag == "TrunkSlide") {
					ItemsPool.Instance.DestroyItem (col.transform.parent.gameObject);
				} else {
					ItemsPool.Instance.DestroyItem (col.gameObject);
				}
                DisablePowerUp();
			}
		}
	}
	
	public void EnablePowerBoostState(){
		powerBoost = true;
	}

    public void DisablePowerBoostState()
    {
        powerBoost = false;
    }

	public void SetPowerUpNone (){
		magneticCollider.SetActive (false);
		SetPowerUp(PowerUps.None);
		powerBoost = false;
		powerShield = false;
	}

	public void EnableMagneticCollider(){
        MagneticController.instance.Enable();
        //magneticCollider.SetActive(true);
    }

    public PowerUps GetRandomPowerUp()
    {
        int countPowerUps;
        countPowerUps = PowerUps.GetNames(typeof(PowerUps)).Length;
        int rand = Random.Range(2, countPowerUps);

        return (PowerUps)rand;
    }

	//public void setPowerBoostTrue (){powerBoost = true;}
	
	//public bool isPowerBoost (){return powerBoost;}
	
	//public bool IspowerShield (){return powerShield;}
	
	//public void SetPowershieldTrue (){powerShield = true;}
	
	//public void SetpowershieldFalse (){powerShield = false;}
	
	//public void PowerBoost (){scoreManager.PowerBoost ();}
	
	/*public void MagnetcBoost () {
		magneticCollider.SetActive(true);
		turnOff = true;
	}*/
	
	//public PowerUps getPoweUp (){return powerUps;}
	
	//public void EnableMagneticCollider(){magneticCollider.SetActive(true);}
	
	//public void DisableMagneticCollider(){magneticCollider.SetActive(false);}
}

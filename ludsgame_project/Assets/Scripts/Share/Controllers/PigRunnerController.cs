using UnityEngine;
using System.Collections;
using Assets.Scripts.Share.Kinect;
using System.Collections.Generic;
using Runner.Pool;
using Runner.Managers;
using Assets.Scripts.Share.Managers;
using Share.Managers;
using Assets.Scripts.Share.Enums;
using Share.EventsSystem;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//PigRunner
public class PigRunnerController : MonoBehaviour
{

	public static PigRunnerController instance;

    private string collider;
    private bool collideOnce = false;
    private bool sideCollision = false;
    public bool timeToRecover = false;

    public Transform pigHead;
	public Text userFeedback_PR;
	//[SerializeField]
	public Sprite gameover_board;

    public bool hitDownFinished;
   	public GameObject simpleHitParticle, hitStarsParticle;
    private CapsuleCollider[] playerColliders;
	//
	private bool runing;

    void Awake()
    {
        instance = this;
        playerColliders = this.GetComponentsInChildren<CapsuleCollider>();
        Iddle();
    }

    public Animator GetAnimator()
    {
        return this.GetComponent<Animator>();
    }

    public void ResetAnimsSpeed()
    {
        this.GetComponent<Animator>().speed = 1;
    }

    public void IncreaseAnimsSpeed()
    {
        if (this.GetComponent<Animator>().speed < 2f)
            this.GetComponent<Animator>().speed = this.GetComponent<Animator>().speed + 0.005f;
    }

    public void SlowDownAnimsSpeed()
    {
        this.GetComponent<Animator>().speed = this.GetComponent<Animator>().speed * 0.65f;
    }

    public void ExecuteMovement(Movement movement)
    {
        var currentPosition = PlayerTrailMovement.GetCurrentPosition();
        switch (movement)
        {
            case Movement.None:
                //print("none");
                break;
            case Movement.Jump:
                PlayerJumpSlideControl.instance.Jump();
                break;
            case Movement.Roll:
                PlayerJumpSlideControl.instance.Roll();
                break;
            case Movement.MoveLeft:
                if (currentPosition > 0)
                    print(" gesto " + movement.ToString());
                	PlayerTrailMovement.SetCurrentPosition(currentPosition - 1);
                break;
            case Movement.MoveRight:
                if (currentPosition < 2)
                    print(" gesto " + movement.ToString());
					PlayerTrailMovement.instance.GoRight();
	                PlayerTrailMovement.SetCurrentPosition(currentPosition + 1);
                break;
			/*case Movement.StopHand:
				if(GameManagerShare.instance.ready_to_call_pause)
					GameManagerShare.instance.ExecuteActionOnHandStop();
				break;*/
            default:
                print("outro gesto " + movement.ToString());
                break;
        }
    }
	private void PlaySlidePig()
	{
		//PigRunnerSoundManager.Instance.PlaySlide();
	}

	public Sprite GetGameOverBoard()
	{
		return gameover_board;
	}
    public void OnTriggerEnter(Collider col)
    {
        var colliderType = col.GetComponent<Item>();
        if (colliderType != null)
        {
            switch (colliderType.container)
            {
                case ContainerType.Collectable:
                    if (!GameManagerShare.instance.resetTime)
                    {
						if(PigRunnerSoundManager.Instance != null)
							PigRunnerSoundManager.Instance.Bite();
                        ItemsPool.Instance.DestroyItem(col.gameObject);
						GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, colliderType.value, 0);

                        //appleUI.GetComponent<Animation>().Play("appleUI");
                    }
                    break;
                case ContainerType.PowerUp:

                    if (col.CompareTag("Boost"))
                    {
                        ItemsPool.Instance.DestroyItem(col.gameObject);
                        PowerUpManager.Instance.EnablePowerUp(PowerUps.Boost);
                    }
                    if (col.CompareTag("DoublePoints"))
                    {
                        ItemsPool.Instance.DestroyItem(col.gameObject);
                        PowerUpManager.Instance.EnablePowerUp(PowerUps.DoublePoints);
                    }
                    if (col.CompareTag("Magnetic"))
                    {
                        ItemsPool.Instance.DestroyItem(col.gameObject);
                        PowerUpManager.Instance.EnablePowerUp(PowerUps.Magnetic);
                    }
                    if (col.CompareTag("Shield"))
                    {
                        ItemsPool.Instance.DestroyItem(col.gameObject);
                        PowerUpManager.Instance.EnablePowerUp(PowerUps.Shield);
                    }
                    if (col.CompareTag("RandomPowerUp"))
                    {
                        ItemsPool.Instance.DestroyItem(col.gameObject);
                        var randomPowerUp = PowerUpManager.Instance.GetRandomPowerUp();
                        PowerUpManager.Instance.EnablePowerUp(randomPowerUp);
                    }

                    break;
                case ContainerType.Obstacle:
					PigRunnerSoundManager.Instance.PlayCrashBoxNtree();
                    if (!sideCollision)
                    {
                        if (!colliderType.beenHit)
                        {
                            InstantiateHitParticles();
                            colliderType.beenHit = true;
							//chamar texto
							userFeedback_PR.text = "BATEU!";
							Invoke ("HideFeedback",1);

                            if (FloorMovementControl.instance.timeBasedGame == false && (!PowerUpManager.Instance.powerBoost) && (!PowerUpManager.Instance.powerShield))
                            {
                                if (DetectSideCollision(col))
                                {
                                    sideCollision = true;
									this.GetComponent<Animator>().SetTrigger("PigSideCollision");
                                    return;
                                }

                                if (!timeToRecover)
                                {
                                    collideOnce = true;
                                    collider = col.name;
									PigRunnerManager.instance.IncLifelost();//para controlador de dados ter total de vidas perdidas
                                    GameManagerShare.instance.LifeLost();

                                    if (!LifesManager.instance.IsDead())
                                    {
                                        StartCoroutine(TimeToRecover());
                                    }
                                }

                                ItemsPool.Instance.DestroyItem(col.gameObject);
                            }
                            colliderType.beenHit = false;
                        }
                        if (PowerUpManager.Instance.powerShield)
                        {
                            PowerUpManager.Instance.PowerShield(col);

                        }
                        if (FloorMovementControl.instance.timeBasedGame == true || (PowerUpManager.Instance.powerBoost))
                        {
                            if (PowerUpManager.Instance.powerShield == false)
                            {
                                ItemsPool.Instance.DestroyItem(col.gameObject);
                            }
                            PowerUpManager.Instance.PowerShield(col);

                        }
                    }
                    break;
            }
        }

        //Impossible Object Tag usado para transi�ao de jogo para MiniGame
        if (col.CompareTag("ImpossibleObject"))
        {
            //movingToMiniGame = true;
            FloorMovementControl.instance.Pause();
			PigRunnerSoundManager.Instance.StopRunSound();
            this.GetComponent<Animator>().SetBool("transition", true);
        }

    }

	private void HideFeedback()
	{
		userFeedback_PR.text = string.Empty;;
	}
    public void InstantiateHitParticles()
    {
        Instantiate(simpleHitParticle, this.transform.position, Quaternion.identity);

		Transform posToInstantiate = GameObject.Find("ikHandle12").transform;//hair_01 no lugar de cabelo
        GameObject particleStar = Instantiate(hitStarsParticle, new Vector3(posToInstantiate.transform.position.x + 0.45f,
                                                                            posToInstantiate.transform.position.y + 1,
                                                                            posToInstantiate.transform.position.z), Quaternion.identity) as GameObject;
        particleStar.transform.SetParent(posToInstantiate);
    }

    public void InitiateMiniGame()
    {
        SceneManager.LoadScene("MiniGame");
    }

    void OnTriggerExit(Collider col)
    {
        var colliderType = col.GetComponent<Item>();

        if (colliderType != null && colliderType.container == ContainerType.Obstacle)
        {
            if (collider == col.name)
            {
                collider = null;
                collideOnce = false;
            }
        }
    }

    IEnumerator TimeToRecover()
    {
        GetComponent<Animator>().SetTrigger("hit");
        GetComponent<Animator>().SetBool("timeToRecover", false);
        timeToRecover = true;

        var colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }
        //NAO MEXER NESSE TEMPO -> 5F (DEPENDENCIA SlowDownAnimsSpeed() )

        yield return new WaitForSeconds(5f);
        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }
        timeToRecover = false;
        GetComponent<Animator>().SetBool("timeToRecover", true);
        if (!GameManagerShare.IsPaused())
            Run();
    }

    public bool DetectSideCollision(Collider col)
    {
        for (int i = 0; i < playerColliders.Length; i++)
        {
            float radius = playerColliders[i].radius;
            Vector3 leftBound = (Vector3.left * radius) + playerColliders[i].bounds.center;
            Vector3 rightBound = (Vector3.right * radius) + playerColliders[i].bounds.center;

            if (leftBound.x > col.bounds.max.x)
            {
                PlayerTrailMovement.instance.GoRight();
                return true;
            }
            else if (rightBound.x < col.bounds.min.x)
            {
                PlayerTrailMovement.instance.GoLeft();
                return true;
            }
        }
        return false;
    }

    public void Iddle()
    {
        this.GetComponent<Animator>().SetTrigger("iddle");
    }


    public void Run()
    {
		PigRunnerSoundManager.Instance.PlayRunSound();
        this.GetComponent<Animator>().SetTrigger("running");
    }

    public void HitDownFinished()
    {
        hitDownFinished = true;
    }
}
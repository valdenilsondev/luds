using UnityEngine;
using System.Collections;
using Runner.Pool;
using Share.Managers;
using Assets.Scripts.Share.Enums;

public class MagneticController : MonoBehaviour {

    public static MagneticController instance;

    void Start()
    {
        instance = this;
        Disable();
    }

    public void OnTriggerEnter(Collider col)
    {
        var apple = col.GetComponent<Item>();

        if (apple != null)
        {
            if (apple.CompareTag("Apple") || apple.CompareTag("ApplesBasket"))
            {
				apple.ItemMagnetic(PigRunnerController.instance.transform.localPosition);
				GameManagerShare.instance.IncreaseScore(ScoreItemsType.Apple, apple.value, 0);
            }
        }
	}

    public void Enable()
    {
        this.gameObject.SetActive(true);
	}

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}

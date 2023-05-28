using UnityEngine;
using System.Collections;

public class GKLimit : MonoBehaviour {

    public GameObject blockade;
    public bool firstTime = true;
    public static GKLimit instance;

    void Awake()
    {
        instance = this;
    }

	void OnTriggerEnter(Collider col) {
        if(col.name == "bola" && firstTime)
        {
            firstTime = false;
            


            if(AnimationControllerGoalkeeper.instance.ChosenSideToSave == BarController.instance.chosenSide)
                blockade.SetActive(true);
            else
                blockade.SetActive(false);
        }
            
	}
}

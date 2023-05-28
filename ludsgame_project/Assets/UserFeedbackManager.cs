using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using Assets.Scripts.Share;

public class UserFeedbackManager : MonoBehaviour {
	public static UserFeedbackManager instance;
	public Text feedBack;

	void Awake()
	{ 
		instance = this;
		feedBack = this.gameObject.GetComponent<Text>();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowFeedBack()
	{
		feedBack.text = "ACERTOU!";
		feedBack.text = "ERROU!";
	}
}

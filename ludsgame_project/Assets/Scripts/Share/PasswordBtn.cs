using UnityEngine;
using System.Collections;

public class PasswordBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenPasswordLink(){
		Application.OpenURL($"{HttpController.urlWeb}/alterarsenha.html");
	}


	public void OpenAccountLink(){
		Application.OpenURL("http://www.eurekamob.com.br/");
	}
}

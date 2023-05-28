using UnityEngine;
using System.Collections;
using Sandbox.PlayerControl;
using Share.Database;
using UnityEngine.SceneManagement;

namespace Sandbox.UI {
	public class RegisterButtons : MonoBehaviour {
		
		public void PlayGame(){
			SceneManager.LoadScene ("Intro");	
		}
		
		
		public void ConsultarPlayer(){
			
			
		//	print (MySQL.instance.GetPlayerMaxScore (Player.GetIdPlayer()));
		}
	}
}

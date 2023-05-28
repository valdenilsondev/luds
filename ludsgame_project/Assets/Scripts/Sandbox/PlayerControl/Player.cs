using UnityEngine;
using System.Collections;

using Share.Database;

namespace Sandbox.PlayerControl {
	public class Player {
		
		private static int idPlayer = -1;
		private static string namePlayer = " ";
		private static char deSex = ' ';
		private static int nuAge = -1;
		private static int idFisio = -1;
		
		
		public static void InitPlayer(int id, string name, char gender, int age, int idFisio) {
			InitPlayer(name, gender, age, idFisio);
			idPlayer = id;
			//		Debug.Log("id player (InitPlayer()): "+idPlayer);
		}
		
		public static void InitPlayer(string name, char gender, int age, int id_fisio) {
			namePlayer = name;
			deSex = gender;
			nuAge = age;
			idFisio = id_fisio;
		}
		
		public static void InitPlayer() {
		//	MySQL.instance.SelectPlayer("Teste");
		}
		
		public static int GetIdPlayer() {
			if(idPlayer == -1){
				InitPlayer();
			}
			//Debug.Log("id player (GetIdPlayer()): "+idPlayer);
			return idPlayer;
		}
		
		public static string GetNamePlayer() {
			if(namePlayer == " "){
				InitPlayer();
			}
			return namePlayer;
		}
		
		public static char GetGender() {
			if(deSex == ' '){
				InitPlayer();
			}
			return deSex;
		}
		
		public static int GetAge() {
			if(nuAge == -1){
				InitPlayer();
			}
			return nuAge;
		}
		
		
		public static int GetIdFisio() {
			if(idFisio == -1){
				InitPlayer();
			}
			return idFisio;
		}
		
		
	}
}

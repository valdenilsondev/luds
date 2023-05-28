using UnityEngine;
using System.Collections;

using Share.KinectUtils;

public enum GameState {
	Menu,
	Game,
	Pause,
	GameOver
}
namespace BridgeGame.GameUtilities {
	public class GameManager : MonoBehaviour {
		
		private static GameManager instance;
		
		private GameState currentGameState = GameState.Menu;
		private KinectGestures.Gestures currentGesture;
		
		void Awake() {
			DontDestroyOnLoad(this);
			instance = this;
		}

		public GameState GetGameState(){
			return currentGameState;
		}
		
		public void SetGameState(GameState newGameState){
			currentGameState = newGameState;
		}
		
		public void SetGesture(KinectGestures.Gestures gesture){
			currentGesture = gesture;
		}
		
		public KinectGestures.Gestures GetGesture(){
			return currentGesture;
		}
		
		/// <summary>
		/// Returns GameManager instance
		/// </summary>
		public static GameManager Instance(){
			if(instance == null){
				instance = FindObjectOfType<GameManager>();
				if(instance == null){
					Debug.LogError("Nenhum GameObject do tipo GameManager foi encontrado");
				}
			}
			
			return instance;
		}
	}
}
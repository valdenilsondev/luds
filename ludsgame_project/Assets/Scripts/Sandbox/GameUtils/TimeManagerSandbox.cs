using UnityEngine;
using System.Collections;

namespace Sandbox.GameUtils {
	public class TimeManagerSandbox {
		
		private float initialTime = 0;
		private float finalTime = 0;
		
		public TimeManagerSandbox() {
			this.initialTime = 0;
			this.finalTime = 0;
		}
		
		private void ResetTimer() {
			initialTime = 0;
			finalTime = 0;
		}
		
		public void StartTimer() {
			initialTime = Time.realtimeSinceStartup;
		}
		
		public int StopTimer() {
			finalTime = Time.realtimeSinceStartup;
			
			int deltaTime = Mathf.RoundToInt((finalTime - initialTime)/60f);
			if(deltaTime == 0) deltaTime++;
			
			ResetTimer();
			
			return deltaTime;
		}
		
	}
}
using UnityEngine;
using System.Collections;

namespace Share.Database {
	public class Gesture {
		public int id_gesture;
		public int gesture;
		public string frames_gesture;
		public int initial_frame_id;
		public int final_frame_id;
		
		public Gesture(int id_gesture, int gesture, string frames_gesture,
		               int initial_frame_id, int final_frame_id) {
			
			this.id_gesture = id_gesture;
			this.gesture = gesture;
			this.frames_gesture = frames_gesture;
			this.initial_frame_id = initial_frame_id;
			this.final_frame_id = final_frame_id;
		}
	}
}
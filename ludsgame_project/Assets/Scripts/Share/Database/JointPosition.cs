using UnityEngine;
using System.Collections;

namespace Share.Database {
	public class JointPosition {
		
		int id_jointposition;
		int id_frame;
		string pos_hips;
		string pos_spine;
		string pos_neck;
		string pos_head;
		string pos_left_shoulder;
		string pos_left_upper_arm;
		string pos_left_elbow;
		string pos_left_hand;
		string pos_right_shoulder;
		string pos_right_upper_arm;
		string pos_right_elbow;
		string pos_right_hand;
		string pos_left_thigh;
		string pos_left_knee;
		string pos_left_foot;
		string pos_left_toes;
		string pos_right_thigh; 
		string pos_right_knee;
		string pos_right_foot;
		string pos_right_toes;
		string pos_root;
		
		
		
		public JointPosition (int id_jointposition, int id_frame, string pos_hips, string pos_spine, string pos_neck, string pos_head, string pos_left_shoulder, 
		                      string pos_left_upper_arm, string pos_left_elbow, string pos_left_hand, string pos_right_shoulder, 
		                      string pos_right_upper_arm, string pos_right_elbow, string pos_right_hand, string pos_left_thigh, 
		                      string pos_left_knee, string pos_left_foot, string pos_left_toes, string pos_right_thigh, 
		                      string pos_right_knee, string pos_right_foot, string pos_right_toes, string pos_root){
			
			this.id_jointposition = id_jointposition;
			this.id_frame = id_frame;
			this.pos_hips = pos_hips;
			this.pos_spine = pos_spine;
			this.pos_neck = pos_neck;
			this.pos_head = pos_head;
			this.pos_left_shoulder = pos_left_shoulder;
			this.pos_left_upper_arm = pos_left_upper_arm;
			this.pos_left_elbow = pos_left_elbow;
			this.pos_left_hand = pos_left_hand;
			this.pos_right_shoulder = pos_right_shoulder;
			this.pos_right_upper_arm = pos_right_upper_arm;
			this.pos_right_elbow = pos_right_elbow;
			this.pos_right_hand = pos_right_hand;
			this.pos_left_thigh = pos_left_thigh;
			this.pos_left_knee = pos_left_knee;
			this.pos_left_foot = pos_left_foot;
			this.pos_left_toes = pos_left_toes;
			this.pos_right_thigh = pos_right_thigh;
			this.pos_right_knee = pos_right_knee;
			this.pos_right_foot = pos_right_foot;
			this.pos_right_toes = pos_right_toes;
			this.pos_root = pos_root;
			
			
		}
	}
}
using UnityEngine;
using System.Collections;

namespace Share.Database {
	public class Frame  {
		public  int id_frame;
		public  string pos_skel;
		public  int num_frame;
		public  int id_partida;
		
		
		public Frame(int id_frame, string pos_skel, int num_frame, int id_partida){
			this.id_frame = id_frame;
			this.pos_skel = pos_skel;
			this.num_frame = num_frame;	
			this.id_partida = id_partida;
		}
	}
}
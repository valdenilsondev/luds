using UnityEngine;
using System.Collections;

public class GestureTimes {
	public int id_gesturetimes;
	public int estagio;
	public int initial_time;
	public int id_gesture;
	
	
	public GestureTimes(int id_gesturetimes, int estagio, int initial_time,
	                    int id_gesture) {
		
		this.id_gesturetimes = id_gesturetimes;
		this.estagio = estagio;
		this.initial_time = initial_time;
		this.id_gesture = id_gesture;
	}
	
	
}

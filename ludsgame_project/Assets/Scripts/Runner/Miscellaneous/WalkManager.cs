using UnityEngine;
using System.Collections;
using Share.EventsSystem;

public class WalkManager : MonoBehaviour {
	Walker[] walks;
	public float time;
	public float set_dist;
	private float dist;
	public GameObject pig;
	Vector3 player;
	public float range;
	private bool Ispaused=false;

	private bool soundPlayed = false;
	public AudioSource bullSFX;
	public AudioSource gallopSFX;

	// Use this for initialization
	void Start () {
	player = pig.transform.position;
		if (range.Equals (null)) {
			range = 90;
		}
		if (set_dist.Equals(null)) { 
			set_dist = 50;
		}
		if (time.Equals(null)) {
			time = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//player = pig.transform.position;
		GameObject[] read_walks = GameObject.FindGameObjectsWithTag ("walk");

		for (int i=0; i<read_walks.Length; i++) {
			print ("distance = " + Vector3.Distance (player, read_walks [0].transform.position));
			if (Vector3.Distance (player, read_walks [i].transform.position) > range) {
				player = pig.transform.position;
				read_walks [i].transform.position = new Vector3 (pig.transform.position.x, read_walks [i].transform.position.y, read_walks [i].transform.position.z);
			}

		}

		walks = new Walker[read_walks.Length];
		for (int i =0; i<walks.Length; i++) {
			walks [i] = new Walker (read_walks [i]);
		}
		//	print("quantidade =" + read_walks.Length);
	
		for (int i =0; i<walks.Length; i++) {
				
			int count = 0;
			//	print ("setando distancia pela "+count+" vez");
			count++;
			walks [i].SetDist (player);
			if (walks [i].isActive()) {
				if (walks [i].GetDist () < set_dist) {
					if (Ispaused == false) {
						if(!soundPlayed) {
							soundPlayed = true;
							
							if (bullSFX)
								bullSFX.Play();
							
							if (gallopSFX)
								gallopSFX.Play ();
						}
						walks [i].SetPosition (player, time);
					}
				}
				if (walks [i].walker_obj.transform.position.z + 20 < player.z) {
					gallopSFX.Stop();
					walks [i].SetActive (false);
				}

			}
		}
	}

	void OnEnable(){
		Events.AddListener<PauseEvent> (OnPause);

		Events.AddListener<UnPauseEvent> (OnUnPause);
	}
	public void OnPause(){
		Ispaused = true;
	}
	
	
	public void OnUnPause(){
		Ispaused = false;
	}

	private class Walker {
		public GameObject walker_obj;
		public float dist;

		public Walker (GameObject walker) {
			this.walker_obj = walker;
			}
		public void SetDist(Vector3 target) {
			dist = Vector3.Distance(target, this.walker_obj.transform.position);
		//	print ("distancia na classe " + dist);
		}
		public float GetDist () {
			return dist;
		}
		public bool isActive() {
			return this.walker_obj.activeSelf;
		}
		public void SetActive (bool flag) {
			this.walker_obj.SetActive (flag);
		}

		public void SetPosition (Vector3 target, float time) {
			Vector3 backPlay = new Vector3 (target.x, target.y, target.z-10);
			this.walker_obj.transform.position = Vector3.Lerp (this.walker_obj.transform.position, backPlay, time*Time.deltaTime); 
		}
}
}
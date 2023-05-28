using UnityEngine;
using System.Collections;
using Runner.Pool;


public enum ContainerType {
	Collectable,
	Obstacle,
	Environment,
    PowerUp
}

public class Item : MonoBehaviour {

	public ItemsType type;
	public ContainerType container;
    public int value;

	private bool magnetic;
	private Vector3 target = Vector3.zero;

    public bool beenHit;

	public void ItemMagnetic(Vector3 playerPos) {
		magnetic = true;
		target = playerPos;
	}

	void Update() {
		if (magnetic) {
			this.transform.position = Vector3.Lerp (this.transform.position, target, Time.deltaTime * 10);	
			if (Vector3.Distance (target, this.transform.position) < 1f) {
				magnetic = false;
				//Instantiate (ScoreManager.instance.particle, this.transform.position, Quaternion.identity);
				ItemsPool.Instance.DestroyItem(this.gameObject);
			}
		}
	}

}

using UnityEngine;
using System.Collections;

namespace Sandbox.UI {
	public class PresentationScreen : MonoBehaviour {
		
		public Transform brilho;
		public float rotationSpeed;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			brilho.Rotate(new Vector3(0,0,1 * Time.deltaTime * rotationSpeed));

			if(Input.GetKeyDown(KeyCode.H)){
				this.gameObject.GetComponent<Animation>().Play("HidePresentationScreen");
			}
		}
	}
}

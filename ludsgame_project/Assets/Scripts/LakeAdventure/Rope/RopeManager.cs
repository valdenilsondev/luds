using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopeManager : MonoBehaviour {
	//array dos cubes (rope)
	private List<GameObject> cubes_array = new List<GameObject>();

	public GameObject [] rod_front;
	private GameObject bait;
	public GameObject [] bait_throw_pos2;
	public GameObject [] bait_throw_pos1;
	public GameObject [] resting_point;
	public float bait_throw_time;
	public float rope_size;
	public float cubes_dist;
	public float line_thickness;
	public float rope_mass;
	public float friction;

	public bool throwing_rope;
	private int currentSpot;
	//instancia da classe
	public static RopeManager instance;

	void Awake(){
		instance = this;
	}

	void Start(){
		currentSpot = Batata_Fishing_Control.instance.GetIndexRdSpot();
		CreateRopeLine();
		RopeToRestingPoint();
	}

	void Update(){
		DrawLines();
	}

	public void CreateRopeLine(){
		//aumentar numero de iteracoes da fisica
		Physics.defaultSolverIterations = 10;

		//instanciando cubo primitivo
		var line = GameObject.CreatePrimitive(PrimitiveType.Cube);

		//adicionar rigidbody
		line.AddComponent<Rigidbody>();
		//adicionar line renderer
		line.AddComponent<LineRenderer>();
		line.GetComponent<LineRenderer>().material.shader = Shader.Find("Legacy Shaders/Particles/Additive");
		line.GetComponent<LineRenderer>().SetColors(Color.white, Color.white);
		//adicionar junta
		line.AddComponent<ConfigurableJoint>();
		line.GetComponent<ConfigurableJoint>().enablePreprocessing = false;

		//fazer cube invisivel
		line.GetComponent<MeshRenderer>().enabled = false;

		//remover colisores
		line.GetComponent<BoxCollider>().enabled = false;

		//basic stats
		line.GetComponent<Rigidbody>().mass = rope_mass;
		line.GetComponent<Rigidbody>().drag = friction;

		//freeze rotations (em teste)
		//line.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

		//mudar parent do topo da linha
		line.transform.position = rod_front[currentSpot].transform.position;
		line.transform.parent = rod_front[currentSpot].transform.parent;

		//adiciona no array
		cubes_array.Add(line);

		//criando varias copias do cubo primitivo(line) um embaixo do outro
		for(int i = 1; i < rope_size; i++){
			GameObject obj = GameObject.Instantiate(line);
			cubes_array.Add(obj);
			
			var obj_pos = line.transform.position;
			cubes_array[i].transform.position = new Vector3(obj_pos.x, cubes_array[i-1].transform.position.y - cubes_dist, obj_pos.z);
		}

		//gerenciando o comportamento das juntas e as conexoes
		for(int i = 0; i < rope_size; i++){
			cubes_array[i].GetComponent<LineRenderer>().SetWidth(line_thickness, line_thickness);
			cubes_array[i].GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Limited;
			cubes_array[i].GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Limited;
			cubes_array[i].GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Limited;
			
			cubes_array[i].GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Limited;
			cubes_array[i].GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Limited;
			cubes_array[i].GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Limited;
			
			if(i != 0)
				cubes_array[i].GetComponent<ConfigurableJoint>().connectedBody = cubes_array[i-1].GetComponent<Rigidbody>();
		}
		line.GetComponent<Rigidbody>().isKinematic = true;

		//adicionando isca no fim da linha
		bait = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		bait.transform.position = cubes_array[cubes_array.Count-1].transform.position;
		bait.AddComponent<FixedJoint>().connectedBody = cubes_array[cubes_array.Count-1].GetComponent<Rigidbody>();
		bait.GetComponent<FixedJoint>().enablePreprocessing = false;
		bait.GetComponent<Rigidbody>().isKinematic = true;
		bait.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
	}

	public void DestroyRope(){
		if(cubes_array.Count == 0){
			return;
		}
		for(int i = 1; i < rope_size-1; i++){
			GameObject.Destroy(cubes_array[i]);
		}
		cubes_array = new List<GameObject>();
		GameObject.Destroy(bait);
	}

	//move a linha durante a animacao de arremeso da isca
	public void AnimateRopeOnThrow(){
		throwing_rope = true;
		var time = bait_throw_time*0.3f;
		iTween.MoveTo(bait, iTween.Hash("position", bait_throw_pos1[currentSpot].transform.position, "oncomplete", "AnimateRopeOnThrow2", "oncompletetarget", this.gameObject, "time", time));
	}

	private void AnimateRopeOnThrow2(){
		var time = bait_throw_time*0.7f;
		iTween.MoveTo(bait, iTween.Hash("position", bait_throw_pos2[currentSpot].transform.position, "time", time));
	}

	public void AllowToThrowBaitAgain(){
		throwing_rope = false;
	}

	public void RopeToRestingPoint(){
		iTween.MoveTo(bait, iTween.Hash("position", resting_point[currentSpot].transform.position, "oncomplete", "AllowToThrowBaitAgain", "oncompletetarget", this.gameObject, "time", bait_throw_time));
	}

	public GameObject GetBait(){
		return bait;
	}

	private void DrawLines(){
		if(cubes_array.Count < 1){
			return;
		}
		for(int i = 0; i < rope_size; i++){
			if(i == rope_size-1){
				return;
			}
			//draw line			
			cubes_array[i].GetComponent<LineRenderer>().SetPosition(0, new Vector3(cubes_array[i].transform.position.x, cubes_array[i].transform.position.y, cubes_array[i].transform.position.z));
			cubes_array[i].GetComponent<LineRenderer>().SetPosition(1, new Vector3(cubes_array[i+1].transform.position.x, cubes_array[i+1].transform.position.y, cubes_array[i+1].transform.position.z));
		}
	}




}


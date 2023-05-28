using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Share.KinectUtils;
public class AngleTest : MonoBehaviour {
	
 	public Text angleText;
	public GameObject top, mid, bot;
	public static AngleTest instance;
 	//public Text angleText;
	 	//public InputField top, mid, bot;
			

	void Awake()
	{
		instance = this;
	}
		
	// Update is called once per frame
	void Update () {
		//print(CalculateAng(top.transform.position,mid.transform.position,bot.transform.position,true));
	}

 	public float CalculateAng(Vector3 first_vec, Vector3 mid_vec, Vector3 last_vec, bool draw_lines){
	 		//mid_vec deve ser o vertice
		 	var sideA = Vector3.Distance(first_vec, mid_vec);
	 		var sideB = Vector3.Distance(mid_vec, last_vec);
	 		var sideC = Vector3.Distance(first_vec, last_vec);
	 		
		 	//debug
	 		if(draw_lines){
		 			Debug.DrawLine(first_vec, mid_vec, Color.red);
		 			Debug.DrawLine(mid_vec, last_vec, Color.red);
		 			Debug.DrawLine(first_vec, last_vec, Color.green);
		 	}
	 
		 	//calcular angulo oposto
			float cAng = (sideA*sideA +sideB*sideB - sideC*sideC)/(2*sideA*sideB);
			float rad = Mathf.Acos(cAng);
			float result = 0;
			result = rad * Mathf.Rad2Deg;
			return result;
	}

}
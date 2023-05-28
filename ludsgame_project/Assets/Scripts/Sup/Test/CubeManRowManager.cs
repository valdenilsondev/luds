using UnityEngine;
using System.Collections;

public class CubeManRowManager : MonoBehaviour {
	
	public GameObject top_plane;
	public GameObject bottom_plane;
	public GameObject cube_man_center;
	public GameObject cube_man_left_hand;
	public GameObject cube_man_right_hand;
	public float hands_distance;

	public static bool handsClose = false;
	public static bool touchedTop = false;
	public static bool touchedBottom = false;
	private static bool row_left = false;
	private static bool row_right = false;

	private GameObject cube;
	private float touchedTop_timer;

	public static CubeManRowManager instance;

	void Awake()
	{
		instance = this;
		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	void Update()
	{
		if(Vector3.Distance(cube_man_left_hand.transform.position, cube_man_right_hand.transform.position) < hands_distance)
		{
			if(!handsClose)
			handsClose = true;
		}
		else
		{
			if(handsClose)
			handsClose = false;
		}

		if(touchedBottom && handsClose)
		{
			//aplicar remada
			Row();
		}
		if(touchedTop)
		{
			ResetTouchedTopOnTime();
		}
		if(handsClose)
		{
			//altera posicao do cube referente a posicao das maos
			ThePointBetweenTwoVectors();
		}
	}

	private void Row()
	{
		if(cube.transform.position.x > cube_man_center.transform.position.x)
		{
			SetRow("left");
			touchedBottom = false;
		}
		else if(cube.transform.position.x < cube_man_center.transform.position.x)
		{
			SetRow("right");
			touchedBottom = false;
		}
	}

	private void ThePointBetweenTwoVectors()
	{
		//altera a posicao do cubo entre as maos
		var result = (cube_man_left_hand.transform.position + cube_man_right_hand.transform.position) / 2;
		cube.transform.position = result;
	}

	private void ResetTouchedTopOnTime()
	{
		touchedTop_timer = touchedTop_timer + Time.deltaTime;
		if(touchedTop_timer > 2)
		{
			if(touchedTop)
			{
				touchedTop = false;
			}
			touchedTop_timer = 0;
		}
	}

	private static bool DidItTouchTop()
	{
		if(touchedTop){
			return true;
		}
		return false;
	}

	private static bool DidItTouchBottom()
	{
		if(touchedBottom){
			return true;
		}
		return false;
	}

	public GameObject GetTopPlane()
	{
		return top_plane;
	}

	public GameObject GetBottomPlane()
	{
		return bottom_plane;
	}

	public static bool IsRowLeft()
	{
		return row_left;
	}

	public static bool IsRowRight()
	{
		return row_right;
	}

	public static void ResetRows()
	{
		row_left = false;
		row_right = false;
	}

	public static void SetRow(string side)
	{
		if(side == "left")
		{
			row_left = true;
			row_right = false;
		}
		else if(side == "right")
		{
			row_left = false;
			row_right = true;
		}
	}

}
















using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniMapManager : MonoBehaviour {

	public Image mini_map;
	public Image player_on_mini_map;
	public GameObject prancha;

	public Terrain terrain;
	private float terrain_width;
	private float terrain_length;
	private float mini_mapWidth;
	private float mini_mapHeight;

	private float new_mini_map_pos_x;
	private float new_mini_map_pos_y;

	void Start()
	{
		//
		terrain_width = terrain.terrainData.size.x;
		terrain_length = terrain.terrainData.size.z;
		//
		mini_mapWidth = mini_map.GetComponent<RectTransform>().sizeDelta.x;
		mini_mapHeight = mini_map.GetComponent<RectTransform>().sizeDelta.y;
	}

	void Update ()
	{
		//
		RotatePlayerMiniMap();
		//
		MoveMiniMap();
	}

	private void RotatePlayerMiniMap()
	{
		//var rot_player_mm = player_mini_map.GetComponent<RectTransform>().rotation;
		var rot_prancha = prancha.transform.localRotation.eulerAngles;
		player_on_mini_map.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,-rot_prancha.y);
	}

	private void MoveMiniMap()
	{
		//mini map -> width 924 ... height 792
		//terrain -> width 1500 ... length 1500
		//razao -> width 1.623 ... height 1.894

		new_mini_map_pos_x = -( prancha.transform.localPosition.x / (terrain_width/mini_mapWidth) );
		new_mini_map_pos_y = -( prancha.transform.localPosition.z / (terrain_length/mini_mapHeight) );

		mini_map.GetComponent<RectTransform>().localPosition = new Vector3(new_mini_map_pos_x,
		                                                              new_mini_map_pos_y, 0);

		/*print ("prancha pos: "+prancha.transform.position.x+" "+prancha.transform.position.z);
		print ("razao: "+terrain_width/mini_mapWidth+" "+(terrain_length/mini_mapHeight));
		print ("result: "+new_mini_map_pos_x+" "+new_mini_map_pos_y);*/
	}






















}

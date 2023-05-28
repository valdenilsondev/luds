#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

using UnityEditor;
using System.Collections.Generic;
using Runner.Pool;



[CustomEditor(typeof(MapExample))]
public class LevelConfiguration : Editor {

	private MapExample map;
	private Transform[] items;
	private GameObject[] containers;
	private string[] containerNames = new string[]{"Collectables", "Obstacles", "Environment"};

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("New")) {
			map = (MapExample)target;

			InitContainers();
			DestroyAllItems();

			if(!string.IsNullOrEmpty(map.mapName)) {
				map.platform = ScriptableObject.CreateInstance<PlatformSO>();
				PlatformSO s = AssetDatabase.LoadAssetAtPath("Assets/Resources/Scriptable Objects/" + map.mapName + ".asset", typeof(PlatformSO)) as PlatformSO;
				if(s == null) {
					AssetDatabase.CreateAsset(map.platform, "Assets/Resources/Scriptable Objects/" + map.mapName + ".asset");
				} else {
					AssetDatabase.CreateAsset(map.platform, "Assets/Resources/Scriptable Objects/New" + map.mapName + ".asset");
				}

			}
		}

		if(GUILayout.Button("Load")) {
			map = (MapExample)target;
			InitContainers();
			DestroyAllItems();
			LoadAllAssetsByLabel("Prefab");
			LoadItems ();
		}

		if(GUILayout.Button("Save")) {
			map = (MapExample)target;
			SaveAll();
		}
		GUILayout.EndHorizontal();
	}

	private void InitContainers() {
		map = (MapExample)target;
		
		if (containers == null) {
			CreateContainers();
		}
	}

	private void CreateContainers() {
		containers = new GameObject[3];
		
		for (int i = 0; i < containers.Length; i++) {
			containers[i] = GameObject.Find(containerNames[i]);
			if (containers[i] == null) {
				containers[i] = new GameObject(containerNames[i]);
				containers[i].transform.SetParent(map.transform);
				containers[i].transform.SetAsFirstSibling();
				containers[i].transform.localPosition = Vector3.zero;
				containers[i].transform.localRotation = Quaternion.identity;
			}
		}
	}
	
	private void DestroyAllItems() {
		if (containers != null) {
			for (int i = 0; i < containers.Length; i++) {
				while (containers[i].transform.childCount > 0) {
					DestroyImmediate (containers [i].transform.GetChild (0).gameObject);
				}
			}
		}
	}

	private void LoadItems() {
		Transform mapTransform = map.transform;
		mapTransform.tag = map.platform.platformTag;
//		map.GetComponent<PlataformInfo>().SetPlataformSide(map.platform.platformInfo);

		for (int i = 0; i < map.platform.GetCount(); i++) {
			if(items[(int)map.platform.GetItemType(i)] == null) {
				Debug.Log(map.platform.GetItemType(i));
			}
			Transform t = Instantiate (items[(int)map.platform.GetItemType(i)]) as Transform;
			t.gameObject.SetActive(true);

			switch (t.GetComponent<Item>().container) {
			case ContainerType.Collectable:
				t.SetParent(containers[(int)ContainerType.Collectable].transform);
				break;
			case ContainerType.Obstacle:
				t.SetParent(containers[(int)ContainerType.Obstacle].transform);
				break;
			case ContainerType.Environment:
				t.SetParent(containers[(int)ContainerType.Environment].transform);
				break;
			default:
				t.SetParent(containers[(int)ContainerType.Collectable].transform);
				break;
			}

			t.localPosition = map.platform.GetItemPos(i);
			t.localEulerAngles = map.platform.GetItemRotation(i);
		}
	}

	private void LoadAllAssetsByLabel(string type) {
		string[] GUIDs = AssetDatabase.FindAssets("t:" + type, new string[]{"Assets/Prefabs/Environment", "Assets/Prefabs/Items"});
		items = new Transform[(int)ItemsType.Count];
		string guid;
		string assetPath;
		Transform asset;
		Item item;
		
		for (int i = 0; i < (int)ItemsType.Count; i++) {
			for(int j = 0; j < GUIDs.Length; j++) {
				guid = GUIDs[j];
				assetPath = AssetDatabase.GUIDToAssetPath(guid);
				asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Transform)) as Transform;

				item = asset.GetComponent<Item>();
				if(item != null && item.type == (ItemsType) i) {
					items[i] = asset;
				}
			}
		}
	}

	private void SaveAll() {
		Transform mapTransform = map.transform;
		map.platform.ClearLists();

		map.platform.platformTag = map.tag;
		//map.platform.platformInfo = map.GetComponent<PlataformInfo>().GetPlataformSide();

		CreateContainers();

		for (int i = 0; i < containers.Length; i++) {
			Transform container = containers[i].transform;
			for (int j = 0; j < container.childCount; j++) {
				Transform mapChild = container.GetChild(j);
				Item item = mapChild.GetComponent<Item>();
			
				if(item != null) {
					ItemsType type = item.type;

					map.platform.AddItem(type, mapChild.localPosition, mapChild.localEulerAngles);
				} else {
					Debug.LogWarning("Item sem script");
				}
			}
		}

		SaveMap();
	}

	private void SaveMap() {
		if(map.platform != null) {
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(map.platform);
			AssetDatabase.SaveAssets();
			Debug.Log ("Map Saved");
		}
	}
}
#endif
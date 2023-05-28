using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner.Pool {
	public enum ItemsType {
		Apple = 0,
		ApplesBasket = 1,
		TrunkSlide = 2,
		TrunkJump = 3,
		Ramp = 4,
		Side = 5,
		Mill = 6,
		Tree = 7,
		Plank = 8,
		Box = 9,
		Bull = 10,
		Goat = 11,
		Tunnel = 12,
		BoxStack = 13,
		BoxDouble = 14,
		BoxFila = 15,
		SuperBox = 16,
		RandomPowerUp = 17,
		PowerUpShield = 18,
		PowerUpBooster = 19,
		PowerUpMagnetic = 20,
		PowerUpDouble = 21,
		Cactus = 22,
		Stone01 = 23,
		Stone02 = 24,
		Stone03 = 25,
		GrassyStone01 = 26,
		GrassyStone02 = 27,
		GrassyStone03 = 28,
		GrassyStone04 = 29,
		GrassyStone05 = 30,
		GrassyStoneGroups = 31,
		PineTree01 = 32,
		PineTree02 = 33,
		PineTree03 = 34,
		PineTree04 = 35,
		PineTree05 = 36,
		Tree01 = 37,
		Tree02 = 38,
		Tree03 = 39,
		Tree04 = 40,
		Tree04withLadders = 41,
		TreeStumpandAxe = 42,
		Wagon = 43,
		TreeStump01 = 44,
		Ladder02 = 45,
		Fence01Horizon = 46,
		CornField = 47,
		StackHaybox = 48,
		StackHaybox2 = 49,
		Haybox01 = 50,
		Haybox02 = 51,
        WagonEnvironment = 52,
		CornTree = 53,
		Count //Sempre por ultimo
	}

	public class ItemsPool : MonoBehaviour {

		private Dictionary<ItemsType, Transform> itemsPrefabs = new Dictionary<ItemsType, Transform>();
		private Transform[] itemsContainer = new Transform[(int)ItemsType.Count];
		private static ItemsPool instance;
		
		public static ItemsPool Instance {
			get {
				if(instance == null) {
					instance = FindObjectOfType<ItemsPool>();
					if(instance == null) {
						Debug.LogWarning("ItemsPool nao foi encontrado");
					}
				}
				
				return instance;
			}
		}
		
		void Awake() {
			InitItemsContainer();
		}
		
		public Transform InstantiateItem(ItemsType item) {
			int itemIndex = GetAvaiableIndex((int)item);
			
			return itemsContainer[(int)item].GetChild(itemIndex);
		}
		
		public void DestroyItem(GameObject item) {
			item.SetActive(false);
			Item i = item.GetComponent<Item>();
			
			if(i != null) {
				item.transform.SetParent(itemsContainer[(int)i.type]);
			} else {
				Debug.Log ("Objeto: {" + item.name + "} nao possui script item");
			}
		}
		
		private void InitItemsContainer() {
			Item[] itemsList = Resources.LoadAll<Item>("Runner/Items");

			// Inicializaçao do dictionary com apenas um prefab de cada item
		
			for (int i = 0; i < itemsList.Length; i++) {
				if(!itemsPrefabs.ContainsKey(itemsList[i].type)) {
					itemsPrefabs.Add(itemsList[i].type, itemsList[i].transform);
				} else {
					Debug.LogError ("Mais de 1 objeto com o tipo " + itemsList[i].type + "!");
				}

				// Criaçao dos containers de cada tipo de item
				GameObject itemContainer = new GameObject(itemsList[i].name);
				// Inicializaçao da lista de containers dos itens
				itemsContainer[(int)itemsList[i].type] = itemContainer.transform;
				itemContainer.transform.SetParent(this.transform);
			}

			// Percorrendo o dictionary para povoar o pool
			Transform newItem;
			foreach (KeyValuePair<ItemsType, Transform> item in itemsPrefabs) {
				// Quantidade de itens que vao ser instanciados
				for (int j = 0; j < FloorMovementControl.instance.GetItemsCount(item.Key); j++) {
					newItem = Instantiate<Transform>(item.Value);
					
					newItem.transform.SetParent(itemsContainer[(int)item.Key]);
					newItem.gameObject.SetActive(false);
					newItem.localPosition = new Vector3(100f, 100f, 100f);
				}
			}
		}

		private Transform InstantiateNewItem(ItemsType type) {
			Transform newItem;
			newItem = Instantiate<Transform>(itemsPrefabs[type]);
			
			newItem.transform.SetParent(itemsContainer[(int)type]);
			newItem.gameObject.SetActive(false);
			newItem.localPosition = new Vector3(100f, 100f, 100f);
			newItem.SetAsFirstSibling();

			return newItem;
		}
		
		private int GetAvaiableIndex(int itemType) {		
			//print (itemsContainer [itemType].name);
			for (int i = 0; i < itemsContainer[itemType].childCount; i++) {

				if(!itemsContainer[itemType].GetChild(i).gameObject.activeSelf) {
					return i;
				}
			}

			InstantiateNewItem((ItemsType) itemType);
			
			return 0;
		}
	}

}

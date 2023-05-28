using UnityEngine;
using System.Collections;

//using UnityEditor;
using System.Collections.Generic;
using System;
using Runner.Pool;
using Assets.Scripts.Share.Enums;

[Serializable]
public class PlatformSO : ScriptableObject {

	[SerializeField]
	public List<Vector3> itemsPos = new List<Vector3>();

	[SerializeField]
	public List<Vector3> itemsRot = new List<Vector3>();

	[SerializeField]
	public List<ItemsType> itemsType = new List<ItemsType>();

    [SerializeField]
    public GroupTypes groupTypes;

	[SerializeField]
	public LevelGroups levelGroups;
	
	[SerializeField]
	public string platformTag;

	public void AddItem(ItemsType type, Vector3 itemPos, Vector3 quaternion) {
		itemsPos.Add(itemPos);
		itemsType.Add(type);
		itemsRot.Add(quaternion);
	}

	public Vector3 GetItemPos(int i) {
		return itemsPos[i];
	}

	public ItemsType GetItemType(int i) {
		return itemsType[i];
	}

	public Vector3 GetItemRotation(int i) {
		return itemsRot[i];
	}

	public void ClearLists() {
		itemsPos.Clear();
		itemsType.Clear();
		itemsRot.Clear();
	}

	public int GetCount() {
		return itemsPos.Count;
	}

}

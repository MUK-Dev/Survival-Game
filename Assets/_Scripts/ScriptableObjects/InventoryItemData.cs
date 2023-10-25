using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public bool isEquipable;
    public bool isCraftable;
    public float weight;
}

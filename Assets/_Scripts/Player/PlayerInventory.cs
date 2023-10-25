using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventoryUI playerInventoryUI;
    private InventorySystem inventorySystem;

    private void Awake()
    {
        //? Initialize the inventory system
        //TODO: save the player inventory when game stops
        inventorySystem = new InventorySystem("p1");
    }

    //* Add item to inventory
    public void AddOneItem(InventoryItemData itemDataToAdd)
    {
        inventorySystem.Add(new InventoryItem(itemDataToAdd));
    }

    //* Returns all items in players inventory
    public List<InventoryItem> GetAllItems() => inventorySystem.GetAllItems();

    //* Removes an item from the list
    public void RemoveItem(InventoryItemData itemToRemove)
    {
        inventorySystem.Remove(new InventoryItem(itemToRemove));
        playerInventoryUI.RerenderList();
    }

    //* Get player inventory system for use in other scripts
    public InventorySystem GetPlayerInventorySystem() => inventorySystem;
}

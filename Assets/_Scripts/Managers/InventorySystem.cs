using System.Collections.Generic;

public class InventorySystem
{
    private List<InventoryItem> inventoryItems;

    //* Initialize the inventory
    public InventorySystem(string playerId = "")
    {
        //TODO: use player id to save and load the player inventory upon initialization
        inventoryItems = new List<InventoryItem>();
    }

    //* Add or stack item in inventory
    public void Add(InventoryItem itemToAdd)
    {
        if (IsInInventory(itemToAdd, out InventoryItem existingItem))
        {
            //? If item is in inventory then stack it
            existingItem.AddToAmount();
        }
        else
        {
            //? If item is not in inventory then add it
            inventoryItems.Add(itemToAdd);
        }
    }

    //* Remove or un stack item from inventory
    public void Remove(InventoryItem itemToRemove)
    {
        if (IsInInventory(itemToRemove, out InventoryItem existingItem))
        {
            //? If item is in inventory
            if (existingItem.amount > 1)
            {
                //? If item is stacked then just reduce the amount
                existingItem.RemoveFromAmount();
            }
            else
            {
                //? If not stacked then just remove it from the inventory
                inventoryItems.Remove(existingItem);
            }
        }
    }

    //* Checks if item is in inventory
    private bool IsInInventory(InventoryItem itemToCheck, out InventoryItem existingItem)
    {
        bool result = false;
        InventoryItem existingEntry = null;
        inventoryItems.ForEach(item =>
        {
            if (item.data.displayName == itemToCheck.data.displayName)
            {
                //? If item is same as the item being checked
                existingEntry = item;
                result = true;
            }
        });
        existingItem = existingEntry;
        return result;
    }

    //* Returns inventory list count
    public int GetItemsCount() => inventoryItems.Count;

    //* Returns list
    public List<InventoryItem> GetAllItems() => inventoryItems;
}

using UnityEngine;


public class InventoryItem
{
    //* Stats of inventory item (Scriptable object)
    public InventoryItemData data { get; private set; }
    //* Amount contined of the item (Used for stacking)
    public int amount;

    //* Initialize the item
    public InventoryItem(InventoryItemData source)
    {
        data = source;
        AddToAmount();
    }

    //* Add item to stack
    public void AddToAmount()
    {
        amount++;
    }

    //* Remove item from stack
    public void RemoveFromAmount()
    {
        amount--;
    }

}

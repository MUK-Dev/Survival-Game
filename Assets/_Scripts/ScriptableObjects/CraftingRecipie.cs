using UnityEngine;

[CreateAssetMenu(menuName = "Crafting Recipies")]
public class CraftingRecipie : ScriptableObject
{
    public InventoryItemData[] inputs;
    public InventoryItemData output;
}

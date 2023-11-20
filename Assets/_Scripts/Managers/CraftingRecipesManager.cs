using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingRecipesManager : MonoBehaviour
{
    //* Handles the logics for all the crafting recipies
    public static CraftingRecipesManager Instance { get; private set; }

    public CraftingRecipie[] craftingRecipies;

    private void Awake()
    {
        Instance = this;
    }

    //* Returns all recipies that can be crafted from an item
    public List<CraftingRecipie> GetCraftingRecipiesOfItem(InventoryItemData item)
    {
        List<CraftingRecipie> foundRecipies = new List<CraftingRecipie>();

        foreach (CraftingRecipie recipie in craftingRecipies)
        {
            if (recipie.inputs.Contains(item))
            {
                foundRecipies.Add(recipie);
            }
        }

        return foundRecipies;
    }

}

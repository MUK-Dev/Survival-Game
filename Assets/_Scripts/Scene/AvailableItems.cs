using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableItems : MonoBehaviour
{
    //* Handles UI logic of all the items available near the player
    public static AvailableItems Instance { get; private set; }

    [SerializeField] private Transform itemsList;
    [SerializeField] private Transform itemTemplate;
    [SerializeField] private Button closeButton;

    private List<GameObject> availableItems;
    private InventorySystem inventorySystem;

    //* Initialization
    private void Awake()
    {
        Instance = this;
        availableItems = new List<GameObject>();
        inventorySystem = new InventorySystem();
    }

    private void Start()
    {
        //* Hide the inventory when game starts and when close button is pressed
        gameObject.SetActive(false);
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    //* Handles overall logic of maintaining data and updating UI
    public void UpdateItems(RaycastHit[] allHits)
    {
        //? Remove items that are no longer near player
        RemoveItems(allHits);

        //? Update items available near player
        foreach (RaycastHit item in allHits)
        {
            if (item.collider.gameObject != null)
            {
                //? If game object is not null
                if (item.collider.gameObject.TryGetComponent<InventoryItemGameObject>(out var inventoryItemGameObject) && !availableItems.Contains(item.collider.gameObject))
                {
                    //? Get the inventory item game object and add it to the list
                    inventorySystem.Add(new InventoryItem(inventoryItemGameObject.data));
                    availableItems.Add(item.collider.gameObject);
                    RerenderList();
                }
            }
        }
    }

    //* Remove item from the available items list
    //* Also destroys the game object from world
    public void PickOneItem(InventoryItemData itemToRemove)
    {
        if (FindItemInArray(availableItems, itemToRemove, out InventoryItemGameObject foundItem, out GameObject foundGameObject))
        {
            //? If item is in array
            if (foundGameObject != null && foundItem != null)
            {
                //? If something is found
                inventorySystem.Remove(new InventoryItem(foundItem.data));
                availableItems.Remove(foundGameObject);
                Destroy(foundGameObject);
            }
        }
        RerenderList();
    }

    //* Check if item is in an array
    //* Designed for inventory items
    //* Returns true if it is found
    //* Also gives the data and the game object of that item
    private bool FindItemInArray(List<GameObject> array, InventoryItemData key, out InventoryItemGameObject foundItem, out GameObject foundGameObject)
    {
        GameObject resultGameObject = null;
        InventoryItemGameObject resultItem = null;
        bool result = false;
        array.ForEach(item =>
        {
            //? Get the inventory item of the item
            InventoryItemGameObject inventoryItem = item.GetComponent<InventoryItemGameObject>();
            if (inventoryItem.data.displayName == key.displayName)
            {
                //? If inventory item is the same as the item being checked
                result = true;
                resultItem = inventoryItem;
                resultGameObject = item;
            }
        });
        foundGameObject = resultGameObject;
        foundItem = resultItem;
        return result;
    }

    //* Remove item from the available items UI list
    //* Used to remove world items from list that are no longer near player
    private void RemoveItems(RaycastHit[] itemsToKeep)
    {
        for (int index = 0; index < availableItems.Count; index++)
        {
            //? Map over the available items list
            if (!ArrayContains(itemsToKeep, availableItems[index]))
            {
                //? If array does not have this item then remove this item
                InventoryItemGameObject inventoryItemGameObject = availableItems[index].GetComponent<InventoryItemGameObject>();
                inventorySystem.Remove(new InventoryItem(inventoryItemGameObject.data));
                availableItems.Remove(availableItems[index]);
                RerenderList();
            }
        }

    }

    //* Checks if item is in array
    private bool ArrayContains(RaycastHit[] array, GameObject key)
    {
        bool result = false;
        foreach (RaycastHit item in array)
        {
            if (item.collider.gameObject != null)
                if (item.collider.gameObject == key)
                    result = true;
        }
        return result;
    }

    //* Rerenders the UI list
    private void RerenderList()
    {
        //* Updates the list
        foreach (Transform child in itemsList)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }
        List<InventoryItem> allItemsAvailable = inventorySystem.GetAllItems();

        allItemsAvailable.ForEach(item =>
        {
            Transform itemTransform = Instantiate(itemTemplate, itemsList);
            itemTransform.gameObject.SetActive(true);
            itemTransform.GetComponent<SingleInventoryItem>().SetInventoryItem(item);
        });
    }

    //* Check if list is being shown
    public bool IsListActive() => gameObject.activeSelf;

    //* Toggle inventory
    public void ToggleInventory() => gameObject.SetActive(!gameObject.activeSelf);

}

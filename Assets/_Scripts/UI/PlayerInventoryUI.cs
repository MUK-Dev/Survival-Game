using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour, IDropHandler
{
    //* Handles the logic of player inventory UI list
    [SerializeField] private Transform inventoryItemsList;
    [SerializeField] private Transform itemTemplate;
    [SerializeField] private Button closeButton;
    [SerializeField] private PlayerInventory playerInventory;

    private void Start()
    {
        gameObject.SetActive(false);
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    //* Adds item to the UI list when it is dropped on the UI
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj.TryGetComponent<SingleInventoryItem>(out var inventoryItemUIObj))
        {
            //? Get the data of the item being dropped
            inventoryItemUIObj.isValidDrop = true;
            AvailableItems.Instance.PickOneItem(inventoryItemUIObj.data);
            playerInventory.AddOneItem(inventoryItemUIObj.data);
        }
    }

    public void RerenderList()
    {
        //* Updates the list
        foreach (Transform child in inventoryItemsList)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }

        List<InventoryItem> allPlayerInventoryItems = playerInventory.GetAllItems();

        allPlayerInventoryItems.ForEach(item =>
        {
            Transform itemTransform = Instantiate(itemTemplate, inventoryItemsList);
            itemTransform.gameObject.SetActive(true);
            itemTransform.GetComponent<SingleInventoryItem>().SetInventoryItem(item);
        });
    }

    public void ToggleInventory() => gameObject.SetActive(!gameObject.activeSelf);
}

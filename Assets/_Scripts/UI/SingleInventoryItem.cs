using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SingleInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //* List item of the UI
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Transform canvas;
    [SerializeField] private Image objBg;
    [SerializeField] private Transform itemsWorld;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Transform dropDownMenu;
    [SerializeField] private Transform dropDownButtonTemplate;

    [HideInInspector] public InventoryItemData data;
    [HideInInspector] public bool isValidDrop = false;

    private const string PLAYER_ITEMS_UI_LIST = "PlayerItemsList";

    [HideInInspector] public Transform parentAfterDrag;

    private Button dropDownButton;

    private void Start()
    {
        dropDownButton = GetComponent<Button>();
        dropDownButton.onClick.AddListener(() =>
        {
            OpenDropDown();
        });
    }

    private void OpenDropDown()
    {
        if (dropDownMenu.gameObject.activeSelf)
            dropDownMenu.gameObject.SetActive(false);
        else
            dropDownMenu.gameObject.SetActive(true);


        dropDownButtonTemplate.gameObject.SetActive(false);

        //* Updates the list
        foreach (Transform child in dropDownMenu)
        {
            if (child == dropDownButtonTemplate) continue;
            Destroy(child.gameObject);
        }

        // if (data.isEquipable)
        // {
        //     //? If item is equipable
        //     Transform equipableButton = Instantiate(dropDownButtonTemplate, dropDownMenu);
        //     equipableButton.gameObject.SetActive(true);
        //     //TODO: Equip item to the player
        //     equipableButton.GetComponent<InventoryDropdownButton>().ToggleButton();
        //     // equipableButton.GetComponent<InventoryDropdownButton>().SetButtonProps("Drop", DropItemFromPlayerInventory);
        // }

        if (data.isCraftable)
        {
            //? If item is craftable
            List<CraftingRecipie> craftingRecipies = CraftingRecipesManager.Instance.GetCraftingRecipiesOfItem(data);
            List<InventoryItem> playerInventoryItems = playerInventory.GetAllItems();
            craftingRecipies.ForEach(recipie =>
            {

                //? Mapping all available recipies of item
                Transform craftableButton = Instantiate(dropDownButtonTemplate, dropDownMenu);
                craftableButton.gameObject.SetActive(true);
                InventoryDropdownButton btn = craftableButton.GetComponent<InventoryDropdownButton>();
                btn.SetButtonProps("Craft " + recipie.output.displayName, () => CraftItem(recipie));

                //? Checking if player has all the items for crafting
                bool hasAllIngredients = true;

                foreach (InventoryItemData itemData in recipie.inputs)
                {
                    if (!playerInventory.ContainsItemForData(itemData))
                    {
                        hasAllIngredients = false;
                    }
                }

                //? Changing the button interactivity depending upon ingredients
                btn.SetInteractivity(hasAllIngredients);

            });
        }

        if (int.Parse(amount.text) > 0)
        {
            Transform itemTransform = Instantiate(dropDownButtonTemplate, dropDownMenu);
            itemTransform.gameObject.SetActive(true);
            itemTransform.GetComponent<InventoryDropdownButton>().SetButtonProps("Drop", DropItemFromPlayerInventory);
        }
    }

    private void CraftItem(CraftingRecipie recipie)
    {
        //? Remove the required items
        foreach (var item in recipie.inputs)
        {
            playerInventory.RemoveItem(item);
        }

        //? Add the crafted item
        playerInventory.AddOneItem(recipie.output);
    }

    //* When list item is instantiated
    public void SetInventoryItem(InventoryItem item)
    {
        //* Sets the UI details of the card
        icon.sprite = item.data.icon;
        itemName.text = item.data.displayName;
        amount.text = item.amount.ToString();

        data = item.data;
    }

    //* When dragging starts
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(canvas);
        transform.SetAsLastSibling();
        objBg.raycastTarget = false;
    }

    //* When object is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = PlayerInputManager.Instance.GetMousePosition();
    }

    //* When dragging stops
    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentAfterDrag.name != PLAYER_ITEMS_UI_LIST)
        {
            //? If parent was not the player inventory list
            if (isValidDrop)
            {
                //? If card is dropped on the player inventory list
                Destroy(gameObject);
            }
            else
            {
                //? If dragging stops and it isn't dropped on another object
                transform.SetParent(parentAfterDrag);
                objBg.raycastTarget = true;
            }
        }
        else if (parentAfterDrag.name == PLAYER_ITEMS_UI_LIST)
        {
            //? If parent is the player inventory UI list
            DropItemFromPlayerInventory();
        }
    }

    private void DropItemFromPlayerInventory()
    {
        Quaternion newItemRotation = new Quaternion(0.70711f, 0.00000f, 0.00000f, 0.70711f);
        Instantiate(data.prefab, playerInventory.transform.position, newItemRotation, itemsWorld);
        playerInventory.RemoveItem(data);
        Destroy(gameObject);
    }

}

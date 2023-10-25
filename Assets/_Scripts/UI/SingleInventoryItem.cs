using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    [HideInInspector] public InventoryItemData data;
    [HideInInspector] public bool isValidDrop = false;

    private const string PLAYER_ITEMS_UI_LIST = "PlayerItemsList";

    [HideInInspector] public Transform parentAfterDrag;

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
            Quaternion newItemRotation = new Quaternion(0.70711f, 0.00000f, 0.00000f, 0.70711f);
            Instantiate(data.prefab, playerInventory.transform.position, newItemRotation, itemsWorld);
            playerInventory.RemoveItem(data);
            Destroy(gameObject);
        }
    }


}

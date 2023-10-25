using UnityEngine;
using UnityEngine.UI;

public class OptionButtons : MonoBehaviour
{
    //* Logic of options menu
    [SerializeField] private Button playerInventoryListButton;
    [SerializeField] private Button availableItemsListButton;
    [SerializeField] private PlayerInventoryUI playerInventoryUI;

    private void Start()
    {
        playerInventoryListButton.onClick.AddListener(() =>
        {
            playerInventoryUI.ToggleInventory();
        });

        availableItemsListButton.onClick.AddListener(() =>
        {
            AvailableItems.Instance.ToggleInventory();
        });
    }
}

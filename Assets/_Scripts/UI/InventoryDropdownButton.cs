using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDropdownButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI btnText;
    [SerializeField] private Button btn;

    public void SetButtonProps(string text, Action callback)
    {
        btnText.text = text;
        btn.onClick.AddListener(() => callback());
    }

    public void ToggleButton()
    {
        btn.interactable = !btn.interactable;
    }

    public void SetInteractivity(bool newInteraction)
    {
        btn.interactable = newInteraction;
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }
}

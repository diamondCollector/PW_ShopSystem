using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image _slotImage;
    [SerializeField] Image _toolTipWindow;
    [SerializeField] TextMeshProUGUI _tooltipName;
    [SerializeField] TextMeshProUGUI _tooltipWeight;
    [SerializeField] TextMeshProUGUI _toolTipPrice;
    Item _assignedItem;

    public Item AssignedItem { get { return _assignedItem; } set { _assignedItem = value; } }

    public void SetSlotItem()
    {
        _slotImage.sprite = _assignedItem._image;
        _tooltipName.text = _assignedItem._name;
        _tooltipWeight.text = "Weight: " + _assignedItem._weight;
        _toolTipPrice.text = "Price: " + _assignedItem._price;
    }

    public void DisplayToolTip(bool shouldDisplay)
    {
        _toolTipWindow.gameObject.SetActive(shouldDisplay);
    }
}

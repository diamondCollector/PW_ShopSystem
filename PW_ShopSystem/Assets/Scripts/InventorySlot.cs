using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image _slotImage;
    Item _assignedItem;

    public Item AssignedItem { get { return _assignedItem; } set { _assignedItem = value; } }

    public void SetSlotImage()
    {
        _slotImage.sprite = _assignedItem._image;
    }
}

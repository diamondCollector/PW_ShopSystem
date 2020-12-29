using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image _slotImage;
    [SerializeField] Image _toolTipWindow;
    [SerializeField] TextMeshProUGUI _tooltipName;
    [SerializeField] TextMeshProUGUI _tooltipWeight;
    [SerializeField] TextMeshProUGUI _toolTipPrice;
    Item _assignedItem;
    bool _isSelected;
    public bool IsSelected { get { return _isSelected; } }
    Image _backgroundImage;

    public Item AssignedItem { get { return _assignedItem; } set { _assignedItem = value; } }

    public event Action OnItemClicked;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        Shop.OnTrade += UnselectSlot;
    }

    private void OnDisable()
    {
        Shop.OnTrade -= UnselectSlot;
    }
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

    public void SelectItem()
    {
        _isSelected = !_isSelected;

        if (_isSelected)
        {
            _backgroundImage.color = Color.blue;
        }
        else
        {
            _backgroundImage.color = Color.white;
        }

        OnItemClicked?.Invoke();
    }

    public void UnselectSlot()
    {
        _isSelected = false;
        _backgroundImage.color = Color.white;
    }
    
}

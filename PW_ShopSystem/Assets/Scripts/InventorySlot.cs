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
    
    Image _backgroundImage;

    Item _assignedItem;
    bool _isSelected;
    bool _isAssignedToShop;

    public Item AssignedItem { get { return _assignedItem; } set { _assignedItem = value; } }
    public bool IsSelected { get { return _isSelected; } }
    public bool IsAssignedToShop { get { return _isAssignedToShop; } }

    public event Action<InventorySlot> OnSlotClicked;

    private void Awake()
    {
        _backgroundImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ShopKeeper.OnTradeComplete += UnselectSlot;
        ShopKeeper.OnShopClosed += UnselectSlot;
    }

    private void OnDisable()
    {
        ShopKeeper.OnTradeComplete -= UnselectSlot;
        ShopKeeper.OnShopClosed -= UnselectSlot;
    }

    public void SetSlotItem()
    {
        _slotImage.sprite = _assignedItem._image;
        _tooltipName.text = _assignedItem._name;
        _tooltipWeight.text = "Weight: " + _assignedItem._weight;
        _toolTipPrice.text = "Price: " + _assignedItem._modifiedPrice;
    }

    public void DisplayToolTip(bool shouldDisplay)
    {
        _toolTipWindow.gameObject.SetActive(shouldDisplay);
    }

    public void ClickOnSlot()
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

        OnSlotClicked?.Invoke(this);
    }

    public void UnselectSlot()
    {
        _isSelected = false;
        _backgroundImage.color = Color.white;
        OnSlotClicked?.Invoke(this);
    }

    public void AssignToShop(bool shouldAssign)
    {
        _isAssignedToShop = shouldAssign;
    }
    
}

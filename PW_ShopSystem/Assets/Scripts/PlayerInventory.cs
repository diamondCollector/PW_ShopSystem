using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] public List<Item> _items;
    [SerializeField] float _moneyAmount;
    
    [SerializeField] List<InventorySlot> _slots;
    float _priceOfSelectedItems;
    public float MoneyAmount { get { return _moneyAmount; } set { _moneyAmount = value; } }
    public float PriceOfSelectedItems { get { return _priceOfSelectedItems; } }
    public Action onPriceOfSelectedItemsCalculated;
    List<Item> _selectedItems = new List<Item>();

    private void OnEnable()
    {
        Shop.OnTrade += ClearSelectedItems;
        Shop.OnTrade += CalculatePriceOfSelectedItems;
        foreach (InventorySlot slot in _slots)
        {
            slot.OnItemClicked += CalculatePriceOfSelectedItems;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayerInventory();
    }

    private void OnDisable()
    {
        Shop.OnTrade -= ClearSelectedItems;
        Shop.OnTrade -= CalculatePriceOfSelectedItems;
        foreach (InventorySlot slot in _slots)
        {
            slot.OnItemClicked -= CalculatePriceOfSelectedItems;
        }
    }

    public void SetupPlayerInventory()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i >= _items.Count)
            {
                return;
            }
            else
            {
                _slots[i].AssignedItem = _items[i];
                _slots[i].SetSlotItem();
            }
        }
    }

    void CalculatePriceOfSelectedItems()
    {
        _priceOfSelectedItems = 0;
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsSelected)
            {
                _priceOfSelectedItems += slot.AssignedItem._price;
                _selectedItems.Add(slot.AssignedItem);
            }
            else
            {
                _selectedItems.Remove(slot.AssignedItem);
            }
        }
        onPriceOfSelectedItemsCalculated?.Invoke();
        Debug.Log("Price of selected Items on player side: " + _priceOfSelectedItems);
    }

    void ClearSelectedItems()
    {
        _selectedItems.Clear();
    }
}

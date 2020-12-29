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
    [SerializeField] float _priceModifier;
    float _priceOfSelectedItems;
    public List<Item> _selectedItems = new List<Item>();
    public float MoneyAmount { get { return _moneyAmount; } set { _moneyAmount = value; } }
    public float PriceOfSelectedItems { get { return _priceOfSelectedItems; } }
    public event Action OnPriceOfSelectedItemsCalculated;
    

    private void OnEnable()
    {
        //When Trade is confirmed
        //Shop.OnTrade += ResetSelectionAndPrice;
        foreach (InventorySlot slot in _slots)
        {
            //slot.OnItemClicked += CalculatePriceOfSelectedItems;
           // slot.OnSlotClicked += UpdateListOfSelectedItems;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ModifyItemPrices();
        SetupPlayerInventory();
    }

    private void OnDisable()
    {
        //Shop.OnTrade -= ResetSelectionAndPrice;
        foreach (InventorySlot slot in _slots)
        {
            //slot.OnItemClicked -= CalculatePriceOfSelectedItems;
            //slot.OnSlotClicked -= UpdateListOfSelectedItems;

        }
    }

    public void SetupPlayerInventory()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i >= _items.Count)
            {
                _slots[i].gameObject.SetActive(false);
            }
            else
            {
                _slots[i].AssignedItem = _items[i];
                _slots[i].SetSlotItem();
                _slots[i].gameObject.SetActive(true);
            }
        }
    }

    void UpdateListOfSelectedItems()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsSelected)
            {
                _selectedItems.Add(slot.AssignedItem);
            }
            else
            {
                _selectedItems.Remove(slot.AssignedItem);
            }
        }
        CalculatePriceOfSelectedItems();
    }

    void CalculatePriceOfSelectedItems()
    {
        _priceOfSelectedItems = 0;
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsSelected)
            {
                _priceOfSelectedItems += slot.AssignedItem._modifiedPrice;
                //_selectedItems.Add(slot.AssignedItem);
            }
           // else
            //{
                //_selectedItems.Remove(slot.AssignedItem);
            //}
        }
        OnPriceOfSelectedItemsCalculated?.Invoke();
        Debug.Log("Price of selected Items on player side: " + _priceOfSelectedItems);
    }


    public void ClearSelectedItems()
    {
        _selectedItems.Clear();
    }

    public void ResetSelectionAndPrice()
    {
        //UpdateListOfSelectedItems();
        ClearSelectedItems();
        CalculatePriceOfSelectedItems();
        
    }

    void ModifyItemPrices()
    {
        foreach (Item item in _items)
        {
            var randomNumber = UnityEngine.Random.Range(0, 1);
            bool isAddingMargin = randomNumber > 0.5f;

            if (isAddingMargin)
            {
                //item._price += item._price * _priceModifier;
                item._modifiedPrice = item._basicPrice + item._basicPrice * _priceModifier;
            }
            else
            {
                //item._price -= item._price * _priceModifier;
                item._modifiedPrice = item._basicPrice - item._basicPrice * _priceModifier;
            }
            //item._price = Mathf.Round(item._price);
            item._modifiedPrice = Mathf.Round(item._modifiedPrice);
        }
    }
}

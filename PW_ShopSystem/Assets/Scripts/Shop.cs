using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Shop : MonoBehaviour
{   


  /*

    private void OnEnable()
    {
        foreach (InventorySlot slot in _slots)
        {
            slot.OnItemClicked += CalculatePriceOfSelectedItems;
        }
    }

    // Start is called before the first frame update
    void Start()
    {       
        ModifyItemPrices();
        SetupShopInventory();
    }

    private void OnDisable()
    {
        foreach (InventorySlot slot in _slots)
        {
            slot.OnItemClicked -= CalculatePriceOfSelectedItems;
        }
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



    void CalculatePriceOfSelectedItems()
    {
        _priceOfSelectedItems = 0;
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsSelected)
            {
                //_priceOfSelectedItems += slot.AssignedItem._price;
                _priceOfSelectedItems += slot.AssignedItem._modifiedPrice;
                _selectedItems.Add(slot.AssignedItem);
            }
            else
            {
                _selectedItems.Remove(slot.AssignedItem);
            }
        }
        DisplayTotalBalanceOfSelectedItems();
    }



    void DisplayTotalBalanceOfSelectedItems()
    {
            _balance = 0;
            _balance = _playerInventory.PriceOfSelectedItems - _priceOfSelectedItems;
            _balanceText.text = "Balance: " + _balance;
    }

    public void Trade()
    {
        if (_playerInventory.MoneyAmount > _balance * -1 && _moneyAmount > _balance)
        {
            _playerInventory.MoneyAmount += _balance;
            _moneyAmount += _balance * -1;
            DisplayCurrentMoneyAmount();

            TransferSelectedItemsToPlayerInventory();           
            TransferSelectedItemsToShopInventory();
            _selectedItems.Clear();
            _playerInventory.ResetSelectionAndPrice();

            SetupShopInventory();
            _playerInventory.SetupPlayerInventory();
            //OnTrade?.Invoke();

            
            
            CalculatePriceOfSelectedItems();
            DisplayTotalBalanceOfSelectedItems();
        }
    }

    private void TransferSelectedItemsToShopInventory()
    {
        foreach (Item item in _playerInventory._selectedItems)
        {
            _playerInventory._items.Remove(item);
            _items.Add(item);
            //_playerInventory.SetupPlayerInventory();


            //SetupShopInventory();
        }
    }

    private void TransferSelectedItemsToPlayerInventory()
    {
        foreach (Item item in _selectedItems)
        {
            _items.Remove(item);
            _playerInventory._items.Add(item);
           // SetupShopInventory();
           // _playerInventory.SetupPlayerInventory();
        }
    }
  */
}


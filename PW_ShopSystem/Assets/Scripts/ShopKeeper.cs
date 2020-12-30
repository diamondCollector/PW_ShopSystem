using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] GameObject _shopUI;
    [SerializeField] TextMeshProUGUI _shopMoneyAmountText;
    [SerializeField] TextMeshProUGUI _playerMoneyAmountText;
    [SerializeField] TextMeshProUGUI _balanceText;

    [SerializeField] List<InventorySlot> _shopSlots;
    [SerializeField] List<InventorySlot> _playerSlots;
    [SerializeField] List<Item> _shopItems;
    [SerializeField] float _priceModifier;
    [SerializeField] float _moneyAmount;

    List<InventorySlot> _selectedShopSlots;
    List<InventorySlot> _selectedPlayerSlots;

    List<Item> _selectedShopItems = new List<Item>();
    List<Item> _selectedPlayerItems = new List<Item>();

    float _shopSelectedItemsPrice;
    float _playerSelectedItemsPrice;
    float _balance;

    PlayerEquipment _playerEquipment;
    List<Item> _shopSelectedItems = new List<Item>();
    List<Item> _playerSelectedItems = new List<Item>();

    private void OnEnable()
    {
        
        foreach (InventorySlot slot in _shopSlots)
        {
            slot.OnSlotClicked += HandleSlotSelection; 
        }

        foreach (InventorySlot slot in _playerSlots)
        {
            slot.OnSlotClicked += HandleSlotSelection;
        }      
    }

    private void Start()
    {
        AssignShopSlots();
    }

    private void AssignShopSlots()
    {
        foreach (InventorySlot slot in _shopSlots)
        {
            slot.AssignToShop(true);
        }
    }

    void HandleSlotSelection(InventorySlot slot)
    {
        if (slot.IsAssignedToShop)
        {
            ManageSelectedItem(slot, _shopSelectedItems);
            _shopSelectedItemsPrice = CalculateSelectedItemsPrice(_shopSelectedItems);
            Debug.Log("Shop items price: " + _shopSelectedItemsPrice);
        }
        else
        {
            ManageSelectedItem(slot, _playerSelectedItems);
            _playerSelectedItemsPrice = CalculateSelectedItemsPrice(_playerSelectedItems);
            Debug.Log("Player items price: " + _playerSelectedItemsPrice);
        }

        _balance = CalculateTradeBalance();
        DisplayCurrentBalance();
    }

    private void ManageSelectedItem(InventorySlot slot, List<Item> selectedItems)
    {
        if (slot.IsSelected)
        {
            selectedItems.Add(slot.AssignedItem);
            //_selectedShopSlots.Add(slot);
            Debug.Log(slot.AssignedItem._name +
                " has been added to selected Shop items. Current number of selected items: "
                + selectedItems.Count);
        }
        else if (!slot.IsSelected && selectedItems.Contains(slot.AssignedItem))
        {
            selectedItems.Remove(slot.AssignedItem);
            //_selectedShopSlots.Remove(slot);
            Debug.Log(slot.AssignedItem._name +
                " has been removed from selected Shop items. Current number of selected items: "
                + selectedItems.Count);
        }
    }

    private float CalculateSelectedItemsPrice(List<Item> selectedItems)
    {
        float price = 0;
        foreach (Item item in selectedItems)
        {
            price += item._modifiedPrice;
        }

        return price;
    }

    private float CalculateTradeBalance()
    {
        float balance = _playerSelectedItemsPrice - _shopSelectedItemsPrice;
        return balance;
    }

    void DisplayCurrentBalance()
    {
        _balanceText.text =$"Balance: {_balance.ToString()}";
    }

    void SetupInventory(List<InventorySlot> slots, List<Item> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i >= items.Count)
            {
                slots[i].gameObject.SetActive(false);
            }
            else
            {
                slots[i].gameObject.SetActive(true);
                slots[i].AssignedItem = items[i];
                slots[i].SetSlotItem();
                slots[i].gameObject.SetActive(true);
            }
        }
    }

    void DisplayCurrentMoneyAmount()
    {
        _playerMoneyAmountText.text = _playerEquipment.MoneyAmount.ToString();
        _shopMoneyAmountText.text = _moneyAmount.ToString();
    }

    void ModifyItemsPrices(List<Item> items, float modifier)
    {
        foreach (Item item in items)
        {
            var randomNumber = UnityEngine.Random.Range(0, 1);
            bool isAddingMargin = randomNumber > 0.5f;

            if (isAddingMargin)
            {
                item._modifiedPrice = item._basicPrice + item._basicPrice * modifier;
            }
            else
            {
                item._modifiedPrice = item._basicPrice - item._basicPrice * modifier;
            }

            item._modifiedPrice = Mathf.Round(item._modifiedPrice);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _playerEquipment = collision.GetComponent<PlayerEquipment>();

        if (_playerEquipment != null)
        {
            ModifyItemsPrices(_shopItems, _priceModifier);
            ModifyItemsPrices(_playerEquipment.Items, _playerEquipment.PriceModifier);
            SetupInventory(_shopSlots, _shopItems);
            SetupInventory(_playerSlots, _playerEquipment.Items);
            DisplayCurrentMoneyAmount();
            _shopUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _playerEquipment = collision.GetComponent<PlayerEquipment>();

        if (_playerEquipment != null)
        {
            _shopUI.SetActive(false);
            _playerEquipment = null;
        }
    }
}

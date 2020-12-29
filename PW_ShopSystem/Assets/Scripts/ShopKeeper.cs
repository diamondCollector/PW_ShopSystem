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

    float _shopPriceOfSelectedItems;
    float _playerPriceOfSelectedItems;
    float _balance;

    PlayerEquipment _playerEquipment;
    List<Item> _shopSelectedItems = new List<Item>();
    List<Item> _playerSelectedItems = new List<Item>();

    private void OnEnable()
    {
        /*
        foreach (InventorySlot slot in _shopSlots)
        {
            slot.GetComponent<Image>(). 
        }

        foreach (InventorySlot slot in _playerSlots)
        {
            slot.OnSlotClicked += CalculatePrice;
        }
        */
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
            if (slot.IsSelected)
            {
                _selectedShopSlots.Add(slot);
            }
            else if (_selectedShopSlots.Contains(slot))
            {
                _selectedShopSlots.Remove(slot);
            }
            
        }
    }

    void CalculatePrice(bool k)
    {
        
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

using System;
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
    [SerializeField] TextMeshProUGUI _tradeText;

    [SerializeField] List<InventorySlot> _shopSlots;
    [SerializeField] List<InventorySlot> _playerSlots;

    List<InventorySlot> _selectedShopSlots;
    List<InventorySlot> _selectedPlayerSlots;

    List<Item> _selectedShopItems = new List<Item>();
    List<Item> _selectedPlayerItems = new List<Item>();

    Item _availableLegendaryItem;

    float _shopSelectedItemsPrice;
    float _playerSelectedItemsPrice;
    float _balance;
    string _tradeSucceed = "Success!";
    string _tradeFailed = "I can't afford that!";

    PlayerEquipment _playerEquipment;
    ShopEquipment _shopEquipment;
    List<Item> _shopSelectedItems = new List<Item>();
    List<Item> _playerSelectedItems = new List<Item>();

    public static event Action OnTradeComplete;
    public static event Action OnShopClosed;

    private void Awake()
    {
        _shopEquipment = GetComponent<ShopEquipment>();
    }

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
        _shopMoneyAmountText.text = _shopEquipment.MoneyAmount.ToString();
    }

    void ModifyItemsPrices(List<Item> items, float modifier)
    {
        foreach (Item item in items)
        {
            var randomNumber = UnityEngine.Random.Range(0, 100);
            bool isAddingMargin = randomNumber > 50;

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

    public void Trade()
    {
        if(_playerEquipment.MoneyAmount >= _balance * -1 && _shopEquipment.MoneyAmount >= _balance)
        {
            TransferItems(_shopSelectedItems, _shopEquipment.Items, _playerEquipment.Items);
            TransferItems(_playerSelectedItems, _playerEquipment.Items, _shopEquipment.Items);
            SetupInventory(_shopSlots, _shopEquipment.Items);
            SetupInventory(_playerSlots, _playerEquipment.Items);
            BalancePayment();
            DisplayCurrentBalance();
            DisplayCurrentMoneyAmount();
            StartCoroutine(DisplayTradeMsg(_tradeSucceed));
            OnTradeComplete?.Invoke();
        } 
        else
        {
            StartCoroutine(DisplayTradeMsg(_tradeFailed));
        }
    }

    IEnumerator DisplayTradeMsg(string msg)
    {
        _tradeText.text = msg;
        _tradeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        _tradeText.gameObject.SetActive(false);
    }

    void BalancePayment()
    {
        _playerEquipment.MoneyAmount += _balance;
        _shopEquipment.MoneyAmount -= _balance;
        _balance = 0;
        _playerSelectedItemsPrice = 0;
        _shopSelectedItemsPrice = 0;
    }

    void TransferItems(List<Item> giverSelectedItems, List<Item> giverAllItems, List<Item> takerItems)
    {
        foreach (Item item in giverSelectedItems)
        {
            takerItems.Add(item);
            giverAllItems.Remove(item);
            if (item == _availableLegendaryItem)
            {
               _shopEquipment._legendaryShopItems.Remove(item);
            }
        }

        giverSelectedItems.Clear();
        foreach(Item item in giverSelectedItems)
        {
            Debug.Log("To zostalo na liscie po wyczyszczeniu: " + item._name);
        }
    }

    private void AddLegendaryItem()
    {
        var randomNumber = UnityEngine.Random.Range(0, 100);
        if (randomNumber <=_shopEquipment.ChanceForLegendaryItem && _shopEquipment._legendaryShopItems.Count > 0)
        {
            var index = UnityEngine.Random.Range(0, _shopEquipment._legendaryShopItems.Count - 1);
            _availableLegendaryItem = _shopEquipment._legendaryShopItems[index];
            _shopEquipment.Items.Add(_availableLegendaryItem);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _playerEquipment = collision.GetComponent<PlayerEquipment>();

        if (_playerEquipment != null)
        {
            AddLegendaryItem();
            ModifyItemsPrices(_shopEquipment.Items, _shopEquipment.PriceModifier);
            ModifyItemsPrices(_playerEquipment.Items, _playerEquipment.PriceModifier);
            SetupInventory(_shopSlots, _shopEquipment.Items);
            SetupInventory(_playerSlots, _playerEquipment.Items);
            DisplayCurrentMoneyAmount();
            DisplayCurrentBalance();
            _shopUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _playerEquipment = collision.GetComponent<PlayerEquipment>();

        if (_playerEquipment != null)
        {
            OnShopClosed?.Invoke();
            _shopUI.SetActive(false);
            _playerEquipment = null;
            _shopEquipment.Items.Remove(_availableLegendaryItem);
            _availableLegendaryItem = null;
        }
    }
}

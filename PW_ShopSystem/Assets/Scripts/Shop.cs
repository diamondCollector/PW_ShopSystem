using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Shop : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    [SerializeField] float _priceModifier;
    [SerializeField] float _moneyAmount;
    [SerializeField] TextMeshProUGUI _shopMoneyAmountText;
    [SerializeField] TextMeshProUGUI _playerMoneyAmountText;
    [SerializeField] List<InventorySlot> _slots;
    [SerializeField] Image _shopInventoryWindow;
    [SerializeField] Image _playerInventoryWindow;
    [SerializeField] TextMeshProUGUI _balanceText;
    PlayerInventory _playerInventory;
    float _priceOfSelectedItems;
    float _balance;
    List<Item> _selectedItems = new List<Item>();

    public static Action OnTrade;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
    }

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
                item._price += item._price * _priceModifier;
            }
            else
            {
                item._price -= item._price * _priceModifier;
            }
        }
    }

    void SetupShopInventory()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i >= _items.Count)
            {
                _slots[i].gameObject.SetActive(false);
            }
            else
            {
                _slots[i].gameObject.SetActive(true);
                _slots[i].AssignedItem = _items[i];
                _slots[i].SetSlotItem();
                _slots[i].gameObject.SetActive(true);
            }
        }

        DisplayCurrentMoneyAmount();
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
        DisplayTotalBalanceOfSelectedItems();
    }

        void DisplayCurrentMoneyAmount()
    {
        _playerMoneyAmountText.text = _playerInventory.MoneyAmount.ToString();
        _shopMoneyAmountText.text = _moneyAmount.ToString();
 
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
            //TransferSelectedItemsToShopInventory();

            OnTrade?.Invoke();
            _selectedItems.Clear();
            CalculatePriceOfSelectedItems();
            DisplayTotalBalanceOfSelectedItems();
            
        }
    }

    private void TransferSelectedItemsToPlayerInventory()
    {
        foreach (Item item in _selectedItems)
        {
            _items.Remove(item);
            _playerInventory._items.Add(item);
            SetupShopInventory();
            _playerInventory.SetupPlayerInventory();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInventory = collision.GetComponent<PlayerInventory>();

        if (_playerInventory != null)
        {
            _playerInventory.onPriceOfSelectedItemsCalculated += DisplayTotalBalanceOfSelectedItems;
            _shopInventoryWindow.gameObject.SetActive(true);
            _playerInventoryWindow.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _playerInventory = collision.GetComponent<PlayerInventory>();

        if (_playerInventory != null)
        {
            _playerInventory.onPriceOfSelectedItemsCalculated -= DisplayTotalBalanceOfSelectedItems;
            _shopInventoryWindow.gameObject.SetActive(false);
            _playerInventoryWindow.gameObject.SetActive(false);
        }
    }
}


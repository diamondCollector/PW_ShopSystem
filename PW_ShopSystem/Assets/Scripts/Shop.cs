using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    [SerializeField] float _priceModifier;
    [SerializeField] int _moneyAmount;
    [SerializeField] TextMeshProUGUI _moneyAmountText;
    [SerializeField] List<InventorySlot> _slots;
    [SerializeField] Image shopInventoryWindow;

    // Start is called before the first frame update
    void Start()
    {       
        ModifyItemPrices();
        SetupShopInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ModifyItemPrices()
    {
        foreach (Item item in _items)
        {
            var randomNumber = Random.Range(0, 1);
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
            _slots[i].AssignedItem = _items[i];
            _slots[i].SetSlotItem();
        }

        DisplayCurrentMoneyAmount();
    }

    private void DisplayCurrentMoneyAmount()
    {
        _moneyAmountText.text = _moneyAmount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            shopInventoryWindow.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            shopInventoryWindow.gameObject.SetActive(false);
        }
    }
}

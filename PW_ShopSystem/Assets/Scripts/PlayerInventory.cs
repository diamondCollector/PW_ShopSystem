using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    [SerializeField] int _moneyAmount;
    [SerializeField] TextMeshProUGUI _moneyAmountText;
    [SerializeField] List<InventorySlot> _slots;

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayerInventory();
    }

    void SetupPlayerInventory()
    {
        DisplayCurrentMoneyAmount();
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

    private void DisplayCurrentMoneyAmount()
    {
        _moneyAmountText.text = _moneyAmount.ToString();
    }
}

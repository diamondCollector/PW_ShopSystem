using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    [SerializeField] float _moneyAmount;
    [SerializeField] float _priceModifier;

    public List<Item> Items { get { return _items; } }
    public float MoneyAmount { get { return _moneyAmount; } }

    public float PriceModifier { get { return _priceModifier; } }
}

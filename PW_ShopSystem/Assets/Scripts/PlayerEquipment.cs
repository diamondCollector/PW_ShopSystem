using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    [SerializeField] float _moneyAmount;
    [SerializeField] float _priceModifier;

    //Czy w tym przypadku poprawnie zastosowano propeties? A może public variables byłyby lepsze?
    public List<Item> Items { get { return _items; } }
    public float MoneyAmount { get { return _moneyAmount; } set { _moneyAmount = value; } }

    public float PriceModifier { get { return _priceModifier; } }
}

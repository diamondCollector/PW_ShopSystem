using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEquipment : Equipment
{
    [SerializeField] List<Item> _legendaryShopItems;

    [SerializeField] int _chanceForLegendaryItem = 10;

    public int ChanceForLegendaryItem { get { return _chanceForLegendaryItem; } }
    public List<Item> LegendaryShopItems { get { return _legendaryShopItems; } }

}

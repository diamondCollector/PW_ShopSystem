using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    [SerializeField] float _priceModifier;
    [SerializeField] List<Image> _slotImages;

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
        for (int i = 0; i < _slotImages.Count; i++)
        {
            _slotImages[i].sprite = _items[i]._image;
        }
    }
}

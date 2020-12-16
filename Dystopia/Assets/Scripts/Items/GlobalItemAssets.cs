using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalItemAssets :  GenericSingletonClass<GlobalItemAssets>
{
    //Here all items to use in game
    [SerializeField] private List<Item> _itemsList;
    public List<Item> itemList { get => _itemsList; }
}

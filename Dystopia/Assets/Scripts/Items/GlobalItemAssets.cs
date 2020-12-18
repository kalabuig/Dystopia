using System;
using System.Collections.Generic;
using UnityEngine;

//PreFabs of items used when loading a game
public class GlobalItemAssets :  GenericSingletonClass<GlobalItemAssets>
{
    //Here all items to use in game
    [SerializeField] private List<Item> _itemsList;
    public List<Item> itemList { get => _itemsList; }
}

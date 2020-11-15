using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSourceInventory : WorldInventory
{
    public void AutoSetTitle() {
        GameObject container = GameHandler.GetSelectedContainer();
        if(container != null) {
            title.text = container.GetComponent<FireSource>()?.GetFireSourceName();
        }
    }

    public void UseFire() {
        if(itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) return;
        DoAction(character.useFireSpeed);
    }

    public void StopUsingFire() {
        StopAction();
    }

    protected override void DoSomething() {
        if(GameHandler.GetSelectedContainer()==null) return;
        FireSource fireSource = GameHandler.GetSelectedContainer().GetComponent<FireSource>();
        if(fireSource!=null && itemSlots!=null && itemSlots[0]!=null && itemSlots[0].item!=null) {
            fireSource.SetItem(new FireSource.ContainerItem {item = itemSlots[0].item, amount = itemSlots[0].amount});
            FireSource.ContainerItem newItem = fireSource.ReplaceItem();
            if(newItem.item != null) {
                itemSlots[0].item = newItem.item.GetCopy();
                itemSlots[0].amount = newItem.amount; //rule: if we have half raw meal, we get half cooked meal
                SoundManager.PlaySound(SoundManager.Sound.ItemFound);
            } else {
                SoundManager.PlaySound(SoundManager.Sound.ItemNotFound);
            }
        }
    }

    public void loadItems(FireSource fs) {
        if(fs==null) return;
        //Reset the slot
        foreach(ItemSlot slot in itemSlots) {
            slot.item = null;
            slot.amount = 0;
        }
        FireSource.ContainerItem itemToLoad = fs.GetItem(); //Get the item to load in the panel
        //Populate slot with the item
        if(itemToLoad.item!=null) {
            itemSlots[0].item = itemToLoad.item.GetCopy(); //Instantiate a new item based on the scriptobject
            itemSlots[0].amount = itemToLoad.amount; //Setting the amount 
        } else {
            itemSlots[0].item = null;
            itemSlots[0].amount = 0; //Setting the amount to 0
        }
    }

    public void storeItems(FireSource fs) {
        if(fs==null) return;
        if(itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) {
            fs.SetItem(FireSource.ContainerItem.Empty());
        } else {
            fs.SetItem(new FireSource.ContainerItem {item = itemSlots[0].item, amount = itemSlots[0].amount});
        }
    }
}

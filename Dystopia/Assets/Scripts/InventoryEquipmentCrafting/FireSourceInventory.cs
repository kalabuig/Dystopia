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

    public void UseFire(int ticksToFinish) {
        Debug.Log(itemSlots[0].item);
        if(itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) return;
        
        DoAction(ticksToFinish);
    }

    public void StopUsingFire() {
        StopAction();
    }

    protected override void DoSomething() {
        if(GameHandler.GetSelectedContainer()==null) return;
        FireSource fireSource = GameHandler.GetSelectedContainer().GetComponent<FireSource>();
        if(fireSource!=null && itemSlots!=null && itemSlots[0]!=null && itemSlots[0].item!=null) {
            fireSource.SetItem(itemSlots[0].item);
            Item newItem = fireSource.ReplaceItem();
            if(newItem != null) {
                itemSlots[0].item = newItem.GetCopy();
                itemSlots[0].amount = newItem.maxMultiUses;
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
        Item itemToLoad = fs.GetItem(); //Get the item to load in the panel
        //Populate slot with the item
        if(itemToLoad!=null) {
            itemSlots[0].item = itemToLoad.GetCopy(); //Instantiate a new item based on the scriptobject
            itemSlots[0].amount = itemToLoad.maxMultiUses; //Setting the amount to maximumStack
        } else {
            itemSlots[0].item = null;
            itemSlots[0].amount = 0; //Setting the amount to 0
        }
    }

    public void storeItems(FireSource fs) {
        if(fs==null || itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) return;
        fs.SetItem(itemSlots[0].item);
    }
}

using UnityEngine;

public class WaterFillerInventory : WorldInventory
{
    public void AutoSetTitle() {
        GameObject container = GameHandler.GetSelectedContainer();
        if(container != null) {
            title.text = container.GetComponent<WaterResource>()?.GetWaterResourceName();
        }
    }

    public void FillWithWater(int ticksToScavenge) {
        DoAction(ticksToScavenge);
    }

    public void StopFillWithWater() {
        StopAction();
    }

    protected override void DoSomething() {
        if(GameHandler.GetSelectedContainer()==null) return;
        WaterResource waterResource = GameHandler.GetSelectedContainer().GetComponent<WaterResource>();
        if(waterResource!=null && itemSlots!=null && itemSlots[0]!=null && itemSlots[0].item!=null) {
            waterResource.SetItem(itemSlots[0].item);
            Item newItem = waterResource.ReplaceItem();
            if(newItem != null) {
                itemSlots[0].item = newItem.GetCopy();
                itemSlots[0].amount = newItem.maxMultiUses;
                SoundManager.PlaySound(SoundManager.Sound.ItemFound);
            } else {
                SoundManager.PlaySound(SoundManager.Sound.ItemNotFound);
            }
        }
        
    }

    public void loadItems(WaterResource wr) {
        if(wr==null) return;
        //Reset the slot
        foreach(ItemSlot slot in itemSlots) {
            slot.item = null;
            slot.amount = 0;
        }
        Item itemToLoad = wr.GetItem(); //Get the item to load in the panel
        //Populate slot with the item
        if(itemToLoad!=null) {
            itemSlots[0].item = itemToLoad.GetCopy(); //Instantiate a new item based on the scriptobject
            itemSlots[0].amount = itemToLoad.maxMultiUses; //Setting the amount to maximumStack (=full bottle)
        } else {
            itemSlots[0].item = null;
            itemSlots[0].amount = 0; //Setting the amount to 0
        }
    }

    public void storeItems(WaterResource wr) {
        if(wr==null || itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) return;
        wr.SetItem(itemSlots[0].item);
    }
}

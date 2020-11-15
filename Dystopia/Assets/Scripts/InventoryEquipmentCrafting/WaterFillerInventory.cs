using UnityEngine;

public class WaterFillerInventory : WorldInventory
{
    public void AutoSetTitle() {
        GameObject container = GameHandler.GetSelectedContainer();
        if(container != null) {
            title.text = container.GetComponent<WaterResource>()?.GetWaterResourceName();
        }
    }

    public void FillWithWater() {
        if(itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) return;
        DoAction(character.fillWaterSpeed);
    }

    public void StopFillWithWater() {
        StopAction();
    }

    protected override void DoSomething() {
        if(GameHandler.GetSelectedContainer()==null) return;
        WaterResource waterResource = GameHandler.GetSelectedContainer().GetComponent<WaterResource>();
        if(waterResource!=null && itemSlots!=null && itemSlots[0]!=null && itemSlots[0].item!=null) {
            waterResource.SetItem(new WaterResource.ContainerItem {item = itemSlots[0].item, amount = itemSlots[0].amount});
            WaterResource.ContainerItem newItem = waterResource.ReplaceItem();
            if(newItem.item != null) {
                itemSlots[0].item = newItem.item.GetCopy();
                itemSlots[0].amount = newItem.item.maxMultiUses; //we want the container full of water when fill it
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
        WaterResource.ContainerItem itemToLoad = wr.GetItem(); //Get the item to load in the panel
        //Populate slot with the item
        if(itemToLoad.item!=null) {
            itemSlots[0].item = itemToLoad.item.GetCopy(); //Instantiate a new item based on the scriptobject
            itemSlots[0].amount = itemToLoad.amount; //Setting the amount
        } else {
            itemSlots[0].item = null;
            itemSlots[0].amount = 0; //Setting the amount to 0
        }
    }

    public void storeItems(WaterResource wr) {
        if(wr==null) return;
        if(itemSlots==null || itemSlots[0]==null || itemSlots[0].item==null) {
            wr.SetItem(WaterResource.ContainerItem.Empty());
        } else {
            wr.SetItem(new WaterResource.ContainerItem {item = itemSlots[0].item, amount = itemSlots[0].amount});
        }
    }
}

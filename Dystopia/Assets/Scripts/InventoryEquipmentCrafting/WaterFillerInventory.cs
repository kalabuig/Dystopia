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
                SoundManager.PlaySound(SoundManager.Sound.ItemFound);
            } else {
                SoundManager.PlaySound(SoundManager.Sound.ItemNotFound);
            }
        }
        
    }
}

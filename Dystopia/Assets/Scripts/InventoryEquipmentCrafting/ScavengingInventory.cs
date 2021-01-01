using UnityEngine;
using System.Collections.Generic;

public class ScavengingInventory : WorldInventory
{
    public void AutoSetTitle() {
        //if(gameHandler != null) {
            GameObject container = GameHandler.GetSelectedContainer(); //gameHandler.GetSelectedContainer();
            if(container != null) {
                title.text = "Scavenging " + container.GetComponent<Container>()?.GetContainerName();
            }
        //}
    }
 
    public void Scavenge() {
        float scavengingSpeed = character.scavengingSpeed - statsModifiers.GetFloatStatMod(StatsModifiers.Modifier.scavengingSpeed);
        DoAction(scavengingSpeed);
    }

    public void StopScavenge() {
        StopAction();
    }

    protected override void DoSomething() {
        Container selectedContainerComponent = GameHandler.GetSelectedContainer()?.GetComponent<Container>();
        Item scavengedItem = selectedContainerComponent?.GetRandomItem();
        if(scavengedItem!=null) {
            AddItem(scavengedItem, scavengedItem.MaximumStacks); //Add item to the scavenging inventory/panel
            SoundManager.PlaySound(SoundManager.Sound.ItemFound);
            gameHandler.levelSystem.AddExperience(5);
        } else {
            if(selectedContainerComponent!=null) {
                if(UnityEngine.Random.Range(0,100) < selectedContainerComponent.hurtChance) {
                    character.ModifyHealth(-selectedContainerComponent.hurtAmount);
                } else {
                    SoundManager.PlaySound(SoundManager.Sound.ItemNotFound);
                }
            } 
        }
    }

    public void loadItems(Container c) {
        if(c==null) return;
        //Reset the slots
        foreach(ItemSlot slot in itemSlots) {
            slot.item = null;
            slot.amount = 0;
        }
        List<Container.ContainerItem> itemsToLoad = c.GetItems(); //Get the items to load in the panel
         int i = 0;
        //Populate slots with items
        if(itemsToLoad!=null && itemsToLoad.Count>0) {
            for(; i<itemsToLoad.Count && i < itemSlots.Length; i++) {
                if(itemsToLoad[i].item!=null && itemsToLoad[i].amount>0) {
                    itemSlots[i].item = itemsToLoad[i].item.GetCopy(); //Instantiate a new item based on the scriptobject
                    itemSlots[i].amount = itemsToLoad[i].amount; //Setting the amount
                }
            }
        }
        //Populate slots with empty
        for(; i < itemSlots.Length; i++) {
            itemSlots[i].item = null;
            itemSlots[i].amount = 0; //Setting the amount to 0
        }
    }

    public void storeItems(Container c) {
        if(c==null) return;
        //Create the structure to store the items in the structure
        Container.ContainerItem[] itemsToStore = new Container.ContainerItem[itemSlots.Length];
        for(int i = 0; i < itemSlots.Length; i++) {
            itemsToStore[i] = new Container.ContainerItem { item = itemSlots[i].item, amount = itemSlots[i].amount};
        }
        c.SetItems(itemsToStore);
    }

}

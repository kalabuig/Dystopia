using UnityEngine;

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

    public void Scavenge(int ticksToScavenge) {
        DoAction(ticksToScavenge);
    }

    public void StopScavenge() {
        StopAction();
    }

    protected override void DoSomething() {
        Item scavengedItem = GameHandler.GetSelectedContainer()?.GetComponent<Container>()?.GetRandomItem();
        if(scavengedItem!=null) {
            AddItem(scavengedItem); //Add item to the scavenging inventory/panel
            SoundManager.PlaySound(SoundManager.Sound.ItemFound);
        } else {
            SoundManager.PlaySound(SoundManager.Sound.ItemNotFound);
        }
    }

}

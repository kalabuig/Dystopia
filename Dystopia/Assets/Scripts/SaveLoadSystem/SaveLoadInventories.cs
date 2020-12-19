using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadInventories : MonoBehaviour
{
    private GameObject inventory;
    private GameObject equipmentPanel;
    private GameObject craftingPanel;
    private GameObject craftingBookPanel;

    private const string fileNamePlayerInventory = "playerInventory";
    private const string fileNamePlayerEquipment = "playerEquipment";
    private const string fileNameCraftingInventory = "craftingInventory";
    private const string fileNameKnownRecipes = "knownRecipes";

    private void Awake() {
        inventory = GameObject.Find("InventoryPanel");
        equipmentPanel = GameObject.Find("EquipmentPanel");
        craftingPanel = GameObject.Find("CraftingPanel");
        craftingBookPanel = GameObject.Find("CraftingBookPanel");
    }

    public void Save() {
        //Player inventory
        SerializableInventory serInventory = new SerializableInventory(inventory);
        SaveLoadSystem.Save<SerializableInventory>(serInventory, fileNamePlayerInventory);
        //Player equipment
        SerializableEquipmentInventory serEquipmentInventory = new SerializableEquipmentInventory(equipmentPanel);
        SaveLoadSystem.Save<SerializableEquipmentInventory>(serEquipmentInventory, fileNamePlayerEquipment);
        //Crafting panel items
        SerializableCraftingInventory serCraftingInventory = new SerializableCraftingInventory(craftingPanel);
        SaveLoadSystem.Save<SerializableCraftingInventory>(serCraftingInventory, fileNameCraftingInventory);
        //Known recipes
        SerializableRecipesInventory serRecipesInventory = new SerializableRecipesInventory(craftingBookPanel);
        SaveLoadSystem.Save<SerializableRecipesInventory>(serRecipesInventory, fileNameKnownRecipes);
    }

    public void Load() {
        //Get Data
        SerializableInventory serInventory = SaveLoadSystem.Load<SerializableInventory>(fileNamePlayerInventory);
        SerializableEquipmentInventory serEquipmentInventory = SaveLoadSystem.Load<SerializableEquipmentInventory>(fileNamePlayerEquipment);
        SerializableCraftingInventory serCraftingInventory = SaveLoadSystem.Load<SerializableCraftingInventory>(fileNameCraftingInventory);
        SerializableRecipesInventory serRecipesInventory = SaveLoadSystem.Load<SerializableRecipesInventory>(fileNameKnownRecipes);
        //Player inventory
        Inventory inventoryComponent = inventory.GetComponent<Inventory>();
        if(inventoryComponent!=null){
            inventoryComponent.EmptyInventory();
            List<Container.ContainerItem> containerItemList = GetContainerItemList(serInventory.items); 
            inventoryComponent.SetStartingItems(containerItemList.ToArray());
        } else Debug.LogError("Inventory Panel not found.");
        //Player equipment
        EquipmentPanel equipmentPanelComponent = equipmentPanel.GetComponent<EquipmentPanel>();
        if(equipmentPanelComponent!=null) {
            equipmentPanelComponent.EmptyInventory();
            List<Container.ContainerItem> containerItemList = GetContainerItemList(serEquipmentInventory.items);
            foreach(Container.ContainerItem containerItem in containerItemList) {
                if(containerItem.item!=null && containerItem.amount>0) {
                    equipmentPanelComponent.AddItem((EquippableItem)containerItem.item, containerItem.amount, out EquippableItem previousItem, out int previousItemAmount);
                }
            }
        } else Debug.LogError("Equipment Panel not found.");
        //Crafting panel items
        CraftingPanel craftingPanelComponent = craftingPanel.GetComponent<CraftingPanel>();
        if(craftingPanelComponent!=null) {
            craftingPanelComponent.EmptyInventory();
            List<Container.ContainerItem> containerItemList = GetContainerItemList(serCraftingInventory.items); 
            foreach(Container.ContainerItem containerItem in containerItemList) {
                if(containerItem.item!=null && containerItem.amount>0) {
                    craftingPanelComponent.AddItem(containerItem.item, containerItem.amount);
                }
            }
        } else Debug.LogError("Crafting/Investigation Panel not found.");
        //Known recipes
        CraftingBookPanel craftingBookPanelComponent = craftingBookPanel.GetComponent<CraftingBookPanel>();
        if(craftingBookPanelComponent!=null) {
            craftingBookPanelComponent.EmptyInventory();
            foreach(SerializableRecipe recipe in serRecipesInventory.recipes) {
                if(recipe!=null && recipe.materials!=null && recipe.results!=null) { //for each recipe
                    CraftingRecipe newCraftingRecipe = new CraftingRecipe();
                    newCraftingRecipe.materials = GetItemAmountList(recipe.materials);
                    newCraftingRecipe.results = GetItemAmountList(recipe.results);
                    craftingBookPanelComponent.knownCraftingRecipes.Add(newCraftingRecipe);
                }
            }
        } else Debug.LogError("Crafting Book Panel not found");
    }

    private List<Container.ContainerItem> GetContainerItemList(List<SerializableItem> sInv) {
        List<Container.ContainerItem> containerItemList = new List<Container.ContainerItem>();
        foreach(SerializableItem sItem in sInv) {
            Item item = GetItemByID(sItem.ID);
            if(item!=null) {
                containerItemList.Add(new Container.ContainerItem() { item = item.GetCopy(), amount = sItem.amount });
            }
        }
        return containerItemList;
    }

    private Item GetItemByID(string ID) {
        //Search in the GlobalItemAssets the item (objectscript) by its ID.
        Item item = GlobalItemAssets.Instance.itemList?.Find( x => x.ID == ID );
        if(item==null) Debug.Log("Item not found: " + ID);
        return item;
    }

    private List<ItemAmount> GetItemAmountList(List<SerializableItem> sInv) {
        List<ItemAmount> itemAmountList = new List<ItemAmount>();
        foreach(SerializableItem sItem in sInv) {
            Item item = GetItemByID(sItem.ID);
            if(item!=null) {
                itemAmountList.Add(new ItemAmount() { item = item.GetCopy(), amount = sItem.amount });
            }
        }
        return itemAmountList;
    }

}

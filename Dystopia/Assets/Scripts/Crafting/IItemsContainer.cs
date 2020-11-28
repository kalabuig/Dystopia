public interface IItemsContainer
{
    int ItemCount(string itemID);
    Item RemoveItem(string itemID, int amount); //returns the reference of the item removed from the container
    bool RemoveItem(Item item, int amount);
    bool AddItem(Item item, int amount);
    bool IsFull();
    int NumEmptySlots();
}

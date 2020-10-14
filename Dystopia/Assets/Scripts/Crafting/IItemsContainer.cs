public interface IItemsContainer
{
    int ItemCount(string itemID);
    Item RemoveItem(string itemID); //returns the reference of the item removed from the container
    bool RemoveItem(Item item);
    bool AddItem(Item item);
    bool IsFull();
}

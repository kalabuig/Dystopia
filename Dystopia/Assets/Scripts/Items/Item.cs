using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Items / Item")]
public class Item : ScriptableObject
{
    [SerializeField] string id; //id of the item
    public string ID { get { return id;} }  //Public property
    public string itemName; //name of the item
    public Sprite icon; //image of the item
    [Range(1,9999)]
    public int MaximumStacks = 1; //Objects are stackable if this is greater than 1 (the maximum amount of objects in the stack is this number)

    private void OnValidate() { //Only works on editor mode
        string path = AssetDatabase.GetAssetPath(this); 
        id = AssetDatabase.AssetPathToGUID(path); //Getting the unity UID created
    }

    public virtual Item GetCopy() {
        return MaximumStacks>1? this: Instantiate(this); //Create a new instance if the object is not stackable
    }

    public virtual void Destroy() {
        if(MaximumStacks==1) Destroy(this); //We just need to destroy the object if it is not stackable
    }
}

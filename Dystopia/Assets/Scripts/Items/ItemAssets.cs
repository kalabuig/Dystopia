using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : GenericSingletonClass<ItemAssets>
{
    //Here the list of Items to use in the game
    [SerializeField] private List<ItemData> itemsList;
    private int totalWeight;

    private void Start() {
        ReCalculateProbabilities();
    }

    public Item GetRandomItem() {
        if(itemsList.Count==0) return null;

        int totalWeight = 0;
        foreach(ItemData id in itemsList) {
            id.SetMinValue(totalWeight);
            totalWeight += id.weightChance;
            id.SetMaxValue(totalWeight);
        }

        Debug.Log("totalWeight " + totalWeight); // !!!!
        int randNum = UnityEngine.Random.Range(0, totalWeight+1); //Return a number between 0 (inclusive) and totalWeight+1 (exclusive)
        Debug.Log(randNum); // !!!!
        ItemData result = itemsList.Find( x => x.GetMinValue() >= randNum); // && x.GetMaxValue() < randNum);
        return result != null ? result.item : null;
    }

    public void ReCalculateProbabilities() {
        //Calculate the total weight and the minValue and maxValue to be selected of each item
        int totalWeight = 0;
        foreach(ItemData id in itemsList) {
            id.SetMinValue(totalWeight);
            totalWeight += id.weightChance;
            id.SetMaxValue(totalWeight);
        }
    }

    [Serializable]
    public class ItemData {
        public Item item; //The item 
        public int weightChance; //Related to the probability to spam this item (the bigger this number the more chances)
        private int minValue; //min value to be selected
        private int maxValue; //max value to be selected

        public void SetMinValue(int v) {
            if(v<0) v=0;
            minValue = v;
        }

        public int GetMinValue() {
            return minValue;
        }

        public void SetMaxValue(int v) {
            if(v<0) v=0;
            if(v<minValue) v=minValue;
            maxValue = v;
        }

        public int GetMaxValue() {
            return maxValue;
        }
    }
}

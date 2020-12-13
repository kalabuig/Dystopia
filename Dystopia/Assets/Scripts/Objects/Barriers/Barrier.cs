using System;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public enum BarrierType {
        Fence,
        Wooden,
        Brick,
    }

    [Serializable]
    public struct BarrierData {
        public string barrierName;
        public BarrierType barrierType;
        public Sprite sprite;
        public Color color;
        public int health;
    }

    [SerializeField] private BarrierData[] barrierTypes; //all the data of all types of barriers
    private BarrierData barrierData; //the data of the current barrier
 
    private void Awake() {
        RanndomizeBarrier();
        RefreshData();
    }

    private void RanndomizeBarrier() {
        int intRand = UnityEngine.Random.Range(0, barrierTypes.Length);
        barrierData.barrierName = barrierTypes[intRand].barrierName;
        barrierData.barrierType = barrierTypes[intRand].barrierType;
        barrierData.sprite = barrierTypes[intRand].sprite;
        barrierData.color = barrierTypes[intRand].color;
        barrierData.health = barrierTypes[intRand].health;
    }

    private void RefreshData() {
        GetComponent<Hittable>()?.SetHealth(barrierData.health, barrierData.health);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr!=null) {
            sr.sprite = barrierData.sprite;
            sr.color = barrierData.color;
        } 
    }

    public string GetBarrierName() {
        return barrierData.barrierName;
    }

    public Sprite GetSprite() {
        return barrierData.sprite;
    }
}

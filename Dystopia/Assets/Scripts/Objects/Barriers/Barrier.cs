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
    private BarrierData _barrierData; //the data of the current barrier
    public BarrierData barrierData { get => _barrierData; }
 
    private void Awake() {
        RanndomizeBarrier();
        RefreshData();
    }

    private void RanndomizeBarrier() {
        int intRand = UnityEngine.Random.Range(0, barrierTypes.Length);
        _barrierData.barrierName = barrierTypes[intRand].barrierName;
        _barrierData.barrierType = barrierTypes[intRand].barrierType;
        _barrierData.sprite = barrierTypes[intRand].sprite;
        _barrierData.color = barrierTypes[intRand].color;
        _barrierData.health = barrierTypes[intRand].health;
    }

    private void RefreshData() {
        GetComponent<Hittable>()?.SetHealth(_barrierData.health, _barrierData.health);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr!=null) {
            sr.sprite = _barrierData.sprite;
            sr.color = _barrierData.color;
        } 
    }

    public void SetBarrierDataByType(BarrierType barrierType) {
        foreach(BarrierData data in barrierTypes) {
            if(data.barrierType == barrierType) {
                _barrierData.barrierName = data.barrierName;
                _barrierData.barrierType = data.barrierType;
                _barrierData.sprite = data.sprite;
                _barrierData.color = data.color;
                _barrierData.health = data.health;
                RefreshData();
            }
        }
    }

    public string GetBarrierName() {
        return _barrierData.barrierName;
    }

    public Sprite GetSprite() {
        return _barrierData.sprite;
    }
}

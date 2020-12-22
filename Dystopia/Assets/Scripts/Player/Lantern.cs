using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lantern : MonoBehaviour
{
    //EventArgs 
    public class BatteryEventArgs : EventArgs {
        public int currentValue;
        public int maxValue;
    }

    public event EventHandler<BatteryEventArgs> OnBatteryLevelChange; //On Battery Level Change event 

    [SerializeField] private Light2D lantern;
    private EquipmentSlot batterySlot;
 
    private int batteryTick = 0;
    private int batteryTickRate = 50; //ticks until 1% decrease in battery energy (5 ticks = 1 second)

    private void Start() {
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;  //Suscribe to time tick system (5 ticks per second)
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e) {
        if(batterySlot!=null && batterySlot.item!=null && batterySlot.amount>0 && lantern!=null && lantern.gameObject.activeSelf==true) {
            batteryTick++;
            if(batteryTick>=batteryTickRate) {
                batteryTick -= batteryTickRate;
                if(batteryTick <= 0) {
                    batterySlot.amount--;
                    ForceUIUpdate();
                    CheckBatteryStatus();
                }
            }
        }
    }

    public void ForceUIUpdate() {
        if(OnBatteryLevelChange!=null) { //if there are suscribers
            if(batterySlot!=null && batterySlot.item!=null && batterySlot.amount>=0 && lantern!=null) {
                OnBatteryLevelChange(this, new BatteryEventArgs { currentValue = batterySlot.amount, maxValue = batterySlot.item.MaximumStacks}); //Send event to suscribers
            } else {
                OnBatteryLevelChange(this, new BatteryEventArgs { currentValue = 0, maxValue = 1}); //Send event to suscribers
            }
        }
    }

    private void CheckBatteryStatus() {
        if(batterySlot==null) { //no slot assigned
            SwitchOff();
            ForceUIUpdate();
        } else {
            if(batterySlot.item==null || batterySlot.amount<=0) { //no battery in the slot or battery empty
                SwitchOff();
                ForceUIUpdate();
            }
        }
    }

    public void SetBatterySlot(EquipmentSlot slot) {
        if(slot!=null) {
            batterySlot = slot;
            batterySlot.OnItemChanged += OnBatterySlotChage;
        }
    }

    private void OnBatterySlotChage() {
        if(batterySlot!=null) {
            if(batterySlot.item==null || batterySlot.amount<=0) {
                SwitchOff();
            }
        }
        ForceUIUpdate();
    }

    public void SwitchOnOffLantern() {
        if(batterySlot==null) { //no slot assigned
            SwitchOff();
        } else {
            if(batterySlot.item==null || batterySlot.amount<=0) { //no battery in the slot or battery empty
                SwitchOff();
            } else {
                if(lantern.gameObject.activeSelf) { //swap between switch on or switch off
                    SwitchOff();
                } else {
                    SwitchOn();
                }
            }
        }
    }

    public void SwitchOn() {
        if(lantern!=null) {
            lantern.gameObject.SetActive(true);
        }
    }

    public void SwitchOff() {
        if(lantern!=null) {
            lantern.gameObject.SetActive(false);
        }  
    }
}

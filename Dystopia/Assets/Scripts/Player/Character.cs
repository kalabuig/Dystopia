using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    //EventArgs 
    public class AmountEventArgs : EventArgs {
        public int amount;
    }

    public event EventHandler OnHealthZero; //On Health equal to zero event 
    public event EventHandler<AmountEventArgs> OnHealthChange; //On Health Change event 
    public event EventHandler<AmountEventArgs> OnHungryChange; //On Hungry Change event 
    public event EventHandler<AmountEventArgs> OnThirstChange; //On Thirst Change event 
    public event EventHandler<AmountEventArgs> OnVigorChange; //On Vigor Change event 

    [SerializeField] protected int _health = 100;
    public int health { get => _health; }
    [SerializeField] protected int _hungry = 100;
    public int hungry { get => _hungry; }
    [SerializeField] protected int _thirst = 100;
    public int thirst { get => _thirst; }
    [SerializeField] protected int _vigor = 100;
    public int vigor { get => _vigor; }
    public int maxHealth = 100;
    public int maxHungry = 100;
    public int maxThirst = 100;
    public int maxVigor = 100;
    public int attack = 1;
    public int defense = 0;
    
    public float attackSpeed = 1f; //seconds
    [Range(0,100)]
    public int criticalChance = 15;
    public float moveSpeed = 60f;
    
    public float craftSpeed = 5f; //seconds
    public float investigationSpeed = 5f; //seconds
    public float scavengingSpeed = 5f; //seconds
    public float fillWaterSpeed = 3f; //seconds
    public float useFireSpeed = 10f; //seconds

    public float healthRate = 4f; //seconds
    public float thirstRate = 4f; //seconds
    public float hungryRate = 8f; //seconds
    public float vigorRate = 30f; //seconds

    //Note: Timer is set to 5 ticks per second
    private int healthTick;
    private int healthTickRate; //At this rate we decrease the health by 1% (if we are at 0% hungry or thirst)
    private int hungryTick;
    private int hungryTickRate; //At this rate we decrease the hungry by 1%
    private int thirstTick;
    private int thirstTickRate; //At this rate we decrease the thirst by 1%
    private int vigorTick;
    private int vigorTickRate; //At this rate we decrease the fatigue by 1%

    private void Awake() {
        healthTick = 0;
        hungryTick = 0;
        thirstTick = 0;
        vigorTick = 0;
        healthTickRate = (int) (healthRate * TimeTickSystem.GetTicksPerSecond());
        hungryTickRate = (int) (hungryRate * TimeTickSystem.GetTicksPerSecond());
        thirstTickRate = (int) (thirstRate * TimeTickSystem.GetTicksPerSecond());
        vigorTickRate = (int) (vigorRate * TimeTickSystem.GetTicksPerSecond());
    }

    private void Start() {
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;  //Suscribe to time tick system (5 ticks per second)
    }

    //Timer
    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e) {
        healthTick++;
        if(healthTick>=healthTickRate) {
            healthTick -= healthTickRate;
            if(_hungry == 0 || _thirst == 0) {
                ModifyHealth(-1);
            }
        }
        hungryTick++;
        if(hungryTick>=hungryTickRate) {
            hungryTick -= hungryTickRate;
            ModifyHungry(-1);
        }
        thirstTick++;
        if(thirstTick>=thirstTickRate) {
            thirstTick -= thirstTickRate;
            ModifyThirst(-1);
        }
        vigorTick++;
        if(vigorTick>=vigorTickRate) {
            vigorTick -= vigorTickRate;
            ModifyVigor(-1);
        }
    }

    public void ModifyHealth(int amount) {
        _health += amount; //amount can be negative
        if(_health<0) _health = 0;
        if(_health>maxHealth) _health = maxHealth;
        if(OnHealthChange != null) { //if there are subscribers
            OnHealthChange(this, new AmountEventArgs { amount = amount }); //Launch event to subscribers
        }
        if(amount<0) {
            SoundManager.PlaySound(SoundManager.Sound.PlayerDamage);
        }
        if(_health==0 && OnHealthZero!=null) { //if health is zero
            OnHealthZero(this, new EventArgs());
        }
    }

    public void ModifyHungry(int amount) {
        _hungry += amount; //amount can be negative
        if(_hungry<0) _hungry = 0;
        if(_hungry>maxHungry) _hungry = maxHungry;
        if(OnHungryChange != null) { //if there are subscribers
            OnHungryChange(this, new AmountEventArgs { amount = amount }); //Launch event to subscribers
        }
    }

    public void ModifyThirst(int amount) {
        _thirst += amount; //amount can be negative
        if(_thirst<0) _thirst = 0;
        if(_thirst>maxThirst) _thirst = maxThirst;
        if(OnThirstChange != null) { //if there are subscribers
            OnThirstChange(this, new AmountEventArgs { amount = amount }); //Launch event to subscribers
        }
    }

    public void ModifyVigor(int amount) {
        _vigor += amount; //amount can be negative
        if(_vigor<0) _vigor = 0;
        if(_vigor>maxVigor) _vigor = maxVigor;
        if(OnVigorChange != null) { //if there are subscribers
            OnVigorChange(this, new AmountEventArgs { amount = amount }); //Launch event to subscribers
        }
    }

    private void UnSuscribeTimeTickSystem() {
         TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }

    private void OnDestroy() {
        UnSuscribeTimeTickSystem(); //unsubscribe from the time tick system
    }

}

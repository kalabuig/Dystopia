using UnityEngine;
using System;
using System.Collections.Generic;

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
    [SerializeField] protected int _maxHealth = 100;
    public int maxHealth { get => _maxHealth; set => _maxHealth = maxHealth > 1 ? maxHealth : 1; }
    [SerializeField] protected int _maxHungry = 100;
    public int maxHungry { get => _maxHungry; set => _maxHungry = maxHungry > 0 ? maxHungry : 0; }
    [SerializeField] protected int _maxThirst = 100;
    public int maxThirst { get => _maxThirst; set => _maxThirst = maxThirst > 0 ? maxThirst : 0; }
    [SerializeField] protected int _maxVigor = 100;
    public int maxVigor { get => _maxVigor; set => _maxVigor = maxVigor > 0 ? maxVigor : 0; }
    [SerializeField] protected int _attack = 1;
    public int attack { get => _attack; set => _attack = attack > 0 ? attack : 0; }
    [SerializeField] protected int _defense = 0;
    public int defense { get => _defense; set => _defense = defense > 0 ? defense : 0; }

    //public float attackSpeed = 1f; //seconds
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

    //Player Components
    private Skills _skills;
    public Skills skills { get => _skills; }

    private void Awake()
    {
        CalculateTickRates();
        //Components
        _skills = GetComponent<Skills>();
    }

    public void CalculateTickRates()
    {
        healthTick = 0;
        hungryTick = 0;
        thirstTick = 0;
        vigorTick = 0;
        healthTickRate = (int)(healthRate * TimeTickSystem.GetTicksPerSecond());
        hungryTickRate = (int)(hungryRate * TimeTickSystem.GetTicksPerSecond());
        thirstTickRate = (int)(thirstRate * TimeTickSystem.GetTicksPerSecond());
        vigorTickRate = (int)(vigorRate * TimeTickSystem.GetTicksPerSecond());
    }

    private void Start() {
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;  //Suscribe to time tick system (5 ticks per second)
    }

    //Timer
    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e) {
        healthTick++;
        if(healthTick>=healthTickRate) {
            healthTick -= healthTickRate;
            if(_hungry == 0 || _thirst == 0 || _vigor == 0) {
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
        if(amount>0) {
            amount = ApplyHealthSpecialModifiers(amount);
        }
        _health += amount; //amount can be negative
        if(_health<0) _health = 0;
        if(_health>_maxHealth) _health = _maxHealth;
        if(OnHealthChange != null) { //if there are suscribers
            OnHealthChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
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
        if(_hungry>_maxHungry) _hungry = _maxHungry;
        if(OnHungryChange != null) { //if there are suscribers
            OnHungryChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
        }
    }

    public void ModifyThirst(int amount) {
        _thirst += amount; //amount can be negative
        if(_thirst<0) _thirst = 0;
        if(_thirst>_maxThirst) _thirst = _maxThirst;
        if(OnThirstChange != null) { //if there are suscribers
            OnThirstChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
        }
    }

    public void ModifyVigor(int amount) {
        _vigor += amount; //amount can be negative
        if(_vigor<0) _vigor = 0;
        if(_vigor>_maxVigor) _vigor = _maxVigor;
        if(OnVigorChange != null) { //if there are suscribers
            OnVigorChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
        }
    }

    private int ApplyHealthSpecialModifiers(int amount) {
        List<PassiveSkillData> results = _skills.GetSkillsWithSpecialModifier(SpecialModifier.Healing);
        float multiplyFactor = 1f;
        if(results!=null) {
            foreach(PassiveSkillData r in results) {
                multiplyFactor += r.valueAtThisLevel / 100f;
            }
        }
        return (int)(1f * amount * multiplyFactor);
    }

    public void GetDamage(int amount) {
        //calculate de total defense of the player
        int fullDefense = defense;
        StatsModifiers statsModifiers = GetComponent<StatsModifiers>();
        fullDefense += statsModifiers!=null ? statsModifiers.GetIntStatMod(StatsModifiers.Modifier.protection) : 0;
        Skills skills = GetComponent<Skills>();
        fullDefense += skills!=null ? skills.GetStatSkillModifiersAmount(StatsModifiers.Modifier.protection) : 0;
        //calculate de total damage done after defense applied
        int totalDamage = amount - fullDefense;
        if(totalDamage<=0) totalDamage = 1; //mínimum damage = 1
        ModifyHealth(-totalDamage); //do damage
    }

    private void UnSuscribeTimeTickSystem() {
         TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }

    private void OnDestroy() {
        UnSuscribeTimeTickSystem(); //unsubscribe from the time tick system
    }

    public void LoadSerializedData(SerializableCharacter serCharacter) {
        if(serCharacter!=null) {
            _health = serCharacter.health;
            _hungry = serCharacter.hungry;
            _thirst = serCharacter.thirst;
            _vigor = serCharacter.vigor;
            _maxHealth = serCharacter.maxHealth;
            _maxHungry = serCharacter.maxHungry;
            _maxThirst = serCharacter.maxThirst;
            _maxVigor = serCharacter.maxVigor;
            _attack = serCharacter.attack;
            _defense = serCharacter.defense;

            criticalChance = serCharacter.criticalChance;
            moveSpeed = serCharacter.moveSpeed;
            craftSpeed = serCharacter.craftSpeed;
            investigationSpeed = serCharacter.investigationSpeed;
            scavengingSpeed = serCharacter.scavengingSpeed;
            fillWaterSpeed = serCharacter.fillWaterSpeed;
            useFireSpeed = serCharacter.useFireSpeed;

            healthRate = serCharacter.healthRate;
            thirstRate = serCharacter.thirstRate;
            hungryRate = serCharacter.hungryRate;
            vigorRate = serCharacter.vigorRate;

            //Launch events
            int amount = 0;
            if(OnHealthChange!=null) OnHealthChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
            if(OnHungryChange!=null) OnHungryChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
            if(OnThirstChange!=null) OnThirstChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers
            if(OnVigorChange!=null) OnVigorChange(this, new AmountEventArgs { amount = amount }); //Launch event to suscribers

            CalculateTickRates(); //recalculate tick rates
        }
    }
}

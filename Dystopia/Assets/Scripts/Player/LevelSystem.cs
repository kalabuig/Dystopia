using UnityEngine;
using System;

public class LevelSystem 
{
    //EventArgs 
    public class AmountEventArgs : EventArgs {
        public int amount;
    }

    public event EventHandler<AmountEventArgs> OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private int level;
    private int experience;
    private int experienceToNextLevel;

    public LevelSystem() {
        level = 1;
        experience = 0;
        experienceToNextLevel = 100;
    }

    public void AddExperience(int xpAmount) {
        experience += xpAmount;
        if(experience >= experienceToNextLevel) {
            //Level Up
            level++;
            experience -= experienceToNextLevel;
            if(OnLevelChanged != null) {
                OnLevelChanged(this, EventArgs.Empty); //notify event to suscribers
            }
        }
        if(OnExperienceChanged != null) {
            OnExperienceChanged(this, new AmountEventArgs { amount = xpAmount }); //notify event to suscribers
        }
    }

    public int GetLevelNumber() {
        return level;
    }

    public float GetExperienceNormalized() {
        return (float)experience / experienceToNextLevel;
    }

    public int GetExperienceAbsolute() {
        return experience;
    }

    public int GetExperienceToNextLevel(int level)
    {
        return (int)Mathf.Floor(100 * level * Mathf.Pow(level, 0.5f));
    }

    public void LoadLevelExp(int level, int experience) {
        this.level = level;
        this.experience = experience;
        experienceToNextLevel = GetExperienceToNextLevel(level);
        if(OnExperienceChanged != null) {
            OnExperienceChanged(this, new AmountEventArgs { amount = 0 }); //notify event to suscribers
        }
    }
}

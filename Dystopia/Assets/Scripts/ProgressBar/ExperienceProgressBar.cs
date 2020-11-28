using System;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceProgressBar : MonoBehaviour
{
    [SerializeField] private Text level;
    [SerializeField] private Image experienceBar;
    private LevelSystem levelSystem;

    private void SetExperienceBarFillAmount(float experienceNormalized) {
        experienceBar.fillAmount = experienceNormalized;
    }

    private void SetLevel(int levelNumber) {
        level.text = levelNumber.ToString();
    }

    public void SetLevelSystem(LevelSystem myLevelSystem) {
        levelSystem = myLevelSystem;
        //Get starting values from level System:
        SetLevel(levelSystem.GetLevelNumber());
        SetExperienceBarFillAmount(levelSystem.GetExperienceNormalized());
        //Suscribe to events of level system:
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    //Experience changed
    private void LevelSystem_OnExperienceChanged(object sender, LevelSystem.AmountEventArgs e)
    {
        SetExperienceBarFillAmount(levelSystem.GetExperienceNormalized());
    }

    //Level changed
    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        SetLevel(levelSystem.GetLevelNumber());
    }

    private void OnDestroy() {
        levelSystem.OnExperienceChanged -= LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;
    }
}

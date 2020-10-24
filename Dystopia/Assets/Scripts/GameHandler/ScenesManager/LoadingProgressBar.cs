using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingProgressBar : MonoBehaviour
{
    public Image imageProgressBar;
    public TextMeshProUGUI percentatgeTextProgressBar;

    private void Awake() {
        //image = transform.GetComponent<Image>();
        imageProgressBar.fillAmount = 0f;
    }

    private void Update() {
        float progress = Loader.GetProgress();
        imageProgressBar.fillAmount = progress;
        percentatgeTextProgressBar.text = (progress * 100f).ToString("F0") + "%";
    }
}

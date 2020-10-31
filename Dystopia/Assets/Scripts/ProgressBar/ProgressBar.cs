using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int minValue;
    public int maxValue;
    private int _current;
    public Image fill;
    public Image mask;
    public Color fillColor;
    public Color maskColor;
    public TextMeshProUGUI text;

    public int Current { 
        get => _current; 
        set {
            if(value < minValue) _current = minValue;
            else if(value >maxValue) _current = maxValue;
            else _current = value;
        }
    }

    private void Awake() {
        if(fill!=null) fill.color = fillColor;
        if(mask!=null) mask.color = maskColor;
        if(text!=null) text.color = fillColor;
        _current = minValue;
    }

    private void Update() {
        GetCurrentFill();
    }

    private void GetCurrentFill() {
        float currentOffset = _current - minValue;
        float maxOffset = maxValue - minValue;
        float fillAmount = (float)currentOffset / maxOffset;
        if(fill!=null) fill.fillAmount = fillAmount;
        if(text!=null) text.text = fillAmount*100f + "%";
    }
}

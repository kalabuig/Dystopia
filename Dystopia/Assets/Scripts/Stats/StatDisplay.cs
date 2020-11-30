using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ValueText;

    private void OnValidate() { //Only works on editor mode
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        NameText = texts[0];
        ValueText = texts[1];
    }
}

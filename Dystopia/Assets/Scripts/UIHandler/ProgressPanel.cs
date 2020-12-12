using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressPanel : MonoBehaviour
{
    [SerializeField] private Image imageToShow;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private CanvasRenderer panelRenderer;
    private CanvasRenderer[] childrenRenderers;

    private float initialAlpha = 0.8f;

    private void Awake() {
        panelRenderer = GetComponent<CanvasRenderer>();
        childrenRenderers = GetComponentsInChildren<CanvasRenderer>();
    }

    public void ShowPanel(string textToShow, Sprite spriteToShow) {
        textMeshProUGUI.text = textToShow.Trim();
        imageToShow.sprite = spriteToShow;
        //Color c = panelRenderer.GetColor();
        //c.a = initialAlpha;
        //panelRenderer.SetColor(c);
        SetAlpha(initialAlpha);
    }

    public void HidePanel(float fadeTime = 2f) {
        if(this.isActiveAndEnabled) {
            StartCoroutine(FadeOutPanel(fadeTime));
        }
    }

    private IEnumerator FadeOutPanel(float fadeTime) {
        //Fade fase
        float timeElapsed = 0f;
        SetAlpha(initialAlpha);
        while(timeElapsed < fadeTime ){
            SetAlpha(Mathf.Lerp(initialAlpha, 0f, timeElapsed / fadeTime));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //Final fase
        SetAlpha(0f);
        this.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha) {
        //MainPanel
        Color c = panelRenderer.GetColor();
        c.a = alpha;
        panelRenderer.SetColor(c);
        //ChildrenObjects
        foreach(CanvasRenderer childRenderer in childrenRenderers) {
            Color childColor = childRenderer.GetColor();
            childColor.a = alpha;
            childRenderer.SetColor(childColor);
        }
    }

    private void OnDisable() {
        StopCoroutine("FadeOutPanel");
    }

    private void OnDestroy() {
        StopCoroutine("FadeOutPanel");
    }
}

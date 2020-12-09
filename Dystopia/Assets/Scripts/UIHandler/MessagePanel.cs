using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class MessagePanel : GenericSingletonClass<MessagePanel>
{
    [Serializable]
    public struct ImageElement {
        public MessageIcon messageIcon;
        public Sprite sprite;
    }

    public enum MessageIcon {
        None,
        Celebration,
        Fail,
        IDontKnow,
    }

    [SerializeField] private Image imageToShow;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [Space]
    [Range(0,1)]
    [SerializeField] private float initialAlpha = 0.8f;
    [Space]
    [SerializeField] private ImageElement[] imageElements;
    
    private CanvasRenderer panelRenderer;
    private CanvasRenderer[] childrenRenderers;

    private void Awake() {
        panelRenderer = GetComponent<CanvasRenderer>();
        childrenRenderers = GetComponentsInChildren<CanvasRenderer>();
    }

    public void ShowPanel(string textToShow, MessageIcon messageIcon = MessageIcon.None, SoundManager.Sound messageSound = SoundManager.Sound.ItemFound, float showTime = 1.5f, float fadeTime = 2f) {
        textMeshProUGUI.text = textToShow.Trim();
        SetImage(messageIcon);
        Color c = panelRenderer.GetColor();
        c.a = initialAlpha;
        panelRenderer.SetColor(c);
        //this.gameObject.SetActive(true);
        PlaySound(messageSound);
        StartCoroutine(FadeOutPanel(showTime, fadeTime));
    }

    private IEnumerator FadeOutPanel(float waitTime, float fadeTime) {
        //Show fase
        yield return new WaitForSeconds(waitTime);
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

    private void SetImage(MessageIcon messageIcon) {
        foreach(ImageElement ie in imageElements) {
            if(ie.messageIcon == messageIcon && ie.sprite!=null) {
                imageToShow.sprite = ie.sprite;
                return;
            }
        }
        imageToShow.sprite = null;
    }

    private void PlaySound(SoundManager.Sound messageSound) {
        SoundManager.PlaySound(messageSound);
    }

    private void OnDestroy() {
        StopCoroutine("FadeOutPanel");
    }
}

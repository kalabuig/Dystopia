using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroTexts : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textDisplay;

    private void Start() {
        StartCoroutine(Presentation());
    }

    public void NextScene() {
    Loader.Load(Loader.Scene.MainMenuScene);
    }

    private IEnumerator Presentation() {
        float s = 1f;
        float m = 2f;
        float l = 3f;

        textDisplay.text = "";
        yield return new WaitForSeconds(s);
        textDisplay.text = "It's year 2074...";
        yield return new WaitForSeconds(m);
        textDisplay.text = "...the world is cotrolled by big corporations...";
        yield return new WaitForSeconds(m);
        textDisplay.text = "";
        yield return new WaitForSeconds(s);
        textDisplay.text = "You have done a mistake... ";
        yield return new WaitForSeconds(m);
        textDisplay.text = "...you disagree with them...";
        yield return new WaitForSeconds(m);
        textDisplay.text = "";
        yield return new WaitForSeconds(s);
        textDisplay.text = "Now, your place is a city-prison...";
        yield return new WaitForSeconds(m);
        textDisplay.text = "";
        yield return new WaitForSeconds(s);
        textDisplay.text = "Because the world is a...";
        yield return new WaitForSeconds(l);
        textDisplay.text = "<color=#FF0000>DYSTOPIA</color>";
        yield return new WaitForSeconds(l);
        NextScene(); //Go to the next scene
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroTexts : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textDisplay;
    
    private List<string> textToWrite;
    private float timePerCharacter = 0.1f; //in seconds
    private float timer = 0f;
    private int characterIndex = 0;

    private int lineNum = 0;

    private void Start() {
        //StartCoroutine(Presentation());
        textToWrite = new List<string>();
        textToWrite.Add("It's year 2074" + 
            "             " +
            "\nand the world is controlled by big corporations." + 
            "             " + 
            "\n\nThe fact is that you have done a mistake..." +
            "             " +
            "\nyou disagree with them openly..." + 
            "             "
            );
        textToWrite.Add("\n\nNow, your place is a city-prison" + 
            "             " +
            "\nbecause the world is a creepy..." + 
            "                                       " 
            );
    }

    public void NextScene() {
    Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void Update() {
        if(textDisplay!=null && textToWrite.Count>0) {
            timer -= Time.deltaTime;
            while(timer <= 0f) { //Write next character (or characters if framerate is low)
                timer += timePerCharacter;
                textDisplay.text = textToWrite[lineNum].Substring(0,characterIndex);
                characterIndex++;
                if(characterIndex >= textToWrite[lineNum].Length) { //end of the line
                    lineNum++; //go to next line
                    characterIndex = 0; //reset character index
                    if(lineNum>=textToWrite.Count) { //if it is the last line and it is full displayed
                        textToWrite.Clear();
                        StartCoroutine(ShowTitle());
                    }
                    return;
                }
            }
        }
    }

    private IEnumerator ShowTitle() {
        textDisplay.text = "";
        textDisplay.alignment = TextAlignmentOptions.Center;
        textDisplay.fontSize = 64;
        textDisplay.color = Color.red;
        textDisplay.text = "DYSTOPIA";
        yield return new WaitForSeconds(5f); //Wait 3 seconds
        NextScene(); //Go to the next scene
    }

/*
    private IEnumerator Presentation() {
        float s = 1f;
        float m = 2f;
        float l = 3f;

        textDisplay.text = "";
        yield return new WaitForSeconds(m);
        textDisplay.text = "It's year 2074...";
        yield return new WaitForSeconds(m);
        textDisplay.text += "\nand the world is controlled by big corporations...";
        yield return new WaitForSeconds(m);
        textDisplay.text += "\nbut you have done a mistake...";
        yield return new WaitForSeconds(m);
        textDisplay.text += "\nyou disagree with them...";
        yield return new WaitForSeconds(l);
        textDisplay.text = "";
        yield return new WaitForSeconds(s);
        textDisplay.text += "Now, your place is a city-prison...";
        yield return new WaitForSeconds(m);
        textDisplay.text += "\nbecause the world is a creepy...";
        yield return new WaitForSeconds(l);
        textDisplay.text = "";
        yield return new WaitForSeconds(s);
        textDisplay.alignment = TextAlignmentOptions.Center;
        textDisplay.fontSize = 64;
        textDisplay.text += "<color=#FF0000>DYSTOPIA</color>";
        yield return new WaitForSeconds(l);
        NextScene(); //Go to the next scene
    }
*/

}

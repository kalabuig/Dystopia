using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FlashPanel : MonoBehaviour
{
    Character character;
    private Image image;
    private Color c;

    private void Awake() {
        image = GetComponent<Image>();
        c = image.color;
    }

    private void Start() {
        character = GameObject.Find("Player").GetComponent<Character>();
        SuscribeToCharacterEvents();
    }

    IEnumerator Fade() 
    {
        float initialAlfa = 0.4f;
        float stepTime = 0.05f;
        for (float ft = initialAlfa; ft >= 0; ft -= stepTime) 
        {
            c.a = ft;
            image.color = c;
            yield return new WaitForSeconds(stepTime);
        }
        c.a = 0f;
        image.color = c;
    }

    private void SuscribeToCharacterEvents() {
        if(character!=null)
            character.OnHealthChange += Character_OnHealthChange;  //Suscribe to OnHealthChange event
    }

    private void Character_OnHealthChange(object sender, Character.AmountEventArgs e) {
        if(e.amount<0) {
            c = Color.red;
            StartCoroutine("Fade"); //Red flash
        } else if(e.amount>0) {
            c = Color.green;
            StartCoroutine("Fade"); //Green flash
        }
    }
}

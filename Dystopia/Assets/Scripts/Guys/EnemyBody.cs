using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    [SerializeField] private Guy guy;

    private void Awake() {
        RandomizeColors();
    }

    private void DefaultColors() {
        guy.SetColor(Guy.BodyPart.Body, guy.defaultBodyColor);
        guy.SetColor(Guy.BodyPart.Back, guy.defaultBackColor);
        guy.SetColor(Guy.BodyPart.Head, guy.defaultHeadColor);
        guy.SetColor(Guy.BodyPart.Hands, guy.defaultLeftHandColor);
    }

    private void RandomizeColors() {
        DefaultColors();
        guy.SetColor(Guy.BodyPart.Body, RandomBodyColor());
        guy.SetColor(Guy.BodyPart.Head, RandomHeadColor());
    }

    private Color RandomHeadColor() {
        switch(UnityEngine.Random.Range(0,4)) {
            default:
            case 0: return Color.black;
            case 1: return Color.white;
            case 2: return Color.gray;
            case 3: return Color.yellow;
        }
    }

    private Color RandomBodyColor() {
        switch(UnityEngine.Random.Range(0,5)) {
            default:
            case 0: return Color.blue;
            case 1: return Color.red;
            case 2: return Color.magenta;
            case 3: return Color.green;
            case 4: return Color.cyan;
        }
    }

}

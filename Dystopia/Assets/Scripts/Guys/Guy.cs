using UnityEngine;

public class Guy : MonoBehaviour
{
    public enum BodyPart {
        None,
        Back,
        Body,
        Hands,
        Head,
    }

    //Objects to show
    [SerializeField] private SpriteRenderer back;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer leftHand;
    [SerializeField] private SpriteRenderer rightHand;
    [SerializeField] private SpriteRenderer head;

    //Default colors
    [SerializeField] private Color _defaultBackColor;
    public Color defaultBackColor { get => _defaultBackColor; }
    [SerializeField] private Color _defaultBodyColor;
    public Color defaultBodyColor { get => _defaultBodyColor; }
    [SerializeField] private Color _defaultLeftHandColor;
    public Color defaultLeftHandColor { get => _defaultLeftHandColor; }
    [SerializeField] private Color _defaultRightHandColor;
    public Color defaultRightHandColor { get => _defaultRightHandColor; }
    [SerializeField] private Color _defaultHeadColor;
    public Color defaultHeadColor { get => _defaultHeadColor; }

    Color transparentColor = new Color(1,1,1,0);

    public void SetTransparent(BodyPart bodyPart) {
        switch(bodyPart) {
            case BodyPart.Back:
                if(back!=null) {
                    back.color = transparentColor;
                }
                break;
            case BodyPart.Body:
                if(body!=null) {
                    body.color = transparentColor;
                }
                break;
            case BodyPart.Hands:
                if(leftHand!=null) {
                    leftHand.color = transparentColor;
                }
                if(rightHand!=null) {
                    rightHand.color = transparentColor;
                }
                break;
            case BodyPart.Head:
                if(head!=null) {
                    head.color = transparentColor;
                }
                break;
            default:
                break;
        }
        
    }

    public void SetColor(BodyPart bodyPart, Color newColor) {
        if(newColor == null) return;
        switch(bodyPart) {
            case BodyPart.Back:
                if(back!=null) {
                    back.color = newColor;
                }
                break;
            case BodyPart.Body:
                if(body!=null) {
                    body.color = newColor;
                }
                break;
            case BodyPart.Hands:
                if(leftHand!=null) {
                    leftHand.color = newColor;
                }
                if(rightHand!=null) {
                    rightHand.color = newColor;
                }
                break;
            case BodyPart.Head:
                if(head!=null) {
                    head.color = newColor;
                }
                break;
            default:
                break;
        }
    }
}

using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    [SerializeField] private float timeToStartFading = 1f; //Time until starts to fade off
    [SerializeField] private float fadeSpeed = 3f; //fading speed
    [SerializeField] private float increaseScaleAmount = 1f;
    [SerializeField] private float decreaseScaleAmount = 1f;
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float moveSpeedDecrease = 8;
    
    private TextMeshPro textMesh; //The text mesh pro component
    private Color textColor; //to manage the alpha when fading off
    private float timerCounter;
    private Vector3 moveVector; 

    private static int sortingOrder;

    public static TextPopup Create(Vector3 position, string textToShow, Color c) {
        Transform textPopupTransform = Instantiate(TextAssets.Instance.pfTextPopup, position, Quaternion.identity);
        TextPopup textPopup = textPopupTransform.GetComponent<TextPopup>();
        textPopup.Setup(textToShow, c);
        return textPopup;
    }

    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(string textToShow, Color c) {
        textMesh.SetText(textToShow);
        textMesh.fontSize = 36;
        textColor = c;
        textMesh.color = c;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        timerCounter = timeToStartFading;
        moveVector = new Vector3(0.7f,1,0) * moveSpeed;
    }

    private void FixedUpdate() {
        //Move control
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * moveSpeedDecrease * Time.deltaTime;
        //Time control
        timerCounter -= Time.deltaTime;
        if(timerCounter <= 0) {
            //Start fading
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a <= 0) {
                Destroy(gameObject);
            }
        }
        //Scale control
        if(timerCounter > timeToStartFading * 0.5f) { //first half of the lifetime
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else { //second half of the lifetime
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
    }
}

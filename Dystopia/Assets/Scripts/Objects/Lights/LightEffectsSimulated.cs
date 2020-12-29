using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffectsSimulated : MonoBehaviour
{
    [SerializeField] private GameObject lightGO;
    [Space]
    [SerializeField] private string myName;
    [Space]
    [Header("Chance to have effects")]
    [Range(0,100)]
    [SerializeField] private int withEffectsChance = 25;
    [Space]
    [Header("Time between effects")]
    [SerializeField] private float minWaitingTime = .5f;
    [SerializeField] private float maxWaitingTime = 5f;
    [Space]
    [Header("Blink effect duration")]
    [SerializeField] private float totalTimeInSeconds = 2f;

    private Transform player;
    private float distanceToDisableLight = 200f; //for performance purposes
    private SpriteRenderer lightSprite;

    public bool bucle = true;

    private void Awake() {
        player = GameObject.Find("Player")?.transform;
        int randItHasEffect = UnityEngine.Random.Range(0, 101);
        if(randItHasEffect>withEffectsChance) {
            bucle = false;
        }
        lightSprite = lightGO.GetComponent<SpriteRenderer>();
    }

    private void Start() {
        StartCoroutine(BlinkEffect());
    }

    private void FixedUpdate() {
        if(player!=null) {
            if(Vector3.Distance(player.position, this.transform.position) >= distanceToDisableLight) {
                lightGO.SetActive(false);
            } else {
                lightGO.SetActive(true);
            }
        }
    }

    public IEnumerator BlinkEffect() {
        float maxAlpha = lightSprite.color.a;
        float halfTime = totalTimeInSeconds / 2;
        while(bucle) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitingTime, maxWaitingTime)); //random waiting time between effects (blinks)
            for(int i = 0; i< UnityEngine.Random.Range(1, 4); i++) { //random núm of blinks: 1 - 3
                if(lightGO.activeSelf) {
                    SoundManager.PlaySound3D(SoundManager.Sound.BZT, this.transform.position);
                    float alpha = maxAlpha;
                    while (lightSprite.color.a > 0f) {
                        alpha -= Time.deltaTime / halfTime; //Decrease intensity
                        Color c = new Color(lightSprite.color.r, lightSprite.color.g, lightSprite.color.b, alpha);
                        lightSprite.color = c;
                        yield return null;
                    }
                    while (lightSprite.color.a < maxAlpha) {
                        alpha += Time.deltaTime / halfTime; //Decrease intensity
                        Color c = new Color(lightSprite.color.r, lightSprite.color.g, lightSprite.color.b, alpha);
                        lightSprite.color = c;
                        yield return null;
                    }
                    Color baseColor = new Color(lightSprite.color.r, lightSprite.color.g, lightSprite.color.b, maxAlpha);
                    lightSprite.color = baseColor;
                }   
            }
        }
        yield return null;
    }

    public string GetName() {
        return myName;
    }

    private void OnDisable() {
        StopCoroutine(BlinkEffect());
    }

    private void OnDestroy() {
        StopCoroutine(BlinkEffect());
    }
}

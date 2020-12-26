using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightEffects : MonoBehaviour
{
    [SerializeField] private Light2D light;
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

    public bool bucle = true;

    private void Awake() {
        player = GameObject.Find("Player")?.transform;
        int randItHasEffect = UnityEngine.Random.Range(0, 101);
        if(randItHasEffect>withEffectsChance) {
            bucle = false;
        }
    }

    private void Start() {
        StartCoroutine(BlinkEffect());
    }

    private void FixedUpdate() {
        if(player!=null) {
            if(Vector3.Distance(player.position, this.transform.position) >= distanceToDisableLight) {
                light.gameObject.SetActive(false);
            } else {
                light.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator BlinkEffect() {
        float maxIntensity = light.intensity;
        float halfTime = totalTimeInSeconds / 2;
        while(bucle) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitingTime, maxWaitingTime)); //random waiting time between effects (blinks)
            for(int i = 0; i< UnityEngine.Random.Range(1, 4); i++) { //random núm of blinks: 1 - 3
                if(light.gameObject.activeSelf) {
                    SoundManager.PlaySound3D(SoundManager.Sound.BZT, this.transform.position);
                    while (light.intensity > 0) {
                        light.intensity -= Time.deltaTime / halfTime; //Decrease intensity
                        yield return null;
                    }
                    while (light.intensity < maxIntensity) {
                        light.intensity += Time.deltaTime / halfTime; // Increase intensity
                        yield return null;
                    }
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

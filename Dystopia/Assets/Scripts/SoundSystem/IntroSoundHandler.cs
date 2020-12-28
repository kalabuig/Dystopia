using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSoundHandler : MonoBehaviour
{
    [SerializeField] private SoundAssets soundAssets;

    private void Start() {
        if(soundAssets!=null) {
            SoundManager.PlayIntroMusic();
        }
    }
}

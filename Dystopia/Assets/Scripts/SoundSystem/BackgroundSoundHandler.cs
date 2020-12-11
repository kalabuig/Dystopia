using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundHandler : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SoundManager.PlayBackgroundSoundLoop());
    }
}

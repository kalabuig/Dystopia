using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SoundManager
{
    public enum Sound {
        None,
        PlayerDamage,
        Search,
        Eat,
        Drink,
        ItemFound,
        ItemNotFound,
        WoodenItemHitted,
        MetalItemHitted,
        AttackMele,
        BZT,
    }

    //Static attributes no use just one instance (performance)
    private static GameObject oneShotGO;
    private static AudioSource oneShotAS;
    private static GameObject backgroundSoundGO;
    private static AudioSource backgroundSoundAS;

    //Static attribute no play background sound
    public static bool playBackgroundSound = true;

    //////////////// SOUNDS //////////////////////

    //2D Sound (One Shot Play)
    public static void PlaySound(Sound sound) {
        SoundAssets.SoundAudioClip soundAudioClip = GetSoundAudioClip(sound);
        if(soundAudioClip!=null && soundAudioClip.CanPlay()) {
            if(oneShotGO == null) {
                oneShotGO = new GameObject("OneShotSound");
                oneShotAS = oneShotGO.AddComponent<AudioSource>();
            }
            oneShotAS.PlayOneShot(soundAudioClip.audioClip);
            soundAudioClip.setLastPlaytTime(Time.time);
        }
    }

    //3D Sound
    public static void PlaySound3D(Sound sound, Vector3 position, float minDistance = 10f, float maxDistance = 50f) {
        SoundAssets.SoundAudioClip soundAudioClip = GetSoundAudioClip(sound);
        if(soundAudioClip.CanPlay()) {
            GameObject soundGO = new GameObject("Sound");
            soundGO.transform.position = position;
            AudioSource audioSource = soundGO.AddComponent<AudioSource>();
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.clip = soundAudioClip.audioClip;
            audioSource.spatialBlend = 1f; //full 3D
            audioSource.Play();
            soundAudioClip.setLastPlaytTime(Time.time);
            UnityEngine.Object.Destroy(soundGO, audioSource.clip.length); //Destroy the object after the length (time) of the clip
        }
    }

    private static SoundAssets.SoundAudioClip GetSoundAudioClip(Sound sound) {
        return SoundAssets.Instance.soundsList.Find(x => x.sound == sound);;
    }

    //////////////// BACKGROUNDS //////////////////////

    public static IEnumerator PlayBackgroundSoundLoop() {
        while(playBackgroundSound) {
            AudioClip backgroundSoundClip = GetRandomBackgroundSoundAudioClip();
            if(backgroundSoundGO == null) {
                backgroundSoundGO = new GameObject("BackgroundSoundAS");
                backgroundSoundAS = backgroundSoundGO.AddComponent<AudioSource>();
            }
            backgroundSoundAS.clip = backgroundSoundClip;
            backgroundSoundAS.loop = true;
            backgroundSoundAS.ignoreListenerPause = true; //The background sound countinues playing while the game is paused
            backgroundSoundAS.Play();
            yield return new WaitForSeconds(backgroundSoundAS.clip.length);
        }
        //yield return null;
    }

    private static AudioClip GetRandomBackgroundSoundAudioClip() {
        int randIndex = UnityEngine.Random.Range(0, SoundAssets.Instance.backgroundSoundsList.Count);
        return SoundAssets.Instance.backgroundSoundsList[randIndex];
    }

    /////////////// INTRO MUSIC ////////////////////

    public static void PlayIntroMusic() {
        AudioClip soundAudioClip = SoundAssets.Instance.introBackgroundMusic;
        if(soundAudioClip!=null) {
            if(oneShotGO == null) {
                oneShotGO = new GameObject("OneShotSound");
                oneShotAS = oneShotGO.AddComponent<AudioSource>();
            }
            oneShotAS.PlayOneShot(soundAudioClip);
        }
    }

}

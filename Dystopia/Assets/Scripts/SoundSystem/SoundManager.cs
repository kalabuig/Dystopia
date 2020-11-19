using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound {
        PlayerDamage,
        Search,
        Eat,
        Drink,
        ItemFound,
        ItemNotFound,
        WoodenItemHitted,
        MetalItemHitted,
        AttackMele,
    }

    //Static attributes no use just one instance (performance)
    private static GameObject oneShotGO;
    private static AudioSource oneShotAS;

    //2D Sound (One Shot Play)
    public static void PlaySound(Sound sound) {
        SoundAssets.SoundAudioClip soundAudioClip = GetSoundAudioClip(sound);
        if(soundAudioClip.CanPlay()) {
            if(oneShotGO == null) {
                oneShotGO = new GameObject("OneShotSound");
                oneShotAS = oneShotGO.AddComponent<AudioSource>();
            }
            oneShotAS.PlayOneShot(soundAudioClip.audioClip);
            soundAudioClip.setLastPlaytTime(Time.time);
        }
    }

    //3D Sound
    public static void PlaySound(Sound sound, Vector3 position) {
        SoundAssets.SoundAudioClip soundAudioClip = GetSoundAudioClip(sound);
        if(soundAudioClip.CanPlay()) {
            GameObject soundGO = new GameObject("Sound");
            AudioSource audioSource = soundGO.AddComponent<AudioSource>();
            audioSource.clip = soundAudioClip.audioClip;
            audioSource.spatialBlend = 1f; //full 3D
            audioSource.Play();
            soundAudioClip.setLastPlaytTime(Time.time);
            Object.Destroy(soundGO, audioSource.clip.length); //Destroy the object after the length (time) of the clip
        }
    }

    private static SoundAssets.SoundAudioClip GetSoundAudioClip(Sound sound) {
        return SoundAssets.Instance.soundsList.Find(x => x.sound == sound);;
    }

}

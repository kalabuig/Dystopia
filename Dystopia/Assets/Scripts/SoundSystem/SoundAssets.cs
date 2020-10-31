using System;
using System.Collections.Generic;
using UnityEngine;

//Class used as a container for the sound clips to use in the game
public class SoundAssets : GenericSingletonClass<SoundAssets>
{
    //Here the list of AudioClips to use in the game
    public List<SoundAudioClip> soundsList;

    [Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        public float delayTime;
        private float lastPlayTime = 0f;

        public void setLastPlaytTime(float t) {
            lastPlayTime = t;
        }

        public float getLastPlayTime() {
            return lastPlayTime;
        }
        
        public bool CanPlay() {
            return  (lastPlayTime + delayTime) < Time.time ? true : false;
        }
    }
}

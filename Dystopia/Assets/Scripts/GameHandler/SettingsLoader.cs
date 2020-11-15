using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsLoader : MonoBehaviour
{
    public AudioMixer mainAudioMixer;

    void Start()
    {
        LoadVideoSettings();
        LoadAudioSettings();
    }

    public void LoadVideoSettings() {
        SaveSettingsSystem.VideoData data = SaveSettingsSystem.LoadVideoSettings();
        Screen.SetResolution(data.resolution[0], data.resolution[1], data.fullScreen);
        Application.targetFrameRate = data.frameRate;
    }

    public void LoadAudioSettings () {
        SaveSettingsSystem.AudioData data = SaveSettingsSystem.LoadAudioSettings();
        mainAudioMixer.SetFloat("MasterVolume", data.masterSoundLevel);
    }
}

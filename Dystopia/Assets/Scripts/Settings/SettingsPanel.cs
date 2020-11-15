using UnityEngine;
using System;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    //public AudioMixer mainAudioMixer;
    public AudioSettings audioSettings;
    public VideoSettings videoSettings; 

    private void Start() {
        LoadAudioSettings();
        LoadVideoSettings();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.S)) {
            ExitWindow();
        }
    }

    private void LoadAudioSettings() {
        //Load audio settings
        SaveSettingsSystem.AudioData audio = SaveSettingsSystem.LoadAudioSettings();
        audioSettings.masterAudioSlider.value = audio.masterSoundLevel;
    }

    private void LoadVideoSettings() {
        //Load video settings
        SaveSettingsSystem.VideoData video = SaveSettingsSystem.LoadVideoSettings();
        videoSettings.fullScreenToggle.isOn = video.fullScreen; //full screen
        Resolution res = Screen.currentResolution;
        res.width = video.resolution[0];
        res.height = video.resolution[1];
        videoSettings.SetResolution(res); //resolution
        videoSettings.SetFrameRate(); //framerate
    }

    public void ExitWindow() {
        SaveAudioSettings();
        SaveVideoSettings();
        //this.gameObject.SetActive(false);
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    public void SaveVideoSettings() {
        //Save the video settings to a file
        SaveSettingsSystem.SaveVideoSettings(Application.targetFrameRate);
    }
    
    public void SaveAudioSettings() {
        //Save the audio settings to a file
        if(audioSettings.mainAudioMixer.GetFloat("MasterVolume", out float volume)) {
            SaveSettingsSystem.SaveAudioSettings(volume);
        }
    }

}

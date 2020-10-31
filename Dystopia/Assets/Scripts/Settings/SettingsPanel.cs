using UnityEngine;
using System;
using UnityEngine.Audio;

public class SettingsPanel : MonoBehaviour
{
    //public AudioMixer mainAudioMixer;
    public AudioSettings audioSettings;
    public VideoSettings videoSettings; 

    private void Start() {
        //Load audio settings
        SaveSettingsSystem.AudioData audio = SaveSettingsSystem.LoadAudioSettings();
        audioSettings.masterAudioSlider.value = audio.masterSoundLevel;
        //Load video settings
        SaveSettingsSystem.VideoData video = SaveSettingsSystem.LoadVideoSettings();
        videoSettings.fullScreenToggle.isOn = video.fullScreen;
        Resolution res = Screen.currentResolution;
        res.width = video.resolution[0];
        res.height = video.resolution[1];
        videoSettings.SetResolution(res);
        Debug.Log(video.fullScreen + " " + video.resolution[0] + " " + video.resolution[1]); 
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.S)) {
            ExitWindow();
        }
    }

    public void ExitWindow() {
        SaveAudioSettings();
        SaveVideoSettings();
        //this.gameObject.SetActive(false);
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    public void SaveVideoSettings() {
        //Save the video settings to a file
        SaveSettingsSystem.SaveVideoSettings();
        Debug.Log("Video Settings saved");
    }
    
    public void SaveAudioSettings() {
        //Save the audio settings to a file
        if(audioSettings.mainAudioMixer.GetFloat("MasterVolume", out float volume)) {
            SaveSettingsSystem.SaveAudioSettings(volume);
            Debug.Log("Audio Settings saved");
        }
    }

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Dropdown resolutionDropDown;
    public Dropdown frameRateDropDown;

    private Resolution[] resolutions;

    private void Awake() {
        FillResolutionDropDown();
        FillFrameRateDropDown();
    }

    private void FillResolutionDropDown() {
        //Fill the drop down with all the possible resolutions, select the used one
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> resOptions = new List<string>();
        int currentRes = 0;
        for(int i = 0; i < resolutions.Length; i++) {
            string resText = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(resText);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentRes = i;
            }
        }
        resolutionDropDown.AddOptions(resOptions);
        resolutionDropDown.value = currentRes;
        resolutionDropDown.RefreshShownValue();
    }

    private void FillFrameRateDropDown() {
        frameRateDropDown.ClearOptions();
        List<string> frOptions = new List<string>();
        frOptions.Add("30"); //0
        frOptions.Add("60"); //1
        frameRateDropDown.AddOptions(frOptions);
        Debug.Log(Application.targetFrameRate + " frame rate");
        switch(Application.targetFrameRate) {
            case 60:
                frameRateDropDown.value = 1;
                break;
            case 30:
            default:
                frameRateDropDown.value = 0;
                break;
        }
    }

    public int GetSelectedFrameRate() {
        return frameRateDropDown.value == 1? 60 : 30;
    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resIndex) {
        Resolution res = resolutions[resIndex];
        SetResolution(res);
    }

    public void SetResolution(Resolution res) {
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFrameRate() {
        Application.targetFrameRate = GetSelectedFrameRate();
    }
}

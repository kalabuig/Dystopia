using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    public Toggle fullScreenToggle;
    public Dropdown resolutionDropDown;

    private Resolution[] resolutions;

    private void Awake() {
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
}

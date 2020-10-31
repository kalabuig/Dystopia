using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mainAudioMixer;
    public Slider masterAudioSlider;

    public void SetVolume (float volume) {
        mainAudioMixer.SetFloat("MasterVolume", volume);
    }
}

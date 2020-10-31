using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSettingsSystem
{
    private static string videoPath = Application.persistentDataPath + "videosettings.sav";
    private static string audioPath = Application.persistentDataPath + "audiosettings.sav";

    public static void SaveVideoSettings() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(videoPath, FileMode.Create);
        VideoData data = new VideoData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static VideoData LoadVideoSettings() {
        if(File.Exists(videoPath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(videoPath, FileMode.Open);
            VideoData data = formatter.Deserialize(stream) as VideoData;
            stream.Close();
            return data;
        } else {
            return null;
        }
    }

    public static void SaveAudioSettings(float masterVolume) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(audioPath, FileMode.Create);
        AudioData data = new AudioData(masterVolume);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static AudioData LoadAudioSettings() {
        if(File.Exists(audioPath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(audioPath, FileMode.Open);
            AudioData data = formatter.Deserialize(stream) as AudioData;
            stream.Close();
            return data;
        } else {
            return null;
        }
    }

    [Serializable]
    public class VideoData {
        public bool fullScreen;
        public int[] resolution;

        public VideoData() {
            fullScreen = Screen.fullScreen;
            resolution = new int[2] {Screen.currentResolution.width, Screen.currentResolution.height};
        }
    }

    [Serializable]
    public class AudioData {
        public float masterSoundLevel;

        public AudioData(float masterVolume) {
            masterSoundLevel = masterVolume;
        }
    }

}

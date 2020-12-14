using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSystem : MonoBehaviour
{
    private static string path = Application.persistentDataPath + "/saves/"; //Path to the save files

    public static void Save<T>(T objectToSave, string key) {
        Directory.CreateDirectory(path);
        BinaryFormatter binFormatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".save", FileMode.Create))
        {
            binFormatter.Serialize(fileStream, objectToSave);
            fileStream.Close();
        }
        Debug.Log("Saved to " + path.ToString());
    }

    public static T Load<T>(string key) {
        BinaryFormatter binFormatter = new BinaryFormatter();
        T dataToReturn = default(T);
        using (FileStream fileStream = new FileStream(path + key + ".save", FileMode.Open))
        {
            dataToReturn = (T)binFormatter.Deserialize(fileStream);
            fileStream.Close();
        }
        return dataToReturn;
    }

    public static bool SaveExists(string key) {
        return File.Exists(path + key + ".save");
    }

    public static void DeleteAllSaveFiles() {
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        directoryInfo.Delete(true);
        Directory.CreateDirectory(path);
    }
}

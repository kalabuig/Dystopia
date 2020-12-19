using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadEnvironmentData : MonoBehaviour
{
    private GameHandler gameHandler;

    private const string fileName = "environment";

    private void Awake() {
        gameHandler = GameObject.Find("GameHandler")?.GetComponent<GameHandler>();
    }

    public void Save() {
        if(gameHandler!=null) {
            SerializableEnvironment serEnvironment = new SerializableEnvironment(gameHandler);
            SaveLoadSystem.Save<SerializableEnvironment>(serEnvironment, fileName);
        }
    }

    public void Load() {
        //Get Data
        SerializableEnvironment serLoadedEnvironment = SaveLoadSystem.Load<SerializableEnvironment>(fileName);
        //Time and Date
        if(serLoadedEnvironment!=null && gameHandler!=null) {
            GameDateTimeHandler gameDateTimeHandler = gameHandler.GetComponent<GameDateTimeHandler>();
            if(gameDateTimeHandler!=null) gameDateTimeHandler.SetTimeDate(serLoadedEnvironment.timeDays, serLoadedEnvironment.timeMinutes);
            //Weather
            WeatherHandler weatherHandler = gameHandler.GetComponent<WeatherHandler>();
            if(weatherHandler!=null) weatherHandler.SetWeather((WeatherHandler.WeatherType)serLoadedEnvironment.weather);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableEnvironment
{
    public int timeDays;
    public int timeMinutes;
    public int weather;

    public SerializableEnvironment(GameHandler gameHandler) {
        DoSerialization(gameHandler);
    }

    public void DoSerialization(GameHandler gameHandler) {
        if(gameHandler!=null) {
            //Time and Date
            GameDateTimeHandler gameDateTimeHandler = gameHandler.GetComponent<GameDateTimeHandler>();
            if(gameDateTimeHandler!=null) gameDateTimeHandler.GetTimeDate(out timeDays, out timeMinutes);
            //Weather
            WeatherHandler weatherHandler = gameHandler.GetComponent<WeatherHandler>();
            if(weatherHandler!=null) weather = (int) weatherHandler.weather;
        }
        
    }
}

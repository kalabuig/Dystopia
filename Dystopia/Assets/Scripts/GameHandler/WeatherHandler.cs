using System;
using UnityEngine;

public class WeatherHandler : GenericSingletonClass<WeatherHandler>
{
    public enum WeatherType {
        None,
        Fog,
        Rain,
    }

    [SerializeField] private int ingameMinutsBetweenWeatherChanges = 480; //480 min = 8 hours
    [Range(0,100)]
    [SerializeField] private int chanceOfWeatherChange = 30;
    [SerializeField] private GameObject fog;
    [SerializeField] private GameObject rain;

    private WeatherType _weather = WeatherType.None;
    public WeatherType weather {
        get => _weather;
    }

    private int counterToChangeWeather;

    private void Start() {
        counterToChangeWeather = 0;
        GameDateTimeHandler gameDateTimeHandler = GetComponent<GameDateTimeHandler>();
        if(gameDateTimeHandler!=null) {
            gameDateTimeHandler.OnTimeChange += GameDateTimeHandler_OnTimeChange;
        }
    }

    private void GameDateTimeHandler_OnTimeChange()
    {
        counterToChangeWeather++;
        if(counterToChangeWeather >= ingameMinutsBetweenWeatherChanges) { //if it's time to change the weather
            counterToChangeWeather -= (ingameMinutsBetweenWeatherChanges);
            int randomNum = UnityEngine.Random.Range(0, 101);
            if(randomNum<chanceOfWeatherChange) {
                //Change Weather
                int numOfTypes = Enum.GetValues(typeof(WeatherType)).Length;
                int randomWeather = (int)weather;
                while(randomWeather==(int)weather && numOfTypes>1) { //Search for a diferent weather type;
                    randomWeather = UnityEngine.Random.Range(0, numOfTypes);
                }
                SetWeather((WeatherType)randomWeather); //new weather type
            }
        }
    }

    private void DeactivateWeather() {
        fog?.SetActive(false);
        rain?.SetActive(false);
    }

    public void SetWeather(WeatherType w) {
        _weather = w;
        DeactivateWeather();
        if(w==WeatherType.Fog) {
            ActivateFog(true);
        }
        if(w==WeatherType.Rain) {
            ActivateRain(true);
        }
    }

    public void ActivateFog(bool b) {
        if(fog!=null) {
            fog.SetActive(b);
        }
    }

    public void ActivateRain(bool b) {
        if(rain!=null) {
            rain.SetActive(b);
        }
    }
}

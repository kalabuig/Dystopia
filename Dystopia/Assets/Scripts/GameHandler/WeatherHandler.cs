using UnityEngine;

public class WeatherHandler : GenericSingletonClass<WeatherHandler>
{

    public enum WeatherType {
        None,
        Fog,
        Rain,
    }

    [SerializeField] private GameObject fog;
    [SerializeField] private GameObject rain;

    private WeatherType _weather = WeatherType.None;
    public WeatherType weather {
        get => _weather;
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

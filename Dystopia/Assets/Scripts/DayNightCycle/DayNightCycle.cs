using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : GenericSingletonClass<DayNightCycle>
{

    public enum DayNightFase {
        Night,
        SunRise,
        Day,
        SunSet
    }

    [SerializeField] private Light2D globalLight2D;
    [Space]
    [Header("Light Intensity Range")]
    [Range(0,1)]
    [SerializeField] private float minIntesity = 0;
    [Range(0,1)]
    [SerializeField] private float maxIntesity = 1;
    [Space]
    [Header("Time Events")]
    [SerializeField] private int StartingSunRiseHour = 6;
    [SerializeField] private int StartingSunSetHour = 18;
    [Range(0, 4)]
    [SerializeField] private int ChangeDurationInHours = 2;
    [Space]
    [Header("Ticks between updates")]
    [Range(0,60)]
    [SerializeField] private int ticksBetweenUpdates = 10;

    private WeatherHandler weatherHandler;
    private GameDateTimeHandler gameDateTimeHandler;
    
    //Values in minutes
    private int startSunRiseMin;
    private int finishSunRiseMin;
    private int startSunSetMin;
    private int finishSunSetMin;

    //Day Night Fase
    private DayNightFase dayNightFase;

    //Ticks Counter
    private int ticksCounter = 0;

    //Inside Buildings
    private bool insideBuildings = false;

    private void Awake() {
        weatherHandler = GetComponent<WeatherHandler>();
        gameDateTimeHandler = GetComponent<GameDateTimeHandler>();
    }

    private void Start() {
        gameDateTimeHandler.OnTimeChange += OnTimeChanged; //Suscribe to DateTime event

        startSunRiseMin = StartingSunRiseHour * 60;
        finishSunRiseMin = (StartingSunRiseHour + ChangeDurationInHours) * 60;
        startSunSetMin = StartingSunSetHour * 60;
        finishSunSetMin = (StartingSunSetHour + ChangeDurationInHours) * 60;
        
        ticksCounter = ticksBetweenUpdates;
        OnTimeChanged();
    }

    private void OnTimeChanged()
    {
        ticksCounter++;
        if(ticksCounter>=ticksBetweenUpdates && insideBuildings==false) { //Not inside a building
            ticksCounter = 0;
            UpdateIntensity();
        }
    }

    //Set the flag to know if the player is inside a building or not
    public void SetInsideBuilding(bool inside) {
        insideBuildings = inside;
        UpdateIntensity();
    }

    public void UpdateIntensity() {
        if(insideBuildings) { //Inside a building
            globalLight2D.intensity = minIntesity;
        } else { //Outside a building
            int hour = gameDateTimeHandler.GetHour();
            int min = gameDateTimeHandler.GetMinute();
            SetIntensity(hour, min); //Set new intensity by hour and minute
        }
    }

    private void SetIntensity(int hour, int min) {
        float intensity = globalLight2D.intensity;
        if(hour >= StartingSunRiseHour && hour <= (StartingSunRiseHour + ChangeDurationInHours) ) { //SunRise
            int currentMin = hour * 60 + min;
            float lerpValue = 1f * (currentMin - startSunRiseMin) / (finishSunRiseMin - startSunRiseMin);
            intensity = Mathf.Lerp(minIntesity, maxIntesity, lerpValue);
            dayNightFase = DayNightFase.SunRise;
        } else if(hour >= StartingSunSetHour && hour <= (StartingSunSetHour + ChangeDurationInHours) ) { //SunSet
            int currentMin = hour * 60 + min;
            float lerpValue = 1f * (currentMin - startSunSetMin) / (finishSunSetMin - startSunSetMin);
            intensity = Mathf.Lerp(maxIntesity, minIntesity, lerpValue);
            dayNightFase = DayNightFase.SunSet;
        } else if(hour > (StartingSunRiseHour + ChangeDurationInHours) && hour < StartingSunSetHour) { //Day
            dayNightFase = DayNightFase.Day;
            intensity = maxIntesity;
        } else if(hour > (StartingSunSetHour + ChangeDurationInHours) && hour < StartingSunRiseHour) { //Night
            dayNightFase = DayNightFase.Night;
            intensity = minIntesity;
        }
        globalLight2D.intensity = intensity;
    }

    public DayNightFase GetDayNightFase() {
        return dayNightFase;
    }
}

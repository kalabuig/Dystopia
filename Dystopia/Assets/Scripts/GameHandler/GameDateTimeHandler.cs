using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameDateTimeHandler : MonoBehaviour
{
    public event Action OnTimeChange;
    public event Action OnDateChange;

    private int _gameDateInDays = 0;
    public int gameDateInDays { get => _gameDateInDays; }
    private int _gameTimeInMinutes = 0;
    public int gameTimeInMinutes { get => _gameTimeInMinutes; }
    
    private bool timeOn = true;
    
    private int gameMinutesToAdd = 1; //minutes to add at each tick
    [SerializeField] private float realSecondsPerGameMinute = 1f; //seconds until next tick

    [Space]
    [SerializeField] [Range(0,5000)] private int initialYear = 2074;
    [SerializeField] [Range(1,12)] private int initialMonth = 12; //1..12
    [SerializeField] [Range(1,30)] private int initialDay = 30; //1..30
    [SerializeField] [Range(0,23)] private int initialHour = 23; //0..23
    [SerializeField] [Range(0,59)] private int initialMinute = 54; //0..59

    private void Start() {
        _gameDateInDays = initialYear * 360 + (initialMonth-1) * 30 + (initialDay-1);
        _gameTimeInMinutes = (initialHour * 60) + initialMinute;
        StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter() {
        while(timeOn) {
            yield return new WaitForSeconds(realSecondsPerGameMinute);
            _gameTimeInMinutes += gameMinutesToAdd; //add minutes
            NormalizeDateTime();
            if(OnTimeChange!=null) OnTimeChange(); //throw event to suscribers
            //Debug.Log(GetGameDateString() + " " + GetGameTimeString());
        }
    }

    //Normalize date and time
    private void NormalizeDateTime() {
        if(_gameTimeInMinutes >= 1440) { // 1 day = 1440 minutes
            _gameDateInDays++;
            _gameTimeInMinutes -= 1440;
            if(OnDateChange!=null) OnDateChange(); //throw event to suscribers
        }
    }

    public string GetGameDateString() {
        return TwoDigits(GetDay()) + "/" + TwoDigits(GetMonth()) + "/" + GetYear();
    }

    public string GetGameTimeString() {
        return TwoDigits(GetHour()) + ":" + TwoDigits(GetMinute());
    }

    public string TwoDigits(int v) {
        return v<10? "0" + v.ToString() : v.ToString();
    }

    public int GetDay() {
        return 1 + (_gameDateInDays % 30);
    }

    public int GetMonth() {
        return 1 + ((_gameDateInDays / 30) % 12);
    }

    public int GetYear() {
        return _gameDateInDays / 360;
    }

    public int GetHour() {
        return _gameTimeInMinutes / 60;
    }

    public int GetMinute() {
        return _gameTimeInMinutes % 60;;
    }

    private void OnDestroy() {
        timeOn=false;
        StopCoroutine(TimeCounter());
    }
}

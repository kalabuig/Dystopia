using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DateTimeDisplay : MonoBehaviour
{
    [SerializeField] private GameDateTimeHandler gameDateTimeHandler;
    private TextMeshProUGUI dateTimeText;

    private void Awake() {
        gameDateTimeHandler = GameObject.Find("GameHandler")?.GetComponent<GameDateTimeHandler>();
        dateTimeText = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        gameDateTimeHandler.OnTimeChange += OnTimeChanged;
        gameDateTimeHandler.OnDateChange += OnDateChanged;
    }

    private void OnDateChanged()
    {
        RefreshDateTimeText();
    }

    private void OnTimeChanged()
    {
        RefreshDateTimeText();
    }

    private void RefreshDateTimeText() {
        dateTimeText.text = gameDateTimeHandler.GetGameTimeString() + " " + gameDateTimeHandler.GetGameDateString();
    }

    private void OnDestroy() {
        gameDateTimeHandler.OnTimeChange -= OnTimeChanged;
        gameDateTimeHandler.OnDateChange -= OnDateChanged;
    }

}

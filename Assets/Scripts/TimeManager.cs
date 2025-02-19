using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private TextMeshProUGUI timeText;

    private LoadNightScene nightSceneScript;

    // So the minutes and seconds can be set outside of this script using TimeManager class
    public static int minutes, seconds;

    // Start is called before the first frame update
    void Start()
    {
        minutes = 0;
        seconds = 0;

        // Let's get the text mesh pro component that inside the time component itself
        timeText = GetComponent<TextMeshProUGUI>();

        nightSceneScript = GameObject.Find("sleepy_btn").GetComponent<LoadNightScene>();

        timeText.text = $"{minutes} : {seconds} ";
    }

    // Update is called once per frame
    void Update()
    {
        // If the time is less than minutes, print the extra 0 inside of the minutes variable in text
        if (minutes < 10)
        {
            // If the time is also less than 10 seconds, add a 0 beside the seconds variable in text
            if (seconds < 10)
            {
                timeText.text = $"0{minutes} : 0{seconds} ";
            }

            // Else if the time is also more than or equal to 10 seconds, remove the 0 beside the seconds variable in text
            else if (seconds >= 10)
            {
                timeText.text = $"0{minutes} : {seconds} ";
            }
        }

        // If the time exceeds 10 minutes, don't print the extra 0 inside of the minutes variable in text
        else if (minutes >= 10)
        {
            // If the time is also less than 10 seconds, add a 0 beside the seconds variable in text
            if (seconds < 10)
            {
                timeText.text = $"{minutes} : 0{seconds} ";
            }

            // Else if the time is also more than or equal to 10 seconds, remove the 0 beside the seconds variable in text
            else if (seconds >= 10)
            {
                timeText.text = $"{minutes} : {seconds} ";
            }
        }

        if (seconds >= 60)
        {
            minutes += 1;

            seconds = 0;
        }

        TransitionToNightEvent();
    }

    private void TransitionToNightEvent()
    {
        if (minutes >= 12 && seconds >= 0)
        {
            nightSceneScript.LoadNight();
        }
    }
}

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

    // So the hours and minutes can be set outside of this script using TimeManager class
    public static int hours, minutes;

    // Start is called before the first frame update
    void Start()
    {
        hours = 0;
        minutes = 0;

        // Let's get the text mesh pro component that inside the time component itself
        timeText = GetComponent<TextMeshProUGUI>();

        nightSceneScript = GameObject.Find("sleepy_btn").GetComponent<LoadNightScene>();

        timeText.text = $"{hours} : {minutes} ";
    }

    // Update is called once per frame
    void Update()
    {
        // If the time is less than hours, print the extra 0 inside of the hours variable in text
        if (hours < 10)
        {
            // If the time is also less than 10 minutes, add a 0 beside the minutes variable in text
            if (minutes < 10)
            {
                timeText.text = $"0{hours} : 0{minutes} ";
            }

            // Else if the time is also more than or equal to 10 minutes, remove the 0 beside the minutes variable in text
            else if (minutes >= 10)
            {
                timeText.text = $"0{hours} : {minutes} ";
            }
        }

        // If the time exceeds 10 hours, don't print the extra 0 inside of the hours variable in text
        else if (hours >= 10)
        {
            // If the time is also less than 10 minutes, add a 0 beside the minutes variable in text
            if (minutes < 10)
            {
                timeText.text = $"{hours} : 0{minutes} ";
            }

            // Else if the time is also more than or equal to 10 minutes, remove the 0 beside the minutes variable in text
            else if (minutes >= 10)
            {
                timeText.text = $"{hours} : {minutes} ";
            }
        }

        if (minutes >= 60)
        {
            hours += 1;

            minutes = minutes - 60;
        }

        TransitionToNightEvent();
    }

    private void TransitionToNightEvent()
    {
        if (hours >= 12 && minutes >= 0)
        {
            nightSceneScript.LoadNight();
        }
    }
}

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
    public GameState gameState;
    public CameraController player;
    public Vector3 roomPosition;
    public Vector3 outRoomPosition;
    public Material[] skyboxes;
    private int currentSkyboxIndex = 0;

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
        ChangeSkybox();

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

        if (hours == 50)
        {
            player.transform.position = outRoomPosition;
            hours = 0;
            minutes = 0;
        }

        TransitionToNightEvent();
    }

    private void ChangeSkybox()
    {
        int newSkyboxIndex = 0; // Default morning

        if (hours >= 2) newSkyboxIndex = 1;  // Late Morning
        if (hours >= 5) newSkyboxIndex = 2; // Afternoon
        if (hours >= 8) newSkyboxIndex = 3; // Evening
        if (hours >= 10) newSkyboxIndex = 4; // Night

        if (newSkyboxIndex != currentSkyboxIndex)
        {
            currentSkyboxIndex = newSkyboxIndex;
            ChangeSkyboxMat();
        }
    }

    private void ChangeSkyboxMat()
    {
        if (skyboxes.Length > currentSkyboxIndex)
        {
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
            DynamicGI.UpdateEnvironment();
        }
    }

    private void TransitionToNightEvent()
    {
        if (hours >= 12 && minutes >= 0)
        {
            if (gameState.currentDay == 0)
            {
                player.transform.position = roomPosition;
            }
            else
            {
                nightSceneScript.LoadNight();
            }
        }
    }
}

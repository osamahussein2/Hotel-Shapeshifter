using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

[System.Serializable]
public class Character : MonoBehaviour
{
    public string characterName; // Name of the character
    public Sprite characterImage; // Image of the character
    public Sprite deadCharImage;
    public bool charDead;
    public Sprite dialogueBox; // Image of the character
    public int trustLevel; // How much the character trusts the player
    public List<TimeLocation> schedule; // Location based on each time
    public Transform currentLocation; // Where the NPC currently is
    public SpriteRenderer sr;
    void Start()
    {
        UpdateLocation(); // Set initial location
    }

    void Update()
    {
        UpdateLocation(); // Update NPC location based on time
        if (charDead)
        {
            sr.sprite = deadCharImage;
        }
    }

    void UpdateLocation()
    {
        foreach (TimeLocation timeSlot in schedule)
        {
            if (IsCurrentTime(timeSlot))
            {
                if (currentLocation != timeSlot.location) // Only move if location changes
                {
                    currentLocation = timeSlot.location;
                    transform.position = timeSlot.location.position;
                }
                break;
            }
        }
    }

    bool IsCurrentTime(TimeLocation timeSlot)
    {
        int currentTime = TimeManager.hours * 60 + TimeManager.minutes;
        int startTime = timeSlot.startHour * 60 + timeSlot.startMinute;
        int endTime = timeSlot.endHour * 60 + timeSlot.endMinute;

        return currentTime >= startTime && currentTime < endTime;
    }
}


[System.Serializable]
public class TimeLocation
{
    public int startHour;
    public int startMinute;
    public int endHour;
    public int endMinute;
    public int day;
    public Transform location;
}
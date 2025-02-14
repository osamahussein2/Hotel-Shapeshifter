using System;
using UnityEngine;

[Serializable]
public class Quest
{
    public string questID; // identifier
    public string questName; // Display name for the quest
    public string description; // Description of what the quest entails
    public int questProgress; // Progress of the quest (0 to 100)
    public bool isActive; // Is the quest currently active?
    public bool isCompleted; // Has the quest been completed?
    public string itemReward; // What thingy will we get
    public int trustReward; // How much will they trust us more
}

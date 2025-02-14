using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestList
{
    public List<Quest> quests; // List of quests

    // Returns the quest with the given questID, or null if we ain't find nun
    public Quest GetQuest(string questID)
    {
        foreach (Quest q in quests)
        {
            if (q.questID == questID)
            {
                return q; // Return the matching quest
            }
        }
        return null; // Nope no matches found
    }

    public bool HasQuest(string questID)
    {
        return GetQuest(questID) != null; // We got a quest
    }

   
}

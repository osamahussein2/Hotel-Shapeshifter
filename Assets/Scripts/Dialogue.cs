using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    [TextArea(3, 9)]
    public string dialogueText;
    public List<Choice> choices; // List of choices for the player to do
    public int nextNodeIndex = -1; // Where the dialogue will progress along to (-1 means end)
    public List<string> clueWords; // Words in this node that are clickable clues
}

[Serializable]
public class Choice
{
    public string choiceText; // Text for the choice
    public int nextNodeIndex; // Where this choice leads

    public int trustGain; // How much trust this choice gives
    public int trustRequirement; // How much trust is needed to see this choice bam!
    public int timeIncrease;

    public bool oneUse;
    [NonSerialized] public bool hasBeenSelected = false;



    [System.Serializable]
    public struct SpecialDialogue
    {
        public bool sleepTime;
    }

    [System.Serializable]
    public struct QuestInfo
    {
        public string questID;
        public int questProgressGain;
        public int questRequirement;
    }
    public QuestInfo quest;
    public SpecialDialogue specialOptions;



}
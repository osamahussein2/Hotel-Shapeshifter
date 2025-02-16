using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalQuestLog : MonoBehaviour
{
    public DialogueManager dialogueManager;
    QuestList queList;

    List<GameObject> questTexts;

    void Start()
    {
        questTexts = new List<GameObject>();

        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            questTexts.Add(transform.GetChild(2).GetChild(i).gameObject);
        }
    }

    void Update()
    {
        UpdateQuestLog();
    }

    public void UpdateQuestLog()
    {
        //reset text
        for (int i = 0; i < questTexts.Count; i++)
        {
            questTexts[i].GetComponent<TextMeshProUGUI>().text = "";
            questTexts[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }

        int questSlotsFilled = 0;

        //add quest info
        queList = dialogueManager.questList;

        for (int i = 0; i < queList.quests.Count; i++)
        {
            if (queList.quests[i].isActive)
            {
                if (!queList.quests[i].isCompleted)
                {
                    questTexts[questSlotsFilled].GetComponent<TextMeshProUGUI>().text = queList.quests[i].questName;
                    questTexts[questSlotsFilled].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = queList.quests[i].description;

                    questSlotsFilled++;
                }
                else
                {

                }
            }
        }
    }
}

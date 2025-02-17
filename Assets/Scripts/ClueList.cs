using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueList : MonoBehaviour
{
    public GameObject cluePanel; // UI panel for the clue list
    public TMP_Text clueText; // Text element to display clues
    public GameObject questPanel; // UI panel for the clue list
    public TMP_Text questText; // Text element to display clues
    public GameObject itemPanel; // UI panel for the clue list
    public TMP_Text itemText; // Text element to display clues
    public GameObject buttonMaster; // UI panel for the clue list
    public GameObject journalMaster; // The head of all journal objects

    public Button showClueButton; // Button to show the clues

    public Button hideClueButton; // Button to hide the clues
    public Button changeClueButton; // Button to change the journal page
    public TMP_Text tempTimer;
    public GameState gameState;

    int currentJournalPage; //0 = clue, 1 = quest, 2 = items

    public List<string> collectedClues = new List<string>(); // List of collected clues
    public List<string> collectedQuests = new List<string>(); // List of collected quests
    public List<string> collectedItems = new List<string>(); // List of collected items

    void Start()
    {
        journalMaster.SetActive(false); // Start with the journal hidden
        questPanel.SetActive(false); // We don't want this page to start off
        itemPanel.SetActive(false); // We don't want this page to start off

        //hideClueButton.gameObject.SetActive(false);

        currentJournalPage = 0; //Setting it to the clues by default

    }

    private void Update()
    {
        tempTimer.text = "" + gameState.time;
    }

    // Add a new clue if it's not already collected
    public void AddClue(string clue, int type)
    {
        if (type == 0)
        {
            if (!collectedClues.Contains(clue))
            {
                collectedClues.Add(clue);
                UpdateClueUI();
            }
        }
        if (type == 1)
        {
            if (!collectedQuests.Contains(clue))
            {
                collectedQuests.Add(clue);
                UpdateQuestUI();
            }
        }
        if (type == 2)
        {
            if (!collectedItems.Contains(clue))
            {
                collectedItems.Add(clue);
                UpdateItemUI();
            }
        }
    }

    // Check if a clue is already in the list
    public bool HasClue(string clue, int type)
    {
        if (type == 0)
        {
            return collectedClues.Contains(clue);
        }
        if (type == 1)
        {
            return collectedQuests.Contains(clue);
        }
        if (type == 2)
        {
            return collectedItems.Contains(clue);
        }
        else
        {
            return true; //To fail the check
        }

    }

    // Update the clue UI
    void UpdateClueUI()
    {
        clueText.text = "Clues:\n";
        foreach (string clue in collectedClues)
        {
            clueText.text += $"- {clue}\n";
        }
    }
    void UpdateQuestUI()
    {
        questText.text = "Quests:\n";
        foreach (string quest in collectedQuests)
        {
            questText.text += $"- {quest}\n";
        }
    }
    void UpdateItemUI()
    {
        itemText.text = "Items:\n";
        foreach (string item in collectedItems)
        {
            itemText.text += $"- {item}\n";
        }
    }

    // Show the clue panel
    public void ShowCluePanel()
    {
        //hideClueButton.gameObject.SetActive(true);
        journalMaster.SetActive(true);
    }

    // Hide the clue panel
    public void HideCluePanel()
    {
        //hideClueButton.gameObject.SetActive(false);
        journalMaster.SetActive(false);
    }

    //We change which page is being shown
    public void ChangeCluePanel()
    {
        currentJournalPage += 1;
        if (currentJournalPage == 3)
        {
            currentJournalPage = 0;
        }
        Debug.Log("currentJournalPage is " + currentJournalPage);

        if (currentJournalPage == 0)
        {
            itemPanel.SetActive(false);
            cluePanel.SetActive(true);
        }
        if (currentJournalPage == 1)
        {
            cluePanel.SetActive(false);
            questPanel.SetActive(true);
        }
        if (currentJournalPage == 2)
        {
            questPanel.SetActive(false);
            itemPanel.SetActive(true);
        }
    }
}
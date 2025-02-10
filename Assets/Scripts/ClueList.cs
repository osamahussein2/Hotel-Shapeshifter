using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueList : MonoBehaviour
{
    public GameObject cluePanel; // UI panel for the clue list
    public TMP_Text clueText; // Text element to display clues
    public Button showClueButton; // Button to show the clues
    public Button hideClueButton; // Button to hide the clues
    public TMP_Text tempTimer;
    public GameState gameState;

    public List<string> collectedClues = new List<string>(); // List of collected clues

    void Start()
    {
        cluePanel.SetActive(false); // Start with the clue stuff hidden
        hideClueButton.gameObject.SetActive(false);

    }

    private void Update()
    {
        tempTimer.text = "" + gameState.time;
    }

    // Add a new clue if it's not already collected
    public void AddClue(string clue)
    {
        if (!collectedClues.Contains(clue))
        {
            collectedClues.Add(clue);
            UpdateClueUI();
        }
    }

    // Check if a clue is already in the list
    public bool HasClue(string clue)
    {
        return collectedClues.Contains(clue);
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

    // Show the clue panel
    public void ShowCluePanel()
    {
        hideClueButton.gameObject.SetActive(true);
        cluePanel.SetActive(true);
    }

    // Hide the clue panel
    public void HideCluePanel()
    {
        hideClueButton.gameObject.SetActive(false);
        cluePanel.SetActive(false);
    }
}
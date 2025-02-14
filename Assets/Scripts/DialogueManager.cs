using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // This is my code

    public GameObject dialogueUI; // Dialogue UI
    public TMP_Text charNameText; // Text for the character's name
    public TMP_Text dialogueText; // Text for the dialogue
    public Image charImage; // Image for the character
    public GameObject choiceContainer; // Container for choice buttons
    public GameObject choiceButtonPrefab; // Prefab for choice buttons
    public GameState gameState;

    public DialogueNode[] dialogueNodes; // All the dialogue nodes
    private int currentNodeIndex = 0; // Current node that we are currently being on

    public float typingSpeed = 0.05f; // text typing speeds
    private bool isTyping = false; // Are we typing text right now?

    private Character currentCharacter; // The character we're talking to

    public bool dialogueActive = false; // Is this dialogue active rn?

    public ClueList clueList;

    public QuestList questList;

    // No stealing

    void Start()
    {

        dialogueUI.SetActive(false); // Hide the dialogue UI at the start of the game
    }

    // Hands off buster

    public void StartDialogue()
    {
        dialogueActive = true; // Dialogue is now active Yippee!
        dialogueUI.SetActive(true); // Show the dialogue UI
        currentNodeIndex = 0; // Start at the first node
        ShowDialogueNode(currentNodeIndex); // Show the first node

    }

    // Better not be trying to get MY dialogue system

    void Update()
    {
        // If the player clicks and we're not showing choices or typing, go to the next node
        if (Input.GetMouseButtonDown(0) && !choiceContainer.activeSelf && !isTyping)
        {
            ProgressToNextNode();
        }
    }

    // I'm watching you

    void ShowDialogueNode(int nodeIndex)
    {
        // If the node index is invalid, we will end the dialogue
        if (nodeIndex < 0 || nodeIndex >= dialogueNodes.Length)
        {
            EndDialogue();
            return;
        }

        // Getting the current node
        DialogueNode currentNode = dialogueNodes[nodeIndex];

        // Setting the character's name and image
        charNameText.text = currentCharacter.characterName;
        charImage.sprite = currentCharacter.characterImage;

        // Process text and check for clue words
        ProcessClueWords(currentNode.dialogueText, currentNode, clueList);
        StartCoroutine(TypeText(currentNode.dialogueText));

        // Show the choices (if there are any)
        ShowChoices(currentNode.choices);
    }

    // Checks for clue words in dialogue
    string ProcessClueWords(string text, DialogueNode currentNode, ClueList clueList)
    {
        // Loop through the clue words in the current node
        foreach (string clue in currentNode.clueWords)
        {
            if (text.Contains(clue) && !clueList.HasClue(clue)) // Check if it's a new clue
            {
                clueList.AddClue(clue); // Add the clue to the clue list
            }
        }
        return text;
    }

    // This is my system and not yours

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the current text so we can get our awesome new fancy text

        // Type out each letter one by one
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false; // We are done typing
    }

    // uh uh

    void ProgressToNextNode()
    {
        // If we're typing, stop typing and show the full text
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueNodes[currentNodeIndex].dialogueText;
            isTyping = false;
            return;
        }

        // Current node time!
        DialogueNode currentNode = dialogueNodes[currentNodeIndex];

        // If there's no next node, end the dialogue
        if (currentNode.nextNodeIndex == -1)
        {
            EndDialogue();
            return;
        }

        // Go to the next node
        currentNodeIndex = currentNode.nextNodeIndex;
        ShowDialogueNode(currentNodeIndex);
    }

    // mine

    void ShowChoices(List<Choice> choices)
    {
        // Delete all old choice buttons they are unnecessary to our goals now...
        foreach (Transform child in choiceContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // If there are no choices, hide the choice container
        if (choices.Count == 0)
        {
            choiceContainer.SetActive(false);
            return;
        }

        // Show the choice container
        choiceContainer.SetActive(true);

        // Create a button for each of the choice
        foreach (Choice choice in choices)
        {
            // Only show the choice if the player meets trust and quest requirements
            bool meetsTrustRequirement = currentCharacter.trustLevel >= choice.trustRequirement;
            bool meetsQuestRequirement = true;

            if (!string.IsNullOrEmpty(choice.quest.questID) && choice.quest.questRequirement > 0)
            {
                Quest quest = questList.GetQuest(choice.quest.questID);
                if (quest == null || quest.questProgress < choice.quest.questRequirement)
                {
                    meetsQuestRequirement = false;
                }
            }

            if (meetsTrustRequirement && meetsQuestRequirement)
            {
                GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer.transform);
                choiceButton.GetComponentInChildren<TMP_Text>().text = choice.choiceText;
                choiceButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    StopAllCoroutines();
                    OnChoiceSelected(choice);
                });
            }
        }
    }

    // Property of ironpenguin222 on github

    void OnChoiceSelected(Choice choice)
    {
        // Increase the character's trust level
        currentCharacter.trustLevel += choice.trustGain;
        gameState.time += choice.timeIncrease;
        Debug.Log(gameState.time);

        // Increase quest progress
        if (!string.IsNullOrEmpty(choice.quest.questID))
        {
            Quest quest = questList.GetQuest(choice.quest.questID);
            if (quest != null)
            {
                if (!quest.isActive) quest.isActive = true;
                quest.questProgress += choice.quest.questProgressGain;
                if (quest.questProgress >= 100)
                {
                    quest.isCompleted = true;
                    quest.isActive = false;
                    currentCharacter.trustLevel += quest.trustReward;
                    Debug.Log("You got a " + quest.itemReward);
                }
            }
        }

        // Go to the next node
        currentNodeIndex = choice.nextNodeIndex;
        ShowDialogueNode(currentNodeIndex);
    }

    // Have fun with the code

    void EndDialogue()
    {
        dialogueActive = false; // Dialogue is no longer active
        dialogueUI.SetActive(false); // Hide the dialogue UI
    }

    // My code

    public void SetDialogueNodes(DialogueNode[] nodes)
    {
        dialogueNodes = nodes; // Set the dialogue nodes
        currentNodeIndex = 0; // Reset the current node
    }

    // Yuh

    public void SetCurrentCharacter(Character character)
    {
        currentCharacter = character; // Setting the current character
    }
}


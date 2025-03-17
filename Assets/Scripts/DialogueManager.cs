using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public Image dialogueBoxIMG; // Image for the box
    public GameObject choiceContainer; // Container for choice buttons
    public GameObject choiceButtonPrefab; // Prefab for choice buttons
    public GameState gameState;
    private bool canSkipText = false; // Prevents skipping immediately

    public DialogueNode[] dialogueNodes; // All the dialogue nodes
    private int currentNodeIndex = 0; // Current node that we are currently being on

    public float typingSpeed = 0.05f; // text typing speeds
    private bool isTyping = false; // Are we typing text right now?

    private Character currentCharacter; // The character we're talking to

    public bool dialogueActive = false; // Is this dialogue active rn?

    public ClueList clueList;

    public QuestList questList;

    public QuestPopUp popUp; // pop up when you get or finish a quest
    // No stealing

    public static bool isDialogueTriggered;

    void Start()
    {

        dialogueUI.SetActive(false); // Hide the dialogue UI at the start of the game

        isDialogueTriggered = false;
    }

    // Hands off buster

    public void StartDialogue()
    {
        dialogueActive = true; // Dialogue is now active Yippee!
        dialogueUI.SetActive(true); // Show the dialogue UI
        currentNodeIndex = 0; // Start at the first node
        canSkipText = false; // Prevent skipping instantly
        StartCoroutine(SkipBuffer());
        ShowDialogueNode(currentNodeIndex); // Show the first node

        isDialogueTriggered = true;

    }

    private IEnumerator SkipBuffer()
    {
        yield return new WaitForSeconds(0.1f); // Short delay to prevent instant skip
        canSkipText = true;
    }

    // Better not be trying to get MY dialogue system

    void Update()
    {

        if (Input.GetKeyDown("p"))
        {
            TelemetryLogger.Log(this, "Player doesn't appreciate what is happening right now");
        }

        // If the player clicks and we're not showing choices or typing, go to the next node
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping &&canSkipText)
            {
                isTyping = false; // Stop typing and display full text instantly
                dialogueText.text = dialogueNodes[currentNodeIndex].dialogueText;
            }
            else if (!choiceContainer.activeSelf) // Only progress if text is done typing
            {
                ProgressToNextNode();
            }
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
            if (text.Contains(clue) && !clueList.HasClue(clue, 0)) // Check if it's a new clue
            {
                TelemetryLogger.Log(this, "Player has obtained clue: " + clue);
                clueList.AddClue(clue, 0); // Add the clue to the clue list
            }
        }
        return text;
    }

    [System.Serializable]
    public struct SkipDialogueData
    {
        public string text;
        public Character character;
    }

    [System.Serializable]
    public struct DialogueChoiceData
    {
        public string choiceText;
        public Character character;
    }

    [System.Serializable]
    public struct StartQuestData
    {
        public string quest;
        public Character character;
    }

    [System.Serializable]
    public struct EndQuestData
    {
        public string quest;
        public Character character;
    }

    // This is my system and not yours

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the current text so we can get our awesome new fancy text
        string fullText = text;

        foreach (char letter in text)
        {
            if (!isTyping) // If interrupted, stop typing
            {
                dialogueText.text = fullText;
                var data = new SkipDialogueData()
                {
                    text = dialogueText.text,
                    character = currentCharacter
                };
                TelemetryLogger.Log(this, "Dialogue Skipped:", data);
                yield break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        canSkipText = true;
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
            bool meetsOneTimeRequirement = !(choice.oneUse && choice.hasBeenSelected);

            if (!string.IsNullOrEmpty(choice.quest.questID) && choice.quest.questRequirement > 0)
            {
                Quest quest = questList.GetQuest(choice.quest.questID);
                if (quest == null || quest.questProgress < choice.quest.questRequirement)
                {
                    meetsQuestRequirement = false;
                }
            }

            if (meetsTrustRequirement && meetsQuestRequirement && meetsOneTimeRequirement)
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

        if (choice.oneUse)
        {
            choice.hasBeenSelected = true;
        }

        // Increase the character's trust level
        currentCharacter.trustLevel += choice.trustGain;
        TimeManager.minutes += choice.timeIncrease;
        var data = new DialogueChoiceData()
        {
            choiceText = choice.choiceText,
            character = currentCharacter
        };
        TelemetryLogger.Log(this, "Option Selected", data);

        if (choice.specialOptions.sleepTime)
        {
            Debug.Log("time");
            gameState.currentDay += 1;
            TimeManager.hours = 50;
            gameState.dialogueProgress += 1;
        }
        // Increase quest progress
        if (!string.IsNullOrEmpty(choice.quest.questID))
        {
            Quest quest = questList.GetQuest(choice.quest.questID);
            if (quest != null)
            {
                if (!quest.isActive) 
                { 
                    quest.isActive = true; 
                    popUp.PopUp(quest.questName, "Quest Got");
                    var data1 = new StartQuestData()
                    {
                        quest = quest.questName,
                        character = currentCharacter
                    };
                    TelemetryLogger.Log(this, "Quest Started", data1);
                    for (int i = 0; i < quest.questItems.Count; i++)
                    {
                        quest.questItems[i].gameObject.SetActive(true);
                        quest.questItems[i].gameObject.GetComponent<QuestItem>().collected = false;
                    }
                }
                quest.questProgress += choice.quest.questProgressGain;
                if (quest.questProgress >= 100)
                {
                    quest.isCompleted = true;
                    quest.isActive = false;
                    var data2 = new EndQuestData()
                    {
                        quest = quest.questName,
                        character = currentCharacter
                    };
                    TelemetryLogger.Log(this, "Quest Completed", data2);
                    popUp.PopUp(quest.questName, "Quest Done");//quest pop up
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

        isDialogueTriggered = false;
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


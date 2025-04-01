using UnityEngine;
using System.Collections.Generic;
using static DialogueManager;
using UnityEngine.EventSystems;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to the DialogueManager
    public Character character; // The character we're talking to

    public List<DialogueSection> dialogueSections; // List of dialogue sections

    public GameState gameState; // Reference to the GameState script

    [System.Serializable]
    public struct CharacterInteractData
    {
        public Vector3 playerPos;
        public string character;
    }

    private void OnMouseDown()
    {
        // If dialogue isn't already active, start it

        if (!EventSystem.current.IsPointerOverGameObject()) // Ensures UI blocks game interaction
        {
            if (!dialogueManager.dialogueActive)
            {
                // Check if the current quest progress is within the bounds of the list
                if (gameState.dialogueProgress >= 0 && gameState.dialogueProgress < dialogueSections.Count)
                {
                    // Load the dialogue nodes for the current section
                    dialogueManager.SetDialogueNodes(dialogueSections[gameState.dialogueProgress].dialogueNodes);
                    var data = new CharacterInteractData()
                    {
                        playerPos = transform.position,
                        character = character.characterName
                    };
                    TelemetryLogger.Log(this, "Dialogue Started", data);
                }
                else
                {
                    Debug.Log("No dialogue section found for  progress: " + gameState.dialogueProgress);
                    return;
                }

                dialogueManager.SetCurrentCharacter(character); // Set the character
                dialogueManager.StartDialogue(); // Start the dialogue
            }
        }
    }
}
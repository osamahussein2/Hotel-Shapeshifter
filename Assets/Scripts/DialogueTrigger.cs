using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to the DialogueManager to get the things for the character
    public Character character; // The character we're talking to

    private void OnMouseDown()
    {
        // If dialogue isn't already active, start it right up
        if (!dialogueManager.dialogueActive)
        {
            dialogueManager.SetDialogueNodes(character.dialogueNodes); // Set the character's dialogue
            dialogueManager.SetCurrentCharacter(character); // Set the character
            dialogueManager.StartDialogue(); // Start the dialogue
        }
    }
}
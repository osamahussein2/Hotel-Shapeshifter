using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private void OnMouseDown()
    {
        if (!dialogueManager.dialogueActive)
        {
            dialogueManager.StartDialogue();
        }
    }
}
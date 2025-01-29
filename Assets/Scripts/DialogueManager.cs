using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TMP_Text dialogueText;
    public GameObject choiceContainer;
    public GameObject choiceButtonPrefab;
    public List<Button> choiceButtons;
    public DialogueNode[] dialogueNodes;
    private int currentNodeIndex = 0;
    public bool dialogueActive = false;

    void Start()
    {
        dialogueUI.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueActive = true;
        dialogueUI.SetActive(true);
        currentNodeIndex = 0;
        ShowDialogue();
    }

    void ShowDialogue()
    {
        DialogueNode currentNode = dialogueNodes[currentNodeIndex];
        dialogueText.text = currentNode.dialogueText;
        ShowChoices(currentNode.choices);
    }

    public void OnChoiceSelected(int nextNodeIndex)
    {
        if (nextNodeIndex == -1)
        {
            EndDialogue();
        }
        else
        {
            currentNodeIndex = nextNodeIndex;
            ShowDialogue();
        }
    }

    void ShowChoices(List<Choice> choices)
    {
        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < choices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choices[i].choiceText;

                int nextNodeIndex = choices[i].nextNodeIndex;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextNodeIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialogueUI.SetActive(false);
    }
}
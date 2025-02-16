using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public GameObject choicelessDialouge;

    public DialogueManager dialogueManager;

    public string questID; // What quest is this for?
    public string itemName; // Name of the quest item
    public string pickUpText; // What should be shown when this is interacted with
    public bool collected; // has this been collected yet?
    public bool disapearOnCollecting; // should this disapear when interacted with? (like if its something you need to pick up)

    private void OnMouseDown()
    {
        Interact();
    }

    public void Interact()
    {
        for(int i = 0; i < dialogueManager.questList.quests.Count; i++)
        {
            //check if this items quest is active 
            if(dialogueManager.questList.quests[i].questID == questID && dialogueManager.questList.quests[i].isActive)
            {
                if (collected)
                {
                    Debug.Log("You allready noted: " + itemName);

                }
                else
                {
                    Debug.Log("Picked up: " + itemName);
                    collected = true;

                    choicelessDialouge.SetActive(true);
                    choicelessDialouge.GetComponent<PopUpChoiceless>().SendText(pickUpText);

                    if (disapearOnCollecting) { gameObject.SetActive(false); }
                }
            }
        }
    }
}

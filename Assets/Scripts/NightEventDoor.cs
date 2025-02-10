using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NightEventDoor : MonoBehaviour
{
    //property of evan :)
    bool open;
    public bool interactable;
    Animator am;
    public GameObject dialougeUI;
    public GameObject choices;
    public GameObject questionChoices;
    NightManager nightMan;

    bool isTyping;
    public TMP_Text dialogueText;
    float typingSpeed = 0.05f;

    bool ignoreQued;
    bool letinQued;
    bool interacted;
    bool questioning;

    private void Start()
    {
        questioning = false;
        nightMan = GameObject.Find("Nightynight manager").GetComponent<NightManager>();
        interacted = false;
        choices.SetActive(false);
        dialougeUI.SetActive(false);
        am = GetComponent<Animator>();
        open = false;
        am.SetBool("Open", false);
    }

    private void Update()
    {
        if (ignoreQued && !isTyping)
        {
            dialougeUI.SetActive(false);

            nightMan.EndNight();
            ignoreQued = false;
        }
        if (questioning) { questionChoices.SetActive(true); } else { questionChoices.SetActive(false); }

        if (letinQued && !isTyping)
        {
            dialougeUI.SetActive(false);

            nightMan.LetInside();
        }
    }

    private void OnMouseDown()
    {
        if (interactable && !interacted)
        {
            interacted=true;
            StartDialouge();
            //if (open) { Close(); }
            //else { Open(); }
        }
    }

    void StartDialouge()
    {
        dialougeUI.SetActive(true);

        StartCoroutine(TypeText("Hello? Is anyone there?"));
    }

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
        choices.SetActive(true);
    }

    public void Respond()
    {
        questionChoices.SetActive(false);
        choices.SetActive(false);

        questioning = true;
    }

    public void Ignore()
    {
        ignoreQued = true;
        choices.SetActive(false);

        StartCoroutine(TypeText("After some time, The knocking stops.           "));

        if (!nightMan.isShapeshifter)
        {
            int oldTrust = PlayerPrefs.GetInt(nightMan.currentChar.characterName + "TRUST");
            PlayerPrefs.SetInt(nightMan.currentChar.characterName + "TRUST", oldTrust - 10);
        }
    }

    public void LetInside()
    {
        letinQued = true;

        StartCoroutine(TypeText("Thank you so much.           "));

        if (!nightMan.isShapeshifter)
        {
            int oldTrust = PlayerPrefs.GetInt(nightMan.currentChar.characterName + "TRUST");
            PlayerPrefs.SetInt(nightMan.currentChar.characterName + "TRUST", oldTrust + 20);
        }
    }

    public void Open()
    {
        if (!open)
        {
            open = true;
            am.SetBool("Open", true);
        }
    }

    public void Close()
    {
        if (open)
        {
            open = false;
            am.SetBool("Open", false);
        }
    }

    public void Knock()
    {
        if (!open)
        {
            am.SetTrigger("Knock");
        }
    }
}

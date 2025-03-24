using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    NightEventQuestions nightQues;

    List<string> unlockedQuestions;
    List<string> allQuestions;
    public List<int> chosenQuestions;

    bool isTyping;
    public TMP_Text dialogueText;
    float typingSpeed = 0.05f;

    public bool shuffleQuestions;
    public int trustLostPerQuestion;
    bool ignoreQued;
    bool letinQued;
    bool interacted;
    bool questioning;

    public Image characterSprite;
    public TextMeshProUGUI nameTxt;

    private void Start()
    {
        chosenQuestions = new List<int>();
        chosenQuestions.Add(0);
        chosenQuestions.Add(1);
        chosenQuestions.Add(2);
        questioning = false;
        nightMan = GameObject.Find("Nightynight manager").GetComponent<NightManager>();
        //nightQues = GameObject.Find("Nightynight manager").GetComponent<NightEventQuestions>();
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

        characterSprite.sprite = nightMan.currentChar.characterImage;
        nameTxt.text = nightMan.currentChar.characterName + "?";

        StopAllCoroutines();
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
        if (!questioning)
        {
            choices.SetActive(true);
        }
    }

    public void Respond()
    {
        questionChoices.SetActive(false);
        choices.SetActive(false);
        PickQuestions();

        questioning = true;
    }

    public void QuestionAsked(int question)
    {

        if (nightMan.isShapeshifter)
        {
            if(Random.Range(1, 3) == 1)
            {
                StopAllCoroutines();
                StartCoroutine(TypeText(nightMan.currentChar.GetComponent<NightAnswers>().answersNutral[chosenQuestions[question]]));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(TypeText(nightMan.currentChar.GetComponent<NightAnswers>().answersLie[chosenQuestions[question]]));
            }
        }
        else
        {
            if (Random.Range(1, 3) == 1)
            {
                StopAllCoroutines();
                StartCoroutine(TypeText(nightMan.currentChar.GetComponent<NightAnswers>().answersNutral[chosenQuestions[question]]));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(TypeText(nightMan.currentChar.GetComponent<NightAnswers>().answersHonest[chosenQuestions[question]]));
            }
        }

        if (!nightMan.isShapeshifter)
        {
            int oldTrust = PlayerPrefs.GetInt(nightMan.currentChar.characterName + "TRUST");
            PlayerPrefs.SetInt(nightMan.currentChar.characterName + "TRUST", oldTrust - trustLostPerQuestion);
        }

        if (shuffleQuestions) { PickQuestions(); }
    }

    public void PickQuestions()
    {
        nightQues = nightMan.currentChar.GetComponent<NightEventQuestions>();

        allQuestions = new List<string>();
        unlockedQuestions = new List<string>();

        foreach(string question in nightQues.questions)
        {
            allQuestions.Add(question);
            unlockedQuestions.Add(question);
        }

        for(int i = 0; i < nightQues.lockedQuestions.Count; i++)
        {
            if (nightMan.collectedClues.Contains(nightQues.lockedQuestionsClueRequirement[i]))
            {
                unlockedQuestions.Add(nightQues.lockedQuestions[i]);
            }
            allQuestions.Add(nightQues.lockedQuestions[i]);
        }

        List<int> unlockedIndexes = new List<int>();

        foreach(string question in unlockedQuestions)
        {
            unlockedIndexes.Add(allQuestions.IndexOf(question));
        }

        chosenQuestions[0] = Random.Range(0, unlockedIndexes.Count);
        chosenQuestions[1] = Random.Range(0, unlockedIndexes.Count);
        chosenQuestions[2] = Random.Range(0, unlockedIndexes.Count);

        //int attemptsMade = 0;

        //make sure all questions are unique
        while (chosenQuestions[2] == chosenQuestions[1] || chosenQuestions[2] == chosenQuestions[0] || chosenQuestions[0] == chosenQuestions[1])
        {
            chosenQuestions[0] = Random.Range(0, unlockedIndexes.Count);
            chosenQuestions[1] = Random.Range(0, unlockedIndexes.Count);
            chosenQuestions[2] = Random.Range(0, unlockedIndexes.Count);
        }

        for(int i = 0; i < questionChoices.GetComponent<NightEventQuestionButtons>().questions.Count; i++)
        {
            questionChoices.GetComponent<NightEventQuestionButtons>().questions[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = allQuestions[chosenQuestions[i]];
            Debug.Log("Question set as: " + allQuestions[chosenQuestions[i]]);
        }
    }

    public void Ignore()
    {
        ignoreQued = true;
        choices.SetActive(false);

        StopAllCoroutines();
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

        StopAllCoroutines();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpChoiceless : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    float skippableTimer;
    bool started;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!started && skippableTimer > 0) { started = true; }
        if (Input.GetMouseButtonDown(0) && started && skippableTimer <= 0f)
        {
            started = false;

            gameObject.SetActive(false);
        }
        skippableTimer -= Time.deltaTime;
    }

    public void SendText(string txt)
    {
        skippableTimer = 0.1f;
        started = true;
        dialogueText.text = txt;
    }
}

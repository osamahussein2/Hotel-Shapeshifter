using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPopUp : MonoBehaviour
{
    public RectTransform myPos;

    public Sprite qGot;
    public Sprite qDone;
    public Sprite cGot;

    public TextMeshProUGUI popText;
    public Image popBg;

    public float popUpSpeed;
    public float popUpDuration;
    float popUpTimer;
    float popStayTimer;
    float popOutTimer;

    public bool active;
    public AudioSource ding;
    bool dingPlayed = false;

    private void Update()
    {
        if (!ding.isPlaying && active && !dingPlayed)
        {
            ding.Play();
            dingPlayed = true;
        }
        if (!active && dingPlayed)
        {
            dingPlayed = false;
        }
        if (popUpTimer < popUpSpeed && active)
        {
            myPos.position = new Vector3(-400 + ((400 * popUpTimer)/popUpSpeed), myPos.position.y, 0);
            popUpTimer += Time.deltaTime;
            if(popUpTimer >= popUpSpeed)
            {
                myPos.position = new Vector3(0, myPos.position.y, 0);
                popStayTimer = popUpDuration;
            }
        }
        if (popStayTimer > 0 && active)
        {
            popStayTimer -= Time.deltaTime;
            if(popStayTimer <= 0)
            {
                popOutTimer = popUpSpeed;
            }
        }
        if (popOutTimer > 0 && active)
        {
            myPos.position = new Vector3(-400 + ((400 * popOutTimer) / popUpSpeed), myPos.position.y, 0);
            popOutTimer -= Time.deltaTime;
            if (popOutTimer <= 0)
            {
                myPos.position = new Vector3(-400, myPos.position.y, 0);
                active = false;
            }
        }
    }

    public void PopUp(string qName, float qProgress, string type)
    {
        if (!active)
        {
            active = true;
            popText.text = qName;

            if (type == "Quest Done")
            {
                popBg.sprite = qDone;
            }

            if (type == "Quest Progress")
            {
                popText.text = qName + "\nQuest Progress:" + qProgress;
            }

            else if (type == "Quest Got")
            {
                popBg.sprite = qGot;
            }
            else if (type == "Clue Got")
            {
                popBg.sprite = cGot;
            }

            popUpTimer = 0;
        }
    }
}

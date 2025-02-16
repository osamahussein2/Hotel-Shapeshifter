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

    public TextMeshProUGUI popText;
    public Image popBg;

    public float popUpSpeed;
    public float popUpDuration;
    float popUpTimer;
    float popStayTimer;
    float popOutTimer;

    public bool active;

    private void Update()
    {
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

    public void PopUp(string qName, bool completed)
    {
        if (!active)
        {
            active = true;
            popText.text = qName;

            if (completed)
            {
                popBg.sprite = qDone;
            }
            else
            {
                popBg.sprite = qGot;
            }

            popUpTimer = 0;
        }
    }
}

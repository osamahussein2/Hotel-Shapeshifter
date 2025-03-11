using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoGatherer : MonoBehaviour
{

    //public GameObject clueHolder;
    public List<GameObject> characters;
    public bool updating;
    public bool backFromNight;

    void Start()
    {
        
        if(PlayerPrefs.GetInt("BackFromNight") == 1)
        {
            backFromNight = true;
        }
        else
        {
            backFromNight= false;
        }

        string charName;
        int charTrust;
        if (backFromNight)
        {
            //if trust values were updated during night, apply the change
            for (int i = 0; i < characters.Count; i++)
            {
                charName = characters[i].name;
                if (PlayerPrefs.HasKey(charName + "TRUST"))
                {
                    //characters[i].GetComponent<DialogueTrigger>().character.trustLevel = PlayerPrefs.GetInt(charName + "TRUST");
                }
            }
            backFromNight = false;
            PlayerPrefs.SetInt("BackFromNight", 0);
        }
        else
        {
            //Store all trust values
            for (int i = 0; i < characters.Count; i++)
            {
                charName = characters[i].name;
                //charTrust = characters[i].GetComponent<DialogueTrigger>().character.trustLevel;

                ///PlayerPrefs.SetInt(charName + "TRUST", charTrust);
            }
        }
        
    }

    public void UpdateInfo()
    {
        updating = true;

        //update trust
        string charName;
        int charTrust;
        for(int i = 0; i < characters.Count; i++)
        {
            charName = characters[i].name;
            charTrust = characters[i].GetComponent<DialogueTrigger>().character.trustLevel;

            PlayerPrefs.SetInt(charName + "TRUST", charTrust);
        }

        updating = false;
    }
}

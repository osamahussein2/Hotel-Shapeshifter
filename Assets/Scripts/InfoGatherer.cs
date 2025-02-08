using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoGatherer : MonoBehaviour
{

    public GameObject clueHolder;
    public List<GameObject> characters;
    public bool updating;

    void Start()
    {
        
    }

    public void UpdateInfo()
    {
        updating = true;

        //update trust
        string charName;
        float charTrust;
        for(int i = 0; i < characters.Count; i++)
        {
            charName = characters[i].name;
            charTrust = characters[i].GetComponent<DialogueTrigger>().character.trustLevel;

            PlayerPrefs.SetFloat(charName + "TRUST", charTrust);
        }

        updating = false;
    }
}

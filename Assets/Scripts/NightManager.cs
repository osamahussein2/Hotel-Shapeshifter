using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    public List<Character> characters;

    public Character currentChar;

    public GameObject door;

    public float shapeshifterChance;
    bool isShapeshifter;

    float knockTimer;
    public float knockFreq;
    public int knockCount;
    int knocks;
    void Start()
    {
        PickCharacter();
        knockTimer = knockFreq;
    }

    void PickCharacter()
    {
        currentChar = characters[Random.Range(0, characters.Count / 2) * 2];
        // door.GetComponent<DialogueTrigger>().character = currentChar;

        if (Random.Range(1, 100) <= shapeshifterChance)
        {
            isShapeshifter = true;
        }
        else
        {
            isShapeshifter = false;
        }
    }


    void Update()
    {
        if (knocks < knockCount)
        {
            if (knockTimer <= 0)
            {
                door.GetComponent<NightEventDoor>().Knock();
                knockTimer = knockFreq;
                knocks++;
            }
            knockTimer -= Time.deltaTime;
        }
        
    }
}

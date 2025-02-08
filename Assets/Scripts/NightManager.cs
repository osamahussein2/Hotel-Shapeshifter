using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    public List<Character> characters;

    public Character currentChar;

    public GameObject door;
    public GameObject light;

    public AnimationCurve curve;
    public float spotlightRadias;
    float spotlightTimer;

    public float shapeshifterChance;
    bool isShapeshifter;

    float knockTimer;
    public float knockFreq;
    public int knockCount;
    int knocks;
    void Start()
    {
        //PickCharacter();

        light.GetComponent<Light>().spotAngle = 0f;

        spotlightTimer = 0f;
        knockTimer = knockFreq + 3f;
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
        if(spotlightTimer <= 1f)
        {
            spotlightTimer += Time.deltaTime / 3f;
            light.GetComponent<Light>().spotAngle = curve.Evaluate(spotlightTimer) * spotlightRadias;
        }
        

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

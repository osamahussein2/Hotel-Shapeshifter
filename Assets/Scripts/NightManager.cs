using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool isShapeshifter;

    float knockTimer;
    public float knockFreq;
    public int knockCount;
    int knocks;

    float endTimer;
    bool ending;

    void Start()
    {
        ending = false;

        PickCharacter();

        light.GetComponent<Light>().spotAngle = 0f;

        spotlightTimer = 0f;
        knockTimer = knockFreq + 3f;
    }

    void PickCharacter()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (PlayerPrefs.HasKey(characters[i].characterName + "TRUST"))
            {
                characters[i].trustLevel = PlayerPrefs.GetInt(characters[i].characterName + "TRUST");
            }
        }

        //for now just pick bob, uncomment the other line and remove this one to make it pick a random char instead
        currentChar = characters[5];

        //currentChar = characters[Random.Range(0, characters.Count)];

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
        //little intro light
        if(spotlightTimer <= 1f)
        {
            spotlightTimer += Time.deltaTime / 3f;
            light.GetComponent<Light>().spotAngle = curve.Evaluate(spotlightTimer) * spotlightRadias;
        }
        
        //little intro knocking
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
        else
        {
            door.GetComponent<NightEventDoor>().interactable = true;
        }

        if (ending)
        {
            endTimer -= Time.deltaTime;
            light.GetComponent<Light>().intensity = endTimer * 4f;
            if(endTimer <= -1f)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void EndNight()
    {
        ending = true;
        endTimer = 1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NightManager : MonoBehaviour
{
    public List<Character> characters;

    public List<Character> day1Chars;
    public List<Character> day2Chars;
    public List<Character> day3Chars;
    public List<Character> day4Chars;

    public List<GameObject> normModels;
    public List<GameObject> evilModels;

    public List<string> allClueNames;
    public List<string> collectedClues;

    public Character currentChar;

    public GameObject door;
    public GameObject doorLight;

    public GameObject normalChar;//character model
    public GameObject evilChar; //shapeshifter model

    public GameObject gameOver;

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

    float walkinTimer;
    bool walkin;

    public AudioSource jump;

    public int curDay;
    public bool daySpesificChars;

    void Start()
    {
        PlayerPrefs.SetInt("CHARKILLED", -1);

        ending = false;

        PickCharacter();

        doorLight.GetComponent<Light>().spotAngle = 0f;

        spotlightTimer = 0f;
        knockTimer = knockFreq + 3f;

        //load clue info
        foreach(string clue in allClueNames)
        {
            if(PlayerPrefs.GetString("CLUECOLLECTED"+clue) == "COLLECTED")
            {
                collectedClues.Add(clue);
            }
        }
        curDay = PlayerPrefs.GetInt("CURRENTDAY");
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
        if (daySpesificChars)
        {
            if(curDay == 1)
            {
                int rand = Random.Range(0, day1Chars.Count);
                currentChar = day1Chars[rand];
                normalChar = normModels[characters.IndexOf(day1Chars[rand])];
                evilChar = evilModels[characters.IndexOf(day1Chars[rand])];
            }
            else if (curDay == 2)
            {
                int rand = Random.Range(0, day2Chars.Count);
                currentChar = day2Chars[rand];
                normalChar = normModels[characters.IndexOf(day2Chars[rand])];
                evilChar = evilModels[characters.IndexOf(day2Chars[rand])];
            }
            else if (curDay == 3)
            {
                int rand = Random.Range(0, day3Chars.Count);
                currentChar = day3Chars[rand];
                normalChar = normModels[characters.IndexOf(day3Chars[rand])];
                evilChar = evilModels[characters.IndexOf(day3Chars[rand])];
            }
            else if (curDay == 4)
            {
                int rand = Random.Range(0, day4Chars.Count);
                currentChar = day4Chars[rand];
                normalChar = normModels[characters.IndexOf(day4Chars[rand])];
                evilChar = evilModels[characters.IndexOf(day4Chars[rand])];
            }
            else
            {
                Debug.LogError("Day was invalid");
            }
        }
        else
        {
            int rand = Random.Range(0, characters.Count);
            currentChar = characters[rand];
            normalChar = normModels[rand];
            evilChar = evilModels[rand];
        }
        

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
            doorLight.GetComponent<Light>().spotAngle = curve.Evaluate(spotlightTimer) * spotlightRadias;
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
            doorLight.GetComponent<Light>().intensity = endTimer * 4f;
            if(endTimer <= -1f)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        if (walkin)
        {

            walkinTimer -= Time.deltaTime;

            if(walkinTimer <= 0f)
            {
                if (isShapeshifter)
                {
                    jump.Play();
                    if (evilChar.transform.position.x < -6)
                    {
                        evilChar.transform.position = evilChar.transform.position + new Vector3(1f * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        
                        GameOver();
                    }
                }
                else
                {
                    if (normalChar.transform.position.x < -6)
                    {
                        normalChar.transform.position = normalChar.transform.position + new Vector3(1f * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        if (!ending) { EndNight(); }
                    }
                }
            }
            
        }
    }

    public void EndNight()
    {
        ending = true;
        endTimer = 1f;
    }

    public void LetInside()
    {
        if (!walkin)
        {
            door.GetComponent<Animator>().speed = 0.2f;
            door.GetComponent<NightEventDoor>().Open();

            walkin = true;
            walkinTimer = 1f;

            normalChar.transform.position = new Vector3(-10f, normalChar.transform.position.y, normalChar.transform.position.z);
            evilChar.transform.position = new Vector3(-10f, evilChar.transform.position.y, evilChar.transform.position.z);
        }
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoGatherer : MonoBehaviour
{

    //held info
    //clues
    [SerializeField] List<string> heldCollectedClues = new List<string>(); // List of collected clues
    [SerializeField] List<string> heldCollectedQuests = new List<string>(); // List of collected quests
    [SerializeField] List<string> heldCollectedItems = new List<string>(); // List of collected items
    //gameState
    [SerializeField] int dialogueProgress = 0; // Tracks progress of game

    public ClueList clueHolder;
    public List<GameObject> characters;
    List<bool> isCharAlive;
    public bool updating;
    public bool backFromNight;
    public int dayNumber;
    public GameState gameManager;

    public string killingChar;

    bool justLoaded;
    int framesTillLoad;

    private void Awake()
    {
        justLoaded = true;
        framesTillLoad = 1;
        if (PlayerPrefs.GetInt("BackFromNight") == 1)
        {
            backFromNight = true;
        }
        else
        {
            backFromNight = false;
        }

        Reassign();

        //UploadInfo();
    }

    private void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name != "NightEvent")
        {
            if (PlayerPrefs.GetInt("BackFromNight") == 1)
            {
                backFromNight = true;
                UploadInfo();
            }
            else
            {
                backFromNight = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt("BackFromNight", 1);
        }
    }

    public void Reassign()
    {
        Debug.Log("Reassigning referances");
        characters = new List<GameObject>();
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.name == "Characters")
            {
                gameManager = root.GetComponentInChildren<GameState>();

                for (int i = 0; i < root.transform.childCount; i++)
                {
                    if (root.transform.GetChild(i).gameObject.TryGetComponent<Character>(out Character charComp))
                    {
                        characters.Add(root.transform.GetChild(i).gameObject);
                    }
                }
            }
            if (root.name == "In-game UI") { clueHolder = root.GetComponentInChildren<ClueList>(); }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("BackFromNight", 0);
    }

    public void UploadInfo()
    {
        Reassign();

        Debug.Log("Uploading...");
        string charName;
        int charTrust;
        if (backFromNight)
        {
            Debug.Log("Back from night event");
            //check what day it is supposed to be
            gameManager.currentDay = PlayerPrefs.GetInt("CURRENTDAY");

            //if trust values were updated during night, apply the change
            for (int i = 0; i < characters.Count; i++)
            {
                charName = characters[i].name;
                if (PlayerPrefs.HasKey(charName + "TRUST"))
                {
                    characters[i].GetComponent<DialogueTrigger>().character.trustLevel = PlayerPrefs.GetInt(charName + "TRUST");
                }
            }
            backFromNight = false;
            PlayerPrefs.SetInt("BackFromNight", 0);

            //kill dead chars
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].GetComponent<Character>().charDead = !isCharAlive[i];
            }

            //check if a character died last night
            if (PlayerPrefs.GetInt("CHARKILLED") != -1)
            {
                characters[PlayerPrefs.GetInt("CHARKILLED")].GetComponent<Character>().charDead = true;

                PlayerPrefs.SetInt("CHARKILLED", -1);
            }

            //give held info
            clueHolder.collectedClues = heldCollectedClues;
            clueHolder.collectedQuests = heldCollectedQuests;
            clueHolder.collectedItems = heldCollectedItems;
            gameManager.dialogueProgress = dialogueProgress;
            clueHolder.UpdateAll();
        }
        else
        {
            //Store all trust values
            for (int i = 0; i < characters.Count; i++)
            {
                charName = characters[i].name;
                charTrust = characters[i].GetComponent<DialogueTrigger>().character.trustLevel;

                PlayerPrefs.SetInt(charName + "TRUST", charTrust);
            }
        }


    }

    public void UpdateInfo()
    {
        Reassign();

        Debug.Log("Updating...");
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

        List<string> collectedClues = clueHolder.collectedClues;
        List<string> allClues = new List<string>();

        //update clues
        foreach (GameObject clue in GameObject.FindGameObjectsWithTag("Clue"))
        {
            string clueName = clue.GetComponent<ClueObject>().objectName;
            allClues.Add(clueName);
            if (collectedClues.Contains(clueName))
            {
                PlayerPrefs.SetString("CLUECOLLECTED" + clueName, "COLLECTED");
            }
            else
            {
                PlayerPrefs.SetString("CLUECOLLECTED" + clueName, "NOTCOLLECTED");
            }
        }

        //update current day
        dayNumber = gameManager.currentDay;
        PlayerPrefs.SetInt("CURRENTDAY", dayNumber);

        //check if a character would kill you
        killingChar = "";
        foreach (GameObject charObj in characters)
        {
            if (charObj.GetComponent<Character>().trustLevel < 0)
            {
                killingChar = charObj.GetComponent<Character>().characterName;
            }
        }

        //save all the chars that died
        isCharAlive = new List<bool>();
        for(int i = 0; i < characters.Count; i++)
        {
            isCharAlive.Add(!characters[i].GetComponent<Character>().charDead);
        }

        //hold info
        heldCollectedClues = clueHolder.collectedClues;
        heldCollectedQuests = clueHolder.collectedQuests;
        heldCollectedItems = clueHolder.collectedItems;
        dialogueProgress = gameManager.dialogueProgress;

        updating = false;
    }
}

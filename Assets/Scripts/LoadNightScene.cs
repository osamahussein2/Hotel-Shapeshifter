using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadNightScene : MonoBehaviour
{
    GameObject infoGatherer;
    GameObject gameoverScreen;

    private void Start()
    {
        gameoverScreen = GameObject.Find("GAMEOVER");
        infoGatherer = GameObject.Find("Information Gatherer");
    }

    public void LoadNight()
    {
        infoGatherer.GetComponent<InfoGatherer>().UpdateInfo();
        //infoGatherer.GetComponent<InfoGatherer>().backFromNight = true;

        if(infoGatherer.GetComponent<InfoGatherer>().killingChar != "")
        {
            gameoverScreen.SetActive(true);
            gameoverScreen.GetComponentInChildren<TextMeshProUGUI>().text = "You were killed by " + infoGatherer.GetComponent<InfoGatherer>().killingChar + " in the night.";
        }
        else
        {
            PlayerPrefs.SetInt("BackFromNight", 1);

            SceneManager.LoadScene("NightEvent");
        }
    }
}

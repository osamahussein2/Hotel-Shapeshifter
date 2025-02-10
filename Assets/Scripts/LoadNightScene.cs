using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNightScene : MonoBehaviour
{
    GameObject infoGatherer;

    private void Start()
    {
        infoGatherer = GameObject.Find("Information Gatherer");
    }

    public void LoadNight()
    {
        infoGatherer.GetComponent<InfoGatherer>().UpdateInfo();

        SceneManager.LoadScene("NightEvent");
    }
}

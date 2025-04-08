using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerPrefs.GetInt("BackFromNight") == 1)
        {
            Debug.Log("Attempting to update...");
            GameObject.Find("Information Gatherer").GetComponent<InfoGatherer>().UpdateInfo();
        }
    }
}

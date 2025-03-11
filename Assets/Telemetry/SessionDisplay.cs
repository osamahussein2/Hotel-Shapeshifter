using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionDisplay : MonoBehaviour
{
    public void OnConnection(int sessionID)
    {
        TryGetComponent(out TextMeshProUGUI textDisplay);
        if(sessionID < 0)
        {
            textDisplay.text = "Offline... ID: " + sessionID;
        }
        else
        {
            textDisplay.text = "Connected with ID: " + sessionID;
        }
    }
    public void OnError(string error)
    {
        TryGetComponent(out TextMeshProUGUI textDisplay);
        textDisplay.text = "Failed: " + error;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionDisplay : MonoBehaviour
{
    public void OnConnectionSuccess(int sessionID)
    {
        TryGetComponent(out TextMeshProUGUI display);
        if (sessionID < 0)
        {
            display.text = $"Offline! Session #{sessionID}";
        }
        else
        {
            display.text = $"Connected! Session #{sessionID}";
        }
    }

    public void OnConnectionFail(string error)
    {
        TryGetComponent(out TextMeshProUGUI display);
        display.text = $"Failed! {error}";
    }

}

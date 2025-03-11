using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SessionDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConnectionSuccess(int sessionID)
    {
        var displayField = GetComponent<TextMeshProUGUI>();
        if (sessionID < 0)
        {
            displayField.text = $"Local Session {sessionID})";
        }
        else
        {
            displayField.text = $"Connected to Server (Session {sessionID})";
        }
    }
    public void OnConnectionFail(string errorMessage)
    {
        var displayField = GetComponent<TextMeshProUGUI>();
        displayField.text = $"Error: {errorMessage}";
    }
}

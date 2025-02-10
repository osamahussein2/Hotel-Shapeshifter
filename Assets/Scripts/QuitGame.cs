using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public void PressQuitButton()
    {
        // Quit the game after pressing the quit button
        Application.Quit();

        // Quit the game inside the editor too
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}

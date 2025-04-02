using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public Canvas inGame;
    public Canvas quitConfirmation;

    public static bool paused;

    private void Start()
    {
        inGame.gameObject.SetActive(true);
        quitConfirmation.gameObject.SetActive(false);
    }

    public void PressQuitButton()
    {
        inGame.gameObject.SetActive(false);
        quitConfirmation.gameObject.SetActive(true);

        paused = true;
    }

    public void PressYesButton()
    {
        // Quit the game after pressing the quit button
        Application.Quit();
    }

    public void PressNoButton()
    {
        inGame.gameObject.SetActive(true);
        quitConfirmation.gameObject.SetActive(false);

        paused = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public Canvas inGame;
    public Canvas restartConfirmation;

    public static bool paused;

    private void Start()
    {
        inGame.gameObject.SetActive(true);
        restartConfirmation.gameObject.SetActive(false);
    }

    public void PressRestartButton()
    {
        inGame.gameObject.SetActive(false);
        restartConfirmation.gameObject.SetActive(true);

        paused = true;
    }

    public void PressYesButton()
    {
        // Load from the beginning
        SceneManager.LoadScene(0);
    }

    public void PressNoButton()
    {
        inGame.gameObject.SetActive(true);
        restartConfirmation.gameObject.SetActive(false);

        paused = false;
    }
}

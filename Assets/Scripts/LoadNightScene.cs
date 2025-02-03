using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNightScene : MonoBehaviour
{
    public void LoadNight()
    {
        SceneManager.LoadScene("NightEvent");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private GameObject playingGame;
    private Canvas introCanvas;

    private TextMeshProUGUI gameIntroText;

    public float alphaSpeed;
    public float maxTimeForGameTitle;

    private float alpha;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // Find the intro game canvas and playing game
        introCanvas = GameObject.Find("GameIntroCanvas").GetComponent<Canvas>();
        playingGame = GameObject.Find("PlayingGame");

        // Find the game intro text
        gameIntroText = GameObject.Find("GameIntroCanvas/GameTitleText").GetComponent<TextMeshProUGUI>();

        // Set intro game canvas to true but playing game to false at start
        introCanvas.gameObject.SetActive(true);
        playingGame.SetActive(false);

        // Set alpha to 0 so the intro text won't be seen as soon as the game starts
        alpha = 0.0f;

        // Set the timer to 0 at start as well
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer overtime
        timer += Time.deltaTime;

        // Update the game intro text color based on its alpha value by frame
        gameIntroText.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        // Increase the alpha value until it hits the max time
        if (timer <= maxTimeForGameTitle)
        {
            alpha += Time.deltaTime * alphaSpeed;
        }

        // Decrease the alpha value after the timer exceeds the max time
        else if (timer > maxTimeForGameTitle)
        {
            alpha -= Time.deltaTime * alphaSpeed;

            // If alpha is at 0, make the playing game visible and hide the intro game canvas
            if (alpha <= 0.0f)
            {
                introCanvas.gameObject.SetActive(false);
                playingGame.SetActive(true);

                // Destroy the object too so it doesn't take up the game's resources
                Destroy(gameObject);
            }
        }
    }
}

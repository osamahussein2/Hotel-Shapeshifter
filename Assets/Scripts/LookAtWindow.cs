using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtWindow : MonoBehaviour
{
    public static bool playerCanLookOutside;
    private Vector3 lastCameraPosition;

    [SerializeField] private Vector3 windowOffset;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        playerCanLookOutside = false;

        lastCameraPosition = Camera.main.transform.position;

        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        LookOutsideHotel();
    }

    private void LookOutsideHotel()
    {
        /* Make sure the last camera position is always equal to the main camera's position if the player can't
        look outside yet */
        if (!playerCanLookOutside)
        {
            lastCameraPosition = Camera.main.transform.position;

            timer = 0.0f; // Don't increment the timer, make sure it's 0
        }

        /* If the player can look outside though, set the camera's position to the invisible window's position
        minus its offset so that the player can see they're looking through the window */
        else if (playerCanLookOutside)
        {
            /* Increment the timer so that the camera position can go back to the way it was once the player
            presses the left mouse button again */
            timer += Time.deltaTime;

            Camera.main.transform.position = transform.position - windowOffset;

            if (Input.GetMouseButtonDown(0) && timer >= 1.0f)
            {
                // Set the camera back to its last saved position before looking at the window
                Camera.main.transform.position = lastCameraPosition;

                // Set this bool false to make the camera go back to its last position
                playerCanLookOutside = false;
            }
        }
    }

    private void OnMouseDown()
    {
        // When the player clicks on the invisible window, make them look through the window
        if (!playerCanLookOutside)
        {
            playerCanLookOutside = true;
        }
    }
}

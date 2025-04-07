using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Transform hinge;
    public Transform door;
    public CameraController cameraController;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public Vector3 stopPosition;
    public Vector3 newAreaPosition;
    public float walkSpeed = 1f;
    public float walkDuration = 2f;
    public float bobbingHeight = 0.1f;
    public float walkStartDelay = 0.5f;
    public float closeSpeed = 2f;
    public float closeDelay = 1f;
    public bool canOpen = true;
    public GameState gameState;

    public AudioSource walking;
    public AudioSource doorSound;
    private bool soundplayed = false;

    private bool isOpening = false;
    private bool isClosing = false;
    private bool isWalking = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float walkTimer = 0f;

    public static bool doorInteracted;

    private void Start()
    {
        initialRotation = hinge.rotation;
        targetRotation = Quaternion.Euler(hinge.eulerAngles + new Vector3(0, openAngle, 0));

        doorInteracted = false;
    }

    void Update()
    {
        if (TimeManager.hours >= 12 || gameState.currentDay == 4)
        {
            canOpen = false;
        } else {
            canOpen = true;
                }
        if (isOpening)
        {
            if (!doorSound.isPlaying && soundplayed == false)
            {
                doorSound.Play();
                soundplayed = true;
            }
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * openSpeed);

            if (Quaternion.Angle(hinge.rotation, targetRotation) < 0.1f)
            {
                isOpening = false;
                StartCoroutine(Delay(walkStartDelay));
            }
        }

        if (doorInteracted)
        {
            cameraController.objectText.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (!isOpening && !isWalking && !isClosing && canOpen && !QuitGame.paused && 
            !DialogueManager.isDialogueTriggered)
        {
            isOpening = true;
            doorInteracted = true;
        }
    }

    private IEnumerator Delay(float delay)
    {
        if (doorSound.isPlaying)
        {
            doorSound.Stop();
        }
        yield return new WaitForSeconds(delay);
        StartCoroutine(Walking());
    }

    private IEnumerator Walking()
    {
        isWalking = true;
        walkTimer = 0f;

        Vector3 start = cameraController.transform.position;
        Vector3 end = stopPosition;
        walking.Play();
        while (walkTimer < walkDuration)
        {
            CameraController.teleporting = true;
            float bobbingOffset = Mathf.Sin(walkTimer * Mathf.PI * 2 * walkSpeed) * bobbingHeight;
            Vector3 newPos = Vector3.Lerp(start, end, walkTimer / walkDuration);
            newPos.y += bobbingOffset;
            cameraController.transform.position = newPos;

            walkTimer += Time.deltaTime;
            yield return null;
        }

        cameraController.transform.position = newAreaPosition;
        doorInteracted = false;
        if (walking.isPlaying)
        {
            walking.Stop();  // Stop the sound immediately
        }
        soundplayed = false;
        isWalking = false;
        CameraController.teleporting = false;

        yield return new WaitForSeconds(closeDelay);
        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor()
    {
        isClosing = true;

        while (Quaternion.Angle(hinge.rotation, initialRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, initialRotation, Time.deltaTime * closeSpeed);
            yield return null;
        }

        hinge.rotation = initialRotation;
        isClosing = false;
    }
}


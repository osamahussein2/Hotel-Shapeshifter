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

    private bool isOpening = false;
    private bool isWalking = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float walkTimer = 0f;

    private void Start()
    {
        initialRotation = door.rotation;
        targetRotation = Quaternion.Euler(hinge.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (isOpening)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * openSpeed);

            if (Quaternion.Angle(hinge.rotation, targetRotation) < 0.1f)
            {
                isOpening = false;
                StartCoroutine(Delay(walkStartDelay));
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("hi");
        if (!isOpening && !isWalking)
        {
            isOpening = true;
            

        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Walking());
    }

    private IEnumerator Walking()
    {
        isWalking = true;
        walkTimer = 0f;

        Vector3 start = cameraController.transform.position;
        Vector3 end = stopPosition;

        while (walkTimer < walkDuration)
        {
            float bobbingOffset = Mathf.Sin(walkTimer * Mathf.PI * 2 * walkSpeed) * bobbingHeight;
            Vector3 newPos = Vector3.Lerp(start, end, walkTimer / walkDuration);
            newPos.y += bobbingOffset;
            cameraController.transform.position = newPos;

            walkTimer += Time.deltaTime;
            yield return null;
        }

        cameraController.transform.position = newAreaPosition;
        isWalking = false;
    }
}

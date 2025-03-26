using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stopDistance = 2f;
    public LayerMask interactableLayer;
    public LayerMask nonoLayer;
    public LayerMask bonoLayer;
    public float closeEnoughThreshold = 0.1f;
    public float rotationSpeed = 10f;
    public float buttonRotationSpeed = 45f;
    public static bool teleporting;
    public AudioSource walking;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float fixedYPosition;
    private Quaternion targetRotation;
    private bool isRotatingWithButton = false;

    void Start()
    {
        fixedYPosition = transform.position.y;
        targetRotation = transform.rotation;
    }

    void Update()
    {

        if (teleporting)
        {
            isMoving = false;
            return;
        }
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) && !DialogueManager.isDialogueTriggered)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
                {
                    Vector3 directionToObject = (hit.point - transform.position).normalized;
                    directionToObject.y = 0;
                    Vector3 target = hit.point - directionToObject * stopDistance;

                    targetPosition = new Vector3(target.x, fixedYPosition, target.z);

                    if (Physics.Raycast(transform.position, directionToObject, out RaycastHit nonoHit, Vector3.Distance(transform.position, targetPosition), nonoLayer))
                    {
                        Debug.Log("Movement blocked by clue: " + nonoHit.collider.name);
                        return;
                    }
                    if (Physics.Raycast(transform.position, directionToObject, out RaycastHit bamHit, Vector3.Distance(transform.position, targetPosition), bonoLayer))
                    {
                        Debug.Log("Movement blocked by object: " + bamHit.collider.name);
                        return;
                    }
                    targetRotation = Quaternion.LookRotation(directionToObject);
                    isMoving = true;
                    walking.Play();
                    isRotatingWithButton = false;
                }
            }

        }

        if (isMoving)
        {
            MoveAndRotateCamera();
        }

        if (isRotatingWithButton)
        {
            SmoothRotate();
        }
    }

    Vector3 _moveVelocity;

    private void MoveAndRotateCamera()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget < closeEnoughThreshold)
        {
            isMoving = false;
            transform.position = targetPosition;
            if (walking.isPlaying)
            {
                walking.Stop();  // Stop the sound immediately
            }
            return;
        }

        //Vector3 moved = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        Vector3 moved = Vector3.SmoothDamp(transform.position, targetPosition, ref _moveVelocity, 0.15f, moveSpeed);
        transform.position = moved;
        
        SmoothRotate();
    }

    public void RotateRight()
    {
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, buttonRotationSpeed, 0));
        isRotatingWithButton = true;
    }

    public void RotateLeft()
    {
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, -buttonRotationSpeed, 0));
        isRotatingWithButton = true;
    }

    private void SmoothRotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            isRotatingWithButton = false;
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stopDistance = 2f;
    public LayerMask interactableLayer;
    public float closeEnoughThreshold = 0.1f;
    public float rotationSpeed = 10f;
    public float buttonRotationSpeed = 45f;
    public static bool teleporting;

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
                    targetRotation = Quaternion.LookRotation(directionToObject);
                    isMoving = true;
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

    private void MoveAndRotateCamera()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget < closeEnoughThreshold)
        {
            isMoving = false;
            transform.position = targetPosition;
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 step = direction * moveSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + step;
        transform.position = new Vector3(newPosition.x, fixedYPosition, newPosition.z);

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

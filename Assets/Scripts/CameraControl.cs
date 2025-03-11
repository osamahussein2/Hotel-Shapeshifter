using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stopDistance = 2f;
    public LayerMask interactableLayer;
    public float closeEnoughThreshold = 0.1f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float fixedYPosition;

    //"Compiler. plz generate code to convert shis object to/from a save file"
    [System.Serializable]
    public struct MoveEventData
    {
        public string movetype;
        public Vector3 movePosition;
        public Vector3 currentPosition;
    }

    void Start()
    {
        fixedYPosition = transform.position.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !DialogueManager.isDialogueTriggered)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                Vector3 directionToObject = (hit.collider.transform.position - transform.position).normalized;
                Vector3 target = hit.collider.transform.position - directionToObject * stopDistance;

                targetPosition = new Vector3(target.x, fixedYPosition, target.z);

                isMoving = true;

                var date = new MoveEventData()
                {
                    movetype = "Walking",
                    movePosition = targetPosition,
                    currentPosition = transform.position,
                };

                TelemetryLogger.Log(this, "Move", date); //Logging when the player moves.
            }
        }

        if (isMoving)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
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
    }

    public void RotateCamera()
    {
        transform.Rotate(0, 180, 0);
    }
}

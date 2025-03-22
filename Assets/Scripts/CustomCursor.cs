using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public LayerMask interactableLayer;
    public TextMeshProUGUI objectNameText;
    private Camera mainCamera;
    public RectTransform cursor;

    void Start()
    {
        objectNameText.gameObject.SetActive(false);
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        objectNameText.transform.position = mousePosition + new Vector2(1, -40);

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            objectNameText.text = hit.collider.gameObject.name;
            objectNameText.gameObject.SetActive(true);
        }
        else
        {
            objectNameText.gameObject.SetActive(false);
        }
    }
}

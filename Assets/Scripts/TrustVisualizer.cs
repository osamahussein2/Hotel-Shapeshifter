using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrustVisualizer : MonoBehaviour
{
    public GameObject textHolder;
    public TextMeshProUGUI trustText;
    DialogueTrigger diaTrig;

    // Start is called before the first frame update
    void Start()
    {
        textHolder.SetActive(false);

        diaTrig = transform.GetComponentInParent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        trustText.text = "Trust: " + diaTrig.character.trustLevel;
    }

    private void OnMouseEnter()
    {
        textHolder.SetActive(true);
    }

    private void OnMouseExit()
    {
        textHolder.SetActive(false);
    }
}

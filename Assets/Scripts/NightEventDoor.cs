using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEventDoor : MonoBehaviour
{
    //property of evan :)
    bool open;
    public bool interactable;
    Animator am;

    private void Start()
    {
        am = GetComponent<Animator>();
        open = false;
        am.SetBool("Open", false);
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (interactable)
        {
            if (open) { Close(); }
            else { Open(); }
        }
    }

    public void Open()
    {
        if (!open)
        {
            open = true;
            am.SetBool("Open", true);
        }
    }

    public void Close()
    {
        if (open)
        {
            open = false;
            am.SetBool("Open", false);
        }
    }

    public void Knock()
    {
        if (!open)
        {
            am.SetTrigger("Knock");
        }
    }
}

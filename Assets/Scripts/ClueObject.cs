using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueObject : MonoBehaviour
{
    public string objectName;
    public int objectNumber;
    public int objectType;
    public int objectState; //0=not found, 1=found, 2=found and looked at in journal

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Clue";
        objectState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (objectState == 1)
        {
            this.gameObject.SetActive(false);
        }
    }
}

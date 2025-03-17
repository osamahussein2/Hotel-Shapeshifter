using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldClue : MonoBehaviour
{
    public int ClueNumber;
    public LayerMask Clue;
    int[] WorldClues = new int[2];
    int[] Items = new int[2];

    public ClueList clueList;
    //public object type journal

    // Start is called before the first frame update
    void Start()
    {
        //List that states weather a clue has been, 0=not found, 1=found,
        WorldClues[0] = 0; //Wood in wall 
        WorldClues[1] = 0; //Painting 
        Items[0] = 0; //Wood in wall 
        Items[0] = 0; //Painting 
    }

    // Update is called once per frame
    void Update()
    {
        //When the player right clicks
        if (Input.GetMouseButtonDown(0))
        {
            //Send out a ray to see what they are clicking
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If it hits an object in the Clue Layermask
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Clue))
            {

                ClueObject objectScript = hit.collider.GetComponent<ClueObject>();
                if (objectScript.objectType == 0)
                {
                    if (WorldClues[objectScript.objectNumber] == 0)
                    {
                        //Updates that the object has now been found
    
                        Debug.Log("You found a clue");
                        //Play ding Sound effect
                            
                        //It can't be found twice
                        WorldClues[objectScript.objectNumber] = 1;
                        Debug.Log("I found a " + objectScript.objectName);
    
                        clueList.AddClue(objectScript.objectName, 0);
                    }
                }
                if (objectScript.objectType == 2)
                {
                    if (Items[objectScript.objectNumber] == 0)
                    {
                        //Updates that the object has now been found

                        Debug.Log("You found a clue");
                        //Play ding Sound effect

                        //It can't be found twice
                        Items[objectScript.objectNumber] = 1;
                        Debug.Log("I found a " + objectScript.objectName);

                        clueList.AddClue(objectScript.objectName, 2);
                        objectScript.objectState = 1;
                    }
                }
            }
        }
    }
}

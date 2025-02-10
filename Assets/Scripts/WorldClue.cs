using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldClue : MonoBehaviour
{
    public int ClueNumber;
    public LayerMask Clue;
    int[] ClueStates = new int[2];

    public ClueList clueList;
    //public object type journal

    // Start is called before the first frame update
    void Start()
    {
        //List that states weather a clue has been, 0=not found, 1=found, 2=found and looked at in journal
        ClueStates[0] = 0; //Wood in wall (ID = -4996)
        ClueStates[1] = 0; //Painting (ID = -10174)
    }

    // Update is called once per frame
    void Update()
    {
        //When the player right clicks
        if (Input.GetMouseButtonDown(1))
        {
            //Send out a ray to see what they are clicking
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //If it hits an object in the Clue Layermask
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Clue))
            {

                ClueObject objectScript = hit.collider.GetComponent<ClueObject>();
                if (ClueStates[objectScript.objectNumber] == 0)
                {
                    //Updates that the object has now been found

                    Debug.Log("You found a clue");
                    //Play ding Sound effect

                    //It can't be found twice
                    ClueStates[objectScript.objectNumber] = 1;
                    Debug.Log("I found a " + objectScript.objectName);

                    clueList.AddClue(objectScript.objectName);
                }
                //Looks for the ID of the object
                if (hit.colliderInstanceID == -4996)
                {
                    //Updates that the object has now been found

                    //Play ding Sound effect
                    ClueStates[0] = 1;

                    Debug.Log("You found a clue");
                    //Update journal here
                }

                if (hit.colliderInstanceID == -10174)
                {
                    //Play ding Sound effect
                    ClueStates[1] = 1;

                    Debug.Log("You found a clue");
                    //Update journal here
                }
            }
        }
    }
}

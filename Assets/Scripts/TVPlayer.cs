using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVPlayer : MonoBehaviour
{
    public GameState State;
    public GameObject TV;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(State.currentDay == 2)
        {
            NewsBroadcast();
        }
        else
        {
            TV.SetActive(false);
        }
    }

    public void NewsBroadcast()
    {
        TV.SetActive(true);
    }



}

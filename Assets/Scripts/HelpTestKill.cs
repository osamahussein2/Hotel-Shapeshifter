using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTestKill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //When k is pressed we take this object out back and tell it to look at the flowers
        if (Input.GetKeyDown("k"))
        {
            Destroy(gameObject);
        }
    }
}

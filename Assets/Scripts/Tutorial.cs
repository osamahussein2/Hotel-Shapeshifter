using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TMP_Text tutText;
    public int currentText;


    // Start is called before the first frame update
    void Start()
    {
        tutText.text = "Welcome sir/ma'am. Could you come over?<br>Just click the ground to move";
        if (currentText != 9)
        {
            currentText = 1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentText == 7)
        {
            tutText.text = tutText.text = "";
            currentText = 9;
            gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0) && currentText == 6)
        {
            tutText.text = tutText.text = "One more thing to note. <br>People who don't trust you are dangerous. <br>Try to not let anyone's trust to 0. <br>If you understand press 'space'";
            currentText = 7;
        }
        if (Input.GetMouseButtonDown(0) && currentText == 5)
        {
            tutText.text = tutText.text = "Well good luck with all that.";
            currentText = 6;
        }
        if (Input.GetMouseButtonDown(0) && currentText == 4)
        {
            tutText.text = "You should get on that. <br>You can click on doors to travel to different rooms";
            currentText = 5;
        }
        if (Input.GetMouseButtonDown(0) && currentText == 3)
        {
            tutText.text = tutText.text = "";
        }
        if (Input.GetMouseButtonDown(0) && currentText == 2)
        {
            tutText.text = "Just click on that man over there to talk to him";
            currentText = 3;
        }
        if (Input.GetMouseButtonDown(0) && currentText == 1)
        {
            tutText.text = "You can also use the turn buttons <br> to rotate your character";
            currentText = 2;
        }
    }
}

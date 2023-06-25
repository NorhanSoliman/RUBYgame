using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f; //how long the dialogue box is displayed
    public GameObject dialogBox; //store the canvas of the dialogbox
    float timerDisplay; //how long to display the dialogue

    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime; //decrement time for each frame
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false); //diable the canvas when time decrements below zero
            }
        }
    }

    public void DisplayDialog() //Ruby will call on interaction with the frog
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true); //enable the canvas
    }
}

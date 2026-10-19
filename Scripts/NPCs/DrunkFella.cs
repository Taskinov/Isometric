using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkFella : MonoBehaviour
{
    private Animator anim;
    private string currentState;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<DialogueBase>().startDialogue)
        {
            if (currentState != "wakeup")
            {
                anim.Play("wakeup");
            }
            currentState = "wakeup";
        } else
        {
            if (currentState != "sleep")
            {
                anim.Play("sleep");
            }
            currentState = "sleep";
        }
    }
}

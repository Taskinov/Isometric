using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public GameObject canvas;
    
    private void Start()
    {
        canvas = GameObject.Find("Canvas");
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            canvas.GetComponent<HideUI>().TouchAction();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            canvas.GetComponent<HideUI>().HitAction();
            
        }
    }
}

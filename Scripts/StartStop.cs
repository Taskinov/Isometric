using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStop : MonoBehaviour
{
    [SerializeField] private GameObject car;
    private float numTriggers = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Transparent"))
        {
            if (numTriggers == 0)
            {
                car.GetComponent<CarDriving>().carSpeed = 0f;
            }
            numTriggers++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Transparent"))
        {
            if (numTriggers == 1)
            {
                car.GetComponent<CarDriving>().carSpeed = 2f;
            }
            numTriggers--;
        }
    }
    
}

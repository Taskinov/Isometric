using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroads : MonoBehaviour
{
    [SerializeField] private GameObject[] stopLines;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ChangeLights", 1f, 30f);
    }

    // Update is called once per frame
    void ChangeLights()
    {
        for (int i = 0; i < stopLines.Length; i++)
        {
            stopLines[i].GetComponent<BoxCollider2D>().enabled = (stopLines[i].GetComponent<BoxCollider2D>().enabled) ? false : true;
        }
    }
}

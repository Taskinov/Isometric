using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRestriction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x < -80) || (transform.position.x > 80))
        {
            transform.position = new Vector3(-transform.position.x * 0.99f, transform.position.y * 0.99f, transform.position.z);
        }
        if ((transform.position.y < -48) || (transform.position.y > 48))
        {
            transform.position = new Vector3(transform.position.x * 0.99f, -transform.position.y * 0.99f, transform.position.z);
        }
    }
}

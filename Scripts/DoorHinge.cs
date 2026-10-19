using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorHinge : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPushed = false;
    private float side = 1;
    [SerializeField] GameObject DoorEdge;
    public float rotation = 1;

    void OnTriggerStay2D(Collider2D collision)
    {
        if ((!collision.gameObject.CompareTag("static") && !collision.gameObject.CompareTag("Transparent")) && (Math.Abs(collision.gameObject.GetComponent<Movement_test>().dirY) + Math.Abs(collision.gameObject.GetComponent<Movement_test>().dirX) > 0.01))
        {
            if ((transform.eulerAngles.z < 90) || (transform.eulerAngles.z > 270))
            {
                transform.Rotate(0, 0, rotation * side * 360 * 0.5f * Time.deltaTime);
            }
        } 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("static") && !collision.gameObject.CompareTag("Transparent"))
        {
            isPushed = true;
            if (collision.transform.position.y > DoorEdge.transform.position.y)
            {
                side = -1;
            } else
            {
                side = 1;
            }
        }
        
    }

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("static") && !collision.gameObject.CompareTag("Transparent"))
        {
            isPushed = false;
        }
    }

    void Update()
    {
        if ((!isPushed) && (transform.eulerAngles.z < 95f))
        {
            transform.eulerAngles = new Vector3(60, 0, Mathf.MoveTowards(transform.eulerAngles.z, 0f, 32f * Time.deltaTime));
        } else if ((!isPushed) && (transform.eulerAngles.z > 265f))
        {
            transform.eulerAngles = new Vector3(60, 0, Mathf.MoveTowards(transform.eulerAngles.z, 360f, 32f * Time.deltaTime));
        }
    }
}

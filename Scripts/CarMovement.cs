using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CarMovement : MonoBehaviour
{
    private Rigidbody2D rbc;
    private float velStep = 0.9f;
    private float carStopPos = 5;
    [SerializeField] private AudioSource stopSoundEffect;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    private double yVel = 0;
    private double xVel = 0;
    private double xDif = 0;
    private double yDif = 0;
    private double h = 1;
    private float numTriggers = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rbc = GetComponent<Rigidbody2D>();
        xDif = end.transform.position.x - transform.position.x;
        yDif = end.transform.position.y - transform.position.y;
        h = Math.Sqrt(yDif*yDif + xDif*xDif);
        xVel = xDif / h * 8;
        yVel = yDif / h * 8;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(end.transform.position, transform.position) < 5f)
        {
            transform.position = new Vector2(start.transform.position.x, start.transform.position.y);
        }
        if (velStep < 1f)
        {
            rbc.velocity = new Vector2((float)xVel * velStep, (float)yVel * velStep);
            if (((transform.position.x - carStopPos) > 1) || ((transform.position.x - carStopPos) < -1))
            {
                velStep = velStep + 0.1f;
                carStopPos = transform.position.x;
            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!collision.gameObject.CompareTag("Transparent"))
        {
            if (numTriggers == 0)
            {
                rbc.velocity = new Vector2(0, 0);
                GetComponent<Animator>().enabled = false;
                //stopSoundEffect.Play();
                carStopPos = transform.position.x;
                velStep = 1f;
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
                velStep = 0.3f;
                GetComponent<Animator>().enabled = true;
            }
            numTriggers--;
        }
    }
}

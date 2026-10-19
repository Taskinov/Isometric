using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class INtro : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject cam;
    private int currentWaypointIndex = 0;
    private Animator anim;
    private Rigidbody2D rb;
    private double yVel = 0;
    private double xVel = 0;
    private double xDif = 0;
    private double yDif = 0;
    private double h = 1;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 1f) //points switch
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                rb.velocity = new Vector2(0, 0);
                anim.enabled = false;
                cam.GetComponent<CameraFollowing>().i = 1;
                GetComponent<INtro>().enabled = false;
            } 
        }
        if (currentWaypointIndex < waypoints.Length)
        {
            xDif = waypoints[currentWaypointIndex].transform.position.x - transform.position.x;
            yDif = waypoints[currentWaypointIndex].transform.position.y - transform.position.y;
            h = Math.Sqrt(yDif*yDif + xDif*xDif);
            xVel = xDif / h * 8;
            yVel = yDif / h * 8;
            rb.velocity = new Vector2((float)xVel, (float)yVel);
        }
    }
}

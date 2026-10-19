using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private Rigidbody2D rb;
    public Vector2 vel; //debug
    private GameObject leftTrigger;
    private GameObject rightTrigger;
    private GameObject straightTrigger;
    public double currentAngle = 0;
    private double xDif = 0;
    private double yDif = 0;
    private double yVel = 0;
    private double xVel = 0;
    private double h = 0;
    private float[] xColl = {-1.5f,-1f,-1f,0f,0f,1f,1f,1.5f,1.5f,1f,1f,0f,0f,-1f,-1f,-1.5f};
    private float[] yColl = {0f,-0.7f,-0.7f,-1f,-1f,-0.7f,-0.7f,0f,0f,0.7f,0.7f,1f,1f,0.7f,0.7f,0f};
    private int direction;
    private int currentDirection;


    [SerializeField] private float speed = 6f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftTrigger = transform.Find("Left").gameObject;
        rightTrigger = transform.Find("Right").gameObject;
        straightTrigger = transform.Find("Straight").gameObject;
    }

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        xDif = waypoints[currentWaypointIndex].transform.position.x - transform.position.x;
        yDif = waypoints[currentWaypointIndex].transform.position.y - transform.position.y;
        h = Math.Sqrt(yDif*yDif + xDif*xDif);
        xVel = xDif / h;
        yVel = yDif / h;
        currentAngle = Math.Atan2(yVel, xVel);
        direction = (int)((currentAngle + 3.142d) * 2.412);
        if (direction != currentDirection)
        {
            if ((direction >= 15) || (direction <= 0))
            {
                straightTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[15], yColl[15]);
                leftTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[1], yColl[1]);
                rightTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[14], yColl[14]);
            } else
            {
                straightTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[direction], yColl[direction]);
                if ((direction + 2) > 15)
                {
                    leftTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[15], yColl[15]);
                }
                else
                {
                    leftTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[direction + 2], yColl[direction + 2]);
                }
                if ((direction - 2) < 0)
                {
                    rightTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[0], yColl[0]);
                }
                else
                {
                    rightTrigger.GetComponent<CapsuleCollider2D>().offset = new Vector2(xColl[direction - 2], yColl[direction - 2]);
                }
                
            }
            currentDirection = direction;
        }

        if (straightTrigger.GetComponent<NPCStraightTrigger>().straight)
        {
            rb.velocity = new Vector2((float)xVel * speed, (float)yVel * speed);
        } else if (rightTrigger.GetComponent<NPCRightTrigger>().right)
        {
            rb.velocity = new Vector2(rightTrigger.GetComponent<CapsuleCollider2D>().offset.x * speed,rightTrigger.GetComponent<CapsuleCollider2D>().offset.y * speed);

        } else if (leftTrigger.GetComponent<NPCLeftTrigger>().left)
        {
            rb.velocity = new Vector2(leftTrigger.GetComponent<CapsuleCollider2D>().offset.x * speed,leftTrigger.GetComponent<CapsuleCollider2D>().offset.y * speed);

        }
        vel = new Vector2(rb.velocity[0],rb.velocity[1]);
    }

    private void OnCollisionExit2D()
    {
        rb.velocity = new Vector2(0,0);
    }
}

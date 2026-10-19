using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarDriving : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private double yVel = 0;
    private double xVel = 0;
    private double xDif = 0;
    private double yDif = 0;
    private double h = 0;
    public double currentAngle = 0;
    public Vector2 carDirection;
    public float carSpeed = 2;
    public float rotationSpeed = 0.2f;
    public Rigidbody2D rb;
    public Animator anim;
    public float angle;
    public float angleTest;
    private double rotateAngle;
    public int angleLight;
    [SerializeField] private GameObject direction;
    [SerializeField] private GameObject directionRotate;
    [SerializeField] private GameObject lightsRotate;
    [SerializeField] private GameObject waypoint;
    [SerializeField] private GameObject waypointRotate;
    private string[] directionArray = {"1","2","2","3","3","4","4","5","5","5","5","6","6","7","7","8",
    "8","9","9","10","10","11","11","12","12","13","13","13","13","14","14","15","15","16","16","17"};
    private int[] lightArray = {-90,-80,-80,-70,-70,-60,-60,-45,-45,-45,-45,-30,-30,-20,-20,-10,
    -10,0,0,10,10,20,20,30,30,45,45,45,45,60,60,70,70,80,80,90};

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = 0.2f;
    }

    void Update()
    {
        
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 1f) //points switch
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
        currentAngle = Math.Atan2(yVel, xVel); // from -3.14 to 3.14, 0 on right
        angleTest = waypointRotate.transform.eulerAngles.z;
        if (angleTest > 180)
        {
            angleTest = angleTest - 360;
        }
        rotateAngle = (180 / Math.PI) * currentAngle - angleTest;
        if (Math.Abs(rotateAngle) > 1) //rotation of direction point in main plane
        {
            if (((rotateAngle) > 180) || (((rotateAngle) < 0) && ((rotateAngle) > -180)))
            {
                waypointRotate.transform.Rotate(0, 0, 360 * -rotationSpeed * Time.deltaTime * carSpeed); // from -180 to 180, 0 up
            } else
            {
                waypointRotate.transform.Rotate(0, 0, 360 * rotationSpeed * Time.deltaTime * carSpeed); // from -180 to 180, 0 up
            }
        }
        directionRotate.transform.eulerAngles = new Vector3(60, 0, (float)(waypointRotate.transform.eulerAngles.z 
            - 17 * Math.Cos((Math.PI / 90) * waypointRotate.transform.eulerAngles.z + (Math.PI / 2)))); 

        carDirection = new Vector2(direction.transform.position.x - directionRotate.transform.position.x, 
            direction.transform.position.y - directionRotate.transform.position.y);

        rb.velocity = new Vector2(carSpeed * carDirection[0], carSpeed * carDirection[1]);
        
        angle = Math.Abs(-directionRotate.transform.eulerAngles.z + 90); // from 0 to 180, 0 up
        if (angle > 180)
        {
            angle = 360 - angle;
        }
        anim.Play(directionArray[(int)(((180 - angle) / 180) * 36)]);
        angleLight = lightArray[(int)(((180 - angle) / 180) * 36)];
        

        if (carDirection[0] < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            lightsRotate.transform.eulerAngles = new Vector3(60, 0, Math.Sign(angleLight)*180 - angleLight);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            lightsRotate.transform.eulerAngles = new Vector3(60, 0, angleLight);
        }
    }
}

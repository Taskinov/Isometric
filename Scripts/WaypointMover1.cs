using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaypointMover1 : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private Rigidbody2D rb;
    private Animator anim;
    private int currentWaypointIndex = 0;
    //private int currentPathPointIndex = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    public Vector3[] pathPoints;
    public double currentAngle = 0;
    public double xDif = 0;
    public double yDif = 0;
    public double yVel = 0;
    public double xVel = 0;
    public double h = 0;
    private int direction;
    [SerializeField] private GameObject enemy;
    //[SerializeField] private float speed = 6f;
    //public NavMeshSurface Surface2D;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        anim = GetComponent<Animator>();
        // rb = GetComponent<Rigidbody2D>();
        // InvokeRepeating("SetDestinationPoint", 1f, 1f);
    }

    private void Update()
    {
        //Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 10f)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        if (anim.GetBool("Damage"))
        {
            agent.SetDestination(enemy.transform.position);
        } else if (waypoints[currentWaypointIndex] != null)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].transform.position);
        }

        // currentPathPointIndex = 0;
        // if (agent.hasPath)
        // {
        //     pathPoints = agent.path.corners;
        //     if (pathPoints.Length != 0)
        //     {
        //         if (Vector2.Distance(pathPoints[currentPathPointIndex], transform.position) < .1f)
        //         {
        //             currentPathPointIndex++;
        //             if (currentPathPointIndex >= pathPoints.Length)
        //             {
        //                 currentPathPointIndex = 0;
        //             }
        //         }
        //         xDif = pathPoints[currentPathPointIndex].x - transform.position.x;
        //         yDif = pathPoints[currentPathPointIndex].y - transform.position.y;
        //         h = Math.Sqrt(yDif*yDif + xDif*xDif);
        //         xVel = xDif / h;
        //         yVel = yDif / h;
        //         currentAngle = Math.Atan2(yVel, xVel);
        //         direction = (int)((currentAngle + 3.142d) * 2.412);
        //         rb.velocity = new Vector2((float)xVel * speed, (float)yVel * speed);
        //     }
        //}
    }

    // private void SetDestinationPoint()
    // {
    //     agent.SetDestination(waypoints[currentWaypointIndex].transform.position);
    // }
}

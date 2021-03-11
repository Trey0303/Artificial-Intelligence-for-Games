using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolController : MonoBehaviour
{
    public Agent agent;//assigns an object
    public float speed = 3.0f;//assigns speed

    //allows you to set pointers for the object to follow
    public Transform []waypoints;
    private int currentWaypointIndex = 0;//keeps track of current waypoint

    public float reachedThreshold = 0.5f;

    //amount of time spent waiting at each waypoint
    public float waitInterval = 2.0f;
    private float waitTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        waitTimer -= Time.deltaTime;
        if(waitTimer > 0.0f)
        {
            agent.velocity = Vector3.zero;
            agent.UpdateMovement();
            return;
        }
        //move towards the current waypoint
        Vector3 offset = waypoints[currentWaypointIndex].position - transform.position;//subtracts the currentwaypoints position with objects current position
        offset.y = 0.0f;//set y to 0

        agent.velocity = offset.normalized * speed;
        agent.UpdateMovement();

        agent.transform.forward = offset.normalized;

        //determine if im at that location
        offset = waypoints[currentWaypointIndex].position - transform.position;
        if(offset.magnitude <= reachedThreshold)
        {
            //if we've reached the waypoint, go to the next one
            //next time we update

            waitTimer = waitInterval;

            ++currentWaypointIndex;
            if(currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

    }
}

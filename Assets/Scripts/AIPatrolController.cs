using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolController : MonoBehaviour
{
    public Agent agent;
    public float speed = 3.0f;

    public Transform []waypoints;
    private int currentWaypointIndex = 0;

    // Update is called once per frame
    void Update()
    {
        //move towards the current waypoint

        //determine if im at that location
    }
}

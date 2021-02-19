using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachines : MonoBehaviour
{
    public Agent agent;
    public float speed = 3.0f;
    public float waypointReachedThreshold = 1.0f;//to know when you reach a waypoint

    public Vector3[] patrolLocations;
    public int currentPatrolIndex;

    public Transform intruderTransform;

    public enum States
    {
        Patrol,
        Seek,
        Attack
    }
    
    [SerializeField]//so it can me marked in the inspector
    private States currentState;

    [SerializeField]//so it can me marked in the inspector
    private Transform[] waypoints;//creates a list of waypoints
    private int currentWaypointIndex;//keeps track of which waypoint its at

    [SerializeField]
    private Transform seekTarget;
    public float detectionRadius = 2.0f;

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                Debug.Log("Patroling");
                break;
            case States.Seek:
                Seek();
                Debug.Log("Seeking");
                //if statement switch to attack
                break;
            case States.Attack:
                Attack();
                Debug.Log("Attacking");
                break;
            default:
                Debug.LogError("Invalid state!");
                break;
        }

        //TODO: add ways to switch from one state to the next
    }

    void Patrol()
    {
        Vector3 curPos = agent.transform.position;//get agent current position
        Vector3 goalPos = waypoints[currentWaypointIndex].position;//where you want to go

        agent.velocity = (goalPos - curPos).normalized * speed;//gets the velocity of agent
        agent.UpdateMovement();//updates movement changes

        if((goalPos - agent.transform.position).magnitude < waypointReachedThreshold)//if goalPos - agent.transform.position is less than waypointReachedThreshold(1.0f)
        {
            ++currentWaypointIndex;
            if(currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

        }
        //have we noticed out target?
        if((seekTarget.position - agent.transform.position).magnitude < detectionRadius){//if less tahn 2.0f radius
            currentState = States.Seek;
        }
    }
    void Seek()
    {

    }
    void Attack()
    {

    }
}

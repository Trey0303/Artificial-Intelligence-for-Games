using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachines : MonoBehaviour
{
    public Agent agent;
    public float speed = 3.0f;
    //public float waypointReachedThreshold = 1.0f;//to know when you reach a waypoint

    //public Vector3[] patrolLocations;
    //public int currentPatrolIndex;

    public Transform intruderTransform;

    public enum States
    {
        Chase,
        Flee,
        Attack
    }

    [SerializeField]//so it can me marked in the inspector
    private States currentState;

    //[SerializeField]//so it can me marked in the inspector
    //private Transform[] waypoints;//creates a list of waypoints
    //private int currentWaypointIndex;//keeps track of which waypoint its at

    [SerializeField]
    private Transform seekTarget;
    public float detectionRadius = 2.0f;
    public float giveupRadius = 5.0f;
    public float AttackRadius = 6.0f;

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case States.Chase:
                Chase();
                Debug.Log("Patroling");
                break;
            case States.Flee:
                Flee();
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

    void Chase()
    {
        //chase target
        Vector3 curPos = agent.transform.position;
        Vector3 goalPos = seekTarget.transform.position;

        agent.velocity = (goalPos - curPos).normalized * speed;
        agent.UpdateMovement();

        //too close to target?
        if ((seekTarget.position - agent.transform.position).magnitude < detectionRadius)
        {//if less than 2.0f radius
            currentState = States.Flee;
        }
        //fire distance
        if((seekTarget.position - agent.transform.position).magnitude < AttackRadius)
        {
            currentState = States.Attack;
        }
    }
    void Flee()
    {
        Vector3 curPos = agent.transform.position;
        Vector3 goalPos = seekTarget.transform.position;

        agent.velocity = (curPos - goalPos).normalized * speed;
        agent.UpdateMovement();

        //some distance between target?
        if ((seekTarget.position - agent.transform.position).magnitude > giveupRadius)//if greater than 5.0f radius
        {
            currentState = States.Chase;
        }
        
    }
    void Attack()
    {
        //fire projectiles


        //too close to target?
        if ((seekTarget.position - agent.transform.position).magnitude < detectionRadius)
        {//if less than 2.0f radius
            currentState = States.Flee;
        }
    }
}

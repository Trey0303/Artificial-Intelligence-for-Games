using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;//NavMesh

public class FiniteStateMachines : MonoBehaviour
{
    public Agent agent;
    public GameObject player; 
    public PlayerHealth playerHealth;

    public static bool gameOver = false;

    //navMesh
    public CustomNavMeshAgent navAgent;

    public float speed = 3.0f;
    public float maxForce = 5.0f;
    public int damage = 1;
    public float waypointReachedThreshold = 1.0f;//to know when you reach a waypoint
    private float elapsed = 0.0f;

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
    public float detectionRadius = 3.0f;
    public float giveupRadius = 6.0f;
    public float attackRadius = 1.5f;

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();

        elapsed = 0.0f;

    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                //Debug.Log("Patroling");
                break;
            case States.Seek:
                Seek();
                //Debug.Log("Seeking");
                //if statement switch to attack
                break;
            case States.Attack:
                Attack();
                //Debug.Log("Attacking");
                break;
            default:
                Debug.LogError("Invalid state!");
                break;
        }

        if (gameOver)//if player died
        {
            this.enabled = false;//disable this script
        }

    }


    void Patrol()
    {

        Vector3 curPos = agent.transform.position;//get agent current position
        Vector3 goalPos = waypoints[currentWaypointIndex].position;//where you want to go

        agent.transform.forward = (goalPos - curPos).normalized;//to change where the enemy is facing visually

        elapsed += Time.deltaTime;
        if (elapsed > 0.5f)
        {
            elapsed -= 0.5f;//set elapsed back to 0.0f
            navAgent.SetDestination(goalPos);

        }

        if ((goalPos - agent.transform.position).magnitude < waypointReachedThreshold)//if goalPos - agent.transform.position is less than waypointReachedThreshold(1.0f)
        {
            ++currentWaypointIndex;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

        }
        //have we noticed out target?
        if((seekTarget.position - agent.transform.position).magnitude < detectionRadius){//if less than 2.0f radius
            goalPos = seekTarget.transform.position;
            navAgent.SetDestination(goalPos);
            currentState = States.Seek;

        }
    }
    void Seek()
    {
        Vector3 curPos = agent.transform.position;
        Vector3 goalPos = seekTarget.transform.position;

        agent.transform.forward = (goalPos - curPos).normalized;//to change where the enemy is facing visually

        //navMesh
        //update every second or .seconds
        elapsed += Time.deltaTime;
        if (elapsed > 0.5f)
        {
            elapsed -= 0.5f;//set elapsed back to 0.0f
            navAgent.SetDestination(goalPos);

        }

        //have we lost our target?
        if ((seekTarget.position - agent.transform.position).magnitude > giveupRadius)//if greater than giveup radius
        {
            goalPos = waypoints[currentWaypointIndex].position;//Update goal position to patrol goalPos
            navAgent.SetDestination(goalPos);
            currentState = States.Patrol;
        }
        else if ((seekTarget.position - agent.transform.position).magnitude < attackRadius)//if in attack radius
        {
            currentState = States.Attack;
            
        }
    }
    void Attack()
    {
        if (playerHealth.health > 0)//if player has health
        {
            playerHealth.health = playerHealth.health - damage;//player takes damage

        }

        //are we out of attack range?
        if ((seekTarget.position - agent.transform.position).magnitude > attackRadius)//if target is out of attack range
        {
            currentState = States.Seek;
            
        }
    }
}

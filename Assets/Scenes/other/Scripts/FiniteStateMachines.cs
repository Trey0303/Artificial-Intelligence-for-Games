using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachines : MonoBehaviour
{

    public Agent agent;
    public GameObject player; 
    public PlayerHealth playerHealth;

    //public Agent enemy;

    public float speed = 3.0f;
    public int damage = 1;
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
    public float giveupRadius = 5.0f;
    public float attackRadius = 1.5f;

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();

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

    }

    void Patrol()
    {
        Vector3 curPos = agent.transform.position;//get agent current position
        Vector3 goalPos = waypoints[currentWaypointIndex].position;//where you want to go

        agent.velocity = (goalPos - curPos).normalized * speed;//gets the velocity of agent
        agent.UpdateMovement();//updates movement changes

        agent.transform.forward = (goalPos - curPos).normalized;//to change where the enemy is facing visually

        if((goalPos - agent.transform.position).magnitude < waypointReachedThreshold)//if goalPos - agent.transform.position is less than waypointReachedThreshold(1.0f)
        {
            ++currentWaypointIndex;
            if(currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

        }
        //have we noticed out target?
        if((seekTarget.position - agent.transform.position).magnitude < detectionRadius){//if less than 2.0f radius
            currentState = States.Seek;

        }
    }
    void Seek()
    {
        Vector3 curPos = agent.transform.position;
        Vector3 goalPos = seekTarget.transform.position;

        agent.velocity = (goalPos - curPos).normalized * speed;

        agent.UpdateMovement();

        agent.transform.forward = (goalPos - curPos).normalized;//to change where the enemy is facing visually

        //have we noticed out target?
        if ((seekTarget.position - agent.transform.position).magnitude > giveupRadius)//if greater than giveup radius
        {
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

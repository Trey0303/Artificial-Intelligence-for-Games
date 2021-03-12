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
    private NavMeshPath path;
    private int currentCornerIndex;
    public float cornerReachedThreshold = 1.0f;

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
    public float detectionRadius = 2.0f;
    public float giveupRadius = 5.0f;
    public float attackRadius = 1.5f;

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();

        //navMesh Pathfinding
        path = new NavMeshPath();

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



        //reached the end of the path
        while (currentCornerIndex < path.corners.Length)
        {
            ++currentCornerIndex;//move on to next corner
            agent.velocity = (path.corners[currentCornerIndex] - curPos).normalized * speed;//gets the velocity of agent
            agent.UpdateMovement();//updates movement changes
        }

        agent.transform.forward = (goalPos - curPos).normalized;//to change where the enemy is facing visually

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

        if ((goalPos - agent.transform.position).magnitude < waypointReachedThreshold)//if goalPos - agent.transform.position is less than waypointReachedThreshold(1.0f)
        {

            //navMesh
            //visually shows target corners
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                currentCornerIndex = 0;//set cornerIndex back to zero
            }

        }
        //have we noticed out target?
        if((seekTarget.position - agent.transform.position).magnitude < detectionRadius){//if less than 2.0f radius
            curPos = agent.transform.position;
            goalPos = seekTarget.transform.position;
            NavMesh.CalculatePath(curPos, goalPos, NavMesh.AllAreas, path);//calculate/re-calculate target path
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

        //navMesh
        //update every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;//set elapsed back to 0.0f
            NavMesh.CalculatePath(curPos, goalPos, NavMesh.AllAreas, path);//calculate/re-calculate target path

        }
        //visually shows target corners
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

        //have we lost our target?
        if ((seekTarget.position - agent.transform.position).magnitude > giveupRadius)//if greater than giveup radius
        {
            goalPos = waypoints[currentWaypointIndex].position;//where you want to go
            NavMesh.CalculatePath(curPos, goalPos, NavMesh.AllAreas, path);//calculate/re-calculate target path
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

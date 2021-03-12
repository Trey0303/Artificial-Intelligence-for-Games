using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;//NavMesh

public class FiniteStateMachines : MonoBehaviour
{
    ////a*
    //[Header("Map Settings")]
    //public AStar graphA;//tiles
    //private TileA startTileA;
    //private TileA endTileA;
    ////enemy
    //public GameObject enemyPrefabA;
    //public TileA[] enemyPathA { get; private set; }
    //private int targetPathIndexA = 0;
    //public float moveSpeedA = 1.0f;
    //public int valueA = 100;


    public Agent agent;
    public GameObject player; 
    public PlayerHealth playerHealth;

    public static bool gameOver = false;

    NavMeshAgent myNavMeshAgent;
    

    public float speed = 3.0f;
    public float maxForce = 5.0f;
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

        ////a* (spawns floor to find path )
        //int startIdxA = (graphA.gridHeight / 2) * graphA.gridWidth;//sets a new int at the start of the graph
        //int endIdxA = startIdxA + graphA.gridWidth - 1;//sets a new int at the end of the graph

        //startTileA = graphA.tilesA[startIdxA];//sets startTile to the first tile using startIdx
        //endTileA = graphA.tilesA[endIdxA];//sets endTile to the last tile at the end of the graph using endIdx


        //enemyPathA = graphA.CalculatePath(startTileA, endTileA);//uses CalculatePath function to make its way from enemy spawner to base/makes its way from startTile to endTile(startTile and endTile are used as start and end points)


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

        //finite state machines 
        agent.velocity = (goalPos - curPos).normalized * speed;//gets the velocity of agent
        agent.UpdateMovement();//updates movement changes

        agent.transform.forward = (goalPos - curPos).normalized;//to change where the enemy is facing visually

        //navMesh
        //GetComponent<NavMeshAgent>().destination = waypoints[currentWaypointIndex].position;


        //a*
        // if path complete, do nothing

        //if (targetPathIndexA == enemyPathA.Length) { return; }

        //agent.velocity = (enemyPathA[targetPathIndexA].transform.position - agent.transform.position).normalized * moveSpeedA;
        //agent.UpdateMovement();

        //if (Vector3.Distance(agent.transform.position, enemyPathA[targetPathIndexA].transform.position) < 0.3f)
        //{
        //    ++targetPathIndexA;
        //}


        //finite state machines
        if ((goalPos - agent.transform.position).magnitude < waypointReachedThreshold)//if goalPos - agent.transform.position is less than waypointReachedThreshold(1.0f)
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

        //trigger navMesh
        //GetComponent<NavMeshAgent>().destination = player.transform.position;

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

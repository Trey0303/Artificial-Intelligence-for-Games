using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateA : MonoBehaviour
{
    [Header("Map Settings")]
    public AStar graphA;//tiles
    private TileA startTileA;
    private TileA endTileA;
    public GameObject spawnPrefabA;//spawner?
    public GameObject defendPrefabA;//base to defend

    public GameObject wallPrefabA;//wall to place

    [Header("Spawn Settings")]
    public GameObject enemyPrefabA;//enemy to spawn/move

    //enemy spawn timer
    public float spawnIntervalA;
    private float spawnTimerA;

    //enemy spawn count
    public int spawnCountPerWaveA;
    private int spawnCountThisWaveA;
    public int activeEnemiesA { get; set; }//active enemies

    private int currentWaveA;//wave number

    [Header("Player Progress")]
    public int livesRemainingA = 10;//base lives/health

    public TileA[] enemyPathA { get; private set; }//enemy path?

    [Header("User Interface")]
    public GameObject failureCanvasA;

    //phases/states
    public enum Phase
    {
        PreSetupA,   // before the game starts
        BuildA,      // construct walls (and towers eventually)
        SimulateA,   // spawn enemies and watch it goooo
        FailureA     // lives depleted, go to failure
    }
    private Phase currentPhaseA = Phase.PreSetupA;//start at PreSetUp phase

    public void Setup()
    {
        int startIdxA = (graphA.gridHeight / 2) * graphA.gridWidth;//sets a new int at the start of the graph
        int endIdxA = startIdxA + graphA.gridWidth - 1;//sets a new int at the end of the graph

        startTileA = graphA.tilesA[startIdxA];//sets startTile to the first tile using startIdx
        endTileA = graphA.tilesA[endIdxA];//sets endTile to the last tile at the end of the graph using endIdx

        Instantiate(spawnPrefabA, startTileA.transform.position, Quaternion.identity);//spawns enemy spawner at startTile position
        GameObject defendInstanceA = Instantiate(defendPrefabA, endTileA.transform.position, Quaternion.identity);//spawns base at endTile position
        defendInstanceA.GetComponent<BaseA>().gameStateA = this;

        enemyPathA = graphA.CalculatePath(startTileA, endTileA);//uses CalculatePath function to make its way from enemy spawner to base/makes its way from startTile to endTile(startTile and endTile are used as start and end points)

        currentPhaseA = Phase.BuildA;//set current state to build
    }

    // Trigger the next wave
    public void StartNextWave()
    {
        spawnTimerA = spawnIntervalA;
        spawnCountThisWaveA = 0;
        currentPhaseA = Phase.SimulateA;
    }

    public void Start()
    {
        //Disable the failure canvas (just in case we left it active)
        failureCanvasA.SetActive(false);
    }

    // TODO: Refactor this to use of a more elegant FSM system
    private void Update()
    {
        if (currentPhaseA == Phase.BuildA)//if in build phase
        {
            if (Input.GetMouseButtonDown(0))//if mouse click
            {
                Debug.Log("Clicked");
                bool clickedA = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitA);//get mouse click position
                if (!clickedA) { return; }//if nothing valid was clicked

                if (hitA.collider.gameObject.TryGetComponent<Tile>(out var tileA))//if tile was clicked
                {
                    Instantiate(wallPrefabA, tileA.transform.position, tileA.transform.rotation);//spawn wall
                    tileA.traversible = false;//set tile traversible to false so that enemies cant walk through wall

                    enemyPathA = graphA.CalculatePath(startTileA, endTileA);//Update enemy path with new information
                }

            }
        }
        else if (currentPhaseA == Phase.SimulateA)//simulate phase
        {
            // still need to check spawns?
            if (spawnCountThisWaveA < spawnCountPerWaveA)
            {
                spawnIntervalA -= Time.deltaTime;
                if (spawnIntervalA <= 0.0f)
                {
                    GameObject newEnemyA = Instantiate(enemyPrefabA, startTileA.transform.position, startTileA.transform.rotation);//spawn enemy
                    spawnIntervalA += spawnTimerA;//spawn timer
                    ++spawnCountThisWaveA;//will move on to the next enemy to spawn

                    newEnemyA.GetComponent<EnemyA>().gameStateA = this;
                    ++activeEnemiesA;//increases activeEnemies count by 1 because a new enemy has spawned
                }
            }

            // failure check
            if (livesRemainingA <= 0)//if base has no health left
            {
                currentPhaseA = Phase.FailureA;//switch to failure state
            }
            // success check
            else if (spawnCountThisWaveA == spawnCountPerWaveA && activeEnemiesA == 0)//if all enemies where defeated
            {
                currentPhaseA = Phase.BuildA;//switch back to build phase to prep for next wave
            }
        }
        else if (currentPhaseA == Phase.FailureA)//if in Failure Phase
        {
            failureCanvasA.SetActive(true);//end game and show lose screen
        }
    }

    private void OnDrawGizmosSelected()//displays enemy path?
    {
        if (enemyPathA == null) { return; }

        Gizmos.color = Color.blue;
        Vector3 drawOffset = new Vector3(0, 1.5f, 0.0f);

        // draw a line from one node to the next
        for (int i = 0; i < enemyPathA.Length - 1; ++i)
        {
            Gizmos.DrawLine(enemyPathA[i].transform.position + drawOffset,
                            enemyPathA[i + 1].transform.position + drawOffset);
        }
    }
}

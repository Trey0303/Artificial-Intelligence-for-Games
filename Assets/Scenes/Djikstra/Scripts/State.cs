using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    [Header("Map Settings")]
    public DjikstraAlgorithm graph;//tiles
    private Tile startTile;
    private Tile endTile;
    public GameObject spawnPrefab;//spawner?
    public GameObject defendPrefab;//base to defend

    public GameObject wallPrefab;//wall to place

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;//enemy to spawn/move

    //enemy spawn timer
    public float spawnInterval;
    private float spawnTimer;

    //enemy spawn count
    public int spawnCountPerWave;
    private int spawnCountThisWave;
    public int activeEnemies { get; set; }//active enemies

    private int currentWave;//wave number

    [Header("Player Progress")]
    public int livesRemaining = 10;//base lives/health

    public Tile[] enemyPath { get; private set; }//enemy path?

    //[Header("User Interface")]
    //public GameObject failureCanvas;

    //phases/states
    public enum Phase
    {
        PreSetup,   // before the game starts
        Build,      // construct walls (and towers eventually)
        Simulate,   // spawn enemies and watch it goooo
        Failure     // lives depleted, go to failure
    }
    private Phase currentPhase = Phase.PreSetup;//start at PreSetUp phase

    public void Setup()
    {
        int startIdx = (graph.gridHeight / 2) * graph.gridWidth;//sets a new int at the start of the graph
        int endIdx = startIdx + graph.gridWidth - 1;//sets a new int at the end of the graph

        startTile = graph.tiles[startIdx];//sets startTile to the first tile using startIdx
        endTile = graph.tiles[endIdx];//sets endTile to the last tile at the end of the graph using endIdx

        Instantiate(spawnPrefab, startTile.transform.position, Quaternion.identity);//spawns enemy spawner at startTile position
        GameObject defendInstance = Instantiate(defendPrefab, endTile.transform.position, Quaternion.identity);//spawns base at endTile position
        defendInstance.GetComponent<Base>().gameState = this;

        enemyPath = graph.CalculatePath(startTile, endTile);//uses CalculatePath function to make its way from enemy spawner to base/makes its way from startTile to endTile(startTile and endTile are used as start and end points)

        currentPhase = Phase.Build;//set current state to build
    }

    // Trigger the next wave
    public void StartNextWave()
    {
        spawnTimer = spawnInterval;
        spawnCountThisWave = 0;
        currentPhase = Phase.Simulate;
    }

    public void Start()
    {
        // Disable the failure canvas (just in case we left it active)
        //failureCanvas.SetActive(false);
    }

    // TODO: Refactor this to use of a more elegant FSM system
    private void Update()
    {
        if (currentPhase == Phase.Build)//if in build phase
        {
            if (Input.GetMouseButtonDown(0))//if mouse click
            {
                bool clicked = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit);//get mouse click position
                if (!clicked) { return; }//if nothing valid was clicked

                if (hit.collider.gameObject.TryGetComponent<Tile>(out var tile))//if tile was clicked
                {
                    Instantiate(wallPrefab, tile.transform.position, tile.transform.rotation);//spawn wall
                    tile.traversible = false;//set tile traversible to false so that enemies cant walk through wall

                    enemyPath = graph.CalculatePath(startTile, endTile);//Update enemy path with new information
                }

            }
        }
        else if (currentPhase == Phase.Simulate)//simulate phase
        {
            // still need to check spawns?
            if (spawnCountThisWave < spawnCountPerWave)
            {
                spawnInterval -= Time.deltaTime;
                if (spawnInterval <= 0.0f)
                {
                    GameObject newEnemy = Instantiate(enemyPrefab, startTile.transform.position, startTile.transform.rotation);//spawn enemy
                    spawnInterval += spawnTimer;//spawn timer
                    ++spawnCountThisWave;//will move on to the next enemy to spawn

                    newEnemy.GetComponent<Enemy>().gameState = this;
                    ++activeEnemies;//increases activeEnemies count by 1 because a new enemy has spawned
                }
            }

            // failure check
            if (livesRemaining <= 0)//if base has no health left
            {
                currentPhase = Phase.Failure;//switch to failure state
            }
            // success check
            else if (spawnCountThisWave == spawnCountPerWave && activeEnemies == 0)//if all enemies where defeated
            {
                currentPhase = Phase.Build;//switch back to build phase to prep for next wave
            }
        }
        else if (currentPhase == Phase.Failure)//if in Failure Phase
        {
            //failureCanvas.SetActive(true);//end game and show lose screen
        }
    }

    private void OnDrawGizmosSelected()//displays enemy path?
    {
        if (enemyPath == null) { return; }

        Gizmos.color = Color.blue;
        Vector3 drawOffset = new Vector3(0, 1.5f, 0.0f);

        // draw a line from one node to the next
        for (int i = 0; i < enemyPath.Length - 1; ++i)
        {
            Gizmos.DrawLine(enemyPath[i].transform.position + drawOffset,
                            enemyPath[i + 1].transform.position + drawOffset);
        }
    }
}

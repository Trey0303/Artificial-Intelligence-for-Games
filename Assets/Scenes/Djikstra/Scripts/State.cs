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
        int startIdx = (graph.gridHeight / 2) * graph.gridWidth;
        int endIdx = startIdx + graph.gridWidth - 1;

        startTile = graph.tiles[startIdx];
        endTile = graph.tiles[endIdx];

        Instantiate(spawnPrefab, startTile.transform.position, Quaternion.identity);
        GameObject defendInstance = Instantiate(defendPrefab, endTile.transform.position, Quaternion.identity);
        defendInstance.GetComponent<Base>().gameState = this;

        enemyPath = graph.CalculatePath(startTile, endTile);

        currentPhase = Phase.Build;
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
        if (currentPhase == Phase.Build)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool clicked = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit);
                if (!clicked) { return; }

                if (hit.collider.gameObject.TryGetComponent<Tile>(out var tile))
                {
                    Instantiate(wallPrefab, tile.transform.position, tile.transform.rotation);
                    tile.traversible = false;

                    enemyPath = graph.CalculatePath(startTile, endTile);
                }

            }
        }
        else if (currentPhase == Phase.Simulate)
        {
            // still need to check spawns?
            if (spawnCountThisWave < spawnCountPerWave)
            {
                spawnInterval -= Time.deltaTime;
                if (spawnInterval <= 0.0f)
                {
                    GameObject newEnemy = Instantiate(enemyPrefab, startTile.transform.position, startTile.transform.rotation);
                    spawnInterval += spawnTimer;
                    ++spawnCountThisWave;

                    newEnemy.GetComponent<Enemy>().gameState = this;
                    ++activeEnemies;
                }
            }

            // failure check
            if (livesRemaining <= 0)
            {
                currentPhase = Phase.Failure;
            }
            // success check
            else if (spawnCountThisWave == spawnCountPerWave && activeEnemies == 0)
            {
                currentPhase = Phase.Build;
            }
        }
        else if (currentPhase == Phase.Failure)
        {
            //failureCanvas.SetActive(true);
        }
    }

    private void OnDrawGizmosSelected()
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

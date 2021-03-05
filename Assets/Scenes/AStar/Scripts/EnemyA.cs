using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    public Agent agent;//set an agent

    public State gameStateA { get; set; }
    private int targetPathIndex = 0;

    public float moveSpeed = 1.0f;
    public int value = 100;

    private void FixedUpdate()
    {
        // if path complete, do nothing
        if (targetPathIndex == gameStateA.enemyPath.Length) { return; }

        agent.velocity = (gameStateA.enemyPath[targetPathIndex].transform.position - agent.transform.position).normalized * moveSpeed;
        agent.UpdateMovement();

        if (Vector3.Distance(agent.transform.position, gameStateA.enemyPath[targetPathIndex].transform.position) < 0.3f)
        {
            ++targetPathIndex;
        }
    }

    private void OnDestroy()//will despawn enemy
    {
        if (gameStateA)
        {
            --gameStateA.activeEnemies;//removes enemy form activeEnemies
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour
{
    public Agent agentA;//set an agent

    public StateA gameStateA { get; set; }
    private int targetPathIndexA = 0;

    public float moveSpeedA = 1.0f;
    public int valueA = 100;

    private void FixedUpdateA()
    {
        // if path complete, do nothing
        if (targetPathIndexA == gameStateA.enemyPathA.Length) { return; }

        agentA.velocity = (gameStateA.enemyPathA[targetPathIndexA].transform.position - agentA.transform.position).normalized * moveSpeedA;
        agentA.UpdateMovement();

        if (Vector3.Distance(agentA.transform.position, gameStateA.enemyPathA[targetPathIndexA].transform.position) < 0.3f)
        {
            ++targetPathIndexA;
        }
    }

    private void OnDestroyA()//will despawn enemy
    {
        if (gameStateA)
        {
            --gameStateA.activeEnemiesA;//removes enemy form activeEnemies
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseA : MonoBehaviour
{
    public StateA gameStateA;

    private void OnTriggerEnter(Collider otherA)
    {
        bool isEnemyA = otherA.TryGetComponent<EnemyA>(out var enemyA);//bool to know when enemy touches base
        if (isEnemyA)//if enemy touches base
        {
            Destroy(enemyA.gameObject);//enemy is destroyed
            --gameStateA.livesRemainingA;//lose health
        }
    }
}

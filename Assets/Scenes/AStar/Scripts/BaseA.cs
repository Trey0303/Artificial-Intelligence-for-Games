using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseA : MonoBehaviour
{
    public State gameStateA;

    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = other.TryGetComponent<EnemyA>(out var enemy);//bool to know when enemy touches base
        if (isEnemy)//if enemy touches base
        {
            Destroy(enemy.gameObject);//enemy is destroyed
            --gameStateA.livesRemaining;//lose health
        }
    }
}

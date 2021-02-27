using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public State gameState;

    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = other.TryGetComponent<Enemy>(out var enemy);//bool to know when enemy touches base
        if (isEnemy)//if enemy touches base
        {
            Destroy(enemy.gameObject);//enemy is destroyed
            --gameState.livesRemaining;//lose health
        }
    }
}

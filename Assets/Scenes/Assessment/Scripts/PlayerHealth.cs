using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject player;
    public int health = 1;

    public GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)//if health is less than 0
        {
            //Debug.Log("player died");
            FiniteStateMachines.gameOver = true;
            gameOver.SetActive(true);
            health = 1;//set back to one so that both player controller and enemy ai scripts can function again(otherwise gameOver would just be stuck on false)
        }
    }
}

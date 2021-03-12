using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public bool canEnter = false;

    public bool coinsNeededToPass = false;

    PlayerController playerController;

    public GameObject winPanel;

    public List<GameObject> allCoinObjects = new List<GameObject>();

    void Start()
    {
        allCoinObjects = GameObject.FindGameObjectsWithTag("Pickup").ToList();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (coinsNeededToPass)
        {
            canEnter = false;
        }
    }

    public void Update()
    {
        if (canEnter)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }

        if (coinsNeededToPass)
        {
            if (allCoinObjects.Count > 0 && playerController.coinsPickedUp >= allCoinObjects.Count)
            {
                canEnter = true;
                //Debug.Log("All coins are picked up");
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (canEnter)
        {
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("done"); ;//replace with ui game clear screen
                FiniteStateMachines.gameOver = true;
                winPanel.SetActive(true);
            }
        }
    }


}

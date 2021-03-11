using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public bool coinsNeededToPass;

    PlayerController playerController;

    public List<GameObject> allCoinObjects = new List<GameObject>();

    void Start()
    {
        allCoinObjects = GameObject.FindGameObjectsWithTag("Pickup").ToList();

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (coinsNeededToPass)
        {
            GameObject.FindGameObjectWithTag("Finish").GetComponent<LevelEnd>().canEnter = false;
        }
    }

    private void Update()
    {
        if (coinsNeededToPass)
        {
            if (allCoinObjects.Count > 0 && playerController.coinsPickedUp >= allCoinObjects.Count)
            {
                GameObject.FindGameObjectWithTag("Finish").GetComponent<LevelEnd>().canEnter = true;
                //Debug.Log("All coins are picked up");
            }
        }
    }
}

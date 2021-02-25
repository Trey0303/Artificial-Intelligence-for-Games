using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DjikstraAlgorithm : MonoBehaviour
{
    //Build Phase
    public int gridWidth;
    public int gridHeight;

    public GameObject prefab;

    public Tile[] tiles;

    private void Start()
    {
        tiles = new Tile[gridWidth * gridHeight];

        Vector3 offset = Vector3.zero;

        for(int i = 0; i < gridHeight; ++i)
        {
            for (int j = 0; j < gridWidth; ++j)
            {
                GameObject newTile = Instantiate(prefab, transform.position + offset, transform.rotation);
                tiles[i * gridWidth + j] = newTile.GetComponent<Tile>();
                offset.x += 1.0f;
            }

            offset.x = 0.0f;
            offset.z += 1.0f;
        }
    }

    //Siege Phase

}

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

        // spawn the tiles
        for (int i = 0; i < gridHeight; ++i)
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

        // setup connections
        for (int i = 0; i < tiles.Length; ++i)
        {
            List<Tile> connectedTiles = new List<Tile>();

            // if we're not at the left-edge
            if (i % gridWidth != 0)
            {
                connectedTiles.Add(tiles[i - 1]);
            }

            // if we're not at the right-edge
            if ((i + 1) % gridWidth != 0)
            {
                connectedTiles.Add(tiles[i + 1]);
            }

            // if we're not at the upper-edge
            if (i < gridWidth * gridHeight - gridWidth)
            {
                connectedTiles.Add(tiles[i + gridWidth]);
            }

            // if we're not at the bottom-edge
            if (i > gridWidth)
            {
                connectedTiles.Add(tiles[i - gridWidth]);
            }

            //tiles[i].connectedTiles = connectedTiles.ToArray();
        }
    }

    private Tile GetCheapestTile(Tile[] arr)
    {
        float bestGScore = float.MaxValue;
        Tile bestTile = null;

        for (int i = 0; i < tiles.Length; ++i)
        {
            if (arr[i].gScore < bestGScore)
            {
                bestTile = arr[i];
                bestGScore = arr[i].gScore;
            }
        }

        return bestTile;
    }

    //Siege Phase

    public Tile[] CalculatePath(Tile origin, Tile destination)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(origin);

        while (openList.Count != 0 &&               // still stuff left to explore
               !closedList.Contains(destination))   // AND we haven't reached the destination yet
        {
            // TODO: replace this with a proper sorted array implementation
            Tile current = GetCheapestTile(openList.ToArray());
            openList.Remove(current);

            closedList.Add(current);

            // TODO...
            // calculate g scores for connected tiles
            throw new System.NotImplementedException();
        }

        // TODO: remove this when completed
        return null;
    }

    

}

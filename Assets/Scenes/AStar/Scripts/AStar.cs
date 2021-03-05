using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    //Tile Generation
    public int gridWidth;//set grid width spawn
    public int gridHeight;//set grid height spawn

    public GameObject prefab;//set a gameobject to create

    public TileA[] tilesA;//to create an array of tiles

    private void Start()
    {
        tilesA = new TileA[gridWidth * gridHeight];//set array to grid

        Vector3 offset = Vector3.zero;//create an offset an set to zero

        // spawn the tiles
        for (int i = 0; i < gridHeight; ++i)
        {
            for (int j = 0; j < gridWidth; ++j)//goes through grid a set number of times before going on to the next row on grid height
            {
                GameObject newTile = Instantiate(prefab, transform.position + offset, transform.rotation);//create new tile
                tilesA[i * gridWidth + j] = newTile.GetComponent<TileA>();//add new tile to array
                tilesA[i * gridWidth + j].tilePosition = new Vector2Int(j, i);
                newTile.name = string.Format("{0}, {1}", i, j);//name tile on graph
                offset.x += 1.0f;//when ever an object spawns shift x by one to create a row
            }

            offset.x = 0.0f;//starts on the left most object on grid to reset the offset
            offset.z += 1.0f;//goes up the grid by 1(going to the next row) to create a column
        }

        // setup connections
        for (int i = 0; i < tilesA.Length; ++i)
        {
            List<TileA> connectedTiles = new List<TileA>();

            // if we're not at the left-edge
            if (i % gridWidth != 0)
            {
                connectedTiles.Add(tilesA[i - 1]);
            }

            // if we're not at the right-edge
            if ((i + 1) % gridWidth != 0)
            {
                connectedTiles.Add(tilesA[i + 1]);
            }

            // if we're not at the upper-edge
            if (i < gridWidth * gridHeight - gridWidth)
            {
                connectedTiles.Add(tilesA[i + gridWidth]);
            }

            // if we're not at the bottom-edge
            if (i > gridWidth - 1)
            {
                connectedTiles.Add(tilesA[i - gridWidth]);
            }

            tilesA[i].connectedTilesA = connectedTiles.ToArray();
        }
    }

    //Path Generation

    private TileA GetCheapestTile(TileA[] arr)
    {
        float bestGScore = float.MaxValue;//initialize
        TileA bestTile = null;//initialize

        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i].fScore < bestGScore)//if current tile has the best score
            {
                bestTile = arr[i];//set to current index
                bestGScore = arr[i].fScore;//set to current index score
            }
        }

        return bestTile;
    }

    //private int GetHScoreDjikstra(Tile selectedTile, Tile destructionTile)
    //{
    //    return 0;
    //}

    //                             whats the tile,    whats the destination tile
    private int GetHScoreManhattan(TileA selectedTile, TileA destructionTile)
    {
        Vector2Int tileOffset = selectedTile.tilePosition - destructionTile.tilePosition;
        return Mathf.Abs(tileOffset.x) + Mathf.Abs(tileOffset.y);//returns score as a single int (getting the differences between the x's and y's and putting it into one val) to get h score
    }

    private void ResetNodes()
    {
        for (int i = 0; i < tilesA.Length; ++i)
        {
            tilesA[i].gScore = 0;//set tiles gscore to zero
            tilesA[i].previousTile = null;//set previous tile to null
        }
    }


    public TileA[] CalculatePath(TileA origin, TileA destination)// CalculatePath(start and end)
    {
        ResetNodes();//clear old data

        List<TileA> openListA = new List<TileA>();//nodes that have NOT been traversed through
        List<TileA> closedListA = new List<TileA>();//nodes that have been traversed through

        openListA.Add(origin);//add starting tile 


        while (openListA.Count != 0 &&        // still stuff left to explore
        !closedListA.Contains(destination))   // AND we haven't reached the destination yet
        {
            // TODO: replace this with a proper sorted array implementation
            TileA current = GetCheapestTile(openListA.ToArray());
            openListA.Remove(current);
            closedListA.Add(current);

            // iterate through all connected tiles
            for (int i = 0; i < current.connectedTilesA.Length; ++i)
            {
                // create variable for to current connected tile for readability
                TileA adjTile = current.connectedTilesA[i];

                // skip tiles that were already processed or not traversible
                if (closedListA.Contains(adjTile) || !adjTile.traversible) { continue; }

                // NOTE: hard-coded cost of 1
                int estGScore = current.gScore + 1;

                if (adjTile.previousTile == null ||     // there is no score (no previous tile) OR
                    estGScore < adjTile.gScore)         // this is a cheaper route...
                {
                    adjTile.previousTile = current;
                    adjTile.gScore = estGScore;
                    adjTile.hScore = GetHScoreManhattan(adjTile, destination);
                }

                if (!closedListA.Contains(adjTile) && // if a connected tile is not in the closed list AND
                    !openListA.Contains(adjTile))    // is not *already* in the open list
                {
                    openListA.Add(adjTile);
                }
            }
        }

        List<TileA> path = new List<TileA>();//create list

        if (closedListA.Contains(destination))//if closedList destination was reached
        {
            TileA prevTile = destination;//start at the end of closedList
            while (prevTile != origin)//while on not at the start of closedList
            {
                path.Add(prevTile);//add a previous tile 
                prevTile = prevTile.previousTile;//update previous tile so that it can be added the next loop
            }
            path.Add(prevTile);//add origin/start/first node to the end of the list
        }

        path.Reverse();//reverses path order
        return path.ToArray();
    }
}

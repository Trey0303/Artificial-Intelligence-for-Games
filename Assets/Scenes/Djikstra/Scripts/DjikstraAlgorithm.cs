using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DjikstraAlgorithm : MonoBehaviour
{
    //Tile Generation
    public int gridWidth;//set grid width spawn
    public int gridHeight;//set grid height spawn

    public GameObject prefab;//set a gameobject to create

    public Tile[] tiles;//to create an array of tiles

    private void Start()
    {
        tiles = new Tile[gridWidth * gridHeight];//set array to grid

        Vector3 offset = Vector3.zero;//create an offset an set to zero

        // spawn the tiles
        for (int i = 0; i < gridHeight; ++i)
        {
            for (int j = 0; j < gridWidth; ++j)//goes through grid a set number of times before going on to the next row on grid height
            {
                GameObject newTile = Instantiate(prefab, transform.position + offset, transform.rotation);//create new tile
                tiles[i * gridWidth + j] = newTile.GetComponent<Tile>();//add new tile to array
                offset.x += 1.0f;//when ever an object spawns shift x by one to create a row
            }

            offset.x = 0.0f;//starts on the left most object on grid to reset the offset
            offset.z += 1.0f;//goes up the grid by 1(going to the next row) to create a column
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
    
    //Path Generation

    private Tile GetCheapestTile(Tile[] arr)
    {
        float bestGScore = float.MaxValue;//initialize
        Tile bestTile = null;//initialize

        for (int i = 0; i < tiles.Length; ++i)
        {
            if (arr[i].gScore < bestGScore)//if current tile has the best score
            {
                bestTile = arr[i];//set to current index
                bestGScore = arr[i].gScore;//set to current index score
            }
        }

        return bestTile;
    }


    public Tile[] CalculatePath(Tile origin, Tile destination)// CalculatePath(start and end)
    {
        List<Tile> openList = new List<Tile>();//nodes that have NOT been traversed through
        List<Tile> closedList = new List<Tile>();//nodes that have been traversed through

        //int totalGScore = 0;

        openList.Add(origin);//add starting tile 


        while (openList.Count != 0 &&               // still stuff left to explore
               !closedList.Contains(destination))   // AND we haven't reached the destination yet
        {

            // TODO: replace this with a proper sorted array implementation
            Tile current = GetCheapestTile(openList.ToArray());//update current

            openList.Remove(current);//remove current node from list

            closedList.Add(current);//add current node to list

            // TODO...
            // calculate g scores for connected tiles
            //throw new System.NotImplementedException();
            //current.gScore +=;//
        }

        // TODO: remove this when completed
        return null;
    }

    

}

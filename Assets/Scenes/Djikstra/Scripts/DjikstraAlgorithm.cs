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

    //public Tile debugStart;
    //public Tile debugEnd;

    //private Tile[] debugPath;

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
                newTile.name = string.Format("{0}, {1}", i, j);//name tile on graph
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
            if (i > gridWidth - 1)
            {
                connectedTiles.Add(tiles[i - gridWidth]);
            }

            tiles[i].connectedTiles = connectedTiles.ToArray();
        }
    }
    
    //Path Generation

    private Tile GetCheapestTile(Tile[] arr)
    {
        float bestGScore = float.MaxValue;//initialize
        Tile bestTile = null;//initialize

        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i].gScore < bestGScore)//if current tile has the best score
            {
                bestTile = arr[i];//set to current index
                bestGScore = arr[i].gScore;//set to current index score
            }
        }

        return bestTile;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.G))//visually shows calculatedPath
    //    {
    //        debugPath = CalculatePath(debugStart, debugEnd);
    //    }
    //}

    private void ResetNodes()
    {
        for(int i = 0; i < tiles.Length; ++i)
        {
            tiles[i].gScore = 0;//set tiles gscore to zero
            tiles[i].previousTile = null;//set previous tile to null
        }
    }

    public Tile[] CalculatePath(Tile origin, Tile destination)// CalculatePath(start and end)
    {
        ResetNodes();//clear old data

        List<Tile> openList = new List<Tile>();//nodes that have NOT been traversed through
        List<Tile> closedList = new List<Tile>();//nodes that have been traversed through

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
            //for(int i = 0; i < current.connectedTiles.Length; ++i)//until it reaches the last connected tile
            //{
            //    Tile adjTile = current.connectedTiles[i];//create adjTile and set to current connected tile

            //    int calGScore = current.gScore + adjTile.cost;//calculate gScore = current gScore + travel cost(hard-coded to 1)

            //    if (!adjTile.traversible) { continue; }//if not traversable move on to next node

            //    if (adjTile.previousTile == null || //if adjTile previous tile is equal to null(I think this condition is used because preiousTile starts off null)
            //        calGScore < adjTile.gScore)//or estScore is less than current adjTile gScore
            //    {
            //        adjTile.previousTile = current;//set adjTile previous tile to current(adjTile is one tile ahead of current)
            //        adjTile.gScore = calGScore;
            //    }

            //    if(!closedList.Contains(adjTile) && !openList.Contains(adjTile))//if neither of theses lists have this tile
            //    {
            //        openList.Add(adjTile);//add tile to openList
            //    }
            //}

            for (int i = 0; i < current.connectedTiles.Length; ++i)
            {
                Tile adjTile = current.connectedTiles[i];
                if (closedList.Contains(adjTile) || !adjTile.traversible) { continue; }

                int estGScore = current.gScore + 1;

                if (adjTile.previousTile == null ||
                    estGScore < adjTile.gScore)
                {
                    adjTile.previousTile = current;
                    adjTile.gScore = estGScore;
                }

                if (!closedList.Contains(adjTile) && !openList.Contains(adjTile))
                {
                    openList.Add(adjTile);
                }
            }
        }

        List<Tile> path = new List<Tile>();//create list

        if (closedList.Contains(destination))//if closedList destination was reached
        {
            Tile prevTile = destination;//start at the end of closedList
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

    //private void OnDrawGizmosSelected()
    //{
    //    if (debugPath == null) { return; }
    //    Gizmos.color = Color.blue;

    //    Vector3 drawOffset = new Vector3(0, 1.5f, 0.0f);

    //    for (int i = 0; i < debugPath.Length - 1; ++i)
    //    {
    //        Gizmos.DrawLine(debugPath[i].transform.position + drawOffset, debugPath[i + 1].transform.position + drawOffset);
    //    }
    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileA : MonoBehaviour
{
    public Vector2Int tilePosition { get; set; }//helps retrieve a tiles position
    public TileA previousTile;//keeps track of previous nodes
    public TileA[] connectedTilesA;//store connected tiles
    public int gScore;
    public int hScore;//heuristic score
    public int fScore { get { return gScore + hScore; } }//to find out what the cheapest will be

    public int cost = 1;//cost(hard-coded travel cost to 1)

    public bool traversible = true;

    private void OnDrawGizmosSelected()//used to visually show selected object
    {
        if (connectedTilesA == null) { return; }

        Gizmos.color = Color.green;//set the color

        Vector3 drawOffset = new Vector3(0, 1.5f, 0.0f);

        for (int i = 0; i < connectedTilesA.Length; ++i)
        {
            Gizmos.DrawLine(transform.position + drawOffset,
                           connectedTilesA[i].transform.position + drawOffset);
        }
    }
}

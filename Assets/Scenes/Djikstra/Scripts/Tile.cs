﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int tilePosition { get; set; }//helps retrieve a tiles position
    public Tile previousTile;//keeps track of previous nodes
    public Tile[] connectedTiles;//store connected tiles
    public int gScore;
    public int hScore;//heuristic score
    public int fScore { get { return gScore + hScore; } }//to find out what the cheapest will be

    public int cost = 1;//cost(hard-coded travel cost to 1)

    public bool traversible = true;

    private void OnDrawGizmosSelected()//used to visually show selected object
    {
        if (connectedTiles == null) { return; }

        Gizmos.color = Color.green;//set the color

        Vector3 drawOffset = new Vector3(0, 1.5f, 0.0f);

        for (int i = 0; i < connectedTiles.Length; ++i)
        {
            Gizmos.DrawLine(transform.position + drawOffset,
                           connectedTiles[i].transform.position + drawOffset);
        }
    }
}


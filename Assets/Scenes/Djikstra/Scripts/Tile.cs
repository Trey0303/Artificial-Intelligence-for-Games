using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile previousTile;//keeps track of previous nodes
    public int gScore;//score
    public int cost = 1;//cost
}

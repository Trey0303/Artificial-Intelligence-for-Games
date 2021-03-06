﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomNavMeshAgent : MonoBehaviour
{
    public Agent agent;

    private NavMeshPath currentPath;
    private int currentPathIndex = 0;
    private int speed = 3;

    public float cornerReachedThreshold = 0.5f;

    public bool shouldControlAgent = true;

    public Vector3 destination { get; private set; }

    private void Start()
    {
        currentPath = new NavMeshPath();
    }

    private void Update()
    {
        if (currentPath != null && currentPath.corners != null)
        {
            if (currentPathIndex < currentPath.corners.Length && shouldControlAgent)
            {
                agent.velocity = (currentPath.corners[currentPathIndex] - agent.transform.position).normalized * speed;
                agent.UpdateMovement();

                if ((currentPath.corners[currentPathIndex] - agent.transform.position).magnitude < cornerReachedThreshold)//if corner reached
                {
                    ++currentPathIndex;//move to next corner index
                }
            }
        }
        ////visually shows target corners
        for (int i = 0; i < currentPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(currentPath.corners[i], currentPath.corners[i + 1], Color.red);
        }
    }

    public void SetDestination(Vector3 newDestination)
    {
        if (destination != newDestination || currentPath.corners.Length == 0)//if destination is not the same or if there are no corners calculated
        {
            //calculate/recalculate
            currentPathIndex = 0;
            NavMesh.CalculatePath(agent.transform.position, newDestination, NavMesh.AllAreas, currentPath);
            destination = newDestination;
        }
    }
}

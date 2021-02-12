using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Agent agent;
    public Transform target;

    public float speed = 3.0f;

    // Update is called once per frame
    void Update()
    {

        //calculate the difference between your target location and current location
        //(this give you an offset from your position to your target)
        Vector3 distance = target.position - transform.position;
        Debug.Log("Distance to other : " + distance);

        //Normalize the difference
        //(This reduces the length of the offset to 1(aka unit length))
        distance.y = 0.0f;

        //Scale the difference by the speed you want to move at
        agent.velocity = distance.normalized * speed;
        agent.UpdateMovement();
    }
}

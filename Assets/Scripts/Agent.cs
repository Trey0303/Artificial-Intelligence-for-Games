using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is shared by both human players and AI players in game
public class Agent : MonoBehaviour
{
    //Refers to the rigidbody that will be driven
    //(assign this in the editor)
    [SerializeField]
    protected Rigidbody rbody;

    //Defines the change in players position over time
    public virtual Vector3 velocity { get; set; }

    //integrate movement and update the players position
    public virtual void UpdateMovement()
    {
        //moves the rigidbody to the new position
        //
        //new position is equal to newPos = pos + vel * dT
        rbody.MovePosition(rbody.position + velocity * Time.deltaTime);
    }

}

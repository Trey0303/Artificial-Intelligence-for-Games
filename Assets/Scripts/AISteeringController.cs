using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores data for agent
public class AISteeringController : MonoBehaviour
{

    //agent controlled by this controller
    public Agent agent;

    [Header("Steering Settings")]
    public float maxSpeed = 3.0f;//limits the speed that agent will be moving at
    public float maxForce = 5.0f;//limits the scale of force thats applied to velocity

    public Transform seekTarget;

    public void Start()
    {
        steerings.Add(new SeekSteering { target = seekTarget });
    }

    private void Update()
    {
        Vector3 steeringForce = CalculateSteeringForce();
        //add steering force to velocity -- clamp its magnatude w/ respect to maxSpeed
        agent.velocity = Vector3.ClampMagnitude(agent.velocity + steeringForce, maxSpeed);// agent velocity + steeringForce, 
                                                                                          //if agent velocity + steeringForce is greater than maxSpeed it will shink it down to the maxSpeed
        agent.UpdateMovement();
    }

    //add behaviors to this to consider them when calculating steering forces
    protected List<SteeringBehavior> steerings = new List<SteeringBehavior>();//adds a list of SteeringBehaviors(to combine everything together)

    //returns a Vector3 indicating the steering force to apply to our velocity
    protected Vector3 CalculateSteeringForce()//Vector3 calculates steering force
    {
        //initialize to zero
        Vector3 steeringForce = Vector3.zero;//creates a steeringForce and sets it to zero
        for (int i = 0; i < steerings.Count; ++i)
        {
            //Accumulate steering forces
            steeringForce += steerings[i].Steer(this);//this is used to get the controller object and give it to steeringForce
        }
        //Truncate to match maxForce value
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);//if magnitude is bigger then maxForce it will shrink down and keep the same direction

        return steeringForce;
    }

}


//overriding derived types, returns a Vector3 which is the desired velocity of what it wants
public class SteeringBehavior
{
    public virtual Vector3 Steer(AISteeringController controller)
    {
        return Vector3.zero;//default will return zero
    }
}

public class SeekSteering : SteeringBehavior
{
    public Transform target;//set a target to go towards
    public override Vector3 Steer(AISteeringController controller)//override to return the difference between the target position instead of the default zero
    {
        return (target.position - controller.transform.position).normalized/*normalize to get direction*/ * controller.maxSpeed;//multiply it by max speed

    }

    
}

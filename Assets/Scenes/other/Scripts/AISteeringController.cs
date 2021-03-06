﻿using System.Collections;
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
    public float wanderRadius = 10.0f;
    public float wanderDistance = 10.0f;

    public Transform seekTarget;
    public Transform fleeTarget;
    public Transform wanderTarget;
    public Agent pursueTarget;
    public Agent evadeTarget;

    //everything should revolve around SteeringBehavior(SteeringBehavior is treated like a parent or a main class for subclasses)
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

    WanderBehavior wander;

    public void Start()
    {
        //steerings.Add(new SeekSteering { target = seekTarget });//seeks target
        //steerings.Add(new FleeSteering { target = fleeTarget });//flees target

        wander = new WanderBehavior { radius = wanderRadius, distance = wanderDistance };//wanders
        steerings.Add(wander);

        steerings.Add(new pursueBehavior { target = pursueTarget });//pursue target
        steerings.Add(new evadeBehavior { target = evadeTarget });
    }

    private void Update()
    {
        wander.radius = wanderRadius;
        wander.distance = wanderDistance;

        Vector3 steeringForce = CalculateSteeringForce();
        //add steering force to velocity -- clamp its magnatude w/ respect to maxSpeed
        agent.velocity = Vector3.ClampMagnitude(agent.velocity + steeringForce, maxSpeed);// agent velocity + steeringForce, 
                                                                                          //if agent velocity + steeringForce is greater than maxSpeed it will shink it down to the maxSpeed
        agent.transform.forward = agent.velocity;
        agent.UpdateMovement();
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

//FleeSteering

//inherit from SteeringBehavior
//  FleeSteeringBehavior needs to inherit from SteeringBehavior
//  The idea is that they all inherit from the same type
//  This allows you to create polymorphic steering behaviors and call the correct version of the Steer method depending on which object is actually pushed into the List
public class FleeSteering : SteeringBehavior
{
    public Transform target;//
    public override Vector3 Steer(AISteeringController controller)//override to return the difference between the target position instead of the default zero
    {
        //(controller postion - target position) instead of (target position - controller postion) for fleeStering behavior
        return (controller.transform.position - target.position).normalized/*normalize to get direction*/ * controller.maxSpeed;//multiply it by max speed

    }


}

//WanderBehavior
public class WanderBehavior : SteeringBehavior
{
    public float radius = 5.0f;
    public float distance = 8.0f;

    public override Vector3 Steer(AISteeringController controller)//override to return the difference between the target position instead of the default zero
    {
        //start with a random target on the edge of the sphere with a set radius around the agent
        //You can add Random.onUnitSphere to the agent's position
        //Random.onUnitSphere generates a random point on a sphere -- to have it work with different radii, scale the random value by your radius
        Vector3 offset = Random.onUnitSphere * radius;//sets values to random
        offset.y = 0.0f;//sets y to zero so that only x and z changes
        offset += controller.transform.forward * distance;
        Debug.DrawRay(controller.transform.position, offset);//visually shows the wander function working

        Vector3 randomTargetAroundAgent = controller.transform.position/*gets position of agent*/ + offset/*creates a random sphere radius*/ * radius/*max radius of sphere*/;

        //add a randomised vector to the target, with a magnitude specified by a jitter amount
        

        //bring the target back to the radius of the sphere by normalising it and scaling by the radius
        return (randomTargetAroundAgent - controller.transform.position).normalized * controller.maxSpeed/*radius*/;

        //add the agents heading, multiplied by a distance to the target

    }
}

//pursueSteering

/*
Create a new class called PursueSteering that encapsulates the pursuit behavior. 
Pursuing an object is similar to seeking towards an object, but there's an additional step involved. 

Instead of trailing behind our target all of the time, we will instead try to intercept the target by considering its velocity as well. 
Since we need access to the velocity of the other object, let's designate our target as a Agent instead of a Transform.
*/
public class pursueBehavior : SteeringBehavior
{
    //designate our target as a Agent instead of a Transform so we can access the velocity of other objects
    public Agent target;


    public override Vector3 Steer(AISteeringController controller)
    {
        //calculate the estimated position of our target
        // - add its velocity to its position
        Vector3 estimatedPosition = target.velocity + target.transform.position;

        //calculate a directional vector from your position toward the estimated position
        return (estimatedPosition - controller.transform.position).normalized/*normalize to get direction*/ * controller.maxSpeed;//scale the directional vector by our maximum speed


    }
}

public class evadeBehavior : SteeringBehavior
{
    //designate our target as a Agent instead of a Transform so we can access the velocity of other objects
    public Agent target;


    public override Vector3 Steer(AISteeringController controller)
    {
        //calculate the estimated position of our target
        // - add its velocity to its position
        Vector3 estimatedPosition = target.velocity + target.transform.position;

        //calculate a directional vector from your position toward the estimated position
        return (controller.transform.position - estimatedPosition).normalized/*normalize to get direction*/ * controller.maxSpeed;//scale the directional vector by our maximum speed


    }
}
using UnityEngine;
using System.Collections;

//use the Generic system here to make use of a Flocker array later on
using System.Collections.Generic;

// This script contains operations and variables that all the scene's vehicles share

// The vehicle class is a parent class for all vehicle objects
abstract public class Vehicle : MonoBehaviour
{
    protected GameManager gm; // Accessor for the GameManager script
    protected Vector3 acceleration; // Create the variables for movement (v,a,vd,F)
    protected Vector3 velocity;
    public Vector3 Velocity // Property so that vehicles can make decisions based on each others' velocity
    {
        get { return velocity; }
    }
    protected Vector3 desired;
    protected Vector3 steer;
    private Vector3 vecToCenter; // Vector for distance in obstacle avoidance calculation

    protected Vector3 temp;

    public float maxSpeed; // Limiting variables, should be initialized in each vehicle child
    public float maxForce;
    public float radius;
    public float mass;
    protected bool autoRotate; // Variable to tell if vehicle should automatically rotate towards its direction of travel, set in child classes
    // No relevant gravitational force in the section of space the scene takes place in

    abstract protected void CalcSteeringForces(); // Require vehicle children to implement a steering method


	virtual public void Start() // Initialize the basic variables for every vehicle
    {
        acceleration = Vector3.zero; // No initial movement
        velocity = transform.forward;
        gm = GameObject.Find("ScriptStarter").GetComponent<GameManager>(); // Get access to the GameManager script
    }

	protected void Update() // Update movement variables based on forces, called once per frame
    {
        CalcSteeringForces(); // Get the overall acceleration needed to do what the vehicle wants

        velocity += acceleration * Time.deltaTime; // Apply the acceleration to the velocity, correcting for frame rate
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed); // Limit the velocity

        if(autoRotate && velocity != Vector3.zero) // This rotates the vehicle to face its new direction (if the vehicle should do this)
        {
            transform.forward = velocity.normalized;
        }

        temp = this.transform.position;
        temp += velocity;
        temp.y = this.transform.position.y;

        this.transform.position = temp;
        acceleration = Vector3.zero; // Reset acceleration when done for the frame
	}

    protected void ApplyForce(Vector3 steeringForce) // Takes in a force and applies it to the acceleration
    {
        acceleration += steeringForce/mass; // Apply the force, accounting for mass
        acceleration.y = 0;
    }

    protected Vector3 Seek(Vector3 targetPos) // Returns a basic seeking force towards the inputted target
    {
        desired = targetPos - transform.position; // Get the vector between there and here
        desired.Normalize();
        desired = desired * maxSpeed; // Scale the vector to maxSpeed
        steer = desired - velocity; // Take the desired velocity we just calculated and subtract the current velocity
        return steer; // Return this calculated steering force
    }
}

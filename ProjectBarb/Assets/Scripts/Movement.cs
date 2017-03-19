using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]

public class Movement : MonoBehaviour {

	public List<Vector3> path;
	public Vector3 target;
	protected Vector3 desired;
	protected Vector3 temp;
	protected Vector3 steer;
	public Vector3 velocity;
	public Vector3 Velocity
	{
		get {return velocity; }
	}
	public Vector3 acceleration;
	public float maxSpeed;
	public float maxForce;
	public bool hasArrived;
    private Vector3 followPoint; // Point behind the leader that flockers follow
    public Vector3 FollowPoint
    {
        get { return followPoint; }
    }
    public float followDist; // Distance behind the leader the point is at

    GameObject tempPlayer;
    aStar pinkyScript;


    CharacterController charControl;

	// Use this for initialization
	void Start () {
		acceleration = Vector3.zero;
		//velocity = transform.forward;

		tempPlayer = GameObject.FindGameObjectWithTag("Player");

		path = new List<Vector3>();

		hasArrived = true;
        pinkyScript = this.GetComponent<aStar>();
    }
	
	// Update is called once per frame
	void Update () {

		//check for new path from AStar script
		if(pinkyScript.path.Count > 0)
		{
			path.Clear();
			//Debug.Log("Lengh of the path: " + pinkyScript.path.Count);
			hasArrived = false;

			for(int i = 0; i < pinkyScript.path.Count; i++)
			{
				Vector3 tempNode = pinkyScript.path[i];
				path.Add(tempNode);
				//Debug.Log("X: " + path[i].x + " | Z: " + path[i].z);
			}

			pinkyScript.path.Clear();
			target = path[0];

			//Debug.Log("Lengh of the path: " + path.Count);
		}

		if(!hasArrived)
		{
			//Debug.Log("New target: X: " + target.x + " Y: " + target.y +  " | Z: " + target.z);
			//check if we are already at our target node
			Arrive();
			acceleration = Seek();

			velocity += acceleration * Time.deltaTime;
			velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

			//transform.forward = velocity.normalized;

			temp = this.transform.position;
			temp += velocity;
			temp.y = this.transform.position.y;

			this.transform.position = temp;
		}
        followPoint = (followDist * Vector3.Normalize(-velocity)) + transform.position; // Get the point behind the leader for the flockers to follow
    }

	Vector3 Seek(){
		desired = (target - this.transform.position);
		desired.Normalize();
		steer = desired * maxSpeed;
		return steer;
	}

	void UpdateTarget(){
		path.RemoveAt(0);
		target = path[0];
		Debug.Log("New target: X: " + target.x + " Y: " + target.y +  " | Z: " + target.z);
	}

	void Arrive(){
		Vector3 tempPos = this.transform.position;
		tempPos.y = 0;
		if(Vector3.Distance(tempPos, target) < 10)
		{
			Debug.Log("arriving at next node");
			if(path.Count > 0)
			{
				UpdateTarget();
			}
			else //if path is empty, we have arrived at goal
			{
				hasArrived = true;
			}
		}
	}
}

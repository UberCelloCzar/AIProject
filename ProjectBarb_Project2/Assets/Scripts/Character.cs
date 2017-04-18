using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    Rigidbody rigidBody;
    private float maxSpeed = 7f; // The fastest the player can travel in any direction

    private void Awake()
    {
        rigidBody = GetComponentInParent<Rigidbody>();
    }

    public void move(float xMove, float zMove) // Movement and rotation with keyboard and mouse
    {
        rigidBody.velocity = new Vector3(xMove * maxSpeed, 0, zMove * maxSpeed);

        //float angle = Mathf.Atan2(rigidBody.velocity.z, rigidBody.velocity.x) * Mathf.Rad2Deg;
        //rigidBody.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

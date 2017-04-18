using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour {

    public float moveSpeed = 10.0f;
    public float zoomSpeed = 5.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        float vert = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float horiz = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zoom = Input.GetAxis("Zoom") * zoomSpeed * Time.deltaTime;
        transform.Translate(horiz, vert, zoom);
	}
}

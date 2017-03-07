using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWeight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Hit a trigger");
		Debug.Log(other);
		if(other.tag == "Forest")
		{
			Vector3 newPos = new Vector3();
			newPos.x = this.gameObject.transform.position.x;
			newPos.z = this.gameObject.transform.position.z;
			newPos.y = 100;

			this.gameObject.transform.position = newPos;
		}

		if(other.tag == "Impassable")
		{
			Vector3 newPos = new Vector3();
			newPos.x = this.gameObject.transform.position.x;
			newPos.z = this.gameObject.transform.position.z;
			newPos.y = 10000000;

			this.gameObject.transform.position = newPos;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWeight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.transform.position.y++;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Hit a trigger");
		//Debug.Log(other);
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
			//Debug.Log("Hit impassable terrain");
			Vector3 newPos = new Vector3();
			newPos.x = this.gameObject.transform.position.x;
			newPos.z = this.gameObject.transform.position.z;
			newPos.y = -1;

			this.gameObject.transform.position = newPos;

			//remove text
			this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

			//add invalid tag
			this.gameObject.tag = "Invalid";
		}
	}
}

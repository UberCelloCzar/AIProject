using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateGraph : MonoBehaviour {

	public GameObject vertex;

	// Use this for initialization
	void Start () {
		int x = 0;
		Vector3 newPos = new Vector3();
		newPos.x = 0;
		newPos.y = 50;
		newPos.z = 0;
		for(int i = 0; i < 100; i++) 
		{
			newPos.z = 0;
			for(int j = 0; j < 100; j++) 
			{
				//newPos.y = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(newPos.x, newPos.z);
				newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
				GameObject newVertex = (GameObject)Instantiate( vertex, newPos, this.gameObject.transform.rotation);

				//use a trigger to figure out the height of the terrain where it spawns and set that to the height variable used by pathfinding


				newPos.z += 5;
			}
			newPos.x += 5;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

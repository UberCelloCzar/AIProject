using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateGraph : MonoBehaviour {

	public GameObject vertex;

	public int size = 95;

	public GameObject[,] adjMat = new GameObject [90,90];

	// Use this for initialization
	void Start () {
		Vector3 newPos = new Vector3();
		newPos.x = 25;
		newPos.y = 50;
		newPos.z = 0;
		for(int i = 0; i < 90; i++) 
		{
			newPos.z = 25;
			for(int j = 0; j < 90; j++) 
			{
				//newPos.y = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(newPos.x, newPos.z);
				newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
				GameObject newVertex = (GameObject)Instantiate( vertex, newPos, this.gameObject.transform.rotation);

				//store waypoints in 2d array
				adjMat[i,j] = newVertex;

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

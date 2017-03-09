using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateGraph : MonoBehaviour {

	public GameObject vertex;

	public GameObject[,] adjMat = new GameObject [100,100];

	public bool waypointRender;

	// Use this for initialization
	void Start () {
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
				//newVertex.GetComponent<MeshRenderer>().enabled = false;

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
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//reset waypoint colors
			for (int i = 0; i < 100; i++)
			{
				for (int j = 0; j < 100; j++)
				{
					GameObject waypoint = adjMat[i, j];
					waypoint.GetComponent<MeshRenderer>().enabled = !waypointRender;
				}
			}
			waypointRender = !waypointRender;

		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateGraph : MonoBehaviour {

	public GameObject vertex;
    public GameObject[,] adjMat;
	public bool waypointRender;
	public int numNodes = 50;
	public int distBetweenNodes = 10;

    //influnce map variables
    public bool[,] units;
    public GameObject black;

    public int blackStrength = 4;


    // Use this for initialization
    void Start()
    {
        //set up grid
        adjMat = new GameObject[numNodes, numNodes];

        Vector3 newPos = new Vector3();
        newPos.x = 0;
        newPos.y = 50;
        newPos.z = 0;
        for (int i = 0; i < numNodes; i++)
        {
            newPos.z = 0;
            for (int j = 0; j < numNodes; j++)
            {
                //newPos.y = Terrain.activeTerrain.terrainData.GetInterpolatedHeight(newPos.x, newPos.z);
                newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
                GameObject newVertex = (GameObject)Instantiate(vertex, newPos, this.gameObject.transform.rotation);
                //newVertex.GetComponent<MeshRenderer>().enabled = false;

                //store waypoints in 2d array
                adjMat[i, j] = newVertex;

                //use a trigger to figure out the height of the terrain where it spawns and set that to the height variable used by pathfinding


                newPos.z += distBetweenNodes;
            }
            newPos.x += distBetweenNodes;
        }

        //set up unit matrix - false means there is no unit there, true means there is
        units = new bool[numNodes, numNodes];

        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                units[i, j] = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //reset waypoint colors
            for (int i = 0; i < numNodes; i++)
            {
                for (int j = 0; j < numNodes; j++)
                {
                    GameObject waypoint = adjMat[i, j];
                    waypoint.GetComponent<MeshRenderer>().enabled = !waypointRender;
                }
            }
            waypointRender = !waypointRender;

        }


        //spawn a unit when mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 newPos = new Vector3();
            Vector3 cameraPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().position;

            //add unit to units matrix
            int x = (int)(cameraPos.x / distBetweenNodes);
            int z = (int)(cameraPos.z / distBetweenNodes);

            if (units[x, z]) //already exists a unit here
            {
                //don't add a new one
            }
            else
            {
                units[x, z] = true;

                newPos = adjMat[x, z].transform.position;
                GameObject newObject = (GameObject)Instantiate(black, newPos, this.gameObject.transform.rotation);

                //update influence map
                ColorGrid(x, z, blackStrength);
            }
        }
    }
    void ColorGrid(int startX, int startY, int strength)
    {
        for (int i = startX - strength; i < startX + strength; i++)
        {
            for (int j = startY - strength; j < startY + strength; j++)
            {
                GameObject waypoint = adjMat[i, j];
                waypoint.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}

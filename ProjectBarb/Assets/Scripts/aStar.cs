using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStar : MonoBehaviour {

	[HideInInspector] public GameObject[,] nodeObjects;
	private double [,][] nodes; // Stores [0] visited bool/int, [1] g(n) dist from start, [2] h(n) dist from goal, [3] last node
	private int[,][] lastNode; // Index of the last node
    double distMin = 100000; // Minimum dist to next node

    private GameObject player;
    private GameObject pinky;

    private bool isReady;

    public List<Vector3> path;

	// Use this for initialization
	void Start () {
		isReady = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return"))
        {
            if(!isReady)
            {
                isReady = true;
                InitializeAStar();
            }

            //reset waypoint colors
            for(int i = 0; i < 100; i++)
            {
                for(int j = 0; j < 100; j++)
                {
                    GameObject waypoint = nodeObjects[i, j];
                    waypoint.GetComponent<Renderer>().material.color = Color.white;
                }
            }

            //GET PLAYER POSITION
            player = GameObject.FindGameObjectWithTag("Player");
            pinky = GameObject.FindGameObjectWithTag("Pinky");

            int[] endNode = new int[2];
            endNode[0] = (int)(player.transform.position.x / 5);
            endNode[1] = (int)(player.transform.position.z / 5);
            //Debug.Log("player x: " + endNode[0]);
            //Debug.Log("player z: " + endNode[1]);

            int[] startNode = new int[2];
            startNode[0] = (int)(pinky.transform.position.x / 5);
            startNode[1] = (int)(pinky.transform.position.z / 5);

            RunAStar(startNode, endNode);
        }

    }
    public void InitializeAStar()
    {
        nodeObjects = GameObject.FindGameObjectWithTag("ScriptStarter").GetComponent<PopulateGraph>().adjMat;
        nodes = new double[nodeObjects.GetLength(0), nodeObjects.GetLength(1)][];
        lastNode = new int[nodeObjects.GetLength(0), nodeObjects.GetLength(1)][];
    }

	public int RunAStar(int[] startNode, int[] endNode)
    {
		for(int i = 0; i < nodeObjects.GetLength(0); i++)
		{
			for(int j = 0; j < nodeObjects.GetLength(1); j++) 
			{
                nodes[i, j] = new double[3];
				nodes[i,j][0] = 0; // Visited is always false at first
				lastNode[i,j] = new int[2] { -1, -1 }; // Backtrace
				nodes[i,j][1] = 100000000; // Initial g(n)

                // Estimate x dist to goal
                // Estimate z dist to goal
                // set h(n) equal to sum
                Vector3 currentNode = new Vector3(nodeObjects[i, j].transform.position.x, 0, nodeObjects[i, j].transform.position.z);
                Vector3 goalNode = new Vector3(nodeObjects[endNode[0], endNode[1]].transform.position.x, 0, nodeObjects[endNode[0], endNode[1]].transform.position.z);
                nodes[i, j][2] = Vector3.Distance(currentNode, goalNode) * 50;
                //nodes[i, j][2] = Vector3.Distance(currentNode, goalNode);
                //Debug.Log(nodes[i, j][2]);


                //Debug.Log(nodeObjects[i,j].transform.position.y);
            }
		}

        Debug.Log("setting up heap...initializing h(n) worked");

        Heap searchHeap = new Heap(); // Make a min-heap
        nodes[startNode[0], startNode[1]][1] = 0; // Set the first g(n) dist to 0
        searchHeap.push(nodes[startNode[0], startNode[1]][1], startNode); // Add the center to the queue

        while (searchHeap.size > 0)
        {
            distMin = 1000000;
            if (nodes[searchHeap.nodeIndexes[0][0], searchHeap.nodeIndexes[0][1]][0] == 1)
            {
                searchHeap.pop(); // Remove a node if it was visited somehow
                continue;
            }
            if (nodes[searchHeap.nodeIndexes[0][0], searchHeap.nodeIndexes[0][1]][1] < distMin) // Grab the node info
            {
                distMin = nodes[searchHeap.nodeIndexes[0][0], searchHeap.nodeIndexes[0][1]][1];
            }

            if (searchHeap.size == 0) continue; // Go on to fail if no more stuff

            int[] searchNode = searchHeap.pop(); // Get the node
            nodes[searchNode[0], searchNode[1]][0] = 1; // This node is now visited

            if (searchNode[0] == endNode[0] && searchNode[1] == endNode[1]) // If this is the goal, finish
            {
                //Debug.Log("\nPath Finished! Distance: " + nodes[searchNode[0], searchNode[1]][1]);
                //Debug.Log("Printing path: ");
                path = new List<Vector3>();
                printTrace(searchNode);
                return 0;
            }

            checkNeighbor(searchNode, new int[2] {searchNode[0], searchNode[1] - 1}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // If goal is not reached, check neighbors for best path (W)
            //checkNeighbor(searchNode, new int[2] {searchNode[0] - 1, searchNode[1] - 1}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (NW)
            checkNeighbor(searchNode, new int[2] {searchNode[0] - 1, searchNode[1]}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (N)
            //checkNeighbor(searchNode, new int[2] {searchNode[0] - 1, searchNode[1] + 1}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (NE)
            checkNeighbor(searchNode, new int[2] {searchNode[0], searchNode[1] + 1}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (E)
            //checkNeighbor(searchNode, new int[2] {searchNode[0] + 1, searchNode[1] + 1}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (SE)
            checkNeighbor(searchNode, new int[2] {searchNode[0] + 1, searchNode[1]}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (S)
            //checkNeighbor(searchNode, new int[2] {searchNode[0] + 1, searchNode[1] - 1}, nodes[searchNode[0], searchNode[1]][1], ref searchHeap); // (SW)
        }
        Debug.Log("Path not found!");
        return 1;
    }

    public int checkNeighbor(int[] node, int[] neighbor, double nodeDist, ref Heap heap)
    {
        if (neighbor[0] >= 90 || neighbor[0] < 0 || neighbor[1] >= 90 || neighbor[1] < 0) return 1; // Plz no index out of range

        if (nodeObjects[neighbor[0], neighbor[1]].transform.position.y < 0)
            return 0;

        if (nodes[neighbor[0], neighbor[1]][0] == 0) // If the goal is not reached, check its neighbors (right)
        {
            if (nodeDist + nodeObjects[neighbor[0], neighbor[1]].transform.position.y + nodes[neighbor[0], neighbor[1]][2] < nodes[neighbor[0], neighbor[1]][1] + nodes[neighbor[0], neighbor[1]][2]) // If going to the neighbor through this node is shorter than what was found previously
            {
                nodes[neighbor[0], neighbor[1]][1] = nodeDist + nodeObjects[neighbor[0], neighbor[1]].transform.position.y; // Save this as the new shortest distance g(n)
                lastNode[neighbor[0], neighbor[1]] = node; // Keep track of path (how we got to the node) (last node)
                heap.push(nodes[neighbor[0], neighbor[1]][1] + nodes[neighbor[0], neighbor[1]][2], neighbor); // Add the neighbor to the search list, taking into account estimated distance to goal (g(n)+h(n))
            }
        }
        return 0;
    }

    void printTrace(int[] node)
    {
        if (lastNode[node[0], node[1]][0] != -1)
        {
            printTrace(lastNode[node[0], node[1]]);
        }
        //Debug.Log("Node " + node[0] + ", " + node[1]);

        //add node to public path
        Vector3 newNode = new Vector3((node[0])*5, 0, (node[1])*5);
        path.Add(newNode);

        //change the color of the game objects that sit in the path
        GameObject waypoint = nodeObjects[node[0], node[1]];
        waypoint.GetComponent<Renderer>().material.color = Color.blue;
    }
}

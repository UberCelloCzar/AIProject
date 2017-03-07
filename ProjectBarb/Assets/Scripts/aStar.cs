using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStar : MonoBehaviour {

	[HideInInspector] public GameObject[,] nodeObjects;
	private double [,][] nodes; // Stores [0] visited bool/int, [1] g(n) dist from start, [2] h(n) dist from goal
	private int[,] lastNode; // Index of the last node

	public int startI;
	public int endI;
	public int startJ;
	public int endJ;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("space"))
			RunAStar();
	}

	void RunAStar() {
		nodeObjects = GameObject.FindGameObjectWithTag("ScriptStarter").GetComponent<PopulateGraph>().adjMat;
		nodes = new double[nodeObjects.GetLength(0),nodeObjects.GetLength(1)][];
		endI = 45;
		endJ = 45;

		for(int i = 0; i < nodeObjects.GetLength(0); i++)
		{
			for(int j = 0; j < nodeObjects.GetLength(1); j++) 
			{
				nodes[i,j][0] = 0; // Viusited is always false at first
				lastNode[i,j] = -1;
				nodes[i,j][1] = 100000000;

				// Estimate x dist to goal
				// Estimate z dist to goal
				// set h(n) equal to sum

				// Set start g(n) to 0

				Debug.Log(nodeObjects[i,j].transform.position.y);
			}
		}
	}
}

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
	public GameObject yellow;
	public GameObject blue;
	public GameObject white;

    public int blackStrength = 4;
	public int yellowStrength = 3;
	public int blueStrength = 2;
	public int whiteStrength = 1;

	private string currentColor = "black";

	private string currentTeam = "green";

    GameObject activeVertex;

    public int standardVertexHeight = 50;

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
				newVertex.GetComponent<Renderer>().material.color = Color.gray;
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

        activeVertex = adjMat[0, 0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toggleGrid();
        }
        if (Input.GetKeyDown(KeyCode.R))
		{
			currentTeam = "red";
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			currentTeam = "green";
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			currentColor = "white";
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentColor = "blue";
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			currentColor = "yellow";
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			currentColor = "black";
		}
		//update current color in UI
		string text = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<UnityEngine.UI.Text>().text;
		int index = text.LastIndexOf("Current Color");
		if (index > 0)
			text = text.Substring(0, index);
		text += "Current Color: " + currentColor;
		text += "\nCurrent Team: " + currentTeam;
		GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<UnityEngine.UI.Text>().text = text;


        //get camera pos
        Vector3 newPos = new Vector3();
        Vector3 cameraPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().position;

        Vector3 pos = Input.mousePosition;
        pos.z = Mathf.Abs(cameraPos.y - standardVertexHeight);
        pos = Camera.main.ScreenToWorldPoint(pos);

        int x = (int)(pos.x / distBetweenNodes);
        int z = (int)(pos.z / distBetweenNodes);

            if (x < 0 || x >= numNodes || z < 0 || z >= numNodes) //already exists a unit here
            {
                //don't add a new one
            }
            else
            {
            if (adjMat[x, z].tag == "Invalid")
            {
                //no
            }
            else
            {
                activeVertex.transform.localScale = new Vector3(7, 1, 7);
                activeVertex = adjMat[x, z];
                activeVertex.transform.localScale = new Vector3(12, 1, 12);

                if (Input.GetMouseButtonDown(0) && !units[x,z])
                {
                    units[x, z] = true;

                    newPos = adjMat[x, z].transform.position;

                    //update influence map
                    GameObject newObject;
                    int strength = 0;
                    switch (currentColor)
                    {
                        case "white":
                            strength = whiteStrength;
                            newObject = (GameObject)Instantiate(white, newPos, this.gameObject.transform.rotation);
                            break;
                        case "blue":
                            strength = blueStrength;
                            newObject = (GameObject)Instantiate(blue, newPos, this.gameObject.transform.rotation);
                            break;
                        case "yellow":
                            strength = yellowStrength;
                            newObject = (GameObject)Instantiate(yellow, newPos, this.gameObject.transform.rotation);
                            break;
                        case "black":
                            strength = blackStrength;
                            newObject = (GameObject)Instantiate(black, newPos, this.gameObject.transform.rotation);
                            break;
                        default:
                            newObject = (GameObject)Instantiate(black, newPos, this.gameObject.transform.rotation);
                            strength = blackStrength;
                            break;
                    }
                    ColorGrid(x, z, strength - 1);
                }
            }
        }
    }
    void ColorGrid(int startX, int startY, int strengthMax)
    {
		int strength = 1;
		for (int i = startX - strengthMax; i <= startX + strengthMax; i++)
        {
			for (int j = startY - strengthMax; j <= startY + strengthMax; j++)
            {
				if(i < 0 || j < 0 || i >= numNodes || j >= numNodes)
				{
					//no
				}
				else
				{
					GameObject waypoint = adjMat[i, j];

					if(adjMat[i, j].tag != "Invalid")
					{
						int offset = Mathf.Max(Mathf.Abs(startX - i), Mathf.Abs(startY - j));
						offset = strengthMax - offset + 1;

						string winner = "";
						if(currentTeam == "red")
						{
							adjMat[i,j].GetComponent<Influence>().redInfluence += offset;
						}
						if(currentTeam == "green")
						{
							adjMat[i,j].GetComponent<Influence>().greenInfluence += offset;
						}

						if(adjMat[i,j].GetComponent<Influence>().redInfluence > adjMat[i,j].GetComponent<Influence>().greenInfluence)
						{
							waypoint.GetComponent<Renderer>().material.color = Color.red;
						}
						else if(adjMat[i,j].GetComponent<Influence>().redInfluence < adjMat[i,j].GetComponent<Influence>().greenInfluence)
						{
							waypoint.GetComponent<Renderer>().material.color = Color.green;
						}
						else
						{
							waypoint.GetComponent<Renderer>().material.color = Color.gray;
						}

						adjMat[i,j].GetComponent<Influence>().influence = Mathf.Abs(adjMat[i,j].GetComponent<Influence>().greenInfluence - adjMat[i,j].GetComponent<Influence>().redInfluence);

						adjMat[i,j].GetComponentInChildren<TextMesh>().text = adjMat[i,j].GetComponent<Influence>().influence.ToString();
					}
				}
            }
        }
    }

    void toggleGrid()
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

        GameObject[] ivalues = GameObject.FindGameObjectsWithTag("iValue");
        for(int i = 0; i < ivalues.Length; i++)
        {
            ivalues[i].GetComponent<MeshRenderer>().enabled = !waypointRender;
        }


        waypointRender = !waypointRender;
    }
}


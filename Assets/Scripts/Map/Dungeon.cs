using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A dungeon consists of nodes, paths, and interactable objects. Everything within a dungeon is randomized. 2D arrays are used for the map layout
    and the object placement. 
    
    0 - Empty
    1 - Node
    2 - Path (E-W)
    3 - Path (N-S)
    
    */

public class Dungeon : MonoBehaviour
{
    public List<Node> nodes;
    public Node nodePrefab;

    public bool[,] mapArray;
    public int mapWidth {get; set;}     //columns
    public int mapHeight {get; set;}    //rows
    float xOffset, yOffset = 3;
    const float offset = 2;
    int nodeCount;
    int minNodeCount {get;} = 10;
    int totalNodes;                 //number of nodes in dungeon.
    byte nodeID;
    
    // Start is called before the first frame update
    void Start()
    {
        mapWidth = 4;
        mapHeight = 4;
        mapArray = new bool[mapWidth, mapHeight];
        nodeCount = minNodeCount;

        string arrayString = "";
        //create the map
        /*for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                float roll = Random.Range(0, 1f);
                if (Random.value <= 1f)
                {
                    mapArray[i, j] = true;
                    Node node = Instantiate(nodePrefab);
                    node.nodeID = nodeCount;
                    nodeCount++;

                    //if this is a corner node, some paths cannot be visible
                    if (i == 0 && j == 0)
                    {
                        node.paths[node.northPath].ShowPath(false);
                        node.paths[node.westPath].ShowPath(false);

                        //make sure this node is accessible
                        if (node.NoPath())
                        {
                            while (!node.paths[node.eastPath].PathVisible() && !node.paths[node.southPath].PathVisible())
                            {
                                float roll1 = Random.Range(0, 1f);
                                float roll2 = Random.Range(0, 1f);
                                bool eastPathVisible = roll1 <= 0.5f ? true : false;
                                bool southPathVisible = roll2 <= 0.5f ? true : false;
                                node.paths[node.eastPath].ShowPath(eastPathVisible);
                                node.paths[node.southPath].ShowPath(southPathVisible);
                            }
                        }
                    }

                    //place node in game
                    node.transform.SetParent(transform);
                    node.transform.position = new Vector3(i, j, 0);
                    nodes.Add(node);
                }
                arrayString += mapArray[i, j] + " ";
            }
            arrayString += "\n";
        }

        Debug.Log(arrayString);  */
        GenerateDungeon(nodeCount);
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                arrayString += mapArray[i, j] + " ";
            }
            arrayString += "\n";
        }
        Debug.Log(arrayString);
    }

    public void GenerateDungeon(int nodeCount)
    {
        mapArray = new bool[mapWidth, mapHeight];
        totalNodes = nodeCount;

        //once map is generated, create rooms
        GenerateNode(0, 0, nodeCount, firstNode: true);

        //loop through array and create nodes
        bool firstNode = true;
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (mapArray[i, j] == true)
                {
                    Node node = Instantiate(nodePrefab);
                    node.nodeID = nodeID;
                    nodeID++;

                    //check adajacent spots in the array for other rooms. If this node is the first, then there can only be a path to the east and south.
                    if (firstNode)
                    {
                        //node.paths[node.northPath].ShowPath(false);
                        //node.paths[node.westPath].ShowPath(false);

                        while (!node.paths[node.eastPath].PathVisible() && !node.paths[node.southPath].PathVisible())
                        {
                            float pathChance = 0.5f;
                            bool eastPathVisible = Random.value <= pathChance ? true : false;
                            bool southPathVisible = Random.value <= pathChance ? true : false;
                            node.paths[node.eastPath].ShowPath(eastPathVisible);
                            node.paths[node.southPath].ShowPath(southPathVisible);
                        }

                        firstNode = false;
                    }
                    else
                    {
                        //check for any adjacent nodes
                        //check if there's a path leading to the node
                        bool northNode = (j > 0 && mapArray[i, j - 1] == true) ? true : false;
                        bool southNode = (j + 1 < mapHeight && mapArray[i, j + 1] == true) ? true : false;
                        bool eastNode = (i + 1 < mapWidth && mapArray[i + 1, j] == true) ? true : false;
                        bool westNode = (i > 0 && mapArray[i - 1, j] == true) ? true : false;

                        //if there are 2 or more adjacent rooms, see if all paths are opened.
                        float pathChance = 0.6f;
                        if (northNode)
                        {
                            node.paths[node.northPath].ShowPath(true);
                            if (southNode)
                                node.paths[node.southPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (eastNode)
                                node.paths[node.eastPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (westNode)
                                node.paths[node.westPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        else if (southNode)
                        {
                            node.paths[node.southPath].ShowPath(true);
                            if (northNode)
                                node.paths[node.northPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (eastNode)
                                node.paths[node.eastPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (westNode)
                                node.paths[node.westPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        else if (eastNode)
                        {
                            node.paths[node.eastPath].ShowPath(true);
                            if (northNode)
                                node.paths[node.northPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (southNode)
                                node.paths[node.southPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (westNode)
                                node.paths[node.westPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        else if (westNode)
                        {
                            node.paths[node.westPath].ShowPath(true);
                            if (northNode)
                                node.paths[node.northPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (southNode)
                                node.paths[node.southPath].ShowPath(Random.value <= pathChance ? true : false);
                            if (eastNode)
                                node.paths[node.eastPath].ShowPath(Random.value <= pathChance ? true : false);
                        }
                        
                    }

                    //place node in game
                    node.transform.SetParent(transform);
                    node.transform.position = new Vector3(transform.position.x + i, transform.position.y - j, 0);
                    node.transform.position *= offset;  //not quite sure why I have to apply offset this way
                    nodes.Add(node);
                }
            }
        }
    }

    //create node at given position. Once a node is generated, it must create at least one path leading to another node.
    void GenerateNode(int i, int j, int nodeCount, bool firstNode = false)
    {

        //invalid states
        if (i < 0 || i >= mapWidth) return;
        if (j < 0 || j >= mapHeight) return;
        if (nodeCount <= 0) return;         //no more nodes to create
        if (mapArray[i, j] == true) return; //room already exists
        if (nodeCount > mapHeight * mapWidth) return;      //there are more nodes than array capacity

        totalNodes = nodeCount;
        if (firstNode /*i == 0 && j == 0*/)  //first room
        {
            mapArray[i,j] = true;

            //there must be at least one path leading to the next room
            while(mapArray[i + 1, j] == false && mapArray[i, j + 1] == false)
            {
                mapArray[i + 1, j] = Random.value <= 0.5f ? true : false;  //east room
                mapArray[i, j + 1] = Random.value <= 0.5f ? true : false;  //south room
            }
            //firstNode = false;
        }
        else
        {
            mapArray[i,j] = true;

            //check for adjacent nodes

        }
        
        //check which direction we're headed from current spot
        float nodeChance = 0.6f;
        bool goingNorth = (!firstNode && j - 1 >= 0 && Random.value <= nodeChance) ? true : false;
        bool goingSouth = (j + 1 < mapHeight && Random.value <= nodeChance) ? true : false;
        bool goingEast = (i + 1 < mapWidth && Random.value <= nodeChance) ? true : false;
        bool goingWest = (!firstNode && i - 1 >= 0 && Random.value <= nodeChance) ? true : false;

        if (goingNorth)
            GenerateNode(i, j - 1, nodeCount - 1);
        if (goingSouth || firstNode)
        {
            firstNode = false;
            GenerateNode(i, j + 1, nodeCount - 1, firstNode);
        }
        if (goingEast || firstNode)
        {
            firstNode = false;
            GenerateNode(i + 1, j, nodeCount - 1, firstNode);
        }
        if (goingWest)
            GenerateNode(i - 1, j, nodeCount - 1);
    }

    public void GenerateMap(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;
        mapArray = new bool[mapWidth, mapHeight];

        //go through array and decide the rooms. The first room is always at [0,0]

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

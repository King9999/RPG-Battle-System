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
    public Player player;
    public List<MapEnemy> enemies;

    public bool[,] mapArray;
    public int mapWidth {get; set;}     //columns
    public int mapHeight {get; set;}    //rows
    float xOffset, yOffset = 3;
    const float offset = 2;
    int nodeCount;
    int minNodeCount {get;} = 10;
    int totalNodes;                 //number of nodes in dungeon.
    byte nodeID;

    public static Dungeon instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    
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
        /*for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                arrayString += mapArray[i, j] + " ";
            }
            arrayString += "\n";
        }
        Debug.Log(arrayString);*/
        player.MovePlayer(0, 0);
    }

    public void GenerateDungeon(int nodeCount)
    {
        mapArray = new bool[mapWidth, mapHeight];
        this.nodeCount = nodeCount;

        //once map is generated, create rooms
        //GenerateNode(0, 0, this.nodeCount, firstNode: true);
        GenerateNode(this.nodeCount);

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
                    node.row = j;
                    node.col = i;
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

        

        
        if (firstNode)  //first room
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

    void GenerateNode(int nodeCount)
    {
        if (nodeCount < minNodeCount) return;
        if (nodeCount > mapHeight * mapWidth) return;

        int i = 0;
        int j = 0;
        int totalNodes = 0; //tracks how many nodes have been generated, and will not exceed nodeCount
        int iPrevious = 0, jPrevious = 0;   //tracks which index I was in previously so I don't return there.

        //set index 0,0 to true and then have at least one adjacent index set to true
        mapArray[i,j] = true;
        totalNodes++;

        //there must be at least one path leading to the next room
        while(mapArray[i + 1, j] == false && mapArray[i, j + 1] == false)
        {
            mapArray[i + 1, j] = Random.value <= 0.5f ? true : false;  //east room
            mapArray[i, j + 1] = Random.value <= 0.5f ? true : false;  //south room
        }

        

        //find out where we're going next
        if (mapArray[i + 1, j] == true && mapArray[i, j + 1] == true)
        {
           totalNodes += 2;
           if (Random.value <= 0.5f)
           {
               iPrevious = i;
               i += 1;  //going east
           }
           else
           {
               jPrevious = j;
               j += 1;  //going south
           }

        }
        else if (mapArray[i + 1, j] == true)
        {
            iPrevious = i;
            i += 1;
            totalNodes++;
        }
        else if (mapArray[i, j + 1] == true)
        {
            jPrevious = j;
            j += 1;
            totalNodes++;
        }

        while(totalNodes < nodeCount)
        {
            //check surrounding indexes to see where we can move
            float directionChance = 0.8f;
            bool goingNorth = false;
            bool goingSouth = false;
            bool goingEast = false;
            bool goingWest = false;

            while (!goingNorth && !goingSouth && !goingEast && !goingWest)
            {
                goingNorth = (j - 1 >= 0 && Random.value <= directionChance) ? true : false;
                goingSouth = (j + 1 < mapHeight && Random.value <= directionChance) ? true : false;
                goingEast = (i + 1 < mapWidth && Random.value <= directionChance) ? true : false;
                goingWest = (i - 1 >= 0  && Random.value <= directionChance) ? true : false;
            }

            if (goingNorth)
            {
                mapArray[i, j - 1] = true;
                totalNodes++;
            }
            if (goingSouth)
            {
                mapArray[i, j + 1] = true;
                totalNodes++;
            }
            if (goingEast)
            {
                mapArray[i + 1, j] = true;
                totalNodes++;
            }
            if (goingWest)
            {
                mapArray[i - 1, j] = true;
                totalNodes++;
            }

            //where do we move next?
            /*iPrevious = i;
            jPrevious = j;
            int iRandom;
            int jRandom;

            do
            {
                iRandom = Random.Range(i - 1, i + 2);
                jRandom = Random.Range (j - 1, j + 2);
            }
            while (iRandom == i && jRandom == j && (iRandom < 0 || jRandom < 0) && (iRandom >= mapWidth || 
                jRandom >= mapHeight) || mapArray[iRandom, jRandom] == false);
           
            //once we get here, we found an index to generate next set of nodes.
            i = iRandom;
            j = jRandom;*/

            /*if (goingNorth && goingSouth && goingEast && goingWest)
            {
                //pick a random path
                float roll = Random.Range(0, 1f);
                if (roll <= 0.25f)
                    i += 1;     //east
                else if (roll <= 0.5f)
                    i -= 1;     //west
                else if (roll <= 0.75f)
                    j += 1;     //south
                else
                    j -= 1;     //north
            }
            else
            {*/
                bool pathChosen = false;
                directionChance = 0.4f;
                while(pathChosen == false)
                {
                    if (goingNorth && Random.value <= directionChance)
                    {    
                        jPrevious = j;
                        j -= 1;
                        pathChosen = true;
                    }
                    else if (goingSouth && Random.value <= directionChance)
                    {
                        jPrevious = j;
                        j += 1;
                        pathChosen = true;     
                    }
                    else if (goingEast && Random.value <= directionChance)
                    {
                        iPrevious = i;
                        i += 1;
                        pathChosen = true;
                    }
                    else if (goingWest && Random.value <= directionChance)
                    {
                        iPrevious = i;
                        i -= 1;
                        pathChosen = true;
                    }
                    directionChance += 0.2f;    //I do this to limit the time spent in this loop, and in case there's only one direction.
                }
            //}
        }
        
    }

    public void GenerateMapObjects(int width, int height)
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

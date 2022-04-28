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

    public byte[,] mapArray;
    public int mapRow {get; set;}
    public int mapCol {get; set;}
    float xOffset, yOffset = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        mapRow = 2;
        mapCol = 2;
        mapArray = new byte[mapRow, mapCol];

        //create the map
        for (int i = 0; i < mapRow; i++)
        {
            for (int j = 0; j < mapCol; j++)
            {
                float roll = Random.Range(0, 1f);
                if (roll <= 0.5f)
                {
                    mapArray[i, j] = 1;
                    Node node = Instantiate(nodePrefab);

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
                    node.transform.position = new Vector3(node.transform.position.x + j + xOffset, node.transform.position.y - i - yOffset, 0);
                    nodes.Add(node);
                }  
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

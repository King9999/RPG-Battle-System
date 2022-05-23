using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A node is basically a room that consists of 4 paths. The player travels along these paths to reach other nodes.
    when a node is generated, the paths and any objects they contain are random. Exactly one node will always be a starting point,
    and one other node must be an exit to a new area. All other nodes can have various objects, or none at all.

    Nodes can have events which are not known until a player steps into the node with the event. Events are mostly positive,
    but they can be dangerous too.
*/
public class Node : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite eventSprite;
    public enum EventNode {None, HpMpBoost}
    public EventNode eventNode;
    public int nodeID;                 //a way for me to identify nodes.
    public int row, col;                //its location in the map. info is provided by the dungeon when it's generated.
    public bool isOccupied {get; set;}  //if true, then map object cannot be placed here.

    public Path[] paths;
    public int northPath {get;} = 0;
    public int eastPath {get;} = 1;
    public int westPath {get;} = 2;
    public int southPath {get;} = 3;

    // Start is called before the first frame update
    void Start()
    {
        //nodes have random number of paths but it must have at least one. It will be possible to fine tune the node
        //if necessary.
        /*int i = 0;
        while(i < 3)
        {
            int randPath = Random.Range(0, paths.Length);
            float roll = Random.Range(0, 1f);
            bool pathState = roll <= 0.5f ? true : false;
            paths[randPath].ShowPath(pathState);
            i++;
        }*/
    }

    public void ResetNode()
    {
        paths[northPath].ShowPath(false);
        paths[southPath].ShowPath(false);
        paths[eastPath].ShowPath(false);
        paths[westPath].ShowPath(false);
        isOccupied = false;
        //nodeID = -1;
    }

   public bool NoPath()
   {
       return (!paths[northPath].PathVisible() && !paths[southPath].PathVisible() 
        && !paths[eastPath].PathVisible() && !paths[westPath].PathVisible());
   }

    //this method is called when the node is clicked.
   public void MovePlayer()
   {
       //Debug.Log("clicked node " + nodeID);

       //check if player is adjacent to this node
       Player player = Player.instance;
       if (PlayerAdjacentToNode())
       {
           //Debug.Log("Player adjacent");
           player.MovePlayer(col, row);
       }
   }

   public bool PlayerAdjacentToNode()
   {
       bool playerAdjacent = false;
       Player player = Player.instance;
       Dungeon dungeon = Dungeon.instance;

        if (col > 0 && col - 1 == player.col && row == player.row)
        {
            //find the adjacent node and check if there's a path leading to the node we want to move to.
            foreach(Node node in dungeon.nodes)
            {
                if (node.col == col - 1 && node.row == row)
                {
                    if (paths[westPath].PathVisible() || node.paths[eastPath].PathVisible())
                    playerAdjacent = true;
                    break;
                }
            }
        }
        else if (col < dungeon.mapWidth - 1 && col + 1 == player.col && row == player.row)
        {
            foreach(Node node in dungeon.nodes)
            {
                if (node.col == col + 1 && node.row == row)
                {
                    if (paths[eastPath].PathVisible() || node.paths[westPath].PathVisible())
                    playerAdjacent = true;
                    break;
                }
            }
        }
        else if (row > 0 && col == player.col && row - 1 == player.row)
        {   
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row - 1 && node.col == col)
                {
                    if (paths[northPath].PathVisible() || node.paths[southPath].PathVisible())
                    playerAdjacent = true;
                    break;
                }
            }
        }
        else if (row < dungeon.mapHeight - 1 && col == player.col && row + 1 == player.row)
        {
            foreach(Node node in dungeon.nodes)
            {
                if (node.row == row + 1 && node.col == col)
                {
                    if (paths[southPath].PathVisible() || node.paths[northPath].PathVisible())
                    playerAdjacent = true;
                    break;
                }
            }
        }

        return playerAdjacent;

   }

}

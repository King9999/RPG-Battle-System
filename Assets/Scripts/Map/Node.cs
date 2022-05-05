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
    public byte nodeID;                 //a way for me to identify nodes.
    public int row, col;                //its location in the map. info is provided by the dungeon when it's generated.

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

   public bool NoPath()
   {
       return (!paths[northPath].PathVisible() && !paths[southPath].PathVisible() 
        && !paths[eastPath].PathVisible() && !paths[westPath].PathVisible());
   }

    //this method is called when the node is clicked.
   public void MovePlayer()
   {
       Debug.Log("clicked node " + nodeID);

       //check if player is adjacent to this node
       Player player = Player.instance;
       Dungeon dungeon = Dungeon.instance;
       if (PlayerAdjacentToNode())
       {
           Debug.Log("Player adjacent");
           player.MovePlayer(row, col);
       }
   }

   public bool PlayerAdjacentToNode()
   {
       Player player = Player.instance;
       Dungeon dungeon = Dungeon.instance;
       return col > 0 && dungeon.mapArray[col - 1, row] == dungeon.mapArray[player.col, player.row] || 
            col < dungeon.mapWidth && dungeon.mapArray[col + 1, row] == dungeon.mapArray[player.col, player.row] ||
            row > 0 && dungeon.mapArray[col, row - 1] == dungeon.mapArray[player.col, player.row] ||
            row < dungeon.mapHeight && dungeon.mapArray[col, row + 1] == dungeon.mapArray[player.col, player.row];
   }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Stairs allows the player to move to next level upon contact.*/
public class Stairs : MapObject
{
    Player player;
    public static Stairs instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        name = "Stairs";
        player = Player.instance;
    }

    public override void PlaceObject(int col, int row)
    {
        Dungeon dungeon = Dungeon.instance;
        Player player = Player.instance;

        if (col < 0 || col >= dungeon.mapWidth) return;
        if (row < 0 || row >= dungeon.mapHeight) return;
        if (row == player.row && col == player.col) return;

        this.row = row;
        this.col = col;
        
        foreach(Node node in dungeon.nodes)
        {
            if (!node.isOccupied && node.row == row && node.col == col)
            {
                transform.position = new Vector3(node.transform.position.x, node.transform.position.y, node.transform.position.z);
                node.isOccupied = true;
                nodeID = node.nodeID;
                break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //player = Player.instance;
        if (!occupiedByEnemy && row == player.row && col == player.col)
        {
            //display a prompt asking player to advance to new level.
            Debug.Log("Found stairs");
        }
    }
}

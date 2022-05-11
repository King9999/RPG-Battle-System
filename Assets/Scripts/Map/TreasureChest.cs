using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Treasure chests contain loot, including equipment and money. The contents are random, and are pulled from Item Manager. */
public class TreasureChest : MapObject
{
    public Item heldItem;           //the item contained in this chest.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //When object is instantiated, this method must be run.
    public void GenerateLoot()
    {
        //determine item category to pull from.
        //from the category, pick a random item. Devise some rule for controlling what gets picked.
    }

    public void PlaceChest(int col, int row)
    {
        Dungeon dungeon = Dungeon.instance;
        Player player = Player.instance;
        Stairs exit = Stairs.instance;

        if (col < 0 || col >= dungeon.mapWidth) return;
        if (row < 0 || row >= dungeon.mapHeight) return;
        if (row == player.row && col == player.col) return;
        if (row == exit.row && col == exit.col) return;

        this.row = row;
        this.col = col;

        //find node
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
}

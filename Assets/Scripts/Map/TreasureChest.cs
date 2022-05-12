using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Treasure chests contain loot, including equipment and money. The contents are random, and are pulled from Item Manager. */
public class TreasureChest : MapObject
{
    public Item heldItem;           //the item contained in this chest.
    public Sprite emptyChestSprite;
    public Sprite closedChestSprite;

    //When object is instantiated, this method must be run.
    public void GenerateLoot(int tableLevel)
    {
        ItemManager im = ItemManager.instance;
        heldItem = im.GetItem(tableLevel);
    }

    public void AddItemToInventory()
    {
        if (heldItem == null) 
        {
            Debug.Log("Chest is empty!");
            return;
        }
        
        //add item to player inventory. Which inventory depends on item type.
        switch(heldItem.itemType)
        {
            case Item.ItemType.Weapon:
            case Item.ItemType.Armor:
            case Item.ItemType.Trinket:
                //add to equipment
                break;
            
            case Item.ItemType.Consumable:
                //add to consumable inventory
                break;
        }

        //show empty chest
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = emptyChestSprite;

    }

    public override void PlaceObject(int col, int row)
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

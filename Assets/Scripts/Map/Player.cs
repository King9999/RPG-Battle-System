using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script handles the player actions on the dungeon map. The player must click on a node adjacent to the avatar's current position
    to move to that node. The player sprite will be based on who they pick as their first hero. */
public class Player : MapObject
{
    //public int currentCol, currentRow;             //position in map array
    bool hasControl;                                //when false, no input is accepted.

    public static Player instance;

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
    protected override void Start()
    {
        base.Start();
        hasControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(int rowDestination, int colDestination)
    {
        //can only move to an adjacent node that has a path.
        row = rowDestination;
        col = colDestination;

        foreach(Node node in dungeon.nodes)
        {
            if (row == node.row && col == node.col)
            {
                transform.position = node.transform.position;
                break;
            }
        }
        
    }

    IEnumerator AnimateMovement()
    {
        yield return null;
    }
}

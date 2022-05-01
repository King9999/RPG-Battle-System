using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script handles the player actions on the dungeon map. The player must click on a node adjacent to the avatar's current position
    to move to that node. The player sprite will be based on who they pick as their first hero. */
public class Player : MonoBehaviour
{
    public int currentCol, currentRow;             //position in map array
    bool hasControl;                                //when false, no input is accepted.

    // Start is called before the first frame update
    void Start()
    {
        hasControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer()
    {
        //can only move to an adjacent node that has a path.
    }

    IEnumerator AnimateMovement()
    {
        yield return null;
    }
}

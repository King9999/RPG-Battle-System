using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Stairs allows the player to move to next level upon contact.*/
public class Stairs : MapObject
{
    Player player;

    void Start()
    {
        name = "Stairs";
        player = Player.instance;
    }
    // Update is called once per frame
    void Update()
    {
        //player = Player.instance;
        if (row == player.row && col == player.col)
        {
            //advance to new level.
        }
    }
}

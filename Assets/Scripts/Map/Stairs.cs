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
    // Update is called once per frame
    void Update()
    {
        //player = Player.instance;
        if (row == player.row && col == player.col)
        {
            //display a prompt asking player to advance to new level.
            Debug.Log("Found stairs");
        }
    }
}

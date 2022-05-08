using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Map enemies occupy nodes in a dungeon. They will move around a map after a specified number of turns, or they may always be stationary
(to protect something, for example). When an enemy is touched, combat is initiated. The enemies encountered can be random or they can be fixed. */
public class MapEnemy : MapObject
{
    public Sprite minorEnemySprite;
    public Sprite majorEnemySprite;
    public Sprite bossSprite;
    int turnsBeforeMoving;                          //how many times the player moves before this enemy moves. If this value is 0, the enemy always moves when player does.
    public bool isStationary {get; set;}            //if true, enemy does not move.

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //combat against random enemies. The encountered enemies are determined from a table
    public void InitiateCombat()
    {
        //check which enemies can be generated from a table.
        //get a random number of enemies, proportional to the number of heroes.
    }

    //used to battle specific enemies
    public void InitiateCombat(Enemy[] enemies)
    {
        //pick an enemy from Enemy Manager
    }

    public void SetTurnsToMove(int count)
    {
        turnsBeforeMoving = count;
    }
}

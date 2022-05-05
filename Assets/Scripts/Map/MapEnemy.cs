using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Map enemies occupy nodes in a dungeon. They will move around a map after a specified number of turns, or they may always be stationary
(to protect something, for example). When an enemy is touched, combat is initiated. The enemies encountered can be random or they can be fixed. */
public class MapEnemy : MapObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //combat against random enemies. The encountered enemies are determined from a table
    public void InitiateCombat()
    {
        
    }

    //used to battle specific enemies
    public void InitiateCombat(Enemy[] enemies)
    {

    }
}

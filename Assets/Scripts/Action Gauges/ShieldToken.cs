using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Shield tokens travel along the action gauge, moving in the opposite direction of action tokens. Not all enemies will have shield tokens. 
    They can have different sizes or the enemy can have more than 1, making the target more difficult to hit.
    
    Shield tokens will nullify any action an action token lands on, but they are damaged in the process. Once a shield is broken, the player 
    gets bonuses, such as all miss panels being removed, all normal panels becoming critical panels, an enemy losing their turn, etc. The bonuses the
    player receives are random.
    
     */
public class ShieldToken : ActionToken
{
    float defaultSpeed {get;} = 300;
    float shieldSize;       //affects the length of the shield. Default is 1.
    int hitPoints;      //3 hit points by default
    int defaultHitPoints {get;} = 3;
    public bool isEnabled {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        SetTokenSpeed(defaultSpeed);
        isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

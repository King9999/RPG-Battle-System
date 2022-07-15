using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sentry is a basic enemy with decent DFP and a shield token.
public class Sentry : Enemy
{
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  
    }

    public override void ExecuteLogic()
    {
        AttackRandomHero();

        //end turn
        base.ExecuteLogic();
    }
}

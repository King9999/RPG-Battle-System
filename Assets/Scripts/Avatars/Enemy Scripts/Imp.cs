using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//imp is a basic enemy. Its only skill is to run away once the hero is many levels above its own. 
public class Imp : Enemy
{
    
    protected override void Start()
    {
        base.Start();
        skillProb = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //time to run?
    }
}

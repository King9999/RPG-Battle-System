using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wizard primarily uses magic and will only use regular attacks if unable to cast.
public class Wizard : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        skillProb = 0.5f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTheirTurn)
        {
            //pick which spell to cast
            float roll = Random.Range(0, 1f);

            isTheirTurn = false;
        }
    }
}

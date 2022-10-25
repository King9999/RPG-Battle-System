using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rare enemy that grants a lot of XP and money if can be defeated. Has high DFP, high resists and 3 shield tokens. Runs away often.
public class Fortune : Enemy
{
    int runSkill = 0;
    
    protected override void Start()
    {
       base.Start();
       skillProb = 0.25f;  
    }

    public override void ExecuteLogic()
    {
        if (SkillActivated(skillProb))
        {
            //run away!
            skills[runSkill].Activate(this, skillNameBorderColor);
        }
        else
        {
            AttackRandomHero();
        }

        //end turn
        base.ExecuteLogic();
    }
}

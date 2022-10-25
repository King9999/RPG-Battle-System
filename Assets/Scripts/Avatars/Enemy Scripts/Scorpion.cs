using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can inflict poison.
public class Scorpion : Enemy
{
    int stingSkill = 0;
    
    protected override void Start()
    {
       base.Start();
       skillProb = 0.4f;  
    }

    public override void ExecuteLogic()
    {
        if (SkillActivated(skillProb))
        {
            //use Sting skill
            AttackRandomHero(skills[stingSkill]);
        }
        else
        {
            AttackRandomHero();
        }

        //end turn
        base.ExecuteLogic();
    }
}

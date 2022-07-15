using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//High resistance to cold damage, but weak to fire. Can reduce a target's DFP.
public class BlueSlime : Enemy
{
    
    int acidSkill = 0;
   // Start is called before the first frame update
   protected override void Start()
   {
       base.Start();
       skillProb = 0.6f;  
   }

    public override void ExecuteLogic()
    {
        if (SkillActivated(skillProb))
        {
            //use Acid skill
            AttackRandomHero(skills[acidSkill]);
        }
        else
        {
            AttackRandomHero();
        }
    }

}

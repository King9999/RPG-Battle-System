using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Wind Dancer is a speedy enemy that can also boost the speed of its allies, or reduce the heroes' speed */
public class WindDancer : Enemy
{
    int coolWindSkill = 0;
    int heavyWindSkill = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        skillProb = 0.7f;       //chance of executing cool wind
    }

    public override void ExecuteLogic()
    {
        if (SkillActivated(skillProb))
        {
            int randTarget = Random.Range(0, cs.enemiesInCombat.Count);
            skills[coolWindSkill].Activate(cs.enemiesInCombat[randTarget], skillNameBorderColor);
        }
        else
        {
            //use heavy wind skill
            int randTarget = Random.Range(0, cs.heroesInCombat.Count);
            Attack(cs.heroesInCombat[randTarget]);
        }
        base.ExecuteLogic();
    }
}

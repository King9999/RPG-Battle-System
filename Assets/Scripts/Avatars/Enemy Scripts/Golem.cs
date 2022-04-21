using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Golem is a slow but tanky and powerful opponent. They provide an introduction to the shield mechanic. */
public class Golem : Enemy
{
    int haymakerSkill = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        skillProb = 0.6f;      //chance that they attempt the haymaker skill
    }

    public override void ExecuteLogic()
    {
        //Once Golem's HP goes below 50%, each turn golem will attempt a haymaker
        int randHero = Random.Range(0, cs.heroesInCombat.Count);

        if (hitPoints < maxHitPoints * 0.6f && SkillActivated(skillProb))
        {
            skills[haymakerSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
        }
        else
        {
            Attack(cs.heroesInCombat[randHero]);
        }

        //end turn
        base.ExecuteLogic();
    }
}

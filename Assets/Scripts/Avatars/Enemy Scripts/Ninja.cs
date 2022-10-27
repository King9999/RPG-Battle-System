using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//High speed, and can inflict various status ailments
public class Ninja : Enemy
{
     int blindSkill = 0;
     int stunSkill = 1;
     float blindSkillChance = 0.3f;
     float stunSkillChance = 0.4f;
    
    protected override void Start()
    {
       base.Start();
    }

    public override void ExecuteLogic()
    {
        float roll = Random.value;
        if (roll <= blindSkillChance && manaPoints >= skills[blindSkill].manaCost)
        {
            //skills[blindSkill].Activate(this, cs.heroesInCombat, skillNameBorderColor);
            AttackAllHeroes(skills[blindSkill]);
        }
        else if (roll <= stunSkillChance && manaPoints >= skills[stunSkill].manaCost)
        {
            //skills[stunSkill].Activate(this, skillNameBorderColor);
            AttackRandomHero(skills[stunSkill]);
        }
        else
        {
            AttackRandomHero();
        }
        

        //end turn
        base.ExecuteLogic();
    }
}

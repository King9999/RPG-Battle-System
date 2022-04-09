using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wizard primarily uses magic and will only use regular attacks if unable to cast.
public class Wizard : Enemy
{
    int fireballSkill = 0;
    int healSkill = 1;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        skillProb = 0.35f;       //chance that wizard casts heal
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if (isTheirTurn)
        {
            //cast heal if hp is low
            if (hitPoints <= maxHitPoints * 0.3f)
            {
                float roll = Random.Range(0, 1f);
                if (roll <= skillProb)
                {
                    skills[healSkill].Activate(this, this, skillNameBorderColor);
                }
                else
                {
                    //cast fireball to a target
                    int randHero = Random.Range(0, cs.heroesInCombat.Count);
                    skills[fireballSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
                }
            }
            else
            {
                //cast fireball to a target
                int randHero = Random.Range(0, cs.heroesInCombat.Count);
                skills[fireballSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
            }

            //end turn
            PassTurn();
        }
    }*/

    public override void ExecuteLogic()
    {
        //base.ExecuteLogic();

        //if Wizard has no mana left, then attack.
        if (manaPoints >= skills[healSkill].manaCost || manaPoints >= skills[fireballSkill].manaCost)
        {
            //check if an ally needs healing, including self
            bool allyNeedsHealing = false;
            int i = 0;
            while (!allyNeedsHealing && i < cs.enemiesInCombat.Count)
            {   
                if (cs.enemiesInCombat[i].hitPoints <= cs.enemiesInCombat[i].maxHitPoints * 0.3f)
                {
                    allyNeedsHealing = true;
                }
                else
                {
                    i++;
                }
            }

            if (allyNeedsHealing)
            {
                float roll = Random.Range(0, 1f);
                if (roll <= skillProb)
                {
                    skills[healSkill].Activate(this, cs.enemiesInCombat[i], skillNameBorderColor);
                }
                else
                {
                    //cast fireball to a target
                    int randHero = Random.Range(0, cs.heroesInCombat.Count);
                    skills[fireballSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
                }
            }
            else
            {
                //cast fireball to a target
                int randHero = Random.Range(0, cs.heroesInCombat.Count);
                skills[fireballSkill].Activate(this, cs.heroesInCombat[randHero], skillNameBorderColor);
            }
        }
        else    //do regular attack
        {
            int randTarget = Random.Range(0, cs.heroesInCombat.Count);
            Attack(cs.heroesInCombat[randTarget]);
        }

        //end turn
        base.ExecuteLogic();
        //PassTurn();
    }
}

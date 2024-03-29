using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//imp is a basic enemy. Its only skill is to run away once the hero is many levels above its own. 
public class Imp : Enemy
{
    //CombatSystem cs;
    int averageLevel;
    bool levelChecked;
    int runSkill = 0;
    protected override void Start()
    {
        base.Start();
        skillProb = 0.5f;

        //Get the average level of the heroes. Run away skill is enabled once average level crosses a threshold
        averageLevel = 0;
        foreach(Hero hero in cs.heroesInCombat)
        {
            averageLevel += hero.level;
        }

        averageLevel /= cs.heroesInCombat.Count;
        //Debug.Log("Average level is " + averageLevel);
    }


    public override void ExecuteLogic()
    {
        if (!levelChecked)
            CheckHeroLevels();

        if (averageLevel >= 3)
        {
            //skill activation check
            //float roll = Random.Range(0, 1f);
            if (SkillActivated(skillProb))
            {
                //run away
                skills[runSkill].Activate(this, skillNameBorderColor);
            }
            else
            {
                /*int randHero;
                if (cs.heroesInCombat.Count > 1)
                {
                    do
                        randHero = Random.Range(0, cs.heroesInCombat.Count);
                    while (cs.heroesInCombat[randHero].status == Avatar.Status.Hidden);

                    Attack(cs.heroesInCombat[randHero]);
                }
                else
                {
                    Attack(cs.heroesInCombat[0]);
                }*/
                AttackRandomHero();
            }
        }
        else
        {
            /*int randHero;
            if (cs.heroesInCombat.Count > 1)
            {
                do
                    randHero = Random.Range(0, cs.heroesInCombat.Count);
                while (cs.heroesInCombat[randHero].status == Avatar.Status.Hidden);

                Attack(cs.heroesInCombat[randHero]);
            }
            else
            {
                Attack(cs.heroesInCombat[0]);
            }*/
            AttackRandomHero();
        }

        base.ExecuteLogic();
    }

    //checks hero levels to determine if imp will run away
    private void CheckHeroLevels()
    {
        averageLevel = 0;
        
        foreach(Hero hero in cs.heroesInCombat)
        {
            averageLevel += hero.level;
        }

        averageLevel /= cs.heroesInCombat.Count;
        levelChecked = true;
    }

    public override void ResetData()
    {
        base.ResetData();
        levelChecked = false;
    }

}

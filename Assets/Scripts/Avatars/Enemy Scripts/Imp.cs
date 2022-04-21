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
        skillProb = 0.4f;
        //status = Status.Blind;
        //cs = CombatSystem.instance;

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

        if (averageLevel >= 5)
        {
            //skill activation check
            float roll = Random.Range(0, 1f);
            if (roll <= skillProb)
            {
                //run away
                skills[runSkill].Activate(this, skillNameBorderColor);
            }
            else
            {
                Attack(cs.heroesInCombat[0]);
            }
        }
        else
        {
            //attack
            Attack(cs.heroesInCombat[0]);
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

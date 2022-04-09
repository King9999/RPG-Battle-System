using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//imp is a basic enemy. Its only skill is to run away once the hero is many levels above its own. 
public class Imp : Enemy
{
    //CombatSystem cs;
    int averageLevel;
    
    int runSkill = 0;
    protected override void Start()
    {
        base.Start();
        skillProb = 0.4f;
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

    // Update is called once per frame
    /*void Update()
    {
       if (isTheirTurn)
       {
            //Debug.Log("Imp's turn");
            //Imp will attempt to run if heroes' level is too high
            if (averageLevel >= 5)
            {
                //skill activation check
                float roll = Random.Range(0, 1f);
                if (roll <= skillProb)
                {
                    //run away
                    //Debug.Log("Running away!");
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

            PassTurn();
       }
    }*/

    public override void ExecuteLogic()
    {
        //base.ExecuteLogic();
        if (averageLevel >= 5)
        {
            //skill activation check
            float roll = Random.Range(0, 1f);
            if (roll <= skillProb)
            {
                //run away
                //Debug.Log("Running away!");
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
        //PassTurn();
    }

    public override void ResetData()
    {
        base.ResetData();
        //Must get average again in case hero levels have changed.
        averageLevel = 0;
        
        /*foreach(Hero hero in cs.heroesInCombat)
        {
            averageLevel += hero.level;
        }

        averageLevel /= cs.heroesInCombat.Count;
        Debug.Log("Average level is " + averageLevel);*/
    }
}

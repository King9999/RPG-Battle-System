using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//imp is a basic enemy. Its only skill is to run away once the hero is many levels above its own. 
public class Imp : Enemy
{
    CombatSystem cs;
    int averageLevel;
    //int heroCount;
    protected override void Start()
    {
        base.Start();
        skillProb = 0.5f;
        cs = CombatSystem.instance;

        if (cs == null)
        {
            Debug.Log("CS is null");
        }

        //Get the average level of the heroes. Run away skill is enabled once average level crosses a threshold
        averageLevel = 0;
        //heroCount = 0;          //need this to get the actual number of active heroes, and exclude any empty space in array.
        foreach(Hero hero in cs.heroes)
        {
            hero.level += averageLevel;
            //heroCount++;
        }

        averageLevel /= cs.heroes.Count;
        Debug.Log("Average level is " + averageLevel);
    }

    // Update is called once per frame
    void Update()
    {
        //time to run? Get the average level of the heroes
        /*int averageLevel = 0;
        int heroCount = 0;          //need this to get the actual number of active heroes, and exclude any empty space
        foreach(Hero hero in cs.heroes)
        {
            hero.level += averageLevel;
            heroCount++;
        }

        averageLevel /= heroCount;*/

       if (isTheirTurn)
       {
            Debug.Log("Imp's turn");
            if (averageLevel >= 5)
            {
                //skill activation check
                float roll = Random.Range(0, 1);
                if (roll <= skillProb)
                {
                    //run away
                    Debug.Log("Running away!");
                }
                else
                {
                    //attack
                }
            }
            else
            {
                //attack
            }
       }
    }
}

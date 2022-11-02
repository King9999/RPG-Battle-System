using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Titan is a major enemy who attacks after a countdown. Once the attack is performed, the countdown resets and attack power is increased.
   The attack level caps at level 5. Defeating Titan is an ordeal but the player can stun them to delay the attack. */
public class Titan : Enemy
{
    int quakeSkill = 5;
    int quakeLevel;
    
    int maxCountdown;
    int countdown;
    int countdownFive = 0;
    int countdownFour = 1;
    int countdownThree = 2;
    int countdownTwo = 3;
    int countdownOne = 4;

   protected override void Start()
   {
       base.Start();

       maxCountdown = 3;
       countdown = maxCountdown;
       quakeLevel = 1;
       
   }

    public override void ExecuteLogic()
    {
        /* Titan performs a countdown each turn, starting from 3. After the countdown ends, they do an earthquake attack.
            After the attack, the max countdown timer increases by 1 and the earthquake attack power rises.
        */
        
        if (countdown <= 0)
        {
            //quake skill
            AttackAllHeroes(skills[quakeSkill]);

            //raise max countdown and quake level
            maxCountdown++;
            quakeLevel++;
            countdown = maxCountdown;
        }
        else
        {
            switch(countdown)
            {
                case 5:
                    skills[countdownFive].Activate(this, skillNameBorderColor);
                    break;

                case 4:
                    skills[countdownFour].Activate(this, skillNameBorderColor);
                    break;

                case 3:
                    skills[countdownThree].Activate(this, skillNameBorderColor);
                    break;

                case 2:
                    skills[countdownTwo].Activate(this, skillNameBorderColor);
                    break;

                case 1:
                    skills[countdownOne].Activate(this, skillNameBorderColor);
                    break;
            }

            countdown--;
        }

        //end turn
        base.ExecuteLogic();
    }
}

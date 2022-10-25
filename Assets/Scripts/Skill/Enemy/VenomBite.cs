using UnityEngine;

//physical attack with a chance of poison. Hit chance is affected by target's speed.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Venom Bite", fileName = "skill_venombite")]
public class VenomBite : Skill
{
    
   public override void Activate(Avatar user, Avatar target, Color borderColor)
   {
       base.Activate(user, target, borderColor);
        //target receives damage
        user.Attack(target);

        //poison check
        float hitChance = user.spd * 2;
        if (!target.resistPoison)
        {
            float finalResult = (hitChance - target.spd) / 100;
            if (Random.value <= finalResult)
            {
                ui.DisplayStatusUpdate("POISONED", target.transform.position, 1);
                target.status = Avatar.Status.Poisoned;
            }
            /*else
            {
                ui.DisplayStatusUpdate("MISS", target.transform.position, 1);
            }*/
        }
        else
        {
            ui.DisplayStatusUpdate("POISON RESIST", target.transform.position, 1);
        }
   }
}

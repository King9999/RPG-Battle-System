using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hits a single target, then hits another random target for less damage.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Chain Lightning", fileName = "skill_chainLightning")]
public class ChainLightning : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
        
        ReduceMp(user);
        totalDamage = (user.mag * user.magMod) + power;
        totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.res * target.resMod) - (totalDamage * target.lightningResist);

        if (totalDamage >= 0)    
            user.ReduceHitPoints(target, Mathf.Round(totalDamage));
        else //healing
            user.RestoreHitPoints(target, Mathf.Round(totalDamage));
        
        //attack a random target for 50% less damage. Chain only works when there is more than 1 target.
        int randTarget;
        if (cs.heroesInCombat.Count > 1)
        {
            do
            {
                randTarget = Random.Range(0, cs.heroesInCombat.Count);
            }
            while (cs.heroesInCombat[randTarget] == target);

            Hero hero = cs.heroesInCombat[randTarget];
            totalDamage = (user.mag * user.magMod) + power;
            totalDamage += (Random.Range(0, totalDamage * 0.1f) - (hero.res * hero.resMod) - (totalDamage * hero.lightningResist)) / 2;

            if (totalDamage >= 0)    
                user.ReduceHitPoints(cs.heroesInCombat, randTarget, Mathf.Round(totalDamage));
            else //healing
                user.RestoreHitPoints(cs.heroesInCombat, randTarget, Mathf.Round(totalDamage));
        }
  
    }
}

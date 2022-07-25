using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hits all heroes for heavy fire damage.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Fire Storm", fileName = "skill_fireStorm")]
public class FireStorm : Skill
{
   public override void Activate(Avatar user, List<Hero> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        ReduceMp(user);

        for (int i = 0; i < targets.Count; i++)
        {
            totalDamage = (user.mag * user.magMod) + power;
            totalDamage += Random.Range(0, totalDamage * 0.1f) - (targets[i].res * targets[i].resMod) - (totalDamage * targets[i].fireResist);
    
            user.ReduceHitPoints(targets, i, Mathf.Round(totalDamage));
        }
       
    }
}

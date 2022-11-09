using System.Collections.Generic;
using UnityEngine;

//Titan's unique skill. There are multiple versions of this skill, each of them stronger than the last. Its power is based on user's ATP.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Quake", fileName = "skill_quake")]
public class Quake : Skill
{
    public override void Activate(Avatar user, List<Hero> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        for (int i = 0; i < targets.Count; i++)
        {
            totalDamage = (user.atp * user.atpMod) + power;
            totalDamage += Random.Range(0, totalDamage * 0.1f) - (targets[i].dfp * targets[i].dfpMod);
    
            user.ReduceHitPoints(targets, i, Mathf.Round(totalDamage));
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hits a single target for cold damage.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Ice Ball", fileName = "skill_iceBall")]
public class IceBall : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
        
        ReduceMp(user);
        totalDamage = (user.mag * user.magMod) + power;
        totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.res * target.resMod) - (totalDamage * target.coldResist);
  
        user.ReduceHitPoints(target, Mathf.Round(totalDamage));
  
    }
}

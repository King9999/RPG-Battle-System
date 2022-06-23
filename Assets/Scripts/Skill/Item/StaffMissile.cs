using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//staff skill that launchces a magical projectile at a target. Does not cost anything but only uses base MAG for damage.
[CreateAssetMenu(menuName = "Skill/Item Skill/Staff Missile", fileName = "skill_staffMissile")]
public class StaffMissile : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
                
        totalDamage = user.mag;
        totalDamage += Mathf.Round(Random.Range(0, totalDamage * 0.1f)) - (target.res * target.resMod);

        user.ReduceHitPoints(target, totalDamage);
    }
}

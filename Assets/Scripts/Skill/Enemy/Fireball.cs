using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//deals fire damage to one target
[CreateAssetMenu(menuName = "Skill/Offensive/Fireball", fileName = "skill_fireball")]
public class Fireball : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
        
        float totalCost = manaCost * user.mpMod;
        if (user.manaPoints < totalCost)
        {
            ui.DisplayStatusUpdate("NOT ENOUGH MANA", user.transform.position);
            return;
        }

        ReduceMp(user);
        totalDamage = (user.mag * user.magMod) + power;
        totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.res * target.resMod) - (totalDamage * target.fireResist);

        user.ReduceHitPoints(target, Mathf.Round(totalDamage));
    }
}

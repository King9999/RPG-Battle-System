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

        if (user.manaPoints < manaCost)
        {
            Debug.Log("Not enough mana!");
            return;
        }
        
        user.manaPoints -= manaCost;
        totalDamage = (user.mag * user.magMod) + power;
        totalDamage += Mathf.Round(Random.Range(0, totalDamage * 0.1f)) - target.res;

        //TODO: factor in target's affnity to fire
        user.ReduceHitPoints(target, totalDamage);
    }
}

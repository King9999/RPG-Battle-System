using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//restores HP to a single target
[CreateAssetMenu(menuName = "Skill/Recovery/Heal", fileName = "skill_heal")]
public class Heal : Skill
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
        Debug.Log(user.className + " is casting " + skillName);
        float amountRestored = (user.mag * user.magMod) + power;
        amountRestored += Mathf.Round(Random.Range(0, amountRestored * 0.1f));

        //target.hitPoints += amountRestored;
        user.RestoreHitPoints(target, amountRestored);
        
        Debug.Log(amountRestored + " HP restored to " + target.className);
    }
}

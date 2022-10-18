using UnityEngine;
using System.Collections.Generic;

//Non-elemental damaage to all enemies
[CreateAssetMenu(menuName = "Skill/Item Skill/Staff of Extinction", fileName = "skill_staffExtinction")]
public class StaffExtinction : Skill
{
    public override void Activate(Avatar user, List<Enemy> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        for (int i = 0; i < targets.Count; i++)
        {
            totalDamage = Mathf.Round(user.mag * 2.5f);
            totalDamage += Random.Range(0, totalDamage * 0.1f) - (targets[i].res * targets[i].resMod);

            user.ReduceHitPoints(targets, i, Mathf.Round(totalDamage));

        }
        
    }
}

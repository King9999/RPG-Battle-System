using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//restores 30% Hp when used
[CreateAssetMenu(menuName = "Skill/Item Skill/Herb", fileName = "skill_herb")]
public class Herb : Skill
{
    public override void Activate(Avatar target)
    {
        float healAmount = Mathf.Round(target.maxHitPoints * 0.3f);
        target.RestoreHitPoints(healAmount);
    }
}

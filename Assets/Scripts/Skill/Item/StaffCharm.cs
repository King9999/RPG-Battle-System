using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attempt to charm an enemy. Success rate is affected by target's RES
[CreateAssetMenu(menuName = "Skill/Item Skill/Staff Charm", fileName = "skill_staffCharm")]
public class StaffCharm : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        float charmChance = (user.mag - target.res) / 2;

        if (Random.value <= charmChance / 100)
        {
            target.status = Avatar.Status.Charmed;

            if (!target.skillEffects.Contains(this))
            {
                target.skillEffects.Add(this);
            }
        
            ui.DisplayStatusUpdate("CHARMED", target.transform.position);
        }
        else
            ui.DisplayStatusUpdate("MISS", target.transform.position);
    }

    public override void RemoveEffects(Avatar target)
    {
        target.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("CHARM REMOVED", target.transform.position);
    }
}

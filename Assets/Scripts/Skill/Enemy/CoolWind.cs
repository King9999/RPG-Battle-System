using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Target ally received SPD boost for a duration.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Cool Wind", fileName = "skill_coolwind")]
public class CoolWind : Skill
{
    public override void Activate(Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        
        if (target.spdMod < 1.5f)
        {
            target.spdMod = 1.5f;

            if (!target.skillEffects.Contains(this))
            {
                target.skillEffects.Add(this);
            }
        
            cs = CombatSystem.instance;
            cs.speedChanged = true;
            ui.DisplayStatusUpdate("SPD UP +50%", target.transform.position);
        }
        else
            ui.DisplayStatusUpdate("SPD MAX", target.transform.position);
    }

    public override void RemoveEffects(Avatar target)
    {
        target.spdMod = 1;
        ui.DisplayStatusUpdate("SPD BUFF END", target.transform.position);
    }
}

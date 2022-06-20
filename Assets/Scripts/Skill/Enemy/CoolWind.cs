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
        
        if (target.spdMod < target.minSpdMod + 0.5f)
        {
            target.spdMod = target.minSpdMod + 0.5f;

            //durationLeft = turnDuration;
            if (!target.skillEffects.ContainsKey(this))
            {
                target.skillEffects.Add(this, durationLeft);
            }
            else
            {
                target.skillEffects[this] = durationLeft;
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
        target.spdMod = target.minSpdMod;
        ui.DisplayStatusUpdate("SPD BUFF END", target.transform.position);
    }
}

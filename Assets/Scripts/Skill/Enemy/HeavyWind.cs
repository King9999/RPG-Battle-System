using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reduces target's SPD by 50%
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Heavy Wind", fileName = "skill_heavywind")]
public class HeavyWind : Skill
{
    public override void Activate(Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        
        if (target.spdMod > target.minSpdMod - 0.5f)
        {
            target.spdMod = target.minSpdMod - 0.5f;

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
            ui.DisplayStatusUpdate("SPD DOWN -50%", target.transform.position);
        }
        else
            ui.DisplayStatusUpdate("SPD MIN", target.transform.position);
    }

    public override void RemoveEffects(Avatar target)
    {
        target.spdMod = target.minSpdMod;
        ui.DisplayStatusUpdate("SPD DEBUFF END", target.transform.position);
    }
}

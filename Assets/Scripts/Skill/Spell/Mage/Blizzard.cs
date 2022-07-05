using UnityEngine;
using System.Collections.Generic;

//deals fire damage to one target. Crit panel causes all other enemies to be hit, but for less damage.
[CreateAssetMenu(menuName = "Skill/Mage/Blizzard", fileName = "skill_blizzard")]
public class Blizzard : Skill
{
    public override void Activate(Avatar user, List<Enemy> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        float stunChance = 0;
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;

        ReduceMp(user);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    stunChance = -1;
                    break;

                case ActionGauge.ActionValue.Critical:
                    stunChance = 0.65f;
                    break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                totalDamage = Mathf.Round(user.mag * user.magMod) + power;
                totalDamage += Mathf.Round(Random.Range(0, totalDamage * 0.1f) - (targets[i].res * targets[i].resMod) - 
                    (totalDamage * targets[i].coldResist));

                user.ReduceHitPoints(targets, i, totalDamage);

                //stun check
                if (!targets[i].resistParalysis)
                {
                    if (!targets[i].skillEffects.ContainsKey(this))
                    {
                        if (Random.value <= stunChance)
                        {
                            targets[i].status = Avatar.Status.Paralyzed;
                            durationLeft = turnDuration;
                            targets[i].skillEffects.Add(this, durationLeft);
                            ui.DisplayStatusUpdate(i, "STUNNED", targets[i].transform.position, delayDuration: 1);
                        }
                    }
                }
                else
                    ui.DisplayStatusUpdate(i, "STUN RESIST", targets[i].transform.position, delayDuration: 1);
                
            }

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("STUN END", user.transform.position);
    }
}

using UnityEngine;
using System.Collections.Generic;

//inflicts Blind status on all enemies. Crit panel increases success rate
[CreateAssetMenu(menuName = "Skill/Mage/Dark Mist", fileName = "skill_darkMist")]
public class DarkMist : Skill
{
    public override void Activate(Avatar user, List<Enemy> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        float baseChance = user.mag / 200;
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;

        ReduceMp(user);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    //no change
                    break;
                
                 case ActionGauge.ActionValue.Miss:
                    baseChance = 0;
                    break;

                case ActionGauge.ActionValue.Critical:
                    baseChance *= 1.5f;
                    break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                float finalChance = baseChance - (targets[i].res / 100);
                Debug.Log("Dark Mist hit chance vs " + targets[i].className + ": " + finalChance);
                //stun check
                if (targets[i].resistBlind)
                {
                    ui.DisplayStatusUpdate(i, "BLIND RESIST", targets[i].transform.position);
                }
                else if (Random.value <= finalChance)
                {
                    targets[i].status = Avatar.Status.Blind;
                    durationLeft = turnDuration;

                    if (!targets[i].skillEffects.ContainsKey(this))
                        targets[i].skillEffects.Add(this, durationLeft);
                    else
                        targets[i].skillEffects[this] = durationLeft;
                    
                    ui.DisplayStatusUpdate(i, "BLINDED", targets[i].transform.position);
                }
                else
                {
                    ui.DisplayStatusUpdate(i, "MISS", targets[i].transform.position);
                }
            }

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("BLIND END", user.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stuns a target for a duration. Crit panel stuns target for a longer period.
[CreateAssetMenu(menuName = "Skill/Rogue/Shadowbind", fileName = "skill_shadowbind")]
public class Shadowbind : Skill
{
   public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        //base.Activate(user, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float stunChance = user.spd * 2;
        bool landedOnCritPanel = false;
       
        ReduceMp(user);
        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {

                case ActionGauge.ActionValue.Miss:
                    stunChance = 0;
                    break;

                case ActionGauge.ActionValue.Normal:
                    //no change
                    break;

                case ActionGauge.ActionValue.Critical:
                    stunChance *= 1.5f;
                    landedOnCritPanel = true;
                    break;
                
            }

            float finalChance = (stunChance - target.spd) / 100;
            Debug.Log("Shadowbind chance vs " + target.className + ": " + finalChance);
            if (target.resistParalysis)
            {
                ui.DisplayStatusUpdate("STUN RESIST", target.transform.position);
            }
            else if (Random.value <= finalChance)
            {
                target.status = Avatar.Status.Paralyzed;
                if (!target.skillEffects.Contains(this))
                {
                    target.skillEffects.Add(this);
                }

                if (landedOnCritPanel)
                {
                    durationLeft = turnDuration + 2;
                }
                else
                {
                    durationLeft = turnDuration;
                }
                ui.DisplayStatusUpdate("STUNNED", target.transform.position);
            }
            else
            {
                ui.DisplayStatusUpdate("MISS", target.transform.position);
            }
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("STUN REMOVED", user.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Removes status ailments. Crit panel prevents ailments from being inflicted for a duration.
[CreateAssetMenu(menuName = "Skill/Cleric/Cleanse", fileName = "skill_cleanse")]
public class Cleanse : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        //base.Activate(user, target, borderColor);

        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
       
        ReduceMp(user);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    if (target.status != Avatar.Status.Dead && (target.status == Avatar.Status.Blind || target.status == Avatar.Status.Charmed
                        || target.status == Avatar.Status.Paralyzed || target.status == Avatar.Status.Poisoned))
                    {
                        target.status = Avatar.Status.Normal;
                        ui.DisplayStatusUpdate("AILMENT REMOVED", target.transform.position);
                    }
                    else
                    {
                        ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
                    }           
                    break;

                case ActionGauge.ActionValue.Critical:
                    if (target.status != Avatar.Status.Dead && (target.status == Avatar.Status.Blind || target.status == Avatar.Status.Charmed
                        || target.status == Avatar.Status.Paralyzed || target.status == Avatar.Status.Poisoned))
                    {
                        target.status = Avatar.Status.Normal;
                        ui.DisplayStatusUpdate("AILMENT REMOVED", target.transform.position);
                    }
                    else
                    {
                        ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
                    }

                    //apply cleanse effect
                    target.status = Avatar.Status.Cleansed;
                    ui.DisplayStatusUpdate("CLEANSED", target.transform.position, delayDuration: 1);
                    durationLeft = turnDuration;

                    if (!target.skillEffects.ContainsKey(this))
                    {
                        target.skillEffects.Add(this, durationLeft);
                    }
                    else
                    {
                        target.skillEffects[this] = durationLeft;
                    }  
                    break;
                
            }


            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("CLEANSE REMOVED", user.transform.position);
    }
}

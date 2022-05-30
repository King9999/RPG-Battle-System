using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reduces target's ATP temporarily, up to 30%.
[CreateAssetMenu(menuName = "Skill/Barbarian/Intimidate", fileName = "skill_intimidate")]
public class Intimidate : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        //base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float totalCost = manaCost * user.mpMod;

        if (user.manaPoints < totalCost)
        {
            ui.DisplayStatusUpdate("NOT ENOUGH MANA", user.transform.position);
            return;
        }

        ReduceMp(user);
        //user.manaPoints -= manaCost * user.mpMod;
        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Miss:
                    //nothing happens
                    ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
                    Debug.Log("No effect ");
                    break;

                case ActionGauge.ActionValue.Reduced:
                    target.atpMod = 0.9f;
                    durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("ATP DOWN", target.transform.position);
                    if (!target.skillEffects.Contains(this))
                    {
                        target.skillEffects.Add(this);
                    }
                    Debug.Log("ATP debuff, ATP is now " + target.atp * target.atpMod);
                    break;

                case ActionGauge.ActionValue.Critical:
                    target.atpMod = 0.7f;
                    durationLeft = turnDuration;
                    if (!target.skillEffects.Contains(this))
                    {
                        target.skillEffects.Add(this);
                    }
                    ui.DisplayStatusUpdate("ATP CRITICAL DOWN", target.transform.position);
                    Debug.Log("ATP debuff, ATP is now " + target.atp * target.atpMod);
                    break;
                
            }
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }

    public override void RemoveEffects(Avatar user)
    {
        user.atpMod = 1;
        ui.DisplayStatusUpdate("ATP DEBUFF END", user.transform.position);
    }
}

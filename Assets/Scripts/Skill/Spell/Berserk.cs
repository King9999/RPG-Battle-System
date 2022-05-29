using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Raises ATP by up to 80% (additive on top of any other buffs). However, DFP is reduced, and the Barb is uncontrollable until Berserk
//effect ends.
[CreateAssetMenu(menuName = "Skill/Offensive/Berserk", fileName = "skill_berserk")]
public class Berserk : Skill
{ 
   public override void Activate(Avatar user, Color borderColor)
    {
        //base.Activate(user, borderColor);
        ReduceMp(user);
        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
        CombatInputManager cim = CombatInputManager.instance;
        
        float atpValue = 0;
        float dfpValue = 0;     //this reduces DFP
        
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
               
                case ActionGauge.ActionValue.Reduced:
                    atpValue = 0.3f;
                    dfpValue = 0.1f;
                    durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("MIN BERSERK", user.transform.position);
                    if (!user.skillEffects.Contains(this))
                    {
                        user.skillEffects.Add(this);
                    }
                    //Debug.Log("Berserk minimal effect, ATP: " + user.atp * user.atpMod);
                    break;

                case ActionGauge.ActionValue.Normal:
                    atpValue = 0.5f;
                    dfpValue = 0.3f;
                    durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("BERSERK", user.transform.position);
                    if (!user.skillEffects.Contains(this))
                    {
                        user.skillEffects.Add(this);
                    }
                    //Debug.Log("Berserk regular effect, ATP: " + user.atp * user.atpMod);
                    break;

                case ActionGauge.ActionValue.Critical:
                    atpValue = 0.8f;
                    dfpValue = 0.7f;
                    durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("MAX BERSERK", user.transform.position);
                    if (!user.skillEffects.Contains(this))
                    {
                        user.skillEffects.Add(this);
                    }
                    //Debug.Log("Berserk critical effect, ATP: " + user.atp * user.atpMod);
                    break;
                
            }

            user.atpMod += atpValue;
            user.dfpMod -= dfpValue;
            user.status = Avatar.Status.Berserk;
            durationLeft = turnDuration;
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
         
    }

    public override void RemoveEffects(Avatar user)
    {
        user.atpMod = 1;
        user.dfpMod = 1;
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("BERSERK END", user.transform.position);
    }
}
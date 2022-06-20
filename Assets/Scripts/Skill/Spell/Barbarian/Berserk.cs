using UnityEngine;

//Raises ATP by up to 220% (additive on top of any other buffs). However, DFP is reduced, and the Barb is uncontrollable until Berserk
//effect ends.
[CreateAssetMenu(menuName = "Skill/Barbarian/Berserk", fileName = "skill_berserk")]
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
        CombatSystem cs = CombatSystem.instance;
        
        float atpValue = 0;
        float dfpValue = 0;     //this reduces DFP
        
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
               
                case ActionGauge.ActionValue.Reduced:
                    atpValue = 0.5f;
                    dfpValue = 0.1f;
                    //durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("MIN BERSERK", user.transform.position);
                    /*if (!user.skillEffects.ContainsKey(this))
                    {
                        user.skillEffects.Add(this, durationLeft);
                    }
                    else
                    {
                        user.skillEffects[this] = durationLeft;
                    }*/
                    break;

                case ActionGauge.ActionValue.Normal:
                    atpValue = 1f;
                    dfpValue = 0.3f;
                    //durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("BERSERK", user.transform.position);
                    /*if (!user.skillEffects.ContainsKey(this))
                    {
                        user.skillEffects.Add(this, durationLeft);
                    }
                    else
                    {
                        user.skillEffects[this] = durationLeft;
                    }*/
                    break;

                case ActionGauge.ActionValue.Critical:
                    atpValue = 2.2f;
                    dfpValue = 0.7f;
                    //durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("MAX BERSERK", user.transform.position);
                    /*if (!user.skillEffects.ContainsKey(this))
                    {
                        user.skillEffects.Add(this, durationLeft);
                    }
                    else
                    {
                        user.skillEffects[this] = durationLeft;
                    }*/
                    break;
                
            }

            user.atpMod = user.minAtpMod + atpValue;
            user.dfpMod = user.minDfpMod - dfpValue;
            user.status = Avatar.Status.Berserk;
            durationLeft = turnDuration;
            if (!user.skillEffects.ContainsKey(this))
            {
                user.skillEffects.Add(this, durationLeft);
            }
            else
            {
                user.skillEffects[this] = durationLeft;
            }
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
         
    }

    public override void RemoveEffects(Avatar user)
    {
        user.atpMod = user.minAtpMod;
        user.dfpMod = user.minDfpMod;
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("BERSERK END", user.transform.position);
    }
}
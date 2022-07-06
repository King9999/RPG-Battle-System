using UnityEngine;

/* Reduces target's DFP by 30%. Crit panel reduces RES by 30%. */
[CreateAssetMenu(menuName = "Skill/Mage/Corrode", fileName = "skill_corrode")]
public class Corrode : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        ReduceMp(user);
        
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        
        float dfpDebuffAmount = 0;
        float resDebuffAmount = 0;
        
        if (cim.buttonPressed)
        {
            if (!target.skillEffects.ContainsKey(this))
            {
                switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
                {
                    case ActionGauge.ActionValue.Reduced:
                        dfpDebuffAmount = 0.15f;
                        ui.DisplayStatusUpdate("DFP -15%", target.transform.position);
                        break;

                    case ActionGauge.ActionValue.Normal:
                        dfpDebuffAmount = 0.3f;
                        ui.DisplayStatusUpdate("DFP -30%", target.transform.position);
                        break;

                    case ActionGauge.ActionValue.Critical:
                        dfpDebuffAmount = 0.3f;
                        resDebuffAmount = 0.3f;
                        ui.DisplayStatusUpdate("DFP & RES -30%", target.transform.position);
                        break;
                    
                }

                //this skill is not refreshed if target is already being affected.
                durationLeft = turnDuration;
                target.skillEffects.Add(this, durationLeft);

                target.dfpMod = target.dfpMod - dfpDebuffAmount >= 0 ? target.dfpMod - dfpDebuffAmount : 0;
                target.resMod = target.resMod - resDebuffAmount >= 0 ? target.resMod - resDebuffAmount : 0;   
            }
            else
                ui.DisplayStatusUpdate("SKILL IN EFFECT", target.transform.position);
        
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
         
    }

    public override void RemoveEffects(Avatar user)
    {
        user.dfpMod = user.minDfpMod;
        user.resMod = user.minResMod;  
        ui.DisplayStatusUpdate("DFP/RES DEBUFF END", user.transform.position);
    }
}

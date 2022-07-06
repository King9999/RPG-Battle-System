using UnityEngine;

/* Raises target's ATP by up to 30%. Crit panel raises ATP by 50%. */
[CreateAssetMenu(menuName = "Skill/Mage/Sharpen", fileName = "skill_sharpen")]
public class Sharpen : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        ReduceMp(user);
        
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        
        float atpBuffAmount = 0;
        
        if (cim.buttonPressed)
        {
            if (!target.skillEffects.ContainsKey(this))
            {
                switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
                {
                    case ActionGauge.ActionValue.Reduced:
                        atpBuffAmount = 0.15f;
                        ui.DisplayStatusUpdate("ATP +15%", target.transform.position);
                        break;

                    case ActionGauge.ActionValue.Normal:
                        atpBuffAmount = 0.3f;
                        ui.DisplayStatusUpdate("ATP +30%", target.transform.position);
                        break;

                    case ActionGauge.ActionValue.Critical:
                        atpBuffAmount = 0.5f;
                        ui.DisplayStatusUpdate("ATP +50%", target.transform.position);
                        break;
                    
                }

                //this skill is not refreshed if target is already being affected.
                durationLeft = turnDuration;
                target.skillEffects.Add(this, durationLeft);

                target.atpMod += atpBuffAmount;
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
        user.atpMod = user.minAtpMod;
        ui.DisplayStatusUpdate("ATP BUFF END", user.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Raises DFP of entire party by 10%, up to 30%. Crit panel raises DFp by 15% */
[CreateAssetMenu(menuName = "Skill/Cleric/Holy Aura", fileName = "skill_holyAura")]
public class HolyAura : Skill
{
    public override void Activate(Avatar user, List<Hero> allies, Color borderColor)
    {
        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        
        float dfpValue = 0;     

        ReduceMp(user);

        
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
               
                case ActionGauge.ActionValue.Normal:
                    dfpValue = 0.1f;
                    break;

                case ActionGauge.ActionValue.Critical:
                    dfpValue = 0.15f;
                    break;
                
            }

            Vector3[] targetPos = new Vector3[allies.Count];
            string statusEffectMsg = dfpValue == 0.1f ? "DFP UP 10%" : "DFP UP 15%";

            for (int i = 0; i < allies.Count; i++)
            {
                if (allies[i].dfpMod < 1.3f)
                    allies[i].dfpMod += dfpValue;

                if (!allies[i].skillEffects.Contains(this))
                {
                    allies[i].skillEffects.Add(this);
                }              
                ui.DisplayStatusUpdate(i, statusEffectMsg, allies[i].transform.position);
               
            }

            durationLeft = turnDuration;
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
         
    }

    public override void RemoveEffects(Avatar user)
    {
        user.dfpMod = user.minDfpMod;
        ui.DisplayStatusUpdate("HOLY AURA END", user.transform.position);
    }
}

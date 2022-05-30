using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chance to instantly kill a target. Success depends on ATP between barb and the target.
[CreateAssetMenu(menuName = "Skill/Barbarian/Decapitate", fileName = "skill_decapitate")]
public class Decapitate : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float baseChance = user.atp;
       
        ReduceMp(user);
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Miss:
                    baseChance = 0;
                    break;

                case ActionGauge.ActionValue.Normal:
                    //no change
                    break;

                case ActionGauge.ActionValue.Reduced:
                    baseChance *= 0.5f;
                    break;

                case ActionGauge.ActionValue.Critical:
                    baseChance *= 1.5f;
                    break;
                
            }

            float finalRate = (baseChance - target.atp) / 100;
            Debug.Log("Decapitate chance vs " + target.className + ": " + finalRate);
            if (Random.value <= finalRate)
            {
                //death
                user.ReduceHitPoints(target, target.maxHitPoints);
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
}

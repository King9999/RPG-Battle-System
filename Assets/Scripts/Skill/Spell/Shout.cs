using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Barbarian skill. Chance to stun all enemies. success rate is increased if player lands on the critical panel on the action gauge.
//success rate is affected by enemy RES
[CreateAssetMenu(menuName = "Skill/Offensive/Shout", fileName = "skill_shout")]
public class Shout : Skill
{
   
    public override void Activate(Avatar user, List<Avatar> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);
        
        float hitChance = 0.5f;
        float rateMod = 0;  //becomes 1/3 if player lands on reduced panel, 0 if miss
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;

        if (user.manaPoints < manaCost)
        {
            ui.DisplayStatusUpdate("NOT ENOUGH MANA", user.transform.position);
            return;
        }
        
        user.manaPoints -= manaCost;
        if (user.TryGetComponent(out Hero hero))
        {
            hero.SetupActionGauge(cs.actGauge, actGaugeData);
        }
        
        float skillTokenSpeed = cs.actGauge.actionToken.TokenSpeed() * 2;
        cs.actGauge.actionToken.SetTokenSpeed(skillTokenSpeed);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Miss:
                    rateMod = 0;
                    break;

                case ActionGauge.ActionValue.Reduced:
                    rateMod = 0.33f;
                    break;

                case ActionGauge.ActionValue.Critical:
                    rateMod = 1;
                    break;
            }

            foreach(Avatar target in targets)
            {
                float newHitChance = (hitChance - (target.res / 100)) * rateMod;
                if (Random.value <= newHitChance)
                {
                    target.status = Avatar.Status.Paralyzed;
                    target.skillEffects.Add(this);
                    ui.DisplayStatusUpdate("STUNNED", target.transform.position);
                }
                else
                {
                    ui.DisplayStatusUpdate("MISS", target.transform.position);
                } 
            }
        }
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("CHARM REMOVED", user.transform.position);
    }
}

using UnityEngine;
using System.Collections.Generic;

//Restores HP to all heroes. Crit panel restores more HP.
[CreateAssetMenu(menuName = "Skill/Cleric/Great Heal", fileName = "skill_greatHeal")]
public class GreatHeal : Skill
{
    public override void Activate(Avatar user, List<Hero> allies, Color borderColor)
    {
        base.Activate(user, allies, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
       
        ReduceMp(user);

        float healAmount = 0;
        bool landedOnCritPanel = false;

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    //healAmount = user.mag + power;
                    //healAmount += Mathf.Round(Random.Range(0, healAmount * 0.1f));           
                    break;

                case ActionGauge.ActionValue.Critical:
                    landedOnCritPanel = true;
                    //healAmount = (user.mag + power) * 1.5f;
                    //healAmount += Mathf.Round(Random.Range(0, healAmount * 0.1f));  
                    break;
                
            }

            for (int i = 0; i < allies.Count; i++)
            {
                healAmount = landedOnCritPanel ? (user.mag * user.magMod * 1.5f) + power : (user.mag * user.magMod) + power;
                healAmount += Random.Range(0, healAmount * 0.1f);
                user.RestoreHitPoints(allies, i, Mathf.Round(healAmount)); 
            }
           

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}

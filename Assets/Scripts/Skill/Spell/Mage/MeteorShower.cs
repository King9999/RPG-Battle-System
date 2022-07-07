using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//deals non-elemental damage to a random target 3 times. Crit panel raises number of meteors to 5.
[CreateAssetMenu(menuName = "Skill/Mage/Meteor Shower", fileName = "skill_meteorShower")]
public class MeteorShower : Skill
{
    public override void Activate(Avatar user, List<Enemy> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        int meteorCount = 0;

        ReduceMp(user);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    meteorCount = 3;
                    break;

                case ActionGauge.ActionValue.Critical:
                    meteorCount = 5;
                    break;
            }

            for (int i = 0; i < meteorCount; i++)
            {
                int randTarget = Random.Range(0, cs.enemiesInCombat.Count);

                totalDamage = (user.mag * user.magMod) + power;
                totalDamage += Random.Range(0, totalDamage * 0.1f) - (targets[randTarget].res * targets[randTarget].resMod);

                user.ReduceHitPoints(targets, randTarget, Mathf.Round(totalDamage));
                
            }

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
    }
}

using UnityEngine;

//Restores HP to a target. Crit panel restores more HP.
[CreateAssetMenu(menuName = "Skill/Cleric/Healing Light", fileName = "skill_healingLight")]
public class HealingLight : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
       
        ReduceMp(user);

        float healAmount = 0;

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    healAmount = user.mag + power;
                    healAmount += Random.Range(0, healAmount * 0.1f);           
                    break;

                case ActionGauge.ActionValue.Critical:
                    if (target.hpMod < target.minHpMod + 0.2f)
                    {
                        target.hpMod += 0.3f;
                        //target.originalMaxHp = target.maxHitPoints;
                        //target.maxHitPoints = Mathf.Round(target.maxHitPoints * target.hpMod);
                    }
                    //healAmount = (user.mag + power) * 1.5f;
                    healAmount =(user.mag * user.magMod) + power;
                    healAmount += Random.Range(0, healAmount * 0.1f);  
                    break;
                
            }

            user.RestoreHitPoints(target, Mathf.Round(healAmount));

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}

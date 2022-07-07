using UnityEngine;

//deals fire damage to one target. Crit panel causes all other enemies to be hit, but for less damage.
[CreateAssetMenu(menuName = "Skill/Mage/Fire Bolt", fileName = "skill_fireBolt")]
public class FireBolt : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        bool landedOnCritPanel = false;
       
        ReduceMp(user);
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {

                case ActionGauge.ActionValue.Normal:
                    //no change
                    break;

                case ActionGauge.ActionValue.Critical:
                    landedOnCritPanel = true;
                    break;
                
            }

            totalDamage = (user.mag * user.magMod) + power;
            totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.res * target.resMod) - (totalDamage * target.fireResist);
            
            user.ReduceHitPoints(target, Mathf.Round(totalDamage));

            //deal damage to other targets
            if (landedOnCritPanel)
            {
                float splashDamage;
                for (int i = 0; i < cs.enemiesInCombat.Count; i++)
                {
                    if (cs.enemiesInCombat[i] != target)
                    {
                        Enemy enemy = cs.enemiesInCombat[i];
                        splashDamage = Mathf.Round((totalDamage + (Random.Range(0, totalDamage * 0.1f) - (enemy.res * enemy.resMod) - 
                            (totalDamage * enemy.fireResist))) / 3);
                        user.ReduceHitPoints(cs.enemiesInCombat, i, splashDamage);
                    }
                }
            }


            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}

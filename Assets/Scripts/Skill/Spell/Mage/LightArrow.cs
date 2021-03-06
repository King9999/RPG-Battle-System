using UnityEngine;

//deals damage to one target. Crit panel increases damage dealt
[CreateAssetMenu(menuName = "Skill/Mage/Light Arrow", fileName = "skill_lightArrow")]
public class LightArrow : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float dmgMod = 1;
       
        ReduceMp(user);
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {

                case ActionGauge.ActionValue.Normal:
                    //no change
                    break;

                case ActionGauge.ActionValue.Reduced:
                    dmgMod = 0.7f;
                    break;

                case ActionGauge.ActionValue.Critical:
                    dmgMod = 2;
                    ui.damageDisplay.color = ui.criticalDamageColor;
                    break;
                
            }

            totalDamage = (user.mag + power) * dmgMod;
            totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.res * target.resMod);
            
            user.ReduceHitPoints(target, Mathf.Round(totalDamage));

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}

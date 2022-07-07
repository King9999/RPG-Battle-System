using UnityEngine;

//deals lightning damage to one target. Crit panel causes stun with a long duration.
[CreateAssetMenu(menuName = "Skill/Mage/Thunder Strike", fileName = "skill_thunderStrike")]
public class ThunderStrike : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        bool landedOnCritPanel = false;
        float baseStunChance = user.mag / 100;
       
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
            totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.res * target.resMod) - (totalDamage * target.lightningResist);
            
            user.ReduceHitPoints(target, Mathf.Round(totalDamage));

            //stun check
            if (landedOnCritPanel)
            {                
                if (!target.resistParalysis)
                {
                    if (!target.skillEffects.ContainsKey(this))
                    {
                        float finalStunChance = baseStunChance - (target.res / 100);
                        Debug.Log("Thunder Strike stun chance vs " + target.className + ": " + finalStunChance);
                        if (Random.value <= finalStunChance)
                        {
                            target.status = Avatar.Status.Paralyzed;
                            durationLeft = turnDuration;
                            target.skillEffects.Add(this, durationLeft);
                            ui.DisplayStatusUpdate("STUNNED", target.transform.position, delayDuration: 1);
                        }
                    }
                }
                else
                    ui.DisplayStatusUpdate("STUN RESIST", target.transform.position, delayDuration: 1);
            }


            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}

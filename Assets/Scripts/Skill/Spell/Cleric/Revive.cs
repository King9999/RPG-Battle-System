using UnityEngine;

//Brings a dead hero back to life with some HP. Crit panel restores 65% HP.
[CreateAssetMenu(menuName = "Skill/Cleric/Revive", fileName = "skill_revive")]
public class Revive : Skill
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
                    if (target.status == Avatar.Status.Dead)
                    {
                        target.status = Avatar.Status.Normal;
                        healAmount = target.maxHitPoints * 0.33f;
                        healAmount += Random.Range(0, healAmount * 0.1f);
                        user.RestoreHitPoints(target, Mathf.Round(healAmount));
                    }
                    else
                    {
                        ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
                    }           
                    break;

                case ActionGauge.ActionValue.Critical:
                    if (target.status == Avatar.Status.Dead)
                    {
                        target.status = Avatar.Status.Normal;
                        healAmount = target.maxHitPoints * 0.65f;
                        healAmount += Random.Range(0, healAmount * 0.1f);
                        user.RestoreHitPoints(target, Mathf.Round(healAmount));
                    }
                    else
                    {
                        ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
                    }  
                    break;
                
            }

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }
}

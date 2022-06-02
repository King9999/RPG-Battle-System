using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cures Barbarian of poison & blind. Landing on critical panel restores a little HP.
[CreateAssetMenu(menuName = "Skill/Barbarian/Remedy", fileName = "skill_remedy")]
public class Remedy : Skill
{
    public override void Activate(Avatar user, Color borderColor)
    {
        base.Activate(user, borderColor);

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
       
        ReduceMp(user);
        //skillActivated = true;
        //ui = UI.instance;
        //skillNameBorderColor = borderColor;
        //ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {

                case ActionGauge.ActionValue.Normal:
                    if (user.status == Avatar.Status.Blind || user.status == Avatar.Status.Poisoned)
                        ui.DisplayStatusUpdate("BLIND/POISON REMOVED", user.transform.position);
                    else
                        ui.DisplayStatusUpdate("NO EFFECT", user.transform.position);                   
                    break;

                case ActionGauge.ActionValue.Critical:
                    if (user.status == Avatar.Status.Blind || user.status == Avatar.Status.Poisoned)
                        ui.DisplayStatusUpdate("BLIND/POISON REMOVED", user.transform.position);
                    else
                        ui.DisplayStatusUpdate("NO EFFECT", user.transform.position);

                    //restore 15% HP
                    float healAmount = Mathf.Round(user.maxHitPoints * 0.15f);
                    healAmount += Mathf.Round(Random.Range(0, healAmount * 0.1f));

                    user.RestoreHitPoints(user, healAmount, delayDuration: 1);
                    break;
                
            }
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }

}

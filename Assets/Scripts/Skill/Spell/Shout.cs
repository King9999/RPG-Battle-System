using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Barbarian skill. Chance to stun all enemies. success rate is increased if player lands on the critical panel on the action gauge.
//success rate is affected by enemy RES
[CreateAssetMenu(menuName = "Skill/Offensive/Shout", fileName = "skill_shout")]
public class Shout : Skill
{
   
    public override void Activate(Avatar user, List<Enemy> targets, Color borderColor)
    {
        //base.Activate(user, targets, borderColor);

        
        
        float hitChance = 0.5f;
        float rateMod = 0;  //becomes 1/3 if player lands on reduced panel, 0 if miss
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float totalCost = manaCost * user.mpMod;

        if (user.manaPoints < totalCost)
        {
            ui.DisplayStatusUpdate("NOT ENOUGH MANA", user.transform.position);
            return;
        }
        
        ReduceMp(user);

        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
        

        if (cim.buttonPressed)
        {
            //cs.actGauge.actionToken.StopToken();
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

            string[] uiMessage = new string[targets.Count];
            Vector3[] targetPos = new Vector3[targets.Count];
            for (int i = 0; i < targets.Count; i++)
            {
                float newHitChance = (hitChance - (targets[i].res / 100)) * rateMod;
                Debug.Log("Shout Chance " + newHitChance);
                if (Random.value <= newHitChance)
                {
                    targets[i].status = Avatar.Status.Paralyzed;
                    targets[i].skillEffects.Add(this);
                    uiMessage[i] = "STUNNED";
                    targetPos[i] = targets[i].transform.position;
                    ui.DisplayStatusUpdate(i, "STUNNED", targets[i].transform.position);
                }
                else
                {
                    uiMessage[i] = "MISS";
                    targetPos[i] = targets[i].transform.position;
                    ui.DisplayStatusUpdate(i, "MISS", targets[i].transform.position);
                } 
            }

            //ui.DisplayStatusUpdate(targets, uiMessage, targetPos);

            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("STUN REMOVED", user.transform.position);
    }
}

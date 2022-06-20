using UnityEngine;

//Charm a target for a duration. 
[CreateAssetMenu(menuName = "Skill/Rogue/Persuade", fileName = "skill_persuade")]
public class Persuade : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {

        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        float charmChance = user.spd * 2;
        bool landedOnCritPanel = false;
       
        ReduceMp(user);
        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);

        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Miss:
                    charmChance = 0;
                    break;

                case ActionGauge.ActionValue.Normal:
                    //no change
                    break;

                case ActionGauge.ActionValue.Critical:
                    charmChance *= 1.5f;
                    landedOnCritPanel = true;
                    break;
                
            }

            float finalChance = (charmChance - target.spd) / 100;
            Debug.Log("Charm chance vs " + target.className + ": " + finalChance);
            if (target.resistCharm)
            {
                ui.DisplayStatusUpdate("CHARM RESIST", target.transform.position);
            }
            else if (Random.value <= finalChance)
            {
                target.status = Avatar.Status.Charmed;
                durationLeft = landedOnCritPanel ? turnDuration + 2 : turnDuration;
                if (!target.skillEffects.ContainsKey(this))
                {
                    target.skillEffects.Add(this, durationLeft);
                }
                else
                {
                    target.skillEffects[this] = durationLeft;
                }

                /*if (landedOnCritPanel)
                {
                    durationLeft = turnDuration + 2;
                }
                else
                {
                    durationLeft = turnDuration;
                }*/
                ui.DisplayStatusUpdate("CHARMED", target.transform.position);
            }
            else
            {
                ui.DisplayStatusUpdate("MISS", target.transform.position);
            }
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("CHARM REMOVED", user.transform.position);
    }
}

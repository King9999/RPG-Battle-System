using UnityEngine;

//Rogue becomes untargetable and damage is reduced to 0. After coming out of hiding, Rogue get bonus critical panels.
//Landing on normal panel transforms all normal panels to crit panels. Landing on crit panel transforms reduced panels to crit panels.
[CreateAssetMenu(menuName = "Skill/Rogue/Hide", fileName = "skill_hide")]
public class Hide : Skill
{
    bool landedOnCritPanel = false;

    public override void Activate(Avatar user, Color borderColor)
    {
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
       
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
                    //hide fails
                    ui.DisplayStatusUpdate("HIDE FAILED", user.transform.position);
                    break;

                case ActionGauge.ActionValue.Normal:
                    user.status = Avatar.Status.Hidden;
                    user.resMod = 1000;
                    user.dfpMod = 1000;             //prevents all damage.
                    durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("HIDDEN", user.transform.position);
                    if (!user.skillEffects.Contains(this))
                    {
                        user.skillEffects.Add(this);
                    }
                    break;

                case ActionGauge.ActionValue.Critical:
                    user.status = Avatar.Status.Hidden;
                    user.resMod = 1000;
                    user.dfpMod = 1000;
                    landedOnCritPanel = true;
                    durationLeft = turnDuration;
                    ui.DisplayStatusUpdate("HIDDEN", user.transform.position);
                    if (!user.skillEffects.Contains(this))
                    {
                        user.skillEffects.Add(this);
                    }
                    break;
                
            }
           
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
              
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.HideBuffInEffect;
        if (user.TryGetComponent(out Hero hero))
            hero.landedOnCritPanel = landedOnCritPanel;
       
        user.resMod = 1;
        user.dfpMod = 1;
        ui.DisplayStatusUpdate("HIDE REMOVED", user.transform.position);
    }
}

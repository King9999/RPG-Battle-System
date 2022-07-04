using UnityEngine;

/* Increases target's SPD by 30%. Crit panel raises SPD by 50%.*/
[CreateAssetMenu(menuName = "Skill/Mage/Haste", fileName = "skill_haste")]
public class Haste : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        ReduceMp(user);
        /*skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);*/
        CombatInputManager cim = CombatInputManager.instance;
        CombatSystem cs = CombatSystem.instance;
        
        float spdValue = 0;
        
        if (cim.buttonPressed)
        {
            switch(cs.actGauge.actionValues[cs.actGauge.currentIndex])
            {
                case ActionGauge.ActionValue.Normal:
                    spdValue = 0.3f;
                    ui.DisplayStatusUpdate("SPD UP + 30%", target.transform.position);
                    break;

                case ActionGauge.ActionValue.Critical:
                   spdValue = 0.5f;
                    ui.DisplayStatusUpdate("SPD UP +50%", target.transform.position);
                    break;
                
            }

            target.spdMod = target.minSpdMod + spdValue;
            durationLeft = turnDuration;
            if (!target.skillEffects.ContainsKey(this))
            {
                target.skillEffects.Add(this, durationLeft);
            }
            else
            {
                target.skillEffects[this] = durationLeft;
            }

            //find target in the turn order list and then change their position
            Avatar avatar;
            for(int i = 0; i < cs.turnOrder.Count; i++)
            {
                avatar = cs.turnOrder[i];
                if (avatar == target)
                {
                    Debug.Log("Targeted " + avatar.className);
                    //check all other avatars ahead of target and compare speeds
                    if (i == 0)
                    {
                        //target is currently taking their turn. Move them forward after they take their turn.
                        //target.spd *= target.spdMod;
                        cs.speedChanged = true; 
                        break;   
                    }
                    else
                    {
                        int x = i;
                        //avatar.spd *= target.spdMod;
                        bool greaterSpdFound = false; 
                        while (x > 1 && !greaterSpdFound)   //x is > 1 because we can't take over the avatar who's currently taking their turn.
                        {
                            //compare speeds
                            float currentAvatarSpd = cs.turnOrder[x - 1].spd * cs.turnOrder[x - 1].spdMod;
                            if (avatar.spd * target.spdMod > currentAvatarSpd)
                            {
                                //move the target ahead
                                Avatar temp = cs.turnOrder[x - 1];
                                cs.turnOrder[x - 1] = avatar;
                                cs.turnOrder[x] = temp;
                                x--;
                            }
                            else
                            {
                                greaterSpdFound = true;
                            }
                        }

                        //cs.UpdateTurnOrderUI();
                        break;
                    }

                }
            }
            
            //need to do this step to end turn.
            if (user.TryGetComponent(out Hero hero))
                hero.currentActions++;
        }
         
    }

    public override void RemoveEffects(Avatar user)
    {
        user.spdMod = user.minSpdMod;
        ui.DisplayStatusUpdate("SPD BUFF END", user.transform.position);
    }
}

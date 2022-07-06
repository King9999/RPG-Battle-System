using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Target ally receives SPD boost for a duration. Target's position in turn order changes.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Cool Wind", fileName = "skill_coolwind")]
public class CoolWind : Skill
{
    public override void Activate(Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        cs = CombatSystem.instance;

        
        if (!target.skillEffects.ContainsKey(this))
        {
            durationLeft = turnDuration;
            target.skillEffects.Add(this, durationLeft);
            target.spdMod = target.minSpdMod + 0.5f;
            ui.DisplayStatusUpdate("SPD +50%", target.transform.position);
            
            //find target in the turn order list and then change their position
            Avatar avatar;
            for(int i = 0; i < cs.turnOrder.Count; i++)
            {
                avatar = cs.turnOrder[i];
                if (avatar == target)
                {
                    //check all other avatars ahead of target and compare speeds
                    if (i == 0)
                    {
                        //target is currently taking their turn. Move them forward after they take their turn.
                        cs.speedChanged = true; 
                        break;   
                    }
                    else
                    {
                        int x = i;
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

                        break;
                    }

                }
            }    
        }
        else
            ui.DisplayStatusUpdate("SKILL IN EFFECT", target.transform.position);
    }

    public override void RemoveEffects(Avatar target)
    {
        target.spdMod = target.minSpdMod;
        ui.DisplayStatusUpdate("SPD BUFF END", target.transform.position);
    }
}

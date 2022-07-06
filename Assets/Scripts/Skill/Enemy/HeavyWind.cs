using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reduces target's SPD by 50%
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Heavy Wind", fileName = "skill_heavywind")]
public class HeavyWind : Skill
{
    public override void Activate(Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);

         cs = CombatSystem.instance;
        
        if (!target.skillEffects.ContainsKey(this))
        {
            durationLeft = turnDuration;
            target.skillEffects.Add(this, durationLeft);
            target.spdMod = target.minSpdMod - 0.5f;
            ui.DisplayStatusUpdate("SPD -50%", target.transform.position);
        
           
            //find target in the turn order list and then change their position
            Avatar avatar;
            for(int i = 0; i < cs.turnOrder.Count; i++)
            {
                avatar = cs.turnOrder[i];
                if (avatar == target)
                {
                    //check all other avatars ahead of target and compare speeds
                    int x = i;
                    bool lowerSpdFound = false; 
                    while (x < cs.turnOrder.Count - 1 && !lowerSpdFound)
                    {
                        //compare speeds
                        float currentAvatarSpd = cs.turnOrder[x + 1].spd * cs.turnOrder[x + 1].spdMod;
                        if (avatar.spd * target.spdMod < currentAvatarSpd)
                        {
                            //the two avatars swap places, with the target moving back in the queue
                            Avatar temp = cs.turnOrder[x + 1];
                            cs.turnOrder[x + 1] = avatar;
                            cs.turnOrder[x] = temp;
                            x++;
                        }
                        else
                        {
                            lowerSpdFound = true;
                        }
                    }
                    break;
                }
            }
            
        }
        else
            ui.DisplayStatusUpdate("SKILL IN EFFECT", target.transform.position);
    }

    public override void RemoveEffects(Avatar target)
    {
        target.spdMod = target.minSpdMod;
        ui.DisplayStatusUpdate("SPD DEBUFF END", target.transform.position);
    }
}

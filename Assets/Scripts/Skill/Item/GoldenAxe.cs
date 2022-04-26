using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//increases user's ATP by 10%. Effect is stackable up to 30% and lasts for 2 turns
[CreateAssetMenu(menuName = "Skill/Item Skill/Golden Axe", fileName = "skill_goldenAxe")]
public class GoldenAxe : Skill
{ 
   public override void Activate(Avatar user, Color borderColor)
    {
        //base.Activate(user, borderColor);
        skillActivated = true;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
        
        if (user.atpMod < 1.3f)
        {
            user.atpMod += 0.1f;
            Debug.Log("ATP buff, ATP is now " + user.atp * user.atpMod);
            ui.DisplayStatusUpdate("ATP UP +10%", user.transform.position);
            durationLeft = turnDuration;
        }
        else
        {
            ui.DisplayStatusUpdate("MAXED OUT", user.transform.position);
        }   
    }

    public override void RemoveEffects(Avatar user)
    {
        user.atpMod = 1;
        ui.DisplayStatusUpdate("BUFF ENDED", user.transform.position);
    }


}

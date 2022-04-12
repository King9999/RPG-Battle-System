using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//increases user's ATP by 10%. Effect is stackable up to 30% and lasts until an attack is made.
[CreateAssetMenu(menuName = "Skill/Item Skill/Golden Axe", fileName = "skill_goldenAxe")]
public class GoldenAxe : Skill
{ 
   public override void Activate(Avatar user, Color borderColor)
    {
        base.Activate(user, borderColor);
        
        if (user.atpMod < 1.3f)
        {
            user.atpMod += 0.1f;
            Debug.Log("ATP buff, ATP is now " + user.atp * user.atpMod);
        }
        
    }


}

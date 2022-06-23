using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//restores HP to self.
[CreateAssetMenu(menuName = "Skill/Item Skill/Staff Heal", fileName = "skill_staffHeal")]
public class StaffHeal : Skill
{
   public override void Activate(Avatar self, Color borderColor)
    {
        base.Activate(self, borderColor);
        float healAmount = Mathf.Round(self.mag + Random.Range(0, self.mag * 0.1f)); 
        self.RestoreHitPoints(self, healAmount);
    }
}

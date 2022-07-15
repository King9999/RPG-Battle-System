using UnityEngine;

//Reduce target DFP in addition to doing a small amount of damage
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Acid", fileName = "skill_acid")]
public class Acid : Skill
{
   public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        float hitChance = 0.55f;    //chance to reduce DFP

        totalDamage = ((user.atp * user.atpMod) + power) / 2;
        totalDamage += Random.Range(0, totalDamage * 0.1f) - (target.dfp * target.dfpMod);
        user.ReduceHitPoints(target, Mathf.Round(totalDamage));

        if (!target.skillEffects.ContainsKey(this))
        {
            if (Random.value <= hitChance)
            {
                float dfpDebuffAmount = target.dfp * 0.15f;
                durationLeft = turnDuration;
                target.dfp -= Mathf.Round(dfpDebuffAmount);
                target.skillEffects.Add(this, durationLeft);
                ui.DisplayStatusUpdate("DFP -15%", target.transform.position, delayDuration: 1); 
            }
        }
        
    }
}

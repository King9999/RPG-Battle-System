using UnityEngine;

//25% of damage dealt heals the user.
[CreateAssetMenu(menuName = "Skill/Item Skill/Blood Sword", fileName = "skill_bloodSword")]
public class BloodSword : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
        
        //5% chance to inflict a critical. Criticals ignore defense.
        float totalDamage;
        float critChance = 0.05f;

        if (Random.value <= critChance)
        {
            totalDamage = Mathf.Round(user.atp * user.atpMod * 1.5f + Random.Range(0, user.atp * 1.5f * 0.1f));
            ui.damageDisplay.color = ui.criticalDamageColor;
        }
        else
        {
            totalDamage = Mathf.Round(user.atp * user.atpMod + Random.Range(0, user.atp * 0.1f) - (target.dfp * target.dfpMod));
            ui.damageDisplay.color = ui.damageColor;
        }

        //if player is blind, high chance they do 0 damage
        if (user.status == Avatar.Status.Blind)
        {
            float blindHitChance = 0.2f;
            if (Random.value > blindHitChance)
            {
                totalDamage = 0;
            }
        }
        
        if (totalDamage < 0)
            totalDamage = 0;


        //deal damage then heal user.
        float healAmount = Mathf.Round(totalDamage * 0.25f);
        user.ReduceHitPoints(target, totalDamage);
        user.RestoreHitPoints(user, healAmount, 1);
        
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Item Skill/Killer Bow", fileName = "skill_killerBow")]
public class KillerBow : Skill
{
     public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
        
        //5% chance to inflict a critical. Criticals ignore defense. 100% chance to kill if critical
        float totalDamage;
        float critChance = 0.05f;
        bool critLanded = false;

        if (Random.value <= critChance)
        {
            totalDamage = Mathf.Round(user.atp * user.atpMod * 1.5f + Random.Range(0, user.atp * 1.5f * 0.1f));
            ui.damageDisplay.color = ui.criticalDamageColor;
            critLanded = true;
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

        //Instant death check. Cannot kill if damage is 0
        user.ReduceHitPoints(target, totalDamage);

        float killChance = (user.atp - (target.res * 2)) / 100;
        Debug.Log("Chance to kill with killer bow: " + killChance);

        if (target.resistDeath)
        {
            ui.DisplayStatusUpdate("DEATH RESIST", target.transform.position, delayDuration: 1);
        }
        else if (totalDamage > 0 && critLanded || Random.value <= killChance)
        {
            target.status = Avatar.Status.Dead;
            ui.DisplayStatusUpdate("DEATH", target.transform.position, delayDuration: 1);
        }
        
    }
}

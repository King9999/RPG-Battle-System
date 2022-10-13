using UnityEngine;

//Perform a normal attack with a chance to poison. The normal attack has a chance to be a critical; if this happens, poison
//will have a 100% success rate.
[CreateAssetMenu(menuName = "Skill/Item Skill/Venom Dagger", fileName = "skill_venomDagger")]
public class VenomDagger : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);
        
        //5% chance to inflict a critical. Criticals ignore defense. 100% chance to poison if critical
        float totalDamage;
        float critChance = 0.05f;
        bool critLanded = false;
        //float roll = Random.Range(0, 1f);

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
            //Debug.Log(className + " is blind!");
            //roll = Random.Range(0, 1f);
            if (Random.value > blindHitChance)
            {
                totalDamage = 0;
            }
        }
        
        if (totalDamage < 0)
            totalDamage = 0;


        //deal damage then do poison check. Cannot poison if damage is 0
        user.ReduceHitPoints(target, totalDamage);

        float poisonChance = (user.atp - (target.res * 2)) / 100;
        Debug.Log("Chance to poison with venom dagger: " + poisonChance);

        if (target.resistPoison)
        {
            ui.DisplayStatusUpdate("POISON RESIST", target.transform.position, delayDuration: 1);
        }
        else if (totalDamage > 0 && critLanded || Random.value <= poisonChance)
        {
            target.status = Avatar.Status.Poisoned;
            ui.DisplayStatusUpdate("POISONED", target.transform.position, delayDuration: 1);
        }   
        
    }
}

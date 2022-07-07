using UnityEngine;

//low chance of hitting, but if it does, it's a critical hit. The target's SPD affects the hit chance
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Haymaker", fileName = "skill_haymaker")]
public class Haymaker : Skill
{ 
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        float hitChance = 0.3f;
        float rollValue = Random.Range(0, 1f);
        hitChance -= (target.spd / 500);
        Debug.Log("Haymaker hit Chance to " + target.className + ": " + hitChance * 100 + "%");

        if (rollValue <= hitChance)
        {
            totalDamage = (user.atp * user.atpMod) + power;
            totalDamage += Random.Range(0, totalDamage * 0.1f);
            user.ReduceHitPoints(target, Mathf.Round(totalDamage));
            //Debug.Log("Hit! " + totalDamage + " damage to " + target.className);   
        }
        else
        {
            totalDamage = 0;
            //UI ui = UI.instance;
            ui.DisplayStatusUpdate("MISS", target.transform.position);
            //Debug.Log("Miss");  
        }

        //user.ReduceHitPoints(target, totalDamage);
    }
}

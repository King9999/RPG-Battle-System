using UnityEngine;

//steals mana from a target. Amount taken is a random value between 20% and 30% of the target's mana.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Mana Siphon", fileName = "skill_manaSiphon")]
public class ManaSiphon : Skill
{
   public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, target, borderColor);

        totalDamage = Random.Range(target.maxManaPoints * 0.2f, target.maxManaPoints * 0.3f);
        
        user.ReduceManaPoints(target, Mathf.Round(totalDamage));
        user.RestoreManaPoints(user, Mathf.Round(totalDamage));
         
    }
}

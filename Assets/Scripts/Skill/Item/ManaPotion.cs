using UnityEngine;

//restores 33% Mp when used
[CreateAssetMenu(menuName = "Skill/Item Skill/Mana Potion", fileName = "skill_manaPotion")]
public class ManaPotion : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        float healAmount = Mathf.Round(target.maxManaPoints * 0.33f);
        user.RestoreManaPoints(target, healAmount);
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        float healAmount = Mathf.Round(target.maxManaPoints * 0.33f);
        target.RestoreManaPoints(target, healAmount);
    }
}
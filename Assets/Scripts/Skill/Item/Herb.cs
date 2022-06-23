using UnityEngine;

//restores 30% Hp when used
[CreateAssetMenu(menuName = "Skill/Item Skill/Herb", fileName = "skill_herb")]
public class Herb : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        float healAmount = Mathf.Round(target.maxHitPoints * 0.33f);
        user.RestoreHitPoints(target, healAmount);
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        float healAmount = Mathf.Round(target.maxHitPoints * 0.33f);
        target.RestoreHitPoints(target, healAmount);
    }
}

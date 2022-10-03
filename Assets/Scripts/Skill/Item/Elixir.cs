using UnityEngine;

//restores HP and MP to full
[CreateAssetMenu(menuName = "Skill/Item Skill/Elixir", fileName = "skill_elixir")]
public class Elixir : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        user.RestoreHitPoints(target, target.maxHitPoints);
        user.RestoreManaPoints(target, target.maxManaPoints, 1);
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        target.RestoreHitPoints(target, target.maxHitPoints);
        target.RestoreManaPoints(target, target.maxManaPoints, 1);
    }
}

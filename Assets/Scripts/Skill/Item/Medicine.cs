using UnityEngine;

//removes status ailments. Does not work on dead heroes
[CreateAssetMenu(menuName = "Skill/Item Skill/Medicine", fileName = "skill_medicine")]
public class Medicine : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
            target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
        {
            target.status = Avatar.Status.Normal;
            ui.DisplayStatusUpdate("AILMENT REMOVED", target.transform.position);
        }
        else
        {
            ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
        }
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
            target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
        {
            target.status = Avatar.Status.Normal;
            ui.DisplayStatusUpdate("AILMENT REMOVED", target.transform.position);
        }
        else
        {
            ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
        }
    }
}

using UnityEngine;

//Paralyzes a single target
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Shadow Stitch", fileName = "skill_shadowStitch")]
public class ShadowStitch : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(user, borderColor);
        ReduceMp(user);

        float stunChance = ((user.spd * 2) - target.spd) / 100;
        Debug.Log("Shadow Stitch chance vs " + target.className + ": " + stunChance);
        if (target.resistParalysis)
        {
            ui.DisplayStatusUpdate("STUN RESIST", target.transform.position);
        }
        else if (Random.value <= stunChance)
        {
            target.status = Avatar.Status.Paralyzed;
            durationLeft = turnDuration;
            if (!target.skillEffects.ContainsKey(this))
            {
                target.skillEffects.Add(this, durationLeft);
            }
            else
            {
                target.skillEffects[this] = durationLeft;
            }

            ui.DisplayStatusUpdate("STUNNED", target.transform.position);
        }
        else
        {
            ui.DisplayStatusUpdate("MISS", target.transform.position);
        }
        
    }

    public override void RemoveEffects(Avatar user)
    {
        user.status = Avatar.Status.Normal;
        ui.DisplayStatusUpdate("STUN REMOVED", user.transform.position);
    }
}

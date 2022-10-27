using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chance to inflict blind status on all heroes. Does not deal damage.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Smokescreen", fileName = "skill_smokescreen")]
public class Smokescreen : Skill
{
    public override void Activate(Avatar user, List<Hero> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        ReduceMp(user);

        for (int i = 0; i < targets.Count; i++)
        {
            float blindChance = ((user.mag * 5) - targets[i].res) / 100;
            Debug.Log("Chance to blind " + targets[i].className + ": " + blindChance);
            if (targets[i].resistBlind)
            {
                ui.DisplayStatusUpdate(i, "BLIND RESIST", targets[i].transform.position);
            }
            else if (Random.value <= blindChance)
            {
                targets[i].status = Avatar.Status.Blind;
                ui.DisplayStatusUpdate(i, "BLIND", targets[i].transform.position);
            }
            else
            {
                ui.DisplayStatusUpdate(i, "MISS", targets[i].transform.position);
            }
        }
       
    }
}

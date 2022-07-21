using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inflicts poison on all heroes. Poison remains on target until cured. Success rate can be reduced by target's RES.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Poison Mist", fileName = "skill_poisonMist")]
public class PoisonMist : Skill
{
    public override void Activate(Avatar user, List<Hero> targets, Color borderColor)
    {
        base.Activate(user, targets, borderColor);

        ReduceMp(user);

        float hitChance = Mathf.Round(user.mag * 1.5f);

        for (int i = 0; i < targets.Count; i++)
        {
            float finalResult = (hitChance - (targets[i].res * 2)) / 100;
            if (targets[i].resistPoison)
            {
                ui.DisplayStatusUpdate(i, "POISON RESIST", targets[i].transform.position);
            }
            else if (Random.value <= finalResult)
            {
                targets[i].status = Avatar.Status.Poisoned;
                ui.DisplayStatusUpdate(i, "POISONED", targets[i].transform.position);
            }
            else
            {
                ui.DisplayStatusUpdate(i, "MISS", targets[i].transform.position);
            }
        }
       
    }

}

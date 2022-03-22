using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//deals fire damage to one target
[CreateAssetMenu(menuName = "Skill/Offensive/Fireball", fileName = "skill_fireball")]
public class Fireball : Skill
{
    public override void Activate(Avatar target)
    {
        base.Activate(target);
    }
}

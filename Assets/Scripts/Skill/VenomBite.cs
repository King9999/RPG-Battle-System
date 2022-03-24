using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//physical attack with a chance of poison. Hit chance is affected by target's speed.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Venom Bite", fileName = "skill_venombite")]
public class VenomBite : Skill
{
    float hitChance = 0.3f;
   public override void Activate(Avatar target)
   {
        //target receives damage

        //poison check
        float rollValue = Random.Range(0, 1);
        hitChance -= (target.spd / 500);
        if (rollValue <= hitChance)
        {
            Debug.Log("Poisoned");
        }
        else
        {
            Debug.Log("Miss");
        }
   }
}

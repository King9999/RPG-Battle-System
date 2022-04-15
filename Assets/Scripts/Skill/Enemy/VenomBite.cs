using UnityEngine;

//physical attack with a chance of poison. Hit chance is affected by target's speed.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Venom Bite", fileName = "skill_venombite")]
public class VenomBite : Skill
{
    float hitChance = 0.3f;
   public override void Activate(Avatar target, Color borderColor)
   {
       base.Activate(target, borderColor);
        //target receives damage

        //poison check
        if (target.resistPoison == false)
        {
            float rollValue = Random.Range(0, 1f);
            hitChance -= (target.spd / 500);
            if (rollValue <= hitChance)
            {
                Debug.Log("Poisoned");
                target.status = Avatar.Status.Poisoned;
            }
            else
            {
                Debug.Log("Miss");
            }
        }
   }
}

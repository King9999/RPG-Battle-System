using UnityEngine;

//low chance of hitting, but if it does, it's a critical hit. The target's SPD affects the hit chance
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Haymaker", fileName = "skill_haymaker")]
public class Haymaker : Skill
{
    float hitChance = 0.2f;
    public override void Activate(Avatar target)
    {
        float rollValue = Random.Range(0, 1);
        hitChance -= (target.spd / 500);
        if (rollValue <= hitChance)
        {
            Debug.Log("Critical Hit!");
        }
        else
        {
            Debug.Log("Miss");
        }
    }
}

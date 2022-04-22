using UnityEngine;

//Enemy gains an additional shield token
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Armor Boost", fileName = "skill_armorboost")]
public class ArmorBoost : Skill
{
   public override void Activate(Avatar user, Color borderColor)
    {
        base.Activate(user, borderColor);

        Enemy enemy = user.GetComponent<Enemy>();
        enemy.maxShieldTokens += 1;
        enemy.AddShield();
        Debug.Log("Max Shield Tokens is now " + enemy.maxShieldTokens);
    }
}

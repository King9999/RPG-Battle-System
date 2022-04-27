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
        ui.DisplayStatusUpdate("+1 SHIELD", user.transform.position);
    }
}

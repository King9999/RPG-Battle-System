using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Item Skill/Regen Ring", fileName = "skill_regenRing")]
public class RegenRing : Skill
{
    public override void Activate(Avatar user, Color borderColor)
    {
        base.Activate(user, borderColor);
        
        //restore 15% HP at the end of turn
        if (user.TurnTaken())
        {
            //UI ui = UI.instance;
            float hpAmount = user.hitPoints < user.maxHitPoints ? Mathf.Round(user.maxHitPoints * 0.15f) : 0;
            //ui.damageDisplay.color = ui.healColor;
            user.RestoreHitPoints(user, hpAmount);
        }
        
    }
}

using UnityEngine;

//This is a special skill for the Titan major enemy. It doesn't do anything except alert the player how much time is left before Titan attacks.
[CreateAssetMenu(menuName = "Skill/Enemy Skill/Countdown", fileName = "skill_countdown")]
public class Countdown : Skill
{
    public override void Activate(Avatar user, Color borderColor)
    {
        base.Activate(user, borderColor);  
    }
}

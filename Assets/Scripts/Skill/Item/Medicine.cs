using UnityEngine;

//removes status ailments. Does not work on dead heroes
[CreateAssetMenu(menuName = "Skill/Item Skill/Medicine", fileName = "skill_medicine")]
public class Medicine : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
       
        base.Activate(target, borderColor);
        if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
            target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
        {
            target.status = Avatar.Status.Normal;
            ui.DisplayStatusUpdate("CURED", target.transform.position);
        }
        else
        {
            ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
        }
       
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        DungeonUI ui = DungeonUI.instance;
        if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
            target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
        {
            target.status = Avatar.Status.Normal;
            ui.DisplayStatus("CURED", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
        }
        else
        {
            ui.DisplayStatus("NO EFFECT", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
        }
        ui.partyDisplay[ui.currentHero].UpdateUI();
    }
}

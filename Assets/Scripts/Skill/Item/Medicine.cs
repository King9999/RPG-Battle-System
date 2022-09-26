using UnityEngine;

//removes status ailments. Does not work on dead heroes
[CreateAssetMenu(menuName = "Skill/Item Skill/Medicine", fileName = "skill_medicine")]
public class Medicine : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        //code execution depends on game state
        /*GameManager gm = GameManager.instance;

         if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
                target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
        {
            target.status = Avatar.Status.Normal;
            if (gm.gameState == GameManager.GameState.Combat)
            {
                ui.DisplayStatusUpdate("CURED", target.transform.position);
            }
            else
            {
                DungeonUI ui = DungeonUI.instance;
                ui.DisplayStatus("CURED", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.healColor);
                ui.partyDisplay[ui.currentHero].UpdateUI();
            }
        }
        else
        {
            if (gm.gameState == GameManager.GameState.Combat)
            {
                ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
            }
            else
            {
                DungeonUI ui = DungeonUI.instance;
                ui.DisplayStatus("NO EFFECT", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                ui.partyDisplay[ui.currentHero].UpdateUI();
            }
        }*/
        GameManager gm = GameManager.instance;
        if (gm.gameState == GameManager.GameState.Combat)
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
        else    //we're outside combat
        {
            DungeonUI ui = DungeonUI.instance;
            if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
                target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
            {
                target.status = Avatar.Status.Normal;
                ui.DisplayStatus("CURED", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.healColor);
                ui.partyDisplay[ui.currentHero].UpdateUI();
            }
            else
            {
                ui.DisplayStatus("NO EFFECT", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
            }
        }
    }

    //used outside of combat
    /*public override void Activate(Avatar target)
    {
        GameManager gm = GameManager.instance;
        if (gm.gameState == GameManager.GameState.Normal)
        {
            DungeonUI ui = DungeonUI.instance;
            ui.DisplayStatus(amount.ToString(), ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.healColor);
            ui.partyDisplay[ui.currentHero].UpdateUI();
        }
        if (target.status == Avatar.Status.Poisoned || target.status == Avatar.Status.Paralyzed || 
            target.status == Avatar.Status.Charmed || target.status == Avatar.Status.Blind)
        {
            target.status = Avatar.Status.Normal;
            ui.DisplayStatusUpdate("AILMENT REMOVED", target.transform.position);
        }
        else
        {
            ui.DisplayStatusUpdate("NO EFFECT", target.transform.position);
        }
    }*/
}

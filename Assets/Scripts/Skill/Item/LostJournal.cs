using UnityEngine;

//reduces XP to next level to 1. Cannot be used in combat.
[CreateAssetMenu(menuName = "Skill/Item Skill/Lost Journal", fileName = "skill_lostJournal")]
public class LostJournal : Skill
{
    //used outside of combat
    public override void Activate(Avatar target)
    {
        if (target.TryGetComponent(out Hero hero))
        {
            DungeonUI ui = DungeonUI.instance;
            if (!hero.AtMaxLevel())
            {
                hero.xpToNextLevel = 1;
                ui.DisplayStatus("XP GAINED", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                ui.partyDisplay[ui.currentHero].UpdateUI();
            }
            else
            {
                ui.DisplayStatus("MAX LEVEL", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
            }
        }
    }
}

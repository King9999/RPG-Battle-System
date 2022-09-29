using UnityEngine;

//Raises a random stat by 1. The stats are ATP, DFP, MAG, RES, and SPD. Cannot be used in battle.
[CreateAssetMenu(menuName = "Skill/Item Skill/Mystery Seed", fileName = "skill_mysterySeed")]
public class MysterySeed : Skill
{
    public override void Activate(Avatar user, Avatar target, Color borderColor)
    {
        base.Activate(target, borderColor);
        float healAmount = Mathf.Round(target.maxHitPoints * 0.33f);
        user.RestoreHitPoints(target, healAmount);
    }

    //used outside of combat
    public override void Activate(Avatar target)
    {
        DungeonUI ui = DungeonUI.instance;

        //choose a random stat, and raise it by 1 point
        int randStat = Random.Range(0, 5);

        switch(randStat)
        {
            case 0:
                target.minAtpMod += 0.01f;
                target.atpMod = target.minAtpMod;
                ui.DisplayStatus("ATP UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                break;
            
            case 1:
                target.minDfpMod += 0.01f;
                target.dfpMod = target.minDfpMod;
                ui.DisplayStatus("DFP UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                break;
            case 2:
                target.minMagMod += 0.01f;
                target.magMod = target.minMagMod;
                ui.DisplayStatus("MAG UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                break;
            case 3:
                target.minResMod += 0.01f;
                target.resMod = target.minResMod;
                ui.DisplayStatus("RES UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                break;
            case 4:
                target.minSpdMod += 0.01f;
                target.spdMod = target.minSpdMod;
                ui.DisplayStatus("SPD UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                break;
        }
    }
}

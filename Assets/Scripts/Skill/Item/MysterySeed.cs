using UnityEngine;

//Raises a random stat mod by 1%. The stats are ATP, DFP, MAG, RES, and SPD. Cannot be used in battle.
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
        if (target.TryGetComponent(out Hero hero))
        {
            DungeonUI ui = DungeonUI.instance;

            //choose a random stat mod, and raise it by 1%
            int randStat = Random.Range(0, 5);

            switch(randStat)
            {
                case 0:
                    hero.minAtpMod += 0.01f;
                    hero.atpMod = hero.minAtpMod;
                   
                    float baseAtp = hero.atp - hero.weapon.atp;
                    hero.atp = Mathf.Round(baseAtp * hero.atpMod) + hero.weapon.atp;
                    
                    ui.DisplayStatus("ATP UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                    break;
                
                case 1:
                    hero.minDfpMod += 0.01f;
                    hero.dfpMod = hero.minDfpMod;
                    
                    float baseDfp = hero.armor != null ? hero.dfp - hero.armor.dfp : 0;
                    hero.dfp = Mathf.Round(baseDfp * hero.dfpMod) + baseDfp;

                    ui.DisplayStatus("DFP UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                    break;
                case 2:
                    hero.minMagMod += 0.01f;
                    hero.magMod = hero.minMagMod;
                    
                    float baseMag = hero.mag - hero.weapon.mag;
                    hero.mag = Mathf.Round(baseMag * hero.magMod) + hero.weapon.mag;

                    ui.DisplayStatus("MAG UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                    break;
                case 3:
                    hero.minResMod += 0.01f;
                    hero.resMod = hero.minResMod;
                    
                    float baseRes = hero.armor != null ? hero.res - hero.armor.res : 0;
                    hero.res = Mathf.Round(baseRes * hero.resMod) + baseRes;

                    ui.DisplayStatus("RES UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                    break;
                case 4:
                    hero.minSpdMod += 0.01f;
                    hero.spdMod = hero.minSpdMod;
                    
                    float baseSpd = hero.trinket != null ? hero.spd - hero.trinket.spd : 0;
                    hero.spd = Mathf.Round(baseSpd * hero.spdMod) + baseSpd;

                    ui.DisplayStatus("SPD UP", ui.partyDisplay[ui.currentHero].heroSprite.transform.position, ui.normalColor);
                    break;
            }
        }
    }
}

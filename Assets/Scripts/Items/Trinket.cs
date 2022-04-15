using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//trinkets are accessories that can apply bonuses to stats, and also have special effects.
[CreateAssetMenu(menuName = "Item/Trinket", fileName = "trn_")]
public class Trinket : Item
{
    public float maxHitPoints, maxManaPoints, atp, dfp, mag, res, spd, hpMod, mpMod, atpMod, dfpMod, magMod, resMod, spdMod;
    public int attackTokenMod;
    public bool resistPoison;
    public bool resistParalysis;
    public bool resistBlind;
    public bool resistCharm;
    public Skill trinketSkill;

    public override void Equip(Hero hero)
    {
        if (isEquipped || itemType != ItemType.Trinket) return;

        //add all stats
        hero.maxHitPoints += maxHitPoints;
        hero.manaPoints += maxManaPoints;
        hero.atp += atp;
        hero.dfp += dfp;
        hero.mag += mag;
        hero.res += res;
        hero.spd += spd;
        hero.attackTokenMod += attackTokenMod;
        hero.hpMod += hpMod;
        hero.mpMod += mpMod;
        hero.atpMod += atpMod;
        hero.dfpMod += dfpMod;
        hero.magMod += magMod;
        hero.resMod += resMod;
        hero.spdMod += spdMod;

        /* Only change ailment protection if this item provides it */
        hero.resistPoison = resistPoison == true ? true : hero.resistPoison;
        hero.resistCharm = resistCharm == true ? true : hero.resistCharm;
        hero.resistBlind = resistBlind == true ? true : hero.resistBlind;
        hero.resistParalysis = resistParalysis == true ? true : hero.resistParalysis;

        if (trinketSkill != null)
            hero.skills.Add(trinketSkill);
        isEquipped = true;
        
    }

    public override void Unequip(Hero hero)
    {
        if (!isEquipped) return;

        hero.maxHitPoints -= maxHitPoints;
        hero.manaPoints -= maxManaPoints;
        hero.atp -= atp;
        hero.dfp -= dfp;
        hero.mag -= mag;
        hero.res -= res;
        hero.spd -= spd;
        hero.attackTokenMod -= attackTokenMod;
        hero.hpMod -= hpMod;
        hero.mpMod -= mpMod;
        hero.atpMod -= atpMod;
        hero.dfpMod -= dfpMod;
        hero.magMod -= magMod;
        hero.resMod -= resMod;
        hero.spdMod -= spdMod;

         /* Only remove ailment protection if this item was providing it */
        hero.resistPoison = resistPoison == true ? false : hero.resistPoison;
        hero.resistCharm = resistCharm == true ? false : hero.resistCharm;
        hero.resistBlind = resistBlind == true ? false : hero.resistBlind;
        hero.resistParalysis = resistParalysis == true ? false : hero.resistParalysis;

        //find trinket skill to remove in list.       
        if (trinketSkill != null)
        {
            int i = 0;
            bool skillFound = false;
            while(!skillFound && i < hero.skills.Count)
            {
                if (trinketSkill == hero.skills[i])
                {
                    hero.skills.RemoveAt(i);
                    skillFound = true;
                }
                else
                {
                    i++;
                }
            }
        }
        isEquipped = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor", fileName = "armor_")]
public class Armor : Item
{
    [Header("Armor properties")]
    public float dfp;
    public float res;
    public Skill armorSkill;

   /*public override void Equip(Hero hero)
    {
        base.Equip(hero);

        //check weapon type
        if (weaponType == WeaponType.Sword && hero.swordOK || weaponType == WeaponType.Axe && hero.axeOK || 
            weaponType == WeaponType.Bow && hero.bowOK || weaponType == WeaponType.Dagger && hero.daggerOK ||
            weaponType == WeaponType.Staff && hero.staffOK)
        {
            hero.atp += atp;
            hero.mag += mag;
            hero.actGauge = actGauge;
            hero.totalAttackTokens = hero.attackTokenMod + tokenCount;

            if (weaponSkill != null)
                hero.skills.Add(weaponSkill);
            isEquipped = true;
        }
    }

    public override void Unequip(Hero hero)
    {
        base.Unequip(hero);
        hero.atp -= atp;
        hero.mag -= mag;
        hero.actGauge = null;
        hero.totalAttackTokens -= tokenCount;

        //find weaponskill to remove in list.       
        if (weaponSkill != null)
        {
            int i = 0;
            bool skillFound = false;
            while(!skillFound && i < hero.skills.Count)
            {
                if (weaponSkill == hero.skills[i])
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
    }*/
}

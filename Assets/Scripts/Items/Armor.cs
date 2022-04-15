using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor", fileName = "armor_")]
public class Armor : Item
{
    [Header("Armor properties")]
    public float dfp;
    public float res;
    public bool resistPoison;
    public bool resistParalysis;
    public bool resistBlind;
    public bool resistCharm;
    public Skill armorSkill;

   public override void Equip(Hero hero)
    {
        if (isEquipped || itemType != ItemType.Armor) return;

        //check armor type
        /*if (weaponType == WeaponType.Sword && hero.swordOK || weaponType == WeaponType.Axe && hero.axeOK || 
            weaponType == WeaponType.Bow && hero.bowOK || weaponType == WeaponType.Dagger && hero.daggerOK ||
            weaponType == WeaponType.Staff && hero.staffOK)*/
        //{
            hero.dfp += dfp;
            hero.res += res;

            /* Only change ailment protection if this item provides it */
            hero.resistPoison = resistPoison == true ? true : hero.resistPoison;
            hero.resistCharm = resistCharm == true ? true : hero.resistCharm;
            hero.resistBlind = resistBlind == true ? true : hero.resistBlind;
            hero.resistParalysis = resistParalysis == true ? true : hero.resistParalysis;

            if (armorSkill != null)
                hero.skills.Add(armorSkill);
            isEquipped = true;
        //}
    }

    public override void Unequip(Hero hero)
    {
        if (!isEquipped) return;

        hero.dfp -= dfp;
        hero.res -= res;

        /* Only remove ailment protection if this item was providing it */
        hero.resistPoison = resistPoison == true ? false : hero.resistPoison;
        hero.resistCharm = resistCharm == true ? false : hero.resistCharm;
        hero.resistBlind = resistBlind == true ? false : hero.resistBlind;
        hero.resistParalysis = resistParalysis == true ? false : hero.resistParalysis;
        
        //find armor skill to remove in list.       
        if (armorSkill != null)
        {
            int i = 0;
            bool skillFound = false;
            while(!skillFound && i < hero.skills.Count)
            {
                if (armorSkill == hero.skills[i])
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

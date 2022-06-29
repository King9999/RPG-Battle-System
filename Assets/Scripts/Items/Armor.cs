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
    public float fireResist = 1;
    public float coldResist = 1;
    public float lightningResist = 1;
    public Skill armorSkill;

   public override void Equip(Hero hero)
    {
        if (isEquipped || itemType != ItemType.Armor) return;

        //check armor type
        /*if (weaponType == WeaponType.Sword && hero.swordOK || weaponType == WeaponType.Axe && hero.axeOK || 
            weaponType == WeaponType.Bow && hero.bowOK || weaponType == WeaponType.Dagger && hero.daggerOK ||
            weaponType == WeaponType.Staff && hero.staffOK)*/
        //{
            if (hero.armor != null)
                hero.armor.Unequip(hero);
            hero.armor = this;
            hero.dfp += dfp;
            hero.res += res;

            /* Only change ailment protection if this item provides it */
            hero.resistPoison = resistPoison == true ? true : hero.resistPoison;
            hero.resistCharm = resistCharm == true ? true : hero.resistCharm;
            hero.resistBlind = resistBlind == true ? true : hero.resistBlind;
            hero.resistParalysis = resistParalysis == true ? true : hero.resistParalysis;

            //passive skill check
            if (armorSkill != null)
            {
                if (armorSkill.isPassive && !hero.skillEffects.ContainsKey(armorSkill))
                {
                    hero.skillEffects.Add(armorSkill, 0);
                }
                else if (!armorSkill.isPassive)
                {
                    hero.skills.Add(armorSkill);
                }
            }
            isEquipped = true;
        //}
    }

    public override void Unequip(Hero hero)
    {
        if (!isEquipped || hero.armor == null) return;

        hero.armor = null;
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
            if (armorSkill.isPassive)
            {
                int i = 0;
                bool skillFound = false;
                foreach(KeyValuePair<Skill, int> skillEffect in hero.skillEffects)
                {
                    if (armorSkill == skillEffect.Key)
                    {
                        skillEffect.Key.RemoveEffects(hero);
                        hero.skillEffects.Remove(skillEffect.Key);
                        //skillFound = true;
                        break;
                    }
                }
                /*while(!skillFound && i < hero.skillEffects.Count)
                {
                    if (armorSkill == hero.skillEffects[i])
                    {
                        hero.skillEffects[i].RemoveEffects(hero);
                        hero.skillEffects.RemoveAt(i);
                        skillFound = true;
                    }
                    else
                    {
                        i++;
                    }
                }*/
            }
            else    //remove from skill list.
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
        }
        isEquipped = false;
    }
}

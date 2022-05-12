using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", fileName = "wpn_")]
public class Weapon : Item
{
    [Header("Weapon properties")]
    public WeaponType weaponType;
    public float atp;
    public float mag;
    public ActionGaugeData actGauge;
    int defaultTokenCount {get;} = 3;
    public int tokenCount = 3;    //default amount is 3
    public Skill weaponSkill;

    public enum WeaponType
    {
        Sword, Dagger, Staff, Bow, Axe
    }
    

    //A weapon can have a special effect that is triggered during a battle by landing a token in the correct area on the action gauge.
    //public virtual void ActivateSkill() {}

    public override void Equip(Hero hero)
    {
        if (isEquipped || itemType != ItemType.Weapon) return;

        //check weapon type
        if (weaponType == WeaponType.Sword && hero.swordOK || weaponType == WeaponType.Axe && hero.axeOK || 
            weaponType == WeaponType.Bow && hero.bowOK || weaponType == WeaponType.Dagger && hero.daggerOK ||
            weaponType == WeaponType.Staff && hero.staffOK)
        {
            hero.atp += atp;
            hero.mag += mag;
            hero.actGauge = actGauge;
            hero.totalAttackTokens = hero.attackTokenMod + tokenCount;

            //if (weaponSkill != null)
               // hero.skills.Add(weaponSkill);
            isEquipped = true;
        }
    }

    public override void Unequip(Hero hero)
    {
       if (!isEquipped) return;

        hero.atp -= atp;
        hero.mag -= mag;
        hero.actGauge = null;
        hero.totalAttackTokens -= tokenCount;

        //find weaponskill to remove in list.    NOTE: weapon skills are used directly from the weapon and not the hero.   
        /*if (weaponSkill != null)
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
        }*/
        isEquipped = false;
    }
   
}

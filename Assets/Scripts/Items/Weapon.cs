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
    [Header("Staff skills")]
    public bool nonRandomStaffSkill;    //if true, staff has a fixed skill.

    public Skill[] staffSkills;
    
    public enum WeaponType
    {
        Sword, Dagger, Staff, Bow, Axe
    }
    

    public bool CanBeEquipped(Hero hero)
    {
        return weaponType == WeaponType.Sword && hero.swordOK || weaponType == WeaponType.Axe && hero.axeOK || 
            weaponType == WeaponType.Bow && hero.bowOK || weaponType == WeaponType.Dagger && hero.daggerOK ||
            weaponType == WeaponType.Staff && hero.staffOK;
    }
    public override void Equip(Hero hero)
    {
        if (isEquipped || itemType != ItemType.Weapon) return;

        //check weapon type
        if (hero.weapon != null)
            hero.weapon.Unequip(hero);
        hero.weapon = this;
        hero.atp += atp;
        hero.mag += mag;
        hero.actGauge = actGauge;
        hero.totalAttackTokens = hero.attackTokenMod + tokenCount;

        //if (weaponSkill != null)
            // hero.skills.Add(weaponSkill);
        isEquipped = true;
    }

    public override void Unequip(Hero hero)
    {
       if (!isEquipped || hero.weapon == null) return;

        hero.weapon = null;
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

    //FOR STAVES ONLY. Staves come with a random skill that is acquired every time a staff weapon is generated.
    public void GenerateSkill()
    {
        //if (weaponType != WeaponType.Staff) return;
        
        int randSkill = Random.Range(0, staffSkills.Length);
        weaponSkill = staffSkills[randSkill];
    }
   
}

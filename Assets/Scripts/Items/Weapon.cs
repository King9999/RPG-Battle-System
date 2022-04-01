using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", fileName = "wpn_")]
public class Weapon : Item
{
    [Header("Weapon properties")]
    public WeaponType weaponType;
    public float atp;
    public float mag;
    public ActionGauge actGauge;
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
        base.Equip(hero);

        hero.atp += atp;
        hero.mag += mag;
        hero.actGauge = actGauge;

        if (weaponSkill != null)
            hero.skills.Add(weaponSkill);
        isEquipped = true;
    }

    public override void Unequip(Hero hero)
    {
        base.Unequip(hero);
        hero.atp -= atp;
        hero.mag -= mag;
        hero.actGauge = null;

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
    }
   
}
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
   
}

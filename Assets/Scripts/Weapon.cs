using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", fileName = "wpn_")]
public class Weapon : Item
{
    public float attack;
    public ActionGauge actGauge;
    int defaultTokenCount {get;} = 3;
    public int tokenCount = 3;    //default amount is 3
    public Skill weaponSkill;

    //A weapon can have a special effect that is triggered during a battle by landing a token in the correct area on the action gauge.
    public virtual void ActivateSkill()
    {

    }
}

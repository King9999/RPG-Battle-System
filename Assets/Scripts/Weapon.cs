using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon", fileName = "Wpn_")]
public class Weapon : Item
{
    public float attack;
    public string chain;    //consists of special characters that affect a weapon's performance

    //A weapon can have a special effect that is triggered during a battle.
    public virtual void ActivateSkill()
    {

    }
}

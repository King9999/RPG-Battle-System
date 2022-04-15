using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//trinkets are accessories that can apply bonuses to stats, and also have special effects.
[CreateAssetMenu(menuName = "Item/Trinket", fileName = "trn_")]
public class Trinket : Item
{
    public float maxHitPoints, maxManaPoints, atp, dfp, mag, res, spd, attackTokenMod, hpMod, mpMod, atpMod, dfpMod, magMod, resMod, spdMod;
    public Skill trinketSkill;
}

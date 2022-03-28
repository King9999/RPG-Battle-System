using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Armor", fileName = "armor_")]
public class Armor : Item
{
    [Header("Armor properties")]
    public float dfp;
    public Skill armorSkill;

   
}

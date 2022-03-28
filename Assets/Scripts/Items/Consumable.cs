using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable", fileName = "consumable_")]
public class Consumable : Item
{
    [Header("Consumable properties")]
    public Skill itemEffect;
}

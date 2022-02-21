using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all effects in the game, including spells, consumable item effects.
public class Skill : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public string description;
    public int manaCost;
    public float power;         //potency of a skill. It is added to player's MAG stat.

    public enum EffectID
    {
        None, HealMinor, Ice
    }

    public enum Element
    {
        None, Fire, Water, Earth, Air
    }    

    
}

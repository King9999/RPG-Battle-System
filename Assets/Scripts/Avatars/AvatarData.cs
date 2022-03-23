using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* All avatar game objects get their information from a scriptable object. This is the base class. */
public class AvatarData : ScriptableObject
{
    public string className;
    public string details;      //description of the enemy/hero
    public float hitPoints;
    public float maxhitPoints;
    public float manaPoints;
    public float maxManaPoints;
    public float atp;           //attack power
    public float dfp;           //defense power
    public float mag;           //magic power
    public float res;           //resistance

    public List<Skill> skills;
}

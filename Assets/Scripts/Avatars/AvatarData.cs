using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* All avatar game objects get their information from a scriptable object. This is the base class. */
public abstract class AvatarData : ScriptableObject
{
    public string className;
    public string details;      //description of the enemy/hero
    public float maxHitPoints;
    public float maxManaPoints;
    public float atp;           //attack power
    public float dfp;           //defense power
    public float spd;           //speed
    public float mag;           //magic power
    public float res;           //resistance
    public bool resistPoison;
    public bool resistParalysis;
    public bool resistBlind;
    public bool resistCharm;
    public bool resistDeath;

   public List<Skill> skills;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the base class for playable heroes and NPC enemies */
public abstract class Avatar : MonoBehaviour
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
    public enum Status
    {
        Normal, Poisoned, Paralyzed, Blind, Charmed, Dead
    }
    public Status status;

    
}

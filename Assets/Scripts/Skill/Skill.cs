using UnityEngine;

//base class for all effects in the game, including spells, consumable item effects.
public abstract class Skill : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public string description;
    public int manaCost;
    public float power;             //potency of a skill. It is added to either a player's MAG or ATP stat.
    public ActionGauge actGauge;    //skills typically have 1 token
    public bool isPassive;          //if true, skill is always active and has no mana cost.

    public enum Target
    {
        None, Self, One, All
    }

    public Target targetType;

    public virtual void Activate(Avatar skillUser, Avatar target) {}
    public virtual void Activate() {}
    public virtual void Activate(Avatar target) {}

    public enum StatusEffect
    {
        None, Poison, Paralyze, Blind, Charm, Death
    }

    public enum Element
    {
        None, Fire, Water, Earth, Air
    }

    public Element element;
    public StatusEffect statusEffect;    

    
}

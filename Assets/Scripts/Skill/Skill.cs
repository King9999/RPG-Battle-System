using UnityEngine;
using System.Collections.Generic;

//base class for all effects in the game, including spells, consumable item effects.
public abstract class Skill : ScriptableObject
{
    public string skillName;
    protected Color skillNameBorderColor;
    public Sprite skillIcon;
    public string description;
    public int manaCost;
    public float power;             //potency of a skill. It is added to either a player's MAG or ATP stat.
    public ActionGaugeData actGaugeData;    //skills typically have 1 token
    public bool isPassive;          //if true, skill is always active and has no mana cost.
    protected float totalDamage;
    protected bool skillActivated;  //applies mainly to skills that have have a duration
    public bool hasDuration;
    public int turnDuration;        //only counts if a skill has a duration.
    protected int durationLeft {get; set;}

    protected UI ui;
    protected CombatSystem cs;

    //enums
    public enum Target
    {
        None, Self, OneEnemy, OneHero, AllEnemies, AllHeroes
    }

    public enum StatusEffect
    {
        None, Poison, Paralyze, Blind, Charm, Death
    }

    public enum Element
    {
        None, Fire, Water, Earth, Air
    }

    public Target targetType;
    public Element element;
    public StatusEffect statusEffect;

    public virtual void Activate(Avatar skillUser, Avatar target, Color borderColor) 
    {
        skillActivated = true;
        durationLeft = hasDuration == true ? turnDuration : 0;

        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
    }

    //NOTE: Must use ienumerable<Avatar> instead of List<Avatar> due to covariance 
    public virtual void Activate(Avatar skillUser, List<Enemy> target, Color borderColor) 
    {
        skillActivated = true;
        durationLeft = hasDuration == true ? turnDuration : 0;

        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
    }

    public virtual void Activate(Color borderColor)
    {
        skillActivated = true;
        durationLeft = hasDuration == true ? turnDuration : 0;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
        //Debug.Log("Displaying skill name");
    }

    //used when a skill is executed on the user
    public virtual void Activate(Avatar self, Color borderColor) 
    {
        skillActivated = true;
        durationLeft = hasDuration == true ? turnDuration : 0;
        ui = UI.instance;
        skillNameBorderColor = borderColor;
        ui.skillDisplay.ExecuteSkillDisplay(skillName, skillNameBorderColor);
    }

    //This is used outside of combat only, and only for consumable items and skills.
    public virtual void Activate(Avatar target)
    {
        
    }

    public bool SkillActivated() {return skillActivated;}
    public void SetActiveStatus(bool state)
    {
        skillActivated = state;
    }

    public void ReduceDuration()
    {
        if (!hasDuration) return;

        durationLeft--;
        if (durationLeft <= 0)
        {
            skillActivated = false;
        }
    }

    public void ReduceMp(Avatar user)
    {
        user.manaPoints -= manaCost * user.mpMod;
    }

    //only applies to skills with a duration
    public virtual void RemoveEffects(Avatar target) {}
    public bool EffectExpired() {return durationLeft <= 0;}


    
    
}

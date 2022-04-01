using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A hero is a player-controlled entity. A hero has restrictions on which weapons they can use.
public class Hero : Avatar
{
    public HeroData data;
    public int level;
    public Weapon weapon;
    public Armor armor;
    public Trinket trinket;
    public ActionGauge actGauge;        //this comes from equipped weapon. If null, hero can't attack
    public bool swordOK, daggerOK, axeOK, bowOK, staffOK;
    public int currentXp;
    public int xpToNextLevel;   //this will be grabbed from a xp table

    // Start is called before the first frame update
    protected void Start()
    {
        //pull information from a scriptable object
        className = data.className;
        details = data.details;
        maxHitPoints = data.maxHitPoints;
        hitPoints = maxHitPoints;
        maxManaPoints = data.maxManaPoints;
        manaPoints = maxManaPoints;
        atp = data.atp;           
        dfp = data.dfp;           
        mag = data.mag;          
        res = data.res;
        skills = data.skills;
        swordOK = data.swordOK;
        daggerOK = data.daggerOK;
        axeOK = data.axeOK;
        bowOK = data.bowOK;
        staffOK = data.staffOK;
        level = data.level;
        currentXp = 0;
        weapon = data.weapon;
        armor = data.armor;
        trinket = data.trinket;
        //add code to get xpToNextLevel data

        if (level < 1)
            level = 1;
        if (level > data.MaxLevel)
            level = data.MaxLevel;

        //equip check
        if (weapon != null)      
            weapon.Equip(hero: this);
        if (armor != null)
            armor.Equip(hero: this);
        if (trinket != null)
            trinket.Equip(hero: this);
        
    }

    // Update is called once per frame
    void Update()
    {
        //user input
    }
}

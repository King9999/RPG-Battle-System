using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemies are NPCs. Heroes must defeat them. Their actions are randomized based on their skill set and battle conditions.
public abstract class Enemy : Avatar
{
    public EnemyData data;
    public int xp;
    public int money;
    protected float skillProb;      //odds that the enemy will do certain attacks.
    public Item commonItemDrop;
    public Item rareItemDrop;

    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        xp = data.xp;
        money = data.money;
        commonItemDrop = data.commonItemDrop;
        rareItemDrop = data.rareItemDrop;
    }

    public override void Attack(Avatar target)
    {
        float totalDamage = atp - target.dfp;
        target.hitPoints -= totalDamage;
    }

    
}

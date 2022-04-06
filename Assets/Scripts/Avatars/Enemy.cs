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
        //enemy has a 5% chance to inflict a critical. Criticals ignore defense
        float totalDamage;
        float critChance = 0.05f;
        float roll = Random.Range(0, 1f);

        if (roll <= critChance)
        {
            totalDamage = atp + Mathf.Round(Random.Range(0, atp * 0.1f));
        }
        else
        {
            totalDamage = atp + Mathf.Round(Random.Range(0, atp * 0.1f)) - target.dfp;
        }
        
        if (totalDamage < 0)
            totalDamage = 0;
        
        target.hitPoints -= totalDamage;
        Debug.Log(totalDamage + " damage to " + target.className);
    }

    //Used whenever enemy is not instantiated but need a fresh copy
    public void ResetData()
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

    
}

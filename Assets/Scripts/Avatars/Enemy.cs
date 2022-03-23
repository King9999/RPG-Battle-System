using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemies are NPCs. Heroes must defeat them. Their actions are randomized based on their skill set and battle conditions.
public class Enemy : Avatar
{
    public EnemyData data;
    public int xp;
    protected float skillProb;      //odds that the enemy will do certain attacks.

    // Start is called before the first frame update
    protected void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

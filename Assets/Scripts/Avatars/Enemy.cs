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
    public float commonItemDropChance;
    public float rareItemDropChance;

    //protected CombatSystem cs;
    protected EnemyManager em;
    protected Color skillNameBorderColor = new Color(0.7f, 0.1f, 0.1f);       //used to change skill display border color. Always red

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
        spd = data.spd;
        skills = data.skills;
        xp = data.xp;
        money = data.money;
        commonItemDrop = data.commonItemDrop;
        rareItemDrop = data.rareItemDrop;
        commonItemDropChance = data.commonItemDropChance;
        rareItemDropChance = data.rareItemDropChance;

        cs = CombatSystem.instance;
        em = EnemyManager.instance;
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
    public virtual void ResetData()
    {
        //Start();
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
        spd = data.spd;
        skills = data.skills;
        xp = data.xp;
        money = data.money;
        commonItemDrop = data.commonItemDrop;
        rareItemDrop = data.rareItemDrop;
        commonItemDropChance = data.commonItemDropChance;
        rareItemDropChance = data.rareItemDropChance;
    }

    //when enemy dies, they are sent to graveyard
    public void SendToGraveyard()
    {
        if (hitPoints > 0 || status != Status.Dead) return;
      
        em.graveyard.Add(this);
        gameObject.SetActive(false);
    }
    
}

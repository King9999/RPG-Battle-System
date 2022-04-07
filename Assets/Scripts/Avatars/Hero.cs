using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A hero is a player-controlled entity. A hero has restrictions on which weapons they can use.
public class Hero : Avatar
{
    public HeroData data;
    public TextAsset statFile;
    Stats stats;                        //contains data from stat table
    public int level;
    public Weapon weapon;
    public Armor armor;
    public Trinket trinket;
    public ActionGaugeData actGauge;        //this comes from equipped weapon. If null, hero can't attack
    public int totalAttackTokens;        //attack token mod + weapon tokens
    public short attackTokenMod;
    public bool swordOK, daggerOK, axeOK, bowOK, staffOK;
    public int currentXp;
    public int xpToNextLevel;   //this will be grabbed from a xp table
    [HideInInspector]public int currentLevel;   //points to current level in the stat table.
    protected Color skillNameBorderColor = new Color(0.2f, 0.4f, 0.95f);
    protected HeroManager hm;

    // Start is called before the first frame update
    protected void Start()
    {
        /*currentLevel = 3;
        //pull information from a scriptable object
        statFile = data.statFile;
        className = data.className;
        details = data.details;
        
        skills = data.skills;
        swordOK = data.swordOK;
        daggerOK = data.daggerOK;
        axeOK = data.axeOK;
        bowOK = data.bowOK;
        staffOK = data.staffOK;
        //level = data.level;
        currentXp = 0;
        weapon = data.weapon;
        armor = data.armor;
        trinket = data.trinket;
        attackTokenMod = data.attackTokenMod;
    

        //Get stats from JSON
        stats = JsonUtility.FromJson<Stats>(statFile.text);
        level = stats.tableStats[currentLevel].level;
        maxHitPoints = stats.tableStats[currentLevel].hp;
        hitPoints = maxHitPoints;
        maxManaPoints = stats.tableStats[currentLevel].mp;
        manaPoints = maxManaPoints;
        atp = stats.tableStats[currentLevel].atp;           
        dfp = stats.tableStats[currentLevel].dfp;           
        mag = stats.tableStats[currentLevel].mag;          
        res = stats.tableStats[currentLevel].res;
        xpToNextLevel = stats.tableStats[currentLevel].xpToNextLevel;

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
        //Debug.Log("Current Level: " + stats.tableStats[stats.tableStats.Length - 1]);*/
        cs = CombatSystem.instance;
        hm = HeroManager.instance; 
    }

    public void GetData(HeroData data)
    {
        //pull information from a scriptable object
        statFile = data.statFile;
        className = data.className;
        details = data.details;
        skills = data.skills;
        swordOK = data.swordOK;
        daggerOK = data.daggerOK;
        axeOK = data.axeOK;
        bowOK = data.bowOK;
        staffOK = data.staffOK;
        currentXp = 0;
        weapon = data.weapon;
        armor = data.armor;
        trinket = data.trinket;
        attackTokenMod = data.attackTokenMod;
    

        //Get stats from JSON
        stats = JsonUtility.FromJson<Stats>(statFile.text);
        level = stats.tableStats[currentLevel].level;
        maxHitPoints = stats.tableStats[currentLevel].hp;
        hitPoints = maxHitPoints;
        maxManaPoints = stats.tableStats[currentLevel].mp;
        manaPoints = maxManaPoints;
        atp = stats.tableStats[currentLevel].atp;           
        dfp = stats.tableStats[currentLevel].dfp;           
        mag = stats.tableStats[currentLevel].mag;          
        res = stats.tableStats[currentLevel].res;
        spd = stats.tableStats[currentLevel].spd;
        xpToNextLevel = stats.tableStats[currentLevel].xpToNextLevel;

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
        /*if (isTheirTurn)
        {
            //show menu
            Debug.Log("Hero's turn");
            //show action gauge if attacking or using skill
            PassTurn();
        }*/
    }

    public override void TakeAction()
    {
        base.TakeAction();
         //show menu
        Debug.Log("Hero's turn");
        int randEnemy = Random.Range(0, cs.enemiesInCombat.Count);
        Attack(cs.enemiesInCombat[randEnemy]);
        //show action gauge if attacking or using skill
        PassTurn();
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
}
